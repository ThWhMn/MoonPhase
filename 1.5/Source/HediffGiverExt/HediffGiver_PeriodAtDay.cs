using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MoonPhase.HediffGiverExt;

public class HediffGiver_PeriodAtDay : HediffGiver
{
    private List<int> days;
    private int startTimeHour;
    private int endTimeHour { get => startTimeHour + periodHour; }
    private int periodHour;
    private bool sendMsg = false;

    private Dictionary<HediffDef, int> hediff2endtick = new();

    private bool DuringPeriod(Pawn pawn)
    {
        if (!days.Contains(GenLocalDate.DayOfQuadrum(pawn)))
            return false;
        var now = GenLocalDate.HourInteger(pawn);
        return startTimeHour <= now && now < endTimeHour;
        //跨天时，例如23-次日6点，该函数也只在23点返回true并触发带时限的hediff
        //所以无需对跨天数做特殊处理
    }

    private int TicksLeft(Pawn pawn)
    {
        var now = GenLocalDate.HourInteger(pawn);
        return (endTimeHour - now) * GenDate.TicksPerHour;
    }

    public override void OnIntervalPassed(Pawn pawn, Hediff cause)
    {
        if (DuringPeriod(pawn))
        {
            if (!hediff2endtick.ContainsKey(hediff) ||
                GenTicks.TicksAbs > hediff2endtick[hediff])
            {
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
                hediff2endtick[hediff] = GenTicks.TicksAbs + ticksLeft;
                var ret = TryApply(pawn);
                if (ret && sendMsg)
                    SendLetter(pawn, cause);
            }
        }
    }
}
