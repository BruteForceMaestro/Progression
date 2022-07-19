using CommandSystem;
using Exiled.API.Features;
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
            
            if (arguments.Count == 0)
            {
                response = GetTopPlayers(10, sender);
                return true;
            }
            else
            {
                if (int.TryParse(arguments.At(0), out var amount) && amount > 0)
                {
                    response = GetTopPlayers(amount, sender);
                    return true;
                }
                response = "Invalid players amount.";
                return false;
            }
        }

        private string GetTopPlayers(int amount, ICommandSender sender)
        {
            var players = Main.Players.OrderByDescending(o => o.Value.LVL * Main.Instance.Config.XPPerLevel + o.Value.XP);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < amount && i < Main.Players.Count; i++)
            {
                KeyValuePair<string, PlayerLogSer> log = players.ElementAt(i);
                builder.AppendLine($"{i + 1}. ({log.Key}) : LVL{log.Value.LVL}, XP: {log.Value.XP}");
            }

            Player player = Player.Get(sender);
            if (Main.Players.TryGetValue(player.UserId, out PlayerLogSer playerLogSer))
            {
                builder.AppendLine($"\nYou: {players.ToList().FindIndex(o => o.Key == player.UserId) + 1}. LVL{playerLogSer.LVL}, XP: {playerLogSer.XP}");
            }
            return builder.ToString();
        }
    }
}
