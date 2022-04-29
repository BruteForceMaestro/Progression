using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using Exiled.API.Features;

namespace XPSystem
{
    internal class Get : ICommand
    {
        public string Command => "get";

        public static Get Instance { get; } = new Get();

        public string[] Aliases => new string[] { };

        public string Description => "Gets the player's XP and LVL values by userid";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("xps.get"))
            {
                response = "You don't have permission (xps.get) to use this command.";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage : XPSystem get (userid | in-game id)";
                return false;
            }
            PlayerLog log;
            if (!Main.Players.TryGetValue(Player.Get(sender).UserId, out log) && !Main.Players.TryGetValue(arguments.At(0), out log))
            {
                response = "incorrect userid";
                return false;
            }
            response = $"LVL: {log.LVL} | XP: {log.XP}";
            return true;
        }
    }
}
