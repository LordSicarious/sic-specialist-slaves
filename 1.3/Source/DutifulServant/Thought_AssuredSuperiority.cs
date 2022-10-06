// SpecialistSlaves.Thought_AssuredSuperiority
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public class Thought_AssuredSuperiority : Thought_MemorySocial {
	public override string LabelCap => CurStage.label.Formatted(GenderTerm(base.otherPawn.gender));

	private static string GenderTerm(Gender gender) {
		switch(gender) {
			case Gender.Male :
				return "Men".Translate();
			case Gender.Female :
				return "Women".Translate();
			default :
				return "People".Translate();
		}
	}

	public override bool TryMergeWithExistingMemory(out bool showBubble) {
		showBubble = false;
		return false;
	}

	public override bool GroupsWith(Thought other) {
		Thought_AssuredSuperiority thought_AssuredSuperiority = other as Thought_AssuredSuperiority;
		if (thought_AssuredSuperiority == null) {
			return false;
		}
		if (base.GroupsWith(other)) {
			return thought_AssuredSuperiority.otherPawn == base.otherPawn;
		}
		return false;
	}
}
}