using System.Text.Json;

namespace CrashEdit.CE
{
    internal sealed record EntityVisual
    {
        public string AnimName;
        public int AnimFrame;

        public EntityVisual(string name, int frame)
        {
            AnimName = name;
            AnimFrame = frame;
        }

        public static EntityVisualList MapCrash2 = [];

        static EntityVisual()
        {
            // default visuals
            MapCrash2.AddVisual(0, 0, new("Cr10V", 0)); // crash
            MapCrash2.AddVisual(1, 1000 + 7, new("WGB0V", 0)); // warp gate bottom 1
            MapCrash2.AddVisual(1, 1000 + 8, new("WGb0V", 0)); // warp gate bottom exit 1
            MapCrash2.AddVisual(1, 1900 + 7, new("WGT0V", 0)); // warp gate top 1
            MapCrash2.AddVisual(1, 1900 + 8, new("WGt0V", 0)); // warp gate top exit 1
            MapCrash2.AddVisual(1, 4000 + 7, new("WGBdV", 0)); // warp gate bottom 4
            MapCrash2.AddVisual(1, 4000 + 8, new("WGbdV", 0)); // warp gate bottom exit 4
            MapCrash2.AddVisual(1, 4900 + 7, new("WGTdV", 0)); // warp gate top 4
            MapCrash2.AddVisual(1, 4900 + 8, new("WGtdV", 0)); // warp gate top exit 4
            MapCrash2.AddVisual(1, 5000 + 7, new("WGBiV", 0)); // warp gate bottom 4
            MapCrash2.AddVisual(1, 5000 + 8, new("WGbiV", 0)); // warp gate bottom exit 4
            MapCrash2.AddVisual(1, 5900 + 7, new("WGTiV", 0)); // warp gate top 4
            MapCrash2.AddVisual(1, 5900 + 8, new("WGtiV", 0)); // warp gate top exit 4
            // MapCrash2.AddVisual(3, 24, new("Cry1V", 0)); // crystal
            MapCrash2.AddVisual(7, 0, new("Fa1fV", 0)); // fireface
            MapCrash2.AddVisual(12, 0, new("Do1aV", 0)); // sewer door
            MapCrash2.AddVisual(12, 8, new("Ee1aV", 0)); // eel
            MapCrash2.AddVisual(14, 4 + 0000, new("Pl1fV", 0)); // drop plat
            MapCrash2.AddVisual(14, 4 + 1000, new("Pl2fV", 0)); // drop plat
            MapCrash2.AddVisual(14, 4 + 2000, new("Pl3fV", 0)); // drop plat
            MapCrash2.AddVisual(14, 4 + 5000, new("Pl50V", 0)); // drop plat
            MapCrash2.AddVisual(14, 4 + 6000, new("Pl60V", 0)); // drop plat
            MapCrash2.AddVisual(16, 0, new("We1aV", 0)); // welder
            MapCrash2.AddVisual(24, 0, new("Se1eV", 0)); // seal
            MapCrash2.AddVisual(24, 1, new("Se2eV", 8)); // seal
            MapCrash2.AddVisual(26, 0, new("Pr2fV", 0)); // crumbler plat 2
            MapCrash2.AddVisual(26, 0 + 1000, new("Pr1fV", 0)); // crumbler plat 1
            MapCrash2.AddVisual(32, 0, new("Sm1eV", 0)); // smasher
            MapCrash2.AddVisual(32, 1, new("Sm2eV", 0)); // constant smasher
            MapCrash2.AddVisual(32, 2, new("Ro1eV", 0)); // roller
            MapCrash2.AddVisual(32, 3, new("Ic3eV", 0)); // icicle
            MapCrash2.AddVisual(34, 0, new("BT10V", 0)); // box tnt
            MapCrash2.AddVisual(34, 2, new("BN10V", 0)); // box empty
            MapCrash2.AddVisual(34, 3, new("BS10V", 0)); // box spring
            MapCrash2.AddVisual(34, 4, new("BC10V", 0)); // box continue
            MapCrash2.AddVisual(34, 4 + 1000, new("BC1iV", 0)); // box continue (space level)
            MapCrash2.AddVisual(34, 5, new("BI10V", 0)); // box iron
            MapCrash2.AddVisual(34, 6, new("BF10V", 0)); // box fruit
            MapCrash2.AddVisual(34, 7, new("BA10V", 0)); // box action
            MapCrash2.AddVisual(34, 8, new("BL10V", 0)); // box life
            MapCrash2.AddVisual(34, 9, new("BD10V", 0)); // box doctor
            MapCrash2.AddVisual(34, 10, new("Bp10V", 0)); // box pickup
            MapCrash2.AddVisual(34, 11, new("BP10V", 0)); // box pow
            MapCrash2.AddVisual(34, 13, new("BG10V", 0)); // box ghost
            MapCrash2.AddVisual(34, 15, new("BS20V", 0)); // box iron spring
            MapCrash2.AddVisual(34, 16, new("BT10V", 0)); // box tnt (auto grav)
            MapCrash2.AddVisual(34, 17, new("Bp10V", 0)); // box pickup (auto grav)
            MapCrash2.AddVisual(34, 18, new("Bn10V", 0)); // box nitro
            MapCrash2.AddVisual(34, 19, new("BG10V", 0)); // box ghost iron
            MapCrash2.AddVisual(34, 23, new("Bs10V", 0)); // box steel
            MapCrash2.AddVisual(34, 24, new("Ba10V", 0)); // box action nitro
            MapCrash2.AddVisual(35, 3, new("Do1iV", 0)); // space door
            MapCrash2.AddVisual(35, 11, new("Ca1iV", 0)); // space cable
            MapCrash2.AddVisual(35, 13, new("SG60V", 0)); // space gun
            MapCrash2.AddVisual(42, 0, new("SL1iV", 0)); // space lab ass
            MapCrash2.AddVisual(42, 2, new("SF1iV", 0)); // space fire
            MapCrash2.AddVisual(46, 1, new("Dr1cV", 0)); // dragonfly
            MapCrash2.AddVisual(55, 0, new("Pu1gV", 8)); // piston up
            MapCrash2.AddVisual(55, 1 + 0000, new("Pi2gV", 4)); // piston small
            MapCrash2.AddVisual(55, 1 + 1000, new("Pi1gV", 4)); // piston
            MapCrash2.AddVisual(55, 2, new("Pa1gV", 0)); // pad
            MapCrash2.AddVisual(55, 3, new("Gu1gV", 0)); // gun
            MapCrash2.AddVisual(55, 4, new("Gd1gV", 0)); // gun down
            MapCrash2.AddVisual(55, 11, new("Bp1gV", 0)); // bonus plaque
            MapCrash2.AddVisual(55, 5, new("Rw1gV", 0)); // robot walker
            MapCrash2.AddVisual(56, 0, new("AP1gV", 0)); // ass pusher
            MapCrash2.AddVisual(57, 1, new("JuFcV", 0)); // firefly
        }

