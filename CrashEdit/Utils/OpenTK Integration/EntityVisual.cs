using System.Text.Json;

namespace CrashEdit.CE
{
    internal sealed record EntityVisual
    {
        public string AnimName;
        public int AnimFrame;

        public EntityVisual(string name, int frame = -1)
        {
            AnimName = name;
            AnimFrame = frame;
        }

        public static EntityVisualList MapCrash1 = [];
        public static EntityVisualList MapCrash2 = [];
        public static EntityVisualList MapCrash3 = [];

        static EntityVisual()
        {
            // default visuals
            MapCrash1.AddVisual(0, 0, new("WiS1V")); // willy
            MapCrash1.AddVisual(1, 2, new("Mo1fV", 0)); // monkey
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
            MapCrash1.AddVisual(14, 0, new("PoSpV", 0)); // power spring
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
            MapCrash1.AddVisual(25, 0, new("JuPiV")); // jungle plant
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
            MapCrash1.AddVisual(33, 5, new("WT1iV", 0)); // wall torch
            MapCrash1.AddVisual(34, 0, new("BT10V")); // box tnt
            MapCrash1.AddVisual(34, 2, new("BN10V")); // box empty
            MapCrash1.AddVisual(34, 3, new("BS10V", 0)); // box spring
            MapCrash1.AddVisual(34, 4, new("BC10V", 0)); // box continue
            MapCrash1.AddVisual(34, 5, new("BI10V")); // box iron
            MapCrash1.AddVisual(34, 6, new("BF10V", 0)); // box fruit
            MapCrash1.AddVisual(34, 7, new("BA10V", 0)); // box action
            MapCrash1.AddVisual(34, 8, new("BL10V")); // box life
            MapCrash1.AddVisual(34, 9, new("BD10V")); // box doctor
            MapCrash1.AddVisual(34, 10, new("Bp10V")); // box pickup
            MapCrash1.AddVisual(34, 11, new("BP10V")); // box pow
            MapCrash1.AddVisual(34, 13, new("BG10V")); // box ghost
            MapCrash1.AddVisual(34, 15, new("BS20V", 0)); // box iron spring
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
            MapCrash2.AddVisual(1, 1, new("WWP0V")); // warp out
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
            // MapCrash2.AddVisual(3, 24, new("Cry1V")); // crystal
            MapCrash2.AddVisual(2, 0, new("Ts1bV")); // spike turtle
            MapCrash2.AddVisual(2, 5, new("Tu1bV")); // saw turtle
            MapCrash2.AddVisual(7, 0, new("Fa1fV")); // fireface
            MapCrash2.AddVisual(10, 0, new("Mo1fV", 0)); // monkey hop
            MapCrash2.AddVisual(11, 0, new("Go1fV")); // boulder gorilla
            MapCrash2.AddVisual(12, 0, new("Do1aV", 0)); // sewer door
            MapCrash2.AddVisual(12, 8, new("Ee1aV")); // eel
            MapCrash2.AddVisual(14, 4 + 0000, new("Pl1fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 1000, new("Pl2fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 2000, new("Pl3fV")); // drop plat
            MapCrash2.AddVisual(14, 4 + 5000, new("Pl50V")); // drop plat
            MapCrash2.AddVisual(14, 4 + 6000, new("Pl60V")); // drop plat
            MapCrash2.AddVisual(16, 0, new("We1aV")); // welder
            // MapCrash2.AddVisual(18, 0, new("Be2lV", 0)); // bees (+ beehive)
            // MapCrash2.AddVisual(18, 0 + 1000, new("Be4lV", 0)); // bees (+ beehive)
            MapCrash2.AddVisual(24, 0, new("Se1eV")); // seal
            MapCrash2.AddVisual(24, 1, new("Se2eV")); // seal
            MapCrash2.AddVisual(25, 0, new("Pe2eV")); // penguin
            MapCrash2.AddVisual(25, 1, new("Pe2eV")); // penguin pulse
            MapCrash2.AddVisual(26, 0 + 1000, new("Pr1fV", 0)); // crumbler plat 1
            MapCrash2.AddVisual(26, 0 + 2000, new("Pr2fV", 0)); // crumbler plat 2
            MapCrash2.AddVisual(26, 0 + 3000, new("Pr3fV")); // leaner
            MapCrash2.AddVisual(26, 0 + 4000, new("Pr4fV")); // spinner
            MapCrash2.AddVisual(27, 0, new("Po3eV")); // porcupine
            MapCrash2.AddVisual(30, 0, new("Ol1bV")); // ostrich
            MapCrash2.AddVisual(32, 0, new("Sm1eV")); // smasher
            MapCrash2.AddVisual(32, 1, new("Sm2eV")); // constant smasher
            MapCrash2.AddVisual(32, 2, new("Ro1eV")); // roller
            MapCrash2.AddVisual(32, 3, new("Ic3eV")); // icicle
            MapCrash2.AddVisual(33, 0, new("AB10V")); // ass banger
            MapCrash2.AddVisual(34, 0, new("BT10V")); // box tnt
            MapCrash2.AddVisual(34, 2, new("BN10V")); // box empty
            MapCrash2.AddVisual(34, 3, new("BS10V", 0)); // box spring
            MapCrash2.AddVisual(34, 4, new("BC10V", 0)); // box continue
            MapCrash2.AddVisual(34, 4 + 1000, new("BC1iV", 0)); // box continue (space level)
            MapCrash2.AddVisual(34, 5, new("BI10V")); // box iron
            MapCrash2.AddVisual(34, 6, new("BF10V", 0)); // box fruit
            MapCrash2.AddVisual(34, 7, new("BA10V", 0)); // box action
            MapCrash2.AddVisual(34, 8, new("BL10V")); // box life
            MapCrash2.AddVisual(34, 9, new("BD10V")); // box doctor
            MapCrash2.AddVisual(34, 10, new("Bp10V")); // box pickup
            MapCrash2.AddVisual(34, 11, new("BP10V")); // box pow
            MapCrash2.AddVisual(34, 13, new("BG10V")); // box ghost
            MapCrash2.AddVisual(34, 15, new("BS20V", 0)); // box iron spring
            MapCrash2.AddVisual(34, 16, new("BT10V")); // box tnt (auto grav)
            MapCrash2.AddVisual(34, 17, new("Bp10V")); // box pickup (auto grav)
            MapCrash2.AddVisual(34, 18, new("Bn10V")); // box nitro
            MapCrash2.AddVisual(34, 19, new("BG10V")); // box ghost iron
            MapCrash2.AddVisual(34, 20, new("BN10V")); // box empty (auto grav)
            MapCrash2.AddVisual(34, 23, new("Bs10V", 0)); // box steel
            MapCrash2.AddVisual(34, 24, new("Ba10V", 0)); // box action nitro
            MapCrash2.AddVisual(35, 3, new("Do1iV", 0)); // space door
            MapCrash2.AddVisual(35, 6, new("Do2iV", 0)); // space lock
            MapCrash2.AddVisual(35, 6 + 1000, new("Do4iV")); // space lock (lamps)
            MapCrash2.AddVisual(35, 15, new("Sb1iV")); // space bomb ring
            MapCrash2.AddVisual(35, 11, new("Ca1iV")); // space cable
            MapCrash2.AddVisual(35, 13, new("SG60V", 0)); // space gun
            MapCrash2.AddVisual(38, 0, new("Ep1nV")); // spore plant
            MapCrash2.AddVisual(38, 0 + 1000, new("Ep5nV", 0)); // spore plant
            MapCrash2.AddVisual(38, 4, new("JuPpV")); // evil plant
            MapCrash2.AddVisual(39, 0, new("Mi1lV", 0)); // mine
            MapCrash2.AddVisual(39, 2, new("pl1lV")); // plank
            MapCrash2.AddVisual(42, 0, new("SL1iV")); // space lab ass
            MapCrash2.AddVisual(42, 2, new("SF1iV")); // space fire
            MapCrash2.AddVisual(46, 0, new("Dr1cV")); // dragonfly
            MapCrash2.AddVisual(46, 1, new("Dr1cV")); // dragonfly
            MapCrash2.AddVisual(47, 1, new("Hp1pV")); // hippo
            MapCrash2.AddVisual(47, 2, new("Bo1pV")); // board
            MapCrash2.AddVisual(47, 4, new("Mf1pV")); // mine float
            MapCrash2.AddVisual(47, 5, new("Pa1pV", 0)); // piranha fish
            MapCrash2.AddVisual(47, 7, new("Mf1pV")); // mine path
            MapCrash2.AddVisual(50, 8, new("Cl1sV")); // intro crystal light ray
            MapCrash2.AddVisual(50, 10, new("Sw1sV")); // intro star window
            MapCrash2.AddVisual(50, 15, new("Sb1sV")); // intro star window opacity 50%
            MapCrash2.AddVisual(50, 19, new("Li1sV")); // intro ship light
            MapCrash2.AddVisual(55, 0, new("Pu1gV", 8)); // piston up
            MapCrash2.AddVisual(55, 1 + 0000, new("Pi2gV", 4)); // piston small
            MapCrash2.AddVisual(55, 1 + 1000, new("Pi1gV", 4)); // piston
            MapCrash2.AddVisual(55, 2, new("Pa1gV", 0)); // pad
            MapCrash2.AddVisual(55, 3, new("Gu1gV")); // gun
            MapCrash2.AddVisual(55, 4, new("Gd1gV")); // gun down
            MapCrash2.AddVisual(55, 11, new("Bp1gV")); // bonus plaque
            MapCrash2.AddVisual(55, 5, new("Rw1gV")); // robot walker
            MapCrash2.AddVisual(56, 0, new("AP1gV")); // ass pusher
            MapCrash2.AddVisual(57, 1, new("JuFcV")); // firefly
        }

        private const string MapsFileName = "CrashEdit.exe.entityvisuals-v1.json";

        public static void SaveMaps()
        {
            static void write_map(Utf8JsonWriter writer, EntityVisualList map, string name)
            {
                writer.WriteStartObject(name);
                writer.WriteStartArray("models");
                foreach (var kvp in map)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber("type", kvp.Key / 100000);
                    writer.WriteNumber("subtype", kvp.Key % 100000);
                    writer.WriteString("anim", kvp.Value.AnimName);
                    writer.WriteNumber("frame", kvp.Value.AnimFrame);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            using Utf8JsonWriter writer = new(new FileStream(MapsFileName, FileMode.Create), new() { Indented = true });
            writer.WriteStartObject();
            write_map(writer, MapCrash1, "crash1");
            write_map(writer, MapCrash2, "crash2");
            writer.WriteEndObject();
            writer.Flush();
        }

        public static void LoadMaps()
        {
            static void read_map(EntityVisualList map, JsonProperty elt)
            {
                foreach (var vis in elt.Value.GetProperty("models").EnumerateArray())
                {
                    var type = vis.GetProperty("type").GetInt32();
                    var subtype = vis.GetProperty("subtype").GetInt32();
                    var anim = vis.GetProperty("anim").GetString()!;
                    var frame = vis.GetProperty("frame").GetInt32();
                    map.AddVisual(type, subtype, new(anim, frame));
                }
            }

            if (!File.Exists(MapsFileName)) return;
            try
            {
                using var json = JsonDocument.Parse(new System.Buffers.ReadOnlySequence<byte>(File.ReadAllBytes(MapsFileName)));
                foreach (var elt in json.RootElement.EnumerateObject())
                {
                    if (elt.Name == "crash1")
                    {
                        read_map(MapCrash1, elt);
                    }
                    else if (elt.Name == "crash2")
                    {
                        read_map(MapCrash2, elt);
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
