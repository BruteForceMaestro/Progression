using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Linq;
using MEC;
using System.Collections.Generic;

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
            Timing.RunCoroutine(JoinedCoroutine(ev.Player));
        }

        private IEnumerator<float> JoinedCoroutine(Player player)
        {
            yield return Timing.WaitForOneFrame;
            if (!Main.Players.TryGetValue(player.UserId, out PlayerLogSer log))
            {
                log = new PlayerLogSer() { LVL = 0, XP = 0 };
                Main.Players[player.UserId] = log;
            }
            PlayerLog activeLog = new PlayerLog(log, player);
            Main.ActivePlayers[player.UserId] = activeLog;
        }

        public void OnLeaving(LeftEventArgs ev)
        {
            if (ev.Player.DoNotTrack)
            {
                return;
            }
            Main.ActivePlayers.Remove(ev.Player.UserId);
        }

        public void OnKill(DyingEventArgs ev)
        {
            if (ev.Target == null)
            {
                return;
            }

            Player killer = ev.Handler.Type == DamageType.PocketDimension ? Player.Get(RoleType.Scp106).FirstOrDefault() : ev.Killer;
            if (killer == null || killer.DoNotTrack)
            {
                return;
            }

            if (Main.Instance.Config.KillXP.TryGetValue(killer.Role, out var killxpdict) && killxpdict.TryGetValue(ev.Target.Role, out int xp))
            {
                Main.ActivePlayers[ev.Killer.UserId].AddXP(xp);
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            if (Main.ActivePlayers.TryGetValue(ev.Player.UserId, out PlayerLog log))
            {
                log.AddXP(Main.Instance.Config.EscapeXP[ev.Player.Role]);
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.LeadingTeam == ev.LeadingTeam && !player.DoNotTrack)
                {
                    Main.ActivePlayers[player.UserId].AddXP(Main.Instance.Config.TeamWinXP);
                }
            }
            JsonSerialization.Save();
            Main.ActivePlayers.Clear();
        }
    }
}
