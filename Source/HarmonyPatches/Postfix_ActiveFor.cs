//Postfix to [RimWorld.StatPart_Slave.ActiveFor]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public static class Postfix_ActiveFor {
    // Modifies the stat part that reduces slaves' global work speed to exclude willing slaves
    public static void ActiveFor_Postfix(Thing t, ref bool __result) {
        // No need to modify if already false
        if (!__result) { return; }
        // If the pawn is a willing slave, don't subject them to the slave penalty
        Pawn pawn = t as Pawn;
        if(pawn.IsWillingSlave()) {
            __result = false;
        }
    }
}
}