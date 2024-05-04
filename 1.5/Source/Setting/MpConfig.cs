using Verse;

namespace MoonPhase.Setting;

internal class MpConfig : ModSettings
{
    public bool showTips = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref showTips, "ShowTips", true);
    }
}
