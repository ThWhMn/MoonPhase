using Verse;

namespace MoonPhase.Settings;

internal class MpSettings : ModSettings
{
    public bool showTips;
    public PhaseNames initPhase;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref showTips, "ShowTips", true, true);
        Scribe_Values.Look(ref initPhase, "phaseStarter", PhaseNames.Start, true);
    }
}
