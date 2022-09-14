using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
    [StaticConstructorOnStartup]
    public static class ApplyHarmonyPatches {
		// Reference to this class for patches
        static ApplyHarmonyPatches() {
			// Instantiate Harmony
			var harmony = new Harmony("sic.SpecialistSlaves.thisisanid");
			Type patchType;
			MethodInfo original;
			string modified;

			//Postfix to [RimWorld.Precept_Role.ValidatePawn]
            patchType = typeof(Harmony_ValidatePawn);
            original = AccessTools.Method(typeof(Precept_Role), name: "ValidatePawn");
            modified = nameof(Harmony_ValidatePawn.ValidatePawn_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));
        }
    }
}