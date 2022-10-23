//Postfix to [RimWorld.StatWorker_SuppressionFallRate.ShouldShowFor]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public static class Postfix_ShouldShowFor {
    // Modifies the stat part that reduces slaves' global work speed to exclude willing slaves
    public static void ShouldShowFor_Postfix(StatRequest req, ref bool __result) {
        // No need to modify if already false
        if (!__result) { return; }
        // If the pawn is a willing slave, don't need to show suppression fall rate
        Pawn pawn = req.Thing as Pawn;
        if(pawn.IsWillingSlave()) {
            __result = false;
        }
    }
}
}