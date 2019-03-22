using System;
using System.IO;
using System.ComponentModel;

namespace CrashHacks
{
    public static class ISO2PSX
    {
        public static void Run(Stream input,Stream output,BackgroundWorker backgroundworker)
        {
            byte[] buffer = new byte [2352];
            buffer[0] = 0;
            buffer[1] = 0xFF;
            buffer[2] = 0xFF;
            buffer[3] = 0xFF;
            buffer[4] = 0xFF;
            buffer[5] = 0xFF;
            buffer[6] = 0xFF;
            buffer[7] = 0xFF;
            buffer[8] = 0xFF;
            buffer[9] = 0xFF;
            buffer[10] = 0xFF;
            buffer[11] = 0;
            buffer[15] = 2;
            int minutes = 0;
            int seconds = 2;
            int frames = 0;
            while (true)
            {
                buffer[12] = (byte)((minutes / 10 * 16) | (minutes % 10));
                buffer[13] = (byte)((seconds / 10 * 16) | (seconds % 10));
                buffer[14] = (byte)((frames / 10 * 16) | (frames % 10));
                int length = input.Read(buffer,24,2048);
                if (length == 0)
                    break;
                if (length < 2048)
                    throw new ApplicationException();
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
                backgroundworker.ReportProgress((int)(input.Position * 100 / input.Length));
            }
        }
    }
}
