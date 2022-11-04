// SpecialistSlaves.SpecialistSlavesDefOf
using RimWorld;
using Verse;

namespace SpecialistSlaves {
[DefOf]
public static class SpecialistSlavesDefOf {
    public static PreceptDef IdeoRole_DutifulServant;
    static SpecialistSlavesDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(SpecialistSlavesDefOf));
    }
}
}