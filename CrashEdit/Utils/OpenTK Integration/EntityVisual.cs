using System.Text.Json;

namespace CrashEdit.CE
{
    internal sealed record EntityVisual
    {
        public string AnimName;
        public int AnimFrame;

        public EntityVisual(string name, int frame = 0)
        {
            AnimName = name;
            AnimFrame = frame;
        }

        public static EntityVisualList MapCrash1 = [];
        public static EntityVisualList MapCrash2 = [];

        static EntityVisual()
        {
            // default visuals
            MapCrash1.AddVisual(0, 0, new("WiS1V")); // willy
            MapCrash1.AddVisual(1, 2, new("Mo1fV")); // monkey
            MapCrash1.AddVisual(8, 0, new("PoD1V")); // power door
            MapCrash1.AddVisual(8, 1, new("PoD2V")); // power door double left
            MapCrash1.AddVisual(8, 1 + 1000, new("PoD3V")); // power door double right
            MapCrash1.AddVisual(8, 3, new("PoD5V")); // power door double 2 left
            MapCrash1.AddVisual(8, 3 + 1000, new("PoD6V")); // power door double 2 right
            MapCrash1.AddVisual(8, 5, new("PoD4V")); // power door 2
            MapCrash1.AddVisual(8, 6, new("PoD1V")); // locked power door
            MapCrash1.AddVisual(9, 0, new("PRSSV")); // power robot
            MapCrash1.AddVisual(9, 5, new("Psr5V")); // power survey robot
            MapCrash1.AddVisual(9, 6, new("Psr5V")); // power survey robot
            MapCrash1.AddVisual(10, 0, new("PRESV")); // power robot enemy
            MapCrash1.AddVisual(12, 0, new("SliMV")); // slim
            MapCrash1.AddVisual(14, 0, new("PoSpV")); // power spring
            MapCrash1.AddVisual(17, 0, new("FaS1V")); // fat
            MapCrash1.AddVisual(19, 0, new("Tu1iV")); // turtle
            MapCrash1.AddVisual(22, 6, new("JuRcV")); // jungle roller
            MapCrash1.AddVisual(22, 12, new("JuSeV")); // jungle stone
            MapCrash1.AddVisual(22, 13, new("JB4eV")); // jungle barricade high small
            MapCrash1.AddVisual(22, 14, new("JB5eV")); // jungle barricade high medium
            MapCrash1.AddVisual(22, 15, new("JB6eV")); // jungle barricade high large
            MapCrash1.AddVisual(22, 16, new("JB1eV")); // jungle barricade low small
            MapCrash1.AddVisual(22, 17, new("JB2eV")); // jungle barricade low medium
            MapCrash1.AddVisual(22, 18, new("JB3eV")); // jungle barricade low large
            MapCrash1.AddVisual(25, 0, new("JupiV")); // jungle plant
            MapCrash1.AddVisual(27, 0, new("JuOWV")); // jungle ocean wave
            MapCrash1.AddVisual(28, 1, new("Rl1fV")); // river leaf
            MapCrash1.AddVisual(28, 2, new("RB1fV")); // river branch
            MapCrash1.AddVisual(28, 5, new("RF1fV")); // river fish
            MapCrash1.AddVisual(28, 6, new("Rv1fV")); // river venus
            MapCrash1.AddVisual(28, 7, new("RV1fV")); // river venus
            MapCrash1.AddVisual(31, 0, new("Cr19V")); // crab
            MapCrash1.AddVisual(32, 1, new("WWP0V")); // warp out
            MapCrash1.AddVisual(33, 0, new("WP1iV")); // wall plat
            MapCrash1.AddVisual(33, 1, new("WS1iV")); // wall shield
            MapCrash1.AddVisual(33, 3, new("SL1iV")); // spike log
            MapCrash1.AddVisual(33, 4, new("SL1iV")); // spike log
            MapCrash1.AddVisual(33, 5, new("WT1iV")); // wall torch
            MapCrash1.AddVisual(34, 0, new("BT10V")); // box tnt
            MapCrash1.AddVisual(34, 2, new("BN10V")); // box empty
            MapCrash1.AddVisual(34, 3, new("BS10V")); // box spring
            MapCrash1.AddVisual(34, 4, new("BC10V")); // box continue
            MapCrash1.AddVisual(34, 5, new("BI10V")); // box iron
            MapCrash1.AddVisual(34, 6, new("BF10V")); // box fruit
            MapCrash1.AddVisual(34, 7, new("BA10V")); // box action
            MapCrash1.AddVisual(34, 8, new("BL10V")); // box life
            MapCrash1.AddVisual(34, 9, new("BD10V")); // box doctor
            MapCrash1.AddVisual(34, 10, new("Bp10V")); // box pickup
            MapCrash1.AddVisual(34, 11, new("BP10V")); // box pow
            MapCrash1.AddVisual(34, 13, new("BG10V")); // box ghost
            MapCrash1.AddVisual(34, 15, new("BS20V")); // box iron spring
            MapCrash1.AddVisual(34, 16, new("BT10V")); // box tnt (auto grav)
            MapCrash1.AddVisual(34, 17, new("Bp10V")); // box pickup (auto grav)
            MapCrash1.AddVisual(34, 19, new("BG10V")); // box ghost iron
            MapCrash1.AddVisual(34, 20, new("BN10V")); // box empty (auto grav)
            MapCrash1.AddVisual(38, 0, new("Na1iV")); // native
            MapCrash1.AddVisual(38, 1, new("Na1iV")); // native
            MapCrash1.AddVisual(58, 0, new("Gc10V")); // gem clear
            MapCrash1.AddVisual(58, 1, new("Ge20V")); // gem red
            MapCrash1.AddVisual(58, 2, new("Ge10V")); // gem blue
            MapCrash1.AddVisual(58, 3, new("Ge50V")); // gem green
            MapCrash1.AddVisual(58, 4, new("Ge40V")); // gem purple
            MapCrash1.AddVisual(58, 5, new("Ge60V")); // gem yellow
            MapCrash1.AddVisual(58, 6, new("Ge30V")); // gem orange

            MapCrash2.AddVisual(0, 0, new("Cr10V")); // crash
            MapCrash2.AddVisual(1, 1000 + 7, new("WGB0V")); // warp gate bottom 1
            MapCrash2.AddVisual(1, 1000 + 8, new("WGb0V")); // warp gate bottom exit 1
            MapCrash2.AddVisual(1, 1900 + 7, new("WGT0V")); // warp gate top 1
            MapCrash2.AddVisual(1, 1900 + 8, new("WGt0V")); // warp gate top exit 1
            MapCrash2.AddVisual(1, 4000 + 7, new("WGBdV")); // warp gate bottom 4
            MapCrash2.AddVisual(1, 4000 + 8, new("WGbdV")); // warp gate bottom exit 4
            MapCrash2.AddVisual(1, 4900 + 7, new("WGTdV")); // warp gate top 4
            MapCrash2.AddVisual(1, 4900 + 8, new("WGtdV")); // warp gate top exit 4
            MapCrash2.AddVisual(1, 5000 + 7, new("WGBiV")); // warp gate bottom 4
            MapCrash2.AddVisual(1, 5000 + 8, new("WGbiV")); // warp gate bottom exit 4
            MapCrash2.AddVisual(1, 5900 + 7, new("WGTiV")); // warp gate top 4
            MapCrash2.AddVisual(1, 5900 + 8, new("WGtiV")); // warp gate top exit 4
            // MapCrash2.AddVisual(3, 24, new("Cry1V")); // crystal
            MapCrash2.AddVisual(7, 0, new("Fa1fV")); // fireface
            MapCrash2.AddVisual(12, 0, new("Do1aV")); // sewer door
            MapCrash2.AddVisual(12, 8, new("Ee1aV")); // eel
            MapCrash2.AddVisual(14, 4 + 0000, new("Pl1fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 1000, new("Pl2fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 2000, new("Pl3fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 5000, new("Pl50V")); // drop plat
            MapCrash2.AddVisual(14, 4 + 6000, new("Pl60V")); // drop plat
            MapCrash2.AddVisual(16, 0, new("We1aV")); // welder
            MapCrash2.AddVisual(24, 0, new("Se1eV")); // seal
            MapCrash2.AddVisual(24, 1, new("Se2eV", 8)); // seal
            MapCrash2.AddVisual(26, 0, new("Pr2fV")); // crumbler plat 2
            MapCrash2.AddVisual(26, 0 + 1000, new("Pr1fV")); // crumbler plat 1
            MapCrash2.AddVisual(32, 0, new("Sm1eV")); // smasher
            MapCrash2.AddVisual(32, 1, new("Sm2eV")); // constant smasher
            MapCrash2.AddVisual(32, 2, new("Ro1eV")); // roller
            MapCrash2.AddVisual(32, 3, new("Ic3eV")); // icicle
            MapCrash2.AddVisual(34, 0, new("BT10V")); // box tnt
            MapCrash2.AddVisual(34, 2, new("BN10V")); // box empty
            MapCrash2.AddVisual(34, 3, new("BS10V")); // box spring
            MapCrash2.AddVisual(34, 4, new("BC10V")); // box continue
            MapCrash2.AddVisual(34, 4 + 1000, new("BC1iV")); // box continue (space level)
            MapCrash2.AddVisual(34, 5, new("BI10V")); // box iron
            MapCrash2.AddVisual(34, 6, new("BF10V")); // box fruit
            MapCrash2.AddVisual(34, 7, new("BA10V")); // box action
            MapCrash2.AddVisual(34, 8, new("BL10V")); // box life
            MapCrash2.AddVisual(34, 9, new("BD10V")); // box doctor
            MapCrash2.AddVisual(34, 10, new("Bp10V")); // box pickup
            MapCrash2.AddVisual(34, 11, new("BP10V")); // box pow
            MapCrash2.AddVisual(34, 13, new("BG10V")); // box ghost
            MapCrash2.AddVisual(34, 15, new("BS20V")); // box iron spring
            MapCrash2.AddVisual(34, 16, new("BT10V")); // box tnt (auto grav)
            MapCrash2.AddVisual(34, 17, new("Bp10V")); // box pickup (auto grav)
            MapCrash2.AddVisual(34, 18, new("Bn10V")); // box nitro
            MapCrash2.AddVisual(34, 19, new("BG10V")); // box ghost iron
            MapCrash2.AddVisual(34, 20, new("BN10V")); // box empty (auto grav)
            MapCrash2.AddVisual(34, 23, new("Bs10V")); // box steel
            MapCrash2.AddVisual(34, 24, new("Ba10V")); // box action nitro
            MapCrash2.AddVisual(35, 3, new("Do1iV")); // space door
            MapCrash2.AddVisual(35, 11, new("Ca1iV")); // space cable
            MapCrash2.AddVisual(35, 13, new("SG60V")); // space gun
            MapCrash2.AddVisual(42, 0, new("SL1iV")); // space lab ass
            MapCrash2.AddVisual(42, 2, new("SF1iV")); // space fire
            MapCrash2.AddVisual(46, 1, new("Dr1cV")); // dragonfly
            MapCrash2.AddVisual(55, 0, new("Pu1gV", 8)); // piston up
            MapCrash2.AddVisual(55, 1 + 0000, new("Pi2gV", 4)); // piston small
            MapCrash2.AddVisual(55, 1 + 1000, new("Pi1gV", 4)); // piston
            MapCrash2.AddVisual(55, 2, new("Pa1gV")); // pad
            MapCrash2.AddVisual(55, 3, new("Gu1gV")); // gun
            MapCrash2.AddVisual(55, 4, new("Gd1gV")); // gun down
            MapCrash2.AddVisual(55, 11, new("Bp1gV")); // bonus plaque
            MapCrash2.AddVisual(55, 5, new("Rw1gV")); // robot walker
            MapCrash2.AddVisual(56, 0, new("AP1gV")); // ass pusher
            MapCrash2.AddVisual(57, 1, new("JuFcV")); // firefly
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
