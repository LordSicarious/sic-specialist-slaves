//Postfix to [RimWorld.ITab_Pawn_Visitor.FillTab]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public static class Postfix_FillTab {
    // Require access to the slave price listing method
    private static MethodInfo SlavePriceListingMethod = AccessTools.Method(typeof(ITab_Pawn_Visitor), "DoSlavePriceListing");
    
    // Generates a more limited slave interaction tab for slaves without a suppression need
    public static void FillTab_Postfix(ref ITab_Pawn_Visitor __instance, ref Vector2 ___size, ref List<SlaveInteractionModeDef> ___tmpSlaveInteractionModes) {
        // Re-Get SelPawn from Find because it's less overhead than trying to access the protected field from ITab
        Pawn SelPawn = Find.Selector.SingleSelectedThing as Pawn;
        if (SelPawn == null) { return; }
        // Exclude pawns that are not slaves of the colony or have a suppression need, because they're already accounted for
        if(!SelPawn.IsSlaveOfColony || SelPawn.needs.TryGetNeed<Need_Suppression>() != null ) { return; }
        // Set interaction mode to NoInteraction if it's somehow set to Suppress
        if (SelPawn.guest.slaveInteractionMode == SlaveInteractionModeDefOf.Suppress) {
            SelPawn.guest.slaveInteractionMode = SlaveInteractionModeDefOf.NoInteraction;
        }

        // Set up Listing Standard
        Rect rect = new Rect(0f, 0f, ___size.x, ___size.y).ContractedBy(10f);
        Listing_Standard listing_Standard = new Listing_Standard();
		listing_Standard.maxOneColumn = true;
		listing_Standard.Begin(rect);

        // Selling or releasing slaves
        SlavePriceListingMethod.Invoke(__instance, new object[] {listing_Standard, SelPawn});
        Faction faction = SelPawn.SlaveFaction ?? SelPawn.Faction;
        TaggedString GoodwillOnRelease;
        if (faction == null || faction.IsPlayer || !faction.CanChangeGoodwillFor(Faction.OfPlayer, 1)) {
            GoodwillOnRelease = "None".Translate();
        } else {
            bool isHealthy;
            bool isInMentalState;
            int num1 = faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, faction.GetGoodwillGainForPrisonerRelease(SelPawn, out isHealthy, out isInMentalState));
            GoodwillOnRelease = ((isHealthy && !isInMentalState) ? (faction.NameColored + " " + num1.ToStringWithSign()) : ((!isHealthy) ? ("None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")") : ((!isInMentalState) ? "None".Translate() : ("None".Translate() + " (" + SelPawn.MentalState.InspectLine + ")"))));
        }
        Rect rect1 = listing_Standard.Label("SlaveReleasePotentialRelationGains".Translate() + ": " + GoodwillOnRelease);
        TooltipHandler.TipRegionByKey(rect1, "SlaveReleaseRelationGainsDesc");
        Widgets.DrawHighlightIfMouseover(rect1);
        ___tmpSlaveInteractionModes.Clear();
        ___tmpSlaveInteractionModes.AddRange(DefDatabase<SlaveInteractionModeDef>.AllDefs.OrderBy((SlaveInteractionModeDef pim) => pim.listOrder));
        ___tmpSlaveInteractionModes.Remove(SlaveInteractionModeDefOf.Suppress);
        int num2 = 32 * ___tmpSlaveInteractionModes.Count;
        Rect rect2 = listing_Standard.GetRect(num2).Rounded();
        Widgets.DrawMenuSection(rect2);
        Rect rect3 = rect2.ContractedBy(10f);
        Widgets.BeginGroup(rect3);
        SlaveInteractionModeDef currentSlaveIteractionMode = SelPawn.guest.slaveInteractionMode;
        Rect rect4 = new Rect(0f, 0f, rect3.width, 30f);
        foreach (SlaveInteractionModeDef tmpSlaveInteractionMode in ___tmpSlaveInteractionModes) {
            if (Widgets.RadioButtonLabeled(rect4, tmpSlaveInteractionMode.LabelCap, SelPawn.guest.slaveInteractionMode == tmpSlaveInteractionMode))
            {
                if (tmpSlaveInteractionMode == SlaveInteractionModeDefOf.Imprison && RestUtility.FindBedFor(SelPawn, SelPawn, checkSocialProperness: false, ignoreOtherReservations: false, GuestStatus.Prisoner) == null) {
                    Messages.Message("CannotImprison".Translate() + ": " + "NoPrisonerBed".Translate(), SelPawn, MessageTypeDefOf.RejectInput, historical: false);
                    continue;
                }
                SelPawn.guest.slaveInteractionMode = tmpSlaveInteractionMode;
                if (tmpSlaveInteractionMode == SlaveInteractionModeDefOf.Execute && SelPawn.SlaveFaction != null && !SelPawn.SlaveFaction.HostileTo(Faction.OfPlayer)) {
                    Dialog_MessageBox window = new Dialog_MessageBox("ExectueNeutralFactionSlave".Translate(SelPawn.Named("PAWN"), SelPawn.SlaveFaction.Named("FACTION")), "Confirm".Translate(), delegate {}, "Cancel".Translate(), delegate { SelPawn.guest.slaveInteractionMode = currentSlaveIteractionMode; });
                    Find.WindowStack.Add(window);
                }
            }
            if (!tmpSlaveInteractionMode.description.NullOrEmpty() && Mouse.IsOver(rect4)) {
                Widgets.DrawHighlight(rect4);
                string text = tmpSlaveInteractionMode.description;
                if (tmpSlaveInteractionMode == SlaveInteractionModeDefOf.Emancipate) {
                    text = ((SelPawn.SlaveFaction == Faction.OfPlayer) ? ((string)(text + (" " + "EmancipateCololonistTooltip".Translate()))) : ((SelPawn.SlaveFaction == null) ? ((string)(text + (" " + "EmancipateNonCololonistWithoutFactionTooltip".Translate()))) : ((string)(text + (" " + "EmancipateNonCololonistWithFactionTooltip".Translate(SelPawn.SlaveFaction.NameColored))))));
                }
                TooltipHandler.TipRegion(rect4, text);
            }
            rect4.y += 28f;
        }
        Widgets.EndGroup();
        ___tmpSlaveInteractionModes.Clear();
    }
}
}