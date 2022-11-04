// SpecialistSlaves.RoleRequirement_HighMoodToAssign
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public class RoleRequirement_HighMoodToAssign : RoleRequirement {
	static float minimum = 0.8f;
	public override bool Met(Pawn pawn, Precept_Role role) {
        float mood = pawn.needs?.mood?.CurInstantLevel ?? 0f;
		if(mood < minimum) {
			// Mood requirement only required to assign role.
			Precept_Role curRole = pawn.ideo?.Ideo?.GetRole(pawn);
			if (curRole?.def == role?.def) {
				return true;
			} else { return false; }
		}
		return true;
	}
}
}