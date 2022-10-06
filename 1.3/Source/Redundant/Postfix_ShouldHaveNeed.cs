//Postfix to [RimWorld.Pawn_NeedsTracker.ShouldHaveNeed]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public static class Postfix_ShouldHaveNeed {
    // Willing slaves do not need suppression
    public static void ShouldHaveNeed_Postfix(NeedDef nd, ref bool __result, ref Pawn ___pawn) {
        // Only need to modify if pawn SHOULD have SUPPRESSION
        if (nd.defName == "Suppression" && __result) {
            if(pawn.IsWillingSlave()) {{
                Log.Message(___pawn.LabelShort + " should not have suppression");
                __result = false;
            }
        }
    }
}
}