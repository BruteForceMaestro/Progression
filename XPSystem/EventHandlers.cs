using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using System.Linq;
using MEC;
using System.Collections.Generic;
using PlayerRoles;
using Exiled.Events.EventArgs.Server;

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

        public void OnAssist(GainingExperienceEventArgs ev)
        {
            if ((int)ev.GainType > 51 || (int)ev.GainType < 46) // gaintypes for termination of people
            {
                return;
            }
            if (ev.Player.DoNotTrack)
            {
                return;
            }
            Main.ActivePlayers[ev.Player.UserId].AddXP(Main.Instance.Config.AssistXP);
        }

        public void OnKill(DyingEventArgs ev)
        {
            if (ev.Player == null)
            {
                return;
            }

            Player killer = ev.DamageHandler.Type == DamageType.PocketDimension ? Player.Get(RoleTypeId.Scp106).FirstOrDefault() : ev.Attacker;
            if (killer == null || killer.DoNotTrack)
            {
                return;
            }

            if (Main.Instance.Config.KillXP.TryGetValue(killer.Role, out var killxpdict) && killxpdict.TryGetValue(ev.Player.Role, out int xp))
            {
                Main.ActivePlayers[ev.Attacker.UserId].AddXP(xp);
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
