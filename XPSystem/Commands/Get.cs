using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace XPSystem
{
    internal class Get : ICommand
    {
        public string Command => "get";

        public static Get Instance { get; } = new Get();

        public string[] Aliases => new string[] { "stats" };

        public string Description => "Gets the player's XP and LVL values by userid";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                Player player = Player.Get(sender);
                if (player == null || player.DoNotTrack)
                {
                    response = "usage: xps get (userid)";
                    return false;
                }
                var players = Main.Players.OrderByDescending(o => o.Value.LVL * Main.Instance.Config.XPPerLevel + o.Value.XP);
                var playerLogSer = Main.Players[player.UserId];

                response = $"\n#{players.ToList().FindIndex(o => o.Key == player.UserId) + 1}. LVL{playerLogSer.LVL}, XP: {playerLogSer.XP}";
                return true;
            }
            if (!Main.Players.TryGetValue(arguments.At(0), out PlayerLogSer log))
            {
                response = "invalid ID";
                return false;
            }
            response = $"LVL: {log.LVL} | XP: {log.XP}";
            return true;
        }
    }
}
