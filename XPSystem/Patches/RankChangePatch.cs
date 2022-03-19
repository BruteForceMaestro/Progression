using Exiled.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace XPSystem.Patches
{
    [HarmonyPatch(typeof(ServerRoles), nameof(ServerRoles.SetText))]
    public class RankChangePatch
    {

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = 0;
            Label skipLabel = generator.DefineLabel();
            var inserted = new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(ServerRoles), nameof(ServerRoles._hub))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(RankChangePatch), nameof(RankChangePatch.EvaluateRank))) ,
                new CodeInstruction(OpCodes.Starg_S, 1)
            };

            newInstructions.InsertRange(index, inserted);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        static string EvaluateRank(Player player, string badgeText) // more internal, used for transpiler
        {
            if (!Main.Players.TryGetValue(player.UserId, out PlayerLog log))
            {
                return "";
            }

            if (badgeText == log.ChangedBadge) // the settext method gets called twice because questionable NW networking, prevent duplication
            {
                return badgeText;
            }

            string rank = Main.Instance.Config.BadgeStructure
                .Replace("%lvl%", log.LVL.ToString())
                .Replace("%badge%", log.GeneratedBadge)
                .Replace("%oldbadge%", badgeText);

            log.ChangedBadge = rank;
            return rank;
        }
    }
}
