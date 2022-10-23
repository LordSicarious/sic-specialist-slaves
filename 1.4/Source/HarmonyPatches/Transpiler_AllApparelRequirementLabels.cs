//Transpiler for [RimWorld.Precept_Role.AllApparelRequirementLabels]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace SpecialistSlaves {
public static class Transpiler_AllApparelRequirementLabels {
    // Patches AllApparelRequirementLabels to remove the weird presumption of Male gender that breaks roles with Female gendered apparel requirements
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
        bool found = false;
        for (int i = 0; i < codes.Count; i++) {
            // Look for loading 1 (Gender.Male decoded from Enum to Int) immediately before Callvirt, Call, Callvirt
            if (codes[i].opcode == OpCodes.Ldc_I4_1 &&
                codes[i+1].opcode == OpCodes.Callvirt &&
                codes[i+2].opcode == OpCodes.Call &&
                codes[i+3].opcode == OpCodes.Callvirt) {
                    found = true;
                    // Replace 1 (Male) with 0 (None)
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
            } else { yield return codes[i]; }
        }
        if (!found) { Log.Error("Cannot find transpiler target."); }
    }
}
}