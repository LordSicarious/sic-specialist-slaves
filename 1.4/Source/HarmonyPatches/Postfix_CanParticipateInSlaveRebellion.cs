//Postfix to [RimWorld.SlaveRebellionUtility.CanParticipateInSlaveRebellion]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public static class Postfix_CanParticipateInSlaveRebellion {
    // Modifies the slave rebellion logic to exclude willing slaves
    public static void CanParticipateInSlaveRebellion_Postfix(Pawn pawn , ref bool __result) {
        // No need to modify if already false
        if (!__result) { return; }
        if(pawn.IsWillingSlave()) {
            __result = false;
        }
    }
}
}