using System.Collections.Generic;
using System.IO;
using Utf8Json;

namespace XPSystem
{
    internal class JsonSerialization
    {
        static string SP = Main.Instance.Config.SavePath;
        public static void Save()
        {
            using (FileStream fs = File.Create(SP))
            {
                JsonSerializer.Serialize(fs, Main.Players);
            }
        }
        public static void Read()
        {
            string text = File.ReadAllText(SP);
            Main.Players = JsonSerializer.Deserialize<Dictionary<string, PlayerLog>>(text);
        }
    }
}
