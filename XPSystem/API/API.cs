using Exiled.API.Features;

namespace XPSystem
{
    public static class API
    {
        public static void AddXP(Player player, int xp)
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
                ApplyRank(player, log);
            }
            else if (Main.Instance.Config.ShowAddedXP)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
        }
        public static void ApplyRank(Player player, PlayerLog log)
        {
            Badge badge = GetLVLBadge(log);
            string RAColor = player.Group?.BadgeColor;

            log.GeneratedBadge = badge.Name;
            player.RankName = player.Group?.BadgeText ?? "";
            player.RankColor = Main.Instance.Config.OverrideColor && RAColor != null ? RAColor : badge.Color;
        }
        
        private static Badge GetLVLBadge(PlayerLog player)
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
