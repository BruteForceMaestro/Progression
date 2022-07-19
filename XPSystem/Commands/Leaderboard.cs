using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPSystem
{
    public class Leaderboard : ICommand
    {
        public string Command => "leaderboard";

        public static Leaderboard Instance { get; } = new Leaderboard();

        public string[] Aliases => new string[] { "lb" };

        public string Description => "Players, sorted by their LV (Level of Violence). Use: XPSystem leaderboard (amount)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("xps.get"))
            {
                response = "You don't have permission (xps.get) to use this command.";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = GetTopPlayers(10);
                return true;
            }
            else
            {
                if (int.TryParse(arguments.At(0), out var amount) && amount > 0)
                {
                    response = GetTopPlayers(amount);
                    return true;
                }
                response = "Invalid players amount.";
                return false;
            }
        }

        private string GetTopPlayers(int amount)
        {
            var players = Main.Players.OrderByDescending(o => o.Value.LVL).Take(amount);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i <= amount; i++)
            {
                KeyValuePair<string, PlayerLogSer> log = players.ElementAt(i);
                builder.AppendLine($"{i + 1}. ({log.Key}) : LVL{log.Value.LVL}, XP: {log.Value.XP}");
            }
            return builder.ToString();
        }
    }
}
