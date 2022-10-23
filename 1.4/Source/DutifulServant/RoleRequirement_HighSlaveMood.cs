// SpecialistSlaves.RoleRequirement_BecomeWillingSlave
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public class RoleRequirement_HighSlaveMood : RoleRequirement {
	static float minimum = 0.8f;
	public override bool Met(Pawn p, Precept_Role role) {
        float mood = pawn.mood?.mood?.CurInstantLevel ?? 0f;
		if(mood < minimum) {
			// Mood requirement only required to assign role.
			Precept_Role role = pawn.ideo?.Ideo?.GetRole(pawn);
			if (!role?.def?.roleTags.Contains("DutifulServant")) {
				return false;
			}
		}
		return true;
	}
}
}