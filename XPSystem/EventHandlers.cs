using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
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
                return;
            }
            if (!Main.Players.TryGetValue(ev.Player.UserId, out var log))
            {
                log = new PlayerLog()
                {
                    LVL = 0,
                    XP = 0
                };
                Main.Players[ev.Player.UserId] = log;
            }
            Timing.CallDelayed(0.15f, () => API.ApplyRank(ev.Player, log));
        }

        public void OnKill(DyingEventArgs ev)
        {
            if (ev.Target == null)
            {
                return;
            }
            Player killer = ev.Handler.Type == DamageType.PocketDimension ? Player.Get(RoleType.Scp106).FirstOrDefault() : ev.Killer;
            if (killer == null)
            {
                return;
            }
            if (LookUp(killer.Role, ev.Target.Role, out int xp))
            {
                API.AddXP(killer, xp);
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            API.AddXP(ev.Player, Main.Instance.Config.EscapeXP[ev.Player.Role]);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.LeadingTeam == ev.LeadingTeam)
                {
                    API.AddXP(player, Main.Instance.Config.TeamWinXP);
                }
            }
            JsonSerialization.Save();
        }

        public bool LookUp(RoleType killer, Exiled.API.Features.Roles.Role target, out int xp)
        {
            int xp1 = 0;
            bool retValue = Main.Instance.Config.KillXP.TryGetValue(killer, out var xp106) && xp106.TryGetValue(target, out xp1);
            xp = xp1;
            return retValue;
        }
    }
}
