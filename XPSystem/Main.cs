using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using Player = Exiled.Events.Handlers.Player;
using Scp079 = Exiled.Events.Handlers.Scp079;
using Server = Exiled.Events.Handlers.Server;

namespace XPSystem
{
    public class Main : Plugin<Config>
    {
        EventHandlers handlers;
        readonly Harmony harmony = new Harmony("com.nutmaster.rankchangepatch");
        public static Main Instance { get; set; }
        public static Dictionary<string, PlayerLogSer> Players { get; set; } = new Dictionary<string, PlayerLogSer>();
        public static Dictionary<string, PlayerLog> ActivePlayers { get; set; } = new Dictionary<string, PlayerLog>(); // added to prevent situations where it is unknown if player is on the server or not.
        public override Version Version => new Version(1, 2, 1);
        public override Version RequiredExiledVersion => new Version(5, 0, 0);

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
            Player.Verified += handlers.OnJoined;
            Player.Dying += handlers.OnKill;
            Server.RoundEnded += handlers.OnRoundEnd;
            Player.Escaping += handlers.OnEscape;
            Player.Left += handlers.OnLeaving;
            Scp079.GainingExperience += handlers.OnAssist;
            Instance = this;
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
            Player.Left -= handlers.OnLeaving;
            Scp079.GainingExperience -= handlers.OnAssist;
            handlers = null;
            harmony.UnpatchAll();
            base.OnDisabled();
        }
    }
}