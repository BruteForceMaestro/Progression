using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace XPSystem
{
    public class Main : Plugin<Config>
    {
        public override string Author { get; } = "nutmaster#486";
        public override string Name { get; } = "XPSystem";
        public override Version Version => new Version(1, 3, 0);
        public override Version RequiredExiledVersion => new Version(5, 2, 0);
        
        EventHandlers handlers;
        readonly Harmony harmony = new Harmony("com.nutmaster.rankchangepatch");
        public static Main Instance { get; set; }
        public static Dictionary<string, PlayerLog> Players { get; set; } = new Dictionary<string, PlayerLog>();

        private void Deserialize()
        {
            if (!File.Exists(Instance.Config.SavePath)) // prevent filenotfound
            {
                JsonSerialization.Save();
                return;
            }
            JsonSerialization.Read();
        }
        public override void OnEnabled()
        {
            handlers = new EventHandlers();
            Instance = this;
            Player.Verified += handlers.OnJoined;
            Player.Dying += handlers.OnKill;
            Server.RoundEnded += handlers.OnRoundEnd;
            Player.Escaping += handlers.OnEscape;
            Player.UsedItem += handlers.UsedItem;
            Player.InteractingDoor += handlers.InteractingDoor;
            Deserialize();
            harmony.PatchAll();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= handlers.OnJoined;
            Player.Dying -= handlers.OnKill;
            Server.RoundEnded -= handlers.OnRoundEnd;
            Player.Escaping -= handlers.OnEscape;
            Player.UsedItem -= handlers.UsedItem;
            Player.InteractingDoor -= handlers.InteractingDoor;
            handlers = null;
            harmony.UnpatchAll(harmony.Id);
            base.OnDisabled();
        }
    }
}