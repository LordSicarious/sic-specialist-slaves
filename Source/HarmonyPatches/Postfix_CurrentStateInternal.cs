//Postfix to [RimWorld.ThoughtWorker_ExpectationsSlave.CurrentStateInternal]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public static class Postfix_CurrentStateInternal {
    // Modifies the stat part that reduces slaves' global work speed to exclude willing slaves
    public static void CurrentStateInternal_Postfix(Pawn p, ref ThoughtState __result) {
        if(p.IsWillingSlave()) {
            __result = ThoughtState.Inactive;
        }
    }
}
}