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

			//Postfix to [RimWorld.StatPart_Slave.ActiveFor]
            patchType = typeof(Postfix_ActiveFor);
            original = AccessTools.Method(typeof(StatPart_Slave), name: "ActiveFor");
            modified = nameof(Postfix_ActiveFor.ActiveFor_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Postfix to [RimWorld.SlaveRebellionUtility.CanParticipateInSlaveRebellion]
            patchType = typeof(Postfix_CanParticipateInSlaveRebellion);
            original = AccessTools.Method(typeof(SlaveRebellionUtility), name: "CanParticipateInSlaveRebellion");
            modified = nameof(Postfix_CanParticipateInSlaveRebellion.CanParticipateInSlaveRebellion_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Postfix to [RimWorld.ThoughtWorker_ExpectationsSlave.CurrentStateInternal]
            patchType = typeof(Postfix_CurrentStateInternal);
            original = AccessTools.Method(typeof(ThoughtWorker_ExpectationsSlave), name: "CurrentStateInternal");
            modified = nameof(Postfix_CurrentStateInternal.CurrentStateInternal_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Postfix to [RimWorld.ApparelRequirement.HasRequiredTag]
            patchType = typeof(Postfix_HasRequiredTag);
            original = AccessTools.Method(typeof(ApparelRequirement), name: "HasRequiredTag");
            modified = nameof(Postfix_HasRequiredTag.HasRequiredTag_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Postfix to [RimWorld.StatWorker_SuppressionFallRate.ShouldShowFor]
            patchType = typeof(Postfix_ShouldShowFor);
            original = AccessTools.Method(typeof(StatWorker_SuppressionFallRate), name: "ShouldShowFor");
            modified = nameof(Postfix_ShouldShowFor.ShouldShowFor_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Postfix to [RimWorld.Precept_Role.ValidatePawn]
            patchType = typeof(Postfix_ValidatePawn);
            original = AccessTools.Method(typeof(Precept_Role), name: "ValidatePawn");
            modified = nameof(Postfix_ValidatePawn.ValidatePawn_Postfix);
            harmony.Patch(original, postfix: new HarmonyMethod(patchType, modified));

			//Prefix to [RimWorld.Need_Suppression.DrawSuppressionBar]
            patchType = typeof(Prefix_DrawSuppressionBar);
            original = AccessTools.Method(typeof(Need_Suppression), name: "DrawSuppressionBar");
            modified = nameof(Prefix_DrawSuppressionBar.DrawSuppressionBar_Prefix);
            harmony.Patch(original, prefix: new HarmonyMethod(patchType, modified));

			//Prefix to [RimWorld.Need_Suppression.NeedInterval]
            patchType = typeof(Prefix_NeedInterval);
            original = AccessTools.Method(typeof(Need_Suppression), name: "NeedInterval");
            modified = nameof(Prefix_NeedInterval.NeedInterval_Prefix);
            harmony.Patch(original, prefix: new HarmonyMethod(patchType, modified));

			//Transpiler for [RimWorld.Precept_Role.AllApparelRequirementLabels]
            patchType = typeof(Transpiler_AllApparelRequirementLabels);
            original = AccessTools.Method(typeof(Precept_Role), name: "AllApparelRequirementLabels");
            modified = nameof(Transpiler_AllApparelRequirementLabels.Transpiler);
            harmony.Patch(original, transpiler: new HarmonyMethod(patchType, modified));

			//Transpiler for [RimWorld.ITab_Pawn_Visitor.FillTab]
            patchType = typeof(Transpiler_FillTab);
            original = AccessTools.Method(typeof(ITab_Pawn_Visitor), name: "FillTab");
            modified = nameof(Transpiler_FillTab.Transpiler);
            harmony.Patch(original, transpiler: new HarmonyMethod(patchType, modified));
        }
    }
}