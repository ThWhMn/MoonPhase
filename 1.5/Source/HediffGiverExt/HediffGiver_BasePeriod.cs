using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MoonPhase.HediffGiverExt;
public class HediffGiver_BasePeriod : HediffGiver
{
    protected int startTimeHour;
    protected int endTimeHour { get => startTimeHour + periodHour; }
    protected int periodHour;
    protected bool sendMsg = false;

    protected Dictionary<Pawn, Dictionary<HediffDef, int>> pawn2hediff2endtick = new();

    virtual protected bool DuringPeriod(Pawn pawn)
    {
        Log.Error("DuringPeriod not override");
        return false;
    }

    protected int TicksLeft(Pawn pawn)
    {
        var now = GenLocalDate.HourInteger(pawn);
        return (endTimeHour - now) * GenDate.TicksPerHour;
    }

    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
        if (DuringPeriod(pawn))
        {
            if (!Utils.PawnHasHediff(pawn, hediff))
            {
                // Give pawn hediff
                if (hediff.comps.Find(x => x.GetType() == typeof(HediffCompProperties_Disappears)) is not HediffCompProperties_Disappears thatComp)
                {
                    Log.Error("HediffCompProperties_Disappears not found");
                    return;
                }
                var ticksLeft = TicksLeft(pawn);
                thatComp.disappearsAfterTicks = new IntRange
                {
                    min = ticksLeft,
                    max = ticksLeft + GenTicks.TickRareInterval
                };
                pawn2hediff2endtick[pawn] = new()
                {
                    [hediff] = GenTicks.TicksAbs + ticksLeft
                };
                var ret = TryApply(pawn);
                if (ret && sendMsg)
                    SendLetter(pawn, cause);
            }
        }
    }
}
