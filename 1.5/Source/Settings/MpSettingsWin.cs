using UnityEngine;
using Verse;

namespace MoonPhase.Settings;

internal class MpSettingsWin : Mod
{
    public static MpSettings setting;
    public MpSettingsWin(ModContentPack content) : base(content)
    {
        setting = GetSettings<MpSettings>();
    }

    public override string SettingsCategory()
    {
        return "MoonPhase".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.CheckboxLabeled("MP.ShowTips.Label".Translate(), ref setting.showTips);

        listingStandard.End();
        setting.Write();
    }
}
