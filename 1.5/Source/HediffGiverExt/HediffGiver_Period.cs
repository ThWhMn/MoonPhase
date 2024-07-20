using RimWorld;
using Verse;

namespace MoonPhase.Wolfein.HediffGiverExt;

public class HediffGiver_Period : HediffGiver_BasePeriod
{
    protected override bool DuringPeriod(Pawn pawn)
    {
        var now = GenLocalDate.HourInteger(pawn);
        return startTimeHour <= now && now < endTimeHour;
        //跨天时，例如23-次日6点，该函数也只在23点返回true并触发带时限的hediff
        //所以无需对跨天数做特殊处理
    }
}
