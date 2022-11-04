//Prefix to [RimWorld.Need_Suppression.DrawSuppressionBar]
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
public static class Prefix_DrawSuppressionBar {
    // Modifies the slave rebellion logic to exclude willing slaves
    public static bool DrawSuppressionBar_Prefix(Rect rect, Need_Suppression __instance) {
        Pawn pawn = __instance.pawn;
        if(pawn.IsWillingSlave()) {
            Widgets.FillableBar(rect, 1f, SpecialistSlavesUtility.WillingSlaveSuppressionFillTex);
            return false;
        }
        return true;
    }
}
}