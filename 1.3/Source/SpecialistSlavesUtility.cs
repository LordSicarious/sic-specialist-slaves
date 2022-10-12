// SpecialistSlaves.SpecialistSlavesUtility
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
}
}