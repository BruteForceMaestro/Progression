using Exiled.API.Features;
using MEC;
using System.Collections.Generic;

namespace XPSystem
{
    // unserializable version meant for regular use in active players
    public class PlayerLog : PlayerLogSer
    {
        public Player Player { get; set; }       

        public PlayerLog(PlayerLogSer log, Player player)
        {
            XP = log.XP;
            LVL = log.LVL;
            Player = player;
            ApplyRank();
        }

        public void AddXP(int xp)
        {
            XP += xp;
            int lvlsGained = XP / Main.Instance.Config.XPPerLevel;
            if (lvlsGained > 0)
            {
                LVL += lvlsGained;
                XP -= lvlsGained * Main.Instance.Config.XPPerLevel;
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    Player.ShowHint(Main.Instance.Config.AddedLVLHint.Replace("%level%", LVL.ToString()));
                }
                ApplyRank();
            }
            else if (Main.Instance.Config.ShowAddedXP)
            {
                Player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
        }

        public void ApplyRank()
        {
            Badge badge = GetLVLBadge();

            if (!Main.Instance.Config.OverrideRAColor && Player.Group != null)
            {
                Player.RankColor = Player.Group.BadgeColor;
            }
            else
            {
                Player.RankColor = badge.Color;
            }

            string oldBadge = Player.Group?.BadgeText ?? "";

            string newBadge = Main.Instance.Config.BadgeStructure
                .Replace("%lvl%", LVL.ToString())
                .Replace("%badge%", badge.Name)
                .Replace("%oldbadge%", oldBadge);

            Log.Debug(newBadge);

            Player.RankName = newBadge;
        }

        private Badge GetLVLBadge()
        {
            Badge biggestLvl = new Badge
            {
                Name = "UNDEFINED",
                Color = "red"
            };
            foreach (var pair in Main.Instance.Config.LevelsBadge)
            {
                if (LVL < pair.Key)
                {
                    break;
                }
                biggestLvl = pair.Value;
            }
            return biggestLvl;
        }        
    }
}
