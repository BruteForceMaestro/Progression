namespace XPSystem
{
    public class PlayerLog
    {
        public int LVL { get; set; }
        public int XP { get; set; }
        public string GeneratedBadge { get; set; } // badge from the plugin config
        public string ChangedBadge { get; set; } // for preventing duplication, see transpiler
    }
}