        public static void SaveMaps()
        {
            using Utf8JsonWriter writer = new(new FileStream("CrashEdit.exe.entityvisuals.json", FileMode.Create), new() { Indented = true });
            writer.WriteStartObject();
            writer.WriteStartObject("crash2");
            writer.WriteStartArray("models");
            foreach (var kvp in MapCrash2)
            {
                writer.WriteStartObject();
                writer.WriteNumber("type", kvp.Key / 10000);
                writer.WriteNumber("subtype", kvp.Key % 10000);
                writer.WriteString("anim", kvp.Value.AnimName);
                writer.WriteNumber("frame", kvp.Value.AnimFrame);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.Flush();
        }

        public static void LoadMaps()
        {
            if (!File.Exists("CrashEdit.exe.entityvisuals.json")) return;
            try
            {
                using var json = JsonDocument.Parse(new System.Buffers.ReadOnlySequence<byte>(File.ReadAllBytes("CrashEdit.exe.entityvisuals.json")));
                foreach (var elt in json.RootElement.EnumerateObject())
                {
                    if (elt.Name == "crash2")
                    {
                        MapCrash2.Clear();
                        foreach (var vis in elt.Value.GetProperty("models").EnumerateArray())
                        {
                            var type = vis.GetProperty("type").GetInt32();
                            var subtype = vis.GetProperty("subtype").GetInt32();
                            var anim = vis.GetProperty("anim").GetString()!;
                            var frame = vis.GetProperty("frame").GetInt32();
                            MapCrash2.AddVisual(type, subtype, new(anim, frame));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
