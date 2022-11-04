//Postfix to [RimWorld.ApparelRequirement.HasRequiredTag]
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SpecialistSlaves {
public static class Postfix_HasRequiredTag {
    // Makes it so that requiring Male or Female tags can be used to check for apparel gender
    public static void HasRequiredTag_Postfix(ThingDef apparel, ref bool __result, ref List<string> ___requiredTags) {
        // No need to modify if already true or if there aren't any requiredTags
        if (__result || ___requiredTags == null || apparel.apparel == null) { return; }
        // If one of the required tags is "Male" or "Female", check the apparel's gender field as well
        if(___requiredTags.Contains("Male") && apparel.apparel.gender == Gender.Male) { __result = true; }
        else if(___requiredTags.Contains("Female") && apparel.apparel.gender == Gender.Female) { __result = true; }
    }
}
}