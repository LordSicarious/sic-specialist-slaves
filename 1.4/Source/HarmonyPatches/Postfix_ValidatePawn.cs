//Postfix to [RimWorld.Precept_Role.ValidatePawn]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public static class Postfix_ValidatePawn {
    // Modifies the pawn validation logic to allow slaves
    public static void ValidatePawn_Postfix(Pawn p, ref bool __result, ref Precept_Role __instance) {
        // No need to modify if already valid or if invalid for other reasons
        if (__result || p.Faction == null || p.Destroyed || p.Dead) { return; }
        // Modified to use "IsFreeColonist" instead of "IsFreeNonSlaveColonist"
        if (p.Faction.IsPlayer && p.IsFreeColonist && __instance.RequirementsMet(p)) {
            __result = true;
        }
    }
}
}