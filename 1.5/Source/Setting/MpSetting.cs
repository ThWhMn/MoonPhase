using UnityEngine;
using Verse;

namespace MoonPhase.Setting;

internal class MpSetting : Mod
{
    public static MpConfig config;
    public MpSetting(ModContentPack content) : base(content)
    {
        config = GetSettings<MpConfig>();
    }

    public override string SettingsCategory()
    {
        return "MoonPhase".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.CheckboxLabeled("MP.ShowTips.Label".Translate(), ref config.showTips);

        listingStandard.End();
        config.Write();
    }
}
