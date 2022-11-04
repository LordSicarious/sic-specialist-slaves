// SpecialistSlaves.Thought_AssuredSuperiority
using RimWorld;
using Verse;

using static Verse.GenderUtility;
using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public class ThoughtWorker_ExpectationsDutifulServant : ThoughtWorker {
	public override string PostProcessLabel(Pawn p, string label) {
		if (p.gender == p.ideo.Ideo.SupremeGender.Opposite()) {
			return label.Formatted(ServantAdjective(p.gender));
		} else {
			return label.Formatted("Servant".Translate());
		}
	}

	public override string PostProcessDescription(Pawn p, string description) {
		if (p.gender == p.ideo.Ideo.SupremeGender.Opposite()) {
			return description.Formatted(ServantAdjective(p.gender));
		} else {
			return description.Formatted("Servant".Translate());
		}
	}

	public override ThoughtState CurrentStateInternal(Pawn p) {
		if (!p.IsWillingSlave()) {
			return ThoughtState.Inactive;
		}
		ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
		if (expectationDef == null) {
			return ThoughtState.Inactive;
		}
		return ThoughtState.ActiveAtStage(GetThoughtStageForExpectation(expectationDef));
	}

	private static int GetThoughtStageForExpectation(ExpectationDef expectation) {
		if (expectation == ExpectationDefOf.ExtremelyLow) { return 0; }
		if (expectation == ExpectationDefOf.VeryLow) { return 1; }
		if (expectation == ExpectationDefOf.Low) { return 2; }
		if (expectation == ExpectationDefOf.Moderate) { return 3; }
		return 4;
	}

	private static string ServantNoun(Gender gender) {
		switch(gender) {
			case Gender.Male:	return "Man".Translate();
			case Gender.Female:	return "Woman".Translate();
			default:			return "Servant".Translate();
		}
	}
	private static string ServantAdjective(Gender gender) {
		switch(gender) {
			case Gender.Male:	return "Masculine".Translate();
			case Gender.Female:	return "Feminine".Translate();
			default:			return "Servant".Translate();
		}
	}
}
}