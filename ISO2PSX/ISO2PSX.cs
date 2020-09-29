using System;
using System.ComponentModel;
using System.IO;

internal static class BitConv
{
    public static int FromInt32(byte[] str, int offset)
    {
        if (str == null)
            throw new ArgumentNullException("str");
        if (offset < 0)
            throw new ArgumentOutOfRangeException("offset");
        if (offset + 4 > str.Length)
            throw new ArgumentOutOfRangeException("offset");
        int result = 0;
        result |= str[offset + 0] << 8 * 0;
        result |= str[offset + 1] << 8 * 1;
        result |= str[offset + 2] << 8 * 2;
        result |= str[offset + 3] << 8 * 3;
        return result;
    }

    public static void ToInt32(byte[] str, int offset, int value)
    {
        if (str == null)
            throw new ArgumentNullException("str");
        if (offset < 0)
            throw new ArgumentOutOfRangeException("offset");
        if (offset + 4 > str.Length)
            throw new ArgumentOutOfRangeException("offset");
        str[offset] = (byte)value;
        str[offset + 1] = (byte)(value >> 8);
        str[offset + 2] = (byte)(value >> 8 * 2);
        str[offset + 3] = (byte)(value >> 8 * 3);
    }
}

public static class ISO2PSX
{
    private static uint[] edcTable = new uint[256];
    private static byte[] eccFTable = new byte[256];
    private static byte[] eccBTable = new byte[256];

    static ISO2PSX() {
        for (int i = 0; i < 256; i++) {
            uint edc = (uint)i;
            for (int ii = 0; ii < 8; ii++) {
                if ((edc & 1) != 0) {
                    edc >>= 1;
                    edc ^= 0xD8018001U;
                } else {
                    edc >>= 1;
                }
            }
            edcTable[i] = edc;

            byte m = (byte)(i << 1);
            if ((i & 0x80) != 0) {
                m ^= 0x1D;
            }
            eccFTable[i] = m;
            eccBTable[i ^ m] = (byte)i;
        }
    }

    private static void DoEDC(byte[] buffer)
    {
        uint edc = 0;
        for (int i = 0x18; i < 0x818; i++) {
            edc = (edc >> 8) ^ edcTable[(edc & 0xFF) ^ buffer[i]];
        }
        BitConv.ToInt32(buffer, 0x818, (int)edc);
    }

    private static void DoECCBlock(byte[] buffer, int majorCount, int minorCount, int majorMul, int minorInc, int destOffset)
    {
        int addrMode = BitConv.FromInt32(buffer, 12);
        BitConv.ToInt32(buffer, 12, 0);

        for (int major = 0; major < majorCount; major++) {
            byte eccA = 0;
            byte eccB = 0;

            for (int minor = 0; minor < minorCount; minor++) {
                byte v = buffer[0xC + ((major >> 1) * majorMul + (major & 1) + minor * minorInc) % (majorCount * minorCount)];
                eccA = eccFTable[eccA ^ v];
                eccB ^= v;
            }

            eccA = eccBTable[eccFTable[eccA] ^ eccB];
            buffer[destOffset + major] = eccA;
            buffer[destOffset + major + majorCount] = (byte)(eccA ^ eccB);
        }

        BitConv.ToInt32(buffer, 12, addrMode);
    }

    public static void Run(Stream input, Stream output,BackgroundWorker backgroundworker = null)
    {
        byte[] buffer = new byte[2352];
        buffer[0x0] = 0;
        buffer[0x1] = 0xFF;
        buffer[0x2] = 0xFF;
        buffer[0x3] = 0xFF;
        buffer[0x4] = 0xFF;
        buffer[0x5] = 0xFF;
        buffer[0x6] = 0xFF;
        buffer[0x7] = 0xFF;
        buffer[0x8] = 0xFF;
        buffer[0x9] = 0xFF;
        buffer[0xA] = 0xFF;
        buffer[0xB] = 0;
        buffer[0xF] = 2;
        int minutes = 0;
        int seconds = 2;
        int frames = 0;
        while (true)
        {
            buffer[0xC] = (byte)((minutes / 10 * 16) | (minutes % 10));
            buffer[0xD] = (byte)((seconds / 10 * 16) | (seconds % 10));
            buffer[0xE] = (byte)((frames / 10 * 16) | (frames % 10));
            int length = input.Read(buffer,24,2048);
            if (length == 0)
                break;
            if (length < 2048)
                throw new ApplicationException();

            DoEDC(buffer);
            DoECCBlock(buffer, 86, 24, 2, 86, 0x81C);
            DoECCBlock(buffer, 52, 43, 86, 88, 0x8C8);

            frames++;
            if (frames == 75)
            {
                seconds++;
                frames = 0;
            }
            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
            }
            output.Write(buffer,0,2352);
            backgroundworker?.ReportProgress((int)(input.Position * 100 / input.Length));
        }
    }
}
