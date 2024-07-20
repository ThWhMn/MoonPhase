using Verse;

namespace MoonPhase.HediffGiverExt;
internal static class Utils
{

    public static bool PawnHasHediff(Pawn pawn, HediffDef def)
    {
        HediffSet hediffSet = pawn.health.hediffSet;
        foreach (var hediff in hediffSet.hediffs)
        {
            if (hediff.def.defName == def.defName)
            {
                return true;
            }
        }
        return false;
    }
}
