//Transpiler for [RimWorld.ITab_Pawn_Visitor.FillTab]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
// Patches the Slave Interaction tab to adjust the description for willing slaves
public static class Transpiler_FillTab {
    // Requires knowledge of some method and field info
    private static MethodInfo m_get_SelPawn = AccessTools.Method(typeof(ITab), "get_SelPawn");
    private static MethodInfo m_InitiateSlaveRebellionMtbDays = AccessTools.Method(typeof(SlaveRebellionUtility), "InitiateSlaveRebellionMtbDays");
    private static MethodInfo m_IsWillingSlave = AccessTools.Method(typeof(SpecialistSlavesUtility), "IsWillingSlave");
	private static FieldInfo f_SlaveSuppressionFallRate = AccessTools.Field(typeof(StatDefOf), "SlaveSuppressionFallRate");

    // Circumstancially skip part of the Slave interface if the pawn is a willing slave
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
        int insertAt = -1;
        Label jumpLabel = il.DefineLabel();
        for (int i = 0; i < codes.Count-2; i++) { //-2 offset because we're looking 2 ahead
            if (codes[i].opcode == OpCodes.Ldarg_0 &&
                codes[i+1].opcode == OpCodes.Call &&
                codes[i+1].operand as MethodInfo == m_get_SelPawn) {
                // If we're grabbing SuppressionFallRate, we're at the bit we want to skip
                if (codes[i+2].opcode == OpCodes.Ldsfld &&
                    codes[i+2].operand as FieldInfo == f_SlaveSuppressionFallRate) {
                    Log.Message("Inserting at " + codes[i+2].ToString());
                    insertAt = i;
                // If we're grabbing InitiateSlaveRebellionMtbDays, we're at the bit we want to skip to
                } else if (codes[i+2].opcode == OpCodes.Call &&
                           codes[i+2].operand as MethodInfo == m_InitiateSlaveRebellionMtbDays) {
                    Log.Message("Adding jump label to " + codes[i+2].ToString());
                    codes[i].labels.Add(jumpLabel);
                    if (i!=-1) { break; }
                }
            }
        }
        if (insertAt==-1) { Log.Error("Cannot find transpiler target."); }
        else {
            // Additional codes:
            List<CodeInstruction> newCodes = new List<CodeInstruction>();
            newCodes.Add(codes[insertAt]);
            newCodes.Add(codes[insertAt+1]);
            newCodes.Add(new CodeInstruction(OpCodes.Call, m_IsWillingSlave));
            newCodes.Add(new CodeInstruction(OpCodes.Brtrue_S, jumpLabel));
            codes.InsertRange(insertAt, newCodes);
        }
        return codes;
    }
}
}