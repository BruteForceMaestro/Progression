using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace XPSystem
{

    internal class Set : ICommand
    {
        public static Set Instance { get; } = new Set();
        public string Command => "set";
        public string[] Aliases => Array.Empty<string>();
        public string Description => $"Set a certain value in player's lvl variable.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("xps.set"))
            {
                response = "You don't have permission (xps.set) to use this command.";
                return false;
            }
            if (arguments.Count != 2)
            {
                response = "Usage : XPSystem set (UserId) (int amount)";
                return false;
            }
            string id = arguments.At(0);
            if (!Main.Players.TryGetValue(id, out PlayerLogSer log))
            {
                response = "invalid ID";
                return false;
            }
            if (int.TryParse(arguments.At(1), out int lvl))
            {
                log.LVL = lvl;
                response = $"{id}'s LVL is now {log.LVL}";
                if (Main.ActivePlayers.TryGetValue(id, out PlayerLog activeLog))
                {
                    activeLog.ApplyRank();
                }
                return true;
            }
            response = $"Invalid amount of LVLs : {lvl}";
            return false;
        }
    }
}
