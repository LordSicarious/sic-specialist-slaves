//Prefix to [RimWorld.Need_Suppression.NeedInterval]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using Verse;

using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public static class Prefix_NeedInterval {
    // Modifies the slave rebellion logic to exclude willing slaves
    public static bool NeedInterval_Prefix(Need_Suppression __instance) {
        Pawn pawn = __instance.pawn;
        if(pawn.IsWillingSlave()) {
            if (pawn.guest.slaveInteractionMode == SlaveInteractionModeDefOf.Suppress) {
                pawn.guest.slaveInteractionMode = SlaveInteractionModeDefOf.NoInteraction;
            }
            __instance.CurLevel = __instance.MaxLevel;
            return false;
        }
        return true;
    }
}
}