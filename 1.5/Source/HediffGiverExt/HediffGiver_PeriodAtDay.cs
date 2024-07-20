using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MoonPhase.HediffGiverExt;

public class HediffGiver_PeriodAtDay : HediffGiver_BasePeriod
{
    private List<int> days;

    protected override bool DuringPeriod(Pawn pawn)
    {
        if (!days.Contains(GenLocalDate.DayOfQuadrum(pawn)))
            return false;
        var now = GenLocalDate.HourInteger(pawn);
        return startTimeHour <= now && now < endTimeHour;
        //跨天时，例如23-次日6点，该函数也只在23点返回true并触发带时限的hediff
        //所以无需对跨天数做特殊处理
    }
}
