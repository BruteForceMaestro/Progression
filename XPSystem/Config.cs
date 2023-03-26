using Exiled.API.Features;
using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace XPSystem
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Hint shown to the players if they have DNT enabled.")]
        public string DNTHint { get; set; } = "We can't track your stats while you have DNT enabled in your game options!";
        [Description("Badge for players with DNT enabled.")]
        public Badge DNTBadge { get; set; } = new Badge
        {
            Name = "(DNT) anonymous man????",
            Color = "nickel"
        };

        [Description("(You may add your own entries) Role1: Role2: XP player with Role1 gets for killing a person with Role2 ")]
        public Dictionary<RoleTypeId, Dictionary<RoleTypeId, int>> KillXP { get; set; } = new Dictionary<RoleTypeId, Dictionary<RoleTypeId, int>>()
        {
            [RoleTypeId.ClassD] = new Dictionary<RoleTypeId, int>()
            {
                [RoleTypeId.Scientist] = 200,
                [RoleTypeId.FacilityGuard] = 150,
                [RoleTypeId.NtfPrivate] = 200,
                [RoleTypeId.NtfSergeant] = 250,
                [RoleTypeId.NtfCaptain] = 300,
                [RoleTypeId.Scp049] = 500,
                [RoleTypeId.Scp0492] = 100,
                [RoleTypeId.Scp106] = 500,
                [RoleTypeId.Scp173] = 500,
                [RoleTypeId.Scp096] = 500,
                [RoleTypeId.Scp939] = 500
            },
            [RoleTypeId.Scientist] = new Dictionary<RoleTypeId, int>()
            {
                [RoleTypeId.ClassD] = 50,
                [RoleTypeId.ChaosConscript] = 200,
                [RoleTypeId.ChaosRifleman] = 200,
                [RoleTypeId.ChaosRepressor] = 250,
                [RoleTypeId.ChaosMarauder] = 300,
                [RoleTypeId.Scp049] = 500,
                [RoleTypeId.Scp0492] = 100,
                [RoleTypeId.Scp106] = 500,
                [RoleTypeId.Scp173] = 500,
                [RoleTypeId.Scp096] = 500,
                [RoleTypeId.Scp939] = 500
            }
        };
        [Description("XP gained for 079 on assist")]
        public int AssistXP { get; set; } = 300;

        [Description("How many XP should a player get if their team wins.")]
        public int TeamWinXP { get; set; } = 250;

        [Description("How many XP is required to advance a level.")]
        public int XPPerLevel { get; set; } = 1000;

        [Description("Show a mini-hint if a player gets XP")]
        public bool ShowAddedXP { get; set; } = true;

        [Description("Show a hint to the player if he advances a level.")]
        public bool ShowAddedLVL { get; set; } = true;

        [Description("What hint to show if player advances a level. (if ShowAddedLVL = false, this is irrelevant)")]
        public string AddedLVLHint { get; set; } = "NEW LEVEL: <color=red>%level%</color>";

        [Description("(You may add your own entries) How many XP a player gets for escaping")]
        public Dictionary<RoleTypeId, int> EscapeXP { get; set; } = new Dictionary<RoleTypeId, int>()
        {
            [RoleTypeId.ClassD] = 500,
            [RoleTypeId.Scientist] = 300
        };

        [Description("(You may add your own entries) Level threshold and a badge. if you get a TAG FAIL in your console, either change your color, or remove special characters like brackets.")]
        public Dictionary<int, Badge> LevelsBadge { get; set; } = new Dictionary<int, Badge>()
        {
            [0] = new Badge
            {
                Name = "Visitor",
                Color = "cyan"
            },
            [1] = new Badge
            {
                Name = "Junior",
                Color = "orange"
            },
            [5] = new Badge
            {
                Name = "Senior",
                Color = "yellow"
            },
            [10] = new Badge
            {
                Name = "Veteran",
                Color = "red"
            },
            [50] = new Badge
            {
                Name = "Nerd",
                Color = "lime"
            }
        };

        [Description("The structure of the badge displayed in-game. Variables: %lvl% - the level. %badge% earned badge in specified in LevelsBadge. %oldbadge% - base-game badge, like ones specified in config-remoteadmin, or a global badge. can be null.")]
        public string BadgeStructure { get; set; } = "(LVL %lvl% | %badge%) %oldbadge%";
        [Description("Path files get saved to. Requires change on linux.")]
        public string SavePath { get; set; } = Path.Combine(Paths.Configs, @"Players.json");
        [Description("Override colors for people who already have a rank")]
        public bool OverrideRAColor { get; set; } = false;
        [Description("Do not enable this unless the creator told you to, if you don't wanna be spammed with unhelpful messages of course")]
        public bool Debug { get; set; } = false;
    }
}
