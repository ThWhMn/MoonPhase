using Verse;

namespace MoonPhase.Settings;

internal class MpSettings : ModSettings
{
    public bool showTips;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref showTips, "ShowTips", true, true);
    }
}
