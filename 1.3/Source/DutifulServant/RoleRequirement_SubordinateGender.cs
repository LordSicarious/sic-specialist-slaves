// SpecialistSlaves.RoleRequirement_SubordinateGender
using RimWorld;
using Verse;

using static Verse.GenderUtility;

namespace SpecialistSlaves {
public class RoleRequirement_SubordinateGender : RoleRequirement {
	public override string GetLabel(Precept_Role role) {
		return labelKey.Translate(role.ideo.SupremeGender.Opposite().GetLabel());
	}

	public override bool Met(Pawn pawn, Precept_Role role) {
		return pawn.gender == role.ideo.SupremeGender.Opposite();
	}
}
}