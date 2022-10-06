// SpecialistSlaves.SpecialistSlavesDefOf
using RimWorld;
using Verse;

namespace SpecialistSlaves {
[DefOf]
public static class SpecialistSlavesDefOf {
    public static ThoughtDef AssuredSuperiority;
    public static InteractionDef DemureAffection;
    static SpecialistSlavesDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(SpecialistSlavesDefOf));
    }
}
}