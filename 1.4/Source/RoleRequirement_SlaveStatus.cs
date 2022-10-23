// RimWorld.RoleRequirement_SlaveStatus
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public class RoleRequirement_SlaveStatus : RoleRequirement {
	[NoTranslate]
	private string labelCached;
	public override string GetLabel(Precept_Role role) {
		if (labelCached == null) {
			SlaveStatusRequirement slaveOkay = role.def.GetModExtension<SpecialistSlaves_ModExtension>()?.slavePermitted ?? SlaveStatusRequirement.Forbidden;
			switch (slaveOkay) {
				case SlaveStatusRequirement.Forbidden:
					labelCached = "RoleRequirementFree".Translate();
					break;
				case SlaveStatusRequirement.Allowed:
					labelCached = "RoleRequirementFreeOrSlave".Translate();
					break;
				case SlaveStatusRequirement.Required:
					labelCached = "RoleRequirementSlave".Translate();
					break;
				default:
					labelCached = "Error cannot fetch slave status requirement";
					break;
			}
		}
		return labelCached;
	}

	public override bool Met(Pawn p, Precept_Role role) {
		// Get whether or not the role can be held by a slave, assume Forbidden if unspecified.
		SlaveStatusRequirement slaveOkay = role.def.GetModExtension<SpecialistSlaves_ModExtension>()?.slavePermitted ?? SlaveStatusRequirement.Forbidden;
		if (slaveOkay == SlaveStatusRequirement.Required && !p.IsSlave) {return false;}
		if (slaveOkay == SlaveStatusRequirement.Forbidden && p.IsSlave) {return false;}
		return true;
	}
}
}