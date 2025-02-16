using CrashEdit.Crash.GOOLIns;

namespace CrashEdit.Crash
{
    public enum GOOLVersion
    {
        Version0, // 1995 + Proto
        Version1, // Crash 1
        Version2, // Crash 2
        Version3  // Crash 3
    }

    public enum ObjectFields
    {
        self = 0,
        parent = 1,
        brother = 2,
        child = 3,
        creator = 4,
        player = 5,
        collider = 6,
        interrupter = 7,
        x = 8,
        y = 9,
        z = 10,
        rotx = 11,
        roty = 12,
        rotz = 13,
        scax = 14,
        scay = 15,
        scaz = 16,
        velx = 17,
        vely = 18,
        velz = 19,
        trotx = 20,
        troty = 21,
        trotz = 22,
        vecx = 23,
        vecy = 24,
        vecz = 25,
        statusa = 26,
        statusb = 27,
        blockflag = 28,
        subtype = 29,
        id = 30,
        sp = 31,
        pc = 32,
        fp = 33,
        tpc = 34,
        epc = 35,
        hpc = 36,
        misc = 37,
        var = 38,
        frametime = 39,
        statetime = 40,
        stalltime = 41,
        framegroup = 42,
        framenum = 43,
        entity = 44,
        pathprog = 45,
        pathlen = 46,
        groundy = 47,
        stateflag = 48,
        speed = 49,
        displaymode = 50,
        field_51 = 51,
        groundtime = 52,
        groundvel = 53,
        zindex = 54,
        eventreceived = 55,
        vfx = 56,
        yzapproach = 57,
        density = 58,
        voice = 59,
        field_60 = 60,
        field_61 = 61,
        field_62 = 62,
        field_63 = 63,
        mem1 = 64,
        mem2, mem3, mem4, mem5, mem6, mem7, mem8, mem9,
        mem10, mem11, mem12, mem13, mem14, mem15, mem16, mem17, mem18, mem19,
        mem20, mem21, mem22, mem23, mem24, mem25, mem26, mem27, mem28, mem29,
        mem30, mem31, mem32, mem33, mem34, mem35, mem36, mem37, mem38, mem39,
        mem40, mem41, mem42, mem43, mem44, mem45, mem46, mem47, mem48, mem49,
        mem50, mem51, mem52, mem53, mem54, mem55, mem56, mem57, mem58, mem59,
        mem60, mem61, mem62, mem63
    };

    public enum ObjectColors1
    {
        lightmat11 = 0,
        lightmat12,
        lightmat13,
        lightmat21,
        lightmat22,
        lightmat23,
        lightmat31,
        lightmat32,
        lightmat33,
        backr,
        backg,
        backb,
        colormatr1,
        colormatg1,
        colormatb1,
        colormatr2,
        colormatg2,
        colormatb2,
        colormatr3,
        colormatg3,
        colormatb3,
        intr,
        intg,
        intb
    }

    public enum ObjectColors2
    {
        mod1 = 0,
        mod2,
        mod3,
        modfinal,
        colr1,
        colg1,
        colb1,
        colr2,
        colg2,
        colb2,
        colr3,
        colg3,
        colb3,
        finalr,
        finalg,
        finalb
    }

    public enum ControllerButtons
    {
        L2 = 0x0001,
        R2 = 0x0002,
        L1 = 0x0004,
        R1 = 0x0008,
        Triangle = 0x0010,
        Circle = 0x0020,
        X = 0x0040,
        Square = 0x0080,
        Select = 0x0100,
        L3 = 0x0200,
        R3 = 0x0400,
        Start = 0x0800,
        Up = 0x1000,
        Right = 0x2000,
        Down = 0x4000,
        Left = 0x8000
    }

    public static class GOOLInterpreter
    {
        private static string[] GlobalsCrash1 = new string[512];
        private static string[] GlobalsCrash2 = new string[512];
        private static string[] GlobalsCrash3 = new string[512];

        static GOOLInterpreter()
        {
            GlobalsCrash1[0x0] = "*level*";
            GlobalsCrash1[0x44] = "*debug*";

            GlobalsCrash2[0x0] = GlobalsCrash1[0x0];
            GlobalsCrash2[0x44] = GlobalsCrash1[0x44];

            GlobalsCrash3[0x0] = GlobalsCrash2[0x0];
            GlobalsCrash3[0x44] = GlobalsCrash2[0x44];
        }

        public static string GetGlobalName(GOOLVersion version, int index)
        {
            var globals = GlobalsCrash1;
            switch (version)
            {
                case GOOLVersion.Version2: globals = GlobalsCrash2; break;
                case GOOLVersion.Version3: globals = GlobalsCrash3; break;
            }

            if (index < 0 || index >= globals.Length)
                return null;
            return globals[index];
        }

        public static GOOLVersion GetVersion(GameVersion ver)
        {
            switch (ver)
            {
                case GameVersion.Crash1Beta1995:
                case GameVersion.Crash1BetaMAR08:
                    return GOOLVersion.Version0;
                case GameVersion.Crash1:
                case GameVersion.Crash1BetaMAY11:
                default:
                    return GOOLVersion.Version1;
                case GameVersion.Crash2:
                    return GOOLVersion.Version2;
                case GameVersion.Crash3:
                    return GOOLVersion.Version3;
            }
        }

        public static int GetProcessOff(GameVersion ver) => GetProcessOff(GetVersion(ver));

        public static int GetProcessOff(GOOLVersion ver)
        {
            switch (ver)
            {
                default:
                case GOOLVersion.Version0:
                case GOOLVersion.Version1:
                    return 0x60;
                case GOOLVersion.Version2:
                case GOOLVersion.Version3:
                    return 0x40;
            }
        }

        public static bool IsReturnInstruction(GOOLInstruction ins)
        {
            return ins.GetName() == "RET";
        }

        public static bool IsMIPSInstruction(GOOLInstruction ins)
        {
            return ins.GetName() == "MIPS";
        }

        public static string GetColor(GOOLVersion ver, int col)
        {
            switch (ver)
            {
                case GOOLVersion.Version0:
                case GOOLVersion.Version1:
                    return ((ObjectColors1)col).ToString();
                case GOOLVersion.Version2:
                case GOOLVersion.Version3:
                    return ((ObjectColors2)col).ToString();
                default:
                    return col.ToString();
            }
        }
    }
}
