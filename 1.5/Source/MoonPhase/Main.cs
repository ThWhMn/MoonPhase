using HarmonyLib;
using MoonPhase.Settings;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;


namespace MoonPhase;


/// <summary>
/// 因为一quadrum有15天，为了保持15天正好一个轮回
/// 需要第1天和第16天月相一致，但这做不到，干脆直接新增一个阴历纪年法。
/// </summary>
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
    static readonly string harmonyId = "Thwh.MoonPhase";
    static readonly Dictionary<PhaseNames, string> phase2str = new();
    static readonly int NumOfPhase = (int)PhaseNames.End;
    static Main()
    {
        if (!Harmony.HasAnyPatches(harmonyId))
        {
            Harmony harmony = new(harmonyId);
            harmony.Patch(
                original: AccessTools.Method(typeof(GlobalControlsUtility), nameof(GlobalControlsUtility.DoDate)),
                prefix: null,
                postfix: new HarmonyMethod(typeof(Main), nameof(ShouldUpdate)));
            for (var i = PhaseNames.Start + 1; i < PhaseNames.End; i++)
            {
                phase2str[i] = $"{i}";
            }
        }
    }

    private static void ShouldUpdate(ref float curBaseY)
    {
        UpdateTab(ref curBaseY);
    }

    public static bool InPhaseList(List<string> list)
    {
        return list.Contains(phase2str[DayspassedtoPhase()]);
    }

    private static PhaseNames DayspassedtoPhase()
    {
        int days = GenDate.DaysPassed;
        int offset = days % NumOfPhase;
        return offset + PhaseNames.Start + 1;
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

        var phaseType = DayspassedtoPhase();

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

        if (MpSettingsWin.setting.showTips)
        {
            TooltipHandler.TipRegion(zlRect, new TipSignal($"{phaseType}_Tip".Translate()));
        }

        curBaseY -= zlRect.height;
    }
}
