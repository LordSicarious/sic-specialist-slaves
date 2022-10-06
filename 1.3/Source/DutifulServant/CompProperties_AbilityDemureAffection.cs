// SpecialistSlaves.CompProperties_AbilityDemureAffection
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public class CompProperties_AbilityDemureAffection : CompProperties_AbilityEffect {
	[MustTranslate]
	public string successMessage;

	public float baseCertaintyGain = 0.1f;

	public CompProperties_AbilityDemureAffection()
	{
		compClass = typeof(CompAbilityEffect_DemureAffection);
	}
}
}