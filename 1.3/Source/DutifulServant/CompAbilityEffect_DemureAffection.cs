// SpecialistSlaves.CompAbilityEffect_DemureAffection
using System;
using RimWorld;
using Verse;
using Verse.Sound;

using static Verse.GenderUtility;

namespace SpecialistSlaves {
public class CompAbilityEffect_DemureAffection : CompAbilityEffect {
	public new CompProperties_AbilityDemureAffection Props => (CompProperties_AbilityDemureAffection)props;

	public override bool HideTargetPawnTooltip => true;

	// Pawn must be a free human of the supreme gender and correct ideology, and not sleeping or in a mental state
	public override bool Valid(LocalTargetInfo target, bool throwMessages = false) {
		Pawn pawn = target.Pawn;
		if (pawn == null) { return false; }
		if (!AbilityUtility.ValidateMustBeHuman(pawn, throwMessages)) { return false; }
		if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages)) { return false; }
		if (!AbilityUtility.ValidateIsAwake(pawn, throwMessages)) { return false; }
		if (!AbilityUtility.ValidateSameIdeo(parent.pawn, pawn, throwMessages)) { return false; }
		if (!ValidateSupremeGender(parent.pawn.ideo.Ideo, pawn, throwMessages)) { return false; }
		if (!ValidateNotSlave(pawn, throwMessages)) { return false; }
		return true;
	}

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest) {
		if (ModLister.CheckIdeology("Ideoligion certainty")) {
			Pawn initiator = parent.pawn;
			Pawn recipient = target.Pawn;
			float esf = EffectStrengthFactor(initiator, recipient);
			// Give thought
			Thought_AssuredSuperiority thought = ThoughtMaker.MakeThought(SpecialistSlavesDefOf.AssuredSuperiority) as Thought_AssuredSuperiority;
			Log.Message("Effect Strength Factor = " + esf.ToString()); 
			thought.moodOffset = (int)(thought.moodOffset*esf);
			thought.opinionOffset = (int)(thought.opinionOffset*esf);
			recipient.needs.mood.thoughts.memories.TryGainMemory(thought, initiator);
			//Certainty gain
			float certaintyGain = esf * Props.baseCertaintyGain;
			float certainty = recipient.ideo.Certainty;
			recipient.ideo.Reassure(certaintyGain);
			Messages.Message(Props.successMessage.Formatted(initiator.Named("INITIATOR"), recipient.Named("RECIPIENT"), certainty.ToStringPercent().Named("BEFORECERTAINTY"), recipient.ideo.Certainty.ToStringPercent().Named("AFTERCERTAINTY"), initiator.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[2] { initiator, recipient }), MessageTypeDefOf.PositiveEvent);
			PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(SpecialistSlavesDefOf.DemureAffection, initiator, recipient, null);
			Find.PlayLog.Add(entry);
			if (Props.sound != null) {
				Props.sound.PlayOneShot(new TargetInfo(target.Cell, initiator.Map));
			}
		}
	}

	private static bool ValidateSupremeGender(Ideo ideo, Pawn recipient, bool showMessages) {
		if (recipient.gender != ideo.SupremeGender) {
			if (showMessages) {
				Messages.Message("AbilityCantApplyNotSupremeGender".Translate(recipient), recipient, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}

	private static bool ValidateNotSlave(Pawn recipient, bool showMessages) {
		if (recipient.IsSlave) {
			if (showMessages) {
				Messages.Message("AbilityCantApplyIsSlave".Translate(recipient), recipient, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}

	private float EffectStrengthFactor(Pawn initiator, Pawn recipient) {
		float fMood = (float)Math.Pow(initiator.needs.mood.CurInstantLevel + 0.25f, 3); // Ranges from ~1% to 200%, sharply falls off if mood below 0.75
		float fSocial = initiator.GetStatValue(StatDefOf.SocialImpact); // Minimum 0 if deaf, mute and 0 social, up to 1.57 with  20 social and crown
		float fRelation; // Ranges from 1 to 3
		if (PawnRelationUtility.GetMostImportantRelation(initiator, recipient) != null) {
			fRelation = PawnRelationUtility.GetMostImportantRelation(initiator, recipient).importance/70f;
		} else { fRelation = 1f; }
		float fBeauty = 1f; // Ranges from 0 (at -4 fBeauty) to 2 (at +4 fBeauty)
		if (!PawnUtility.IsBiologicallyOrArtificallyBlind(recipient)) {
			fBeauty += initiator.GetStatValue(StatDefOf.PawnBeauty)/4f;
			if (RelationsUtility.IsDisfigured(initiator, recipient)) {
				fBeauty /= 2f;
			}
			if (recipient.story.traits.HasTrait(TraitDef.Named("Kind")) && fBeauty <= 1f) {
				fBeauty = 1f;
			}
		}
		float fConviction = initiator.ideo.Certainty; // Ranges from 0 to 1
		float fTraits = 1f;
		// Beneficial Traits
		if (initiator.story.traits.HasTrait(TraitDef.Named("Kind"))) { fTraits *= 2f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("TorturedArtist"))) { fTraits *= 1.1f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("GreatMemory"))) { fTraits *= 1.5f; }

		// Detrimental Traits
		if (initiator.story.traits.HasTrait(TraitDef.Named("Greedy"))) { fTraits *= 0.75f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("Jealous"))) { fTraits *= 0.75f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("Psychopath"))) { fTraits *= 0.9f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("TooSmart"))) { fTraits *= 0.9f; }
		if (initiator.story.traits.HasTrait(TraitDef.Named("Abrasive"))) { fTraits *= 0.2f; }

		// Traits limited by recipient's kindness
		if (!recipient.story.traits.HasTrait(TraitDef.Named("Kind"))) {
			if (initiator.story.traits.HasTrait(TraitDef.Named("AnnoyingVoice"))) { fTraits *= 0.5f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("CreepyBreathing"))) { fTraits *= 0.5f; }
		}

		// VTE Traits
		if (ModLister.HasActiveModWithName("VanillaExpanded.VanillaTraitsExpanded")) {
			if (initiator.story.traits.HasTrait(TraitDef.Named("Submissive"))) { fTraits *= 2f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("Stoner"))) { fTraits *= 1.2f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("Fun-Loving"))) { fTraits *= 1.5f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("Tycoon"))) { fTraits *= 1.25f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("Anxious"))) { fTraits *= 0.9f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("World Weary"))) { fTraits *= 0.5f; }
			if (initiator.story.traits.HasTrait(TraitDef.Named("Desensitized"))) { fTraits *= 0.8f; }
		}

		return fMood * fSocial * fRelation * fBeauty * fConviction * fTraits;
	}
}
}