using Exiled.API.Features;

namespace XPSystem
{
    static public class API
    {
        static public void AddXP(Player player, int xp)
        {
            if (player.DoNotTrack || xp <= 0)
            {
                return;
            }
            PlayerLog log = Main.Players[player.UserId];
            log.XP += xp;
            int lvlsGained = log.XP / Main.Instance.Config.XPPerLevel;
            if (lvlsGained > 0)
            {
                log.LVL += lvlsGained;
                log.XP -= lvlsGained * Main.Instance.Config.XPPerLevel;
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    player.ShowHint(Main.Instance.Config.AddedLVLHint.Replace("%level%", log.LVL.ToString()));
                }
                EvaluateRank(player, log);
            }
            else if (Main.Instance.Config.ShowAddedXP)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
        }
        static public void EvaluateRank(Player player, PlayerLog log)
        {
            string badgeText = player.Group == null ? string.Empty : player.Group.BadgeText;
            Badge badge = GetLVLBadge(log);
            player.RankName = Main.Instance.Config.BadgeStructure.Replace("%lvl%", log.LVL.ToString()).Replace("%badge%", badge.Name).Replace("%oldbadge%", badgeText);
            player.RankColor = badge.Color;
        }
        static private Badge GetLVLBadge(PlayerLog player)
        {
            Badge biggestLvl = new Badge
            {
                Name = "UNDEFINED",
                Color = "red"
            };
            foreach (var pair in Main.Instance.Config.LevelsBadge)
            {
                if (player.LVL < pair.Key)
                {
                    break;
                }
                biggestLvl = pair.Value;
            }
            return biggestLvl;
        }
    }
}
