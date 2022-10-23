// SpecialistSlaves.Thought_AssuredSuperiority
using RimWorld;
using Verse;

using static Verse.GenderUtility;
using static SpecialistSlaves.SpecialistSlavesUtility;

namespace SpecialistSlaves {
public class ThoughtWorker_ExpectationsDutifulServant : ThoughtWorker {

	protected override ThoughtState CurrentStateInternal(Pawn p) {
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
}
}