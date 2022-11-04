// SpecialistSlaves.SpecialistSlavesUtility
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;

namespace SpecialistSlaves {
[StaticConstructorOnStartup]
public static class SpecialistSlavesUtility {

    public static Texture2D WillingSlaveSuppressionFillTex = SolidColorMaterials.NewSolidColorTexture(new Color32(50, 205, 80, byte.MaxValue));
    public static bool IsWillingSlave(this Pawn pawn) {
        Precept_Role role = pawn.ideo?.Ideo?.GetRole(pawn);
        if(role != null && (role.def?.roleEffects?.Any(effect => effect as RoleEffect_WillingSlave != null) ?? false)) {
            return true;
        }
        return false;
    }

    static SpecialistSlavesUtility() {
        Log.Message("Specialist Slaves Loaded");
        DefModExtension modExtension;
        foreach (PreceptDef p in DefDatabase<PreceptDef>.AllDefs.Where(p => p.GetModExtension<SpecialistSlaves_ModExtension>() != null)) {
            modExtension = p.modExtensions.FindLast(x => x is SpecialistSlaves_ModExtension);
            p.modExtensions.RemoveAll(x => x is SpecialistSlaves_ModExtension);
            p.modExtensions.Add(modExtension);
        }
    }
}
}