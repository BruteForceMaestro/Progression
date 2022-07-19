using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using Utf8Json;

namespace XPSystem
{
    class JsonSerialization
    {
        static string SP = Main.Instance.Config.SavePath;
        public static void Save()
        {
            foreach (var kvp in Main.ActivePlayers)
            {
                Main.Players[kvp.Key] = kvp.Value;
            }
            using (FileStream fs = File.Create(SP))
            {
                JsonSerializer.Serialize(fs, Main.Players);
            }
        }
        public static void Read()
        {
            string text = File.ReadAllText(SP);
            try
            {
                Main.Players = JsonSerializer.Deserialize<Dictionary<string, PlayerLogSer>>(text);
            }
            catch (IndexOutOfRangeException)
            {
                Log.Info("Json file is empty");
            }
        }
    }
}
