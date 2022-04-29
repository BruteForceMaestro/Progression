using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;

namespace XPSystem
{
    class EventHandlers
    {
        public void OnJoined(VerifiedEventArgs ev)
        {
            if (ev.Player.DoNotTrack)
            {
                ev.Player.OpenReportWindow(Main.Instance.Config.DNTHint);
                ev.Player.RankName = Main.Instance.Config.DNTBadge.Name;
                ev.Player.RankColor = Main.Instance.Config.DNTBadge.Color;
                return;
            }
            if (!Main.Players.TryGetValue(ev.Player.UserId, out PlayerLog log))
            {
                log = new PlayerLog(ev.Player);
                Main.Players[ev.Player.UserId] = log;
                log.NewQuest();
                return;
            }
            log.Player = ev.Player;
            log.ApplyRank();
        }

        public void OnKill(DyingEventArgs ev)
        {
            if (ev.Target == null || ev.Target.DoNotTrack)
            {
                return;
            }
            Player killer = ev.Handler.Type == DamageType.PocketDimension ? Player.Get(RoleType.Scp106).FirstOrDefault() : ev.Killer;
            if (killer == null)
            {
                return;
            }
            if (Main.Instance.Config.KillXP.TryGetValue(killer.Role, out var killxpdict) && killxpdict.TryGetValue(ev.Target.Role, out int xp))
            {
                Main.Players[ev.Killer.UserId].AddXP(xp);
            }

            if (Main.Players.TryGetValue(ev.Killer.UserId, out var playerLog) &&
                playerLog.CurrentQuest.Type == QuestTypes.Kills && playerLog.CurrentQuest.Side == ev.Killer.Role.Side)
            {
                playerLog.AddProgress(1);
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            if (Main.Players.TryGetValue(ev.Player.UserId, out PlayerLog log))
            {
                log.AddXP(Main.Instance.Config.EscapeXP[ev.Player.Role]);
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.LeadingTeam == ev.LeadingTeam)
                {
                    Main.Players[player.UserId].AddXP(Main.Instance.Config.TeamWinXP);
                }
            }
            JsonSerialization.Save();
        }

        public void InteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Main.Players.TryGetValue(ev.Player.UserId, out var playerLog) &&
                playerLog.CurrentQuest.Type == QuestTypes.Doors && playerLog.CurrentQuest.Side == ev.Player.Role.Side)
            {
                playerLog.AddProgress(1);
            }
        }

        public void UsedItem(UsedItemEventArgs ev)
        {
            if (Main.Players.TryGetValue(ev.Player.UserId, out var playerLog) &&
                playerLog.CurrentQuest.Type == QuestTypes.UseItem && playerLog.CurrentQuest.Side == ev.Player.Role.Side)
            {
                playerLog.AddProgress(1);
            }
        }
    }
}
