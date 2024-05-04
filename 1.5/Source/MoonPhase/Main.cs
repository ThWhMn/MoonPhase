using HarmonyLib;
using MoonPhase.Setting;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;


namespace MoonPhase;


public enum PhaseNames
{
    Start = -1,
    NewMoon, // 新月
    WaxingCrescent, // 眉月
    FirstQuarter, // 上弦月
    WaxingGibbous, // 上凸月
    FullMoon, // 满月
    WaningGibbous, // 下凸月
    LastQuarter, // 下弦月
    WaningCrescent, // 残月
    End
}

[StaticConstructorOnStartup]
internal class Main
{
    static Dictionary<PhaseNames, string> phase2str = new Dictionary<PhaseNames, string>();
    static Main()
    {
        Harmony harmony = new("Thwh.MoonPhase");
        harmony.Patch(
            original: AccessTools.Method(typeof(GlobalControlsUtility), nameof(GlobalControlsUtility.DoDate)),
            prefix: null,
            postfix: new HarmonyMethod(typeof(Main), nameof(ShouldUpdate)));
        for (var i = PhaseNames.Start + 1; i < PhaseNames.End; i++)
        {
            phase2str.Add(i, $"{i}");
        }
    }

    private static void ShouldUpdate(ref float curBaseY)
    {
        UpdateTab(ref curBaseY);
    }

    public static bool InPhaseList(Pawn pawn, List<string> list)
    {
        return list.Contains(phase2str[GetPhase(pawn)]);
    }

    public static PhaseNames GetPhase(Pawn pawn)
    {
        return TranstoPhase(GenLocalDate.DayOfQuadrum(pawn));
    }

    private static PhaseNames TranstoPhase(int dayOfQuadrum)
    {
        if (dayOfQuadrum <= (int)PhaseNames.Start)
        {
            Log.Error($"Wrong dayOfQuadrum {dayOfQuadrum} when trans to PhaseName");
            return PhaseNames.NewMoon;
        }
        else if (dayOfQuadrum < (int)PhaseNames.End)
        {
            return (PhaseNames)dayOfQuadrum;
        }
        else
        {
            var offset = dayOfQuadrum % (int)PhaseNames.End;
            return (PhaseNames)offset;
        }
    }

    private static void UpdateTab(ref float curBaseY)
    {
        float rightMargin = 7f;
        Rect zlRect = new(UI.screenWidth - Alert.Width, curBaseY - 24f, Alert.Width, 22f);
        Text.Font = GameFont.Small;

        // 鼠标移入时画出强调色
        if (Mouse.IsOver(zlRect))
        {
            Widgets.DrawHighlight(zlRect);
        }

        var phaseType = TranstoPhase(GenLocalDate.DayOfQuadrum(Find.CurrentMap));

        // 在此处创建GUI
        GUI.BeginGroup(zlRect);

        // 文本锚点在右上角
        Text.Anchor = TextAnchor.UpperRight;
        Rect rect = zlRect.AtZero();
        // 横坐标减少
        rect.xMax -= rightMargin;

        // 创建label
        Widgets.Label(rect, $"{phaseType}".Translate());
        // 文本锚点在左上角
        Text.Anchor = TextAnchor.UpperLeft;
        GUI.color = Color.white;
        // GUI创建结束
        GUI.EndGroup();

        if (MpSetting.config.showTips)
        {
            TooltipHandler.TipRegion(zlRect, new TipSignal($"{phaseType}_Tip".Translate()));
        }

        curBaseY -= zlRect.height;
    }
}
