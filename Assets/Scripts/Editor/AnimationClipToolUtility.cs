using System;
using UnityEditor;
using UnityEngine;

// Animation系ツールで共通の選択・対象判定・結果表示。
internal static class AnimationClipToolUtility
{
    public static AnimationClip[] GetSelectedClips()
    {
        return Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);
    }

    public static bool IsEditableClip(AnimationClip clip)
    {
        return clip != null && AssetDatabase.GetAssetPath(clip).EndsWith(
            ".anim", StringComparison.OrdinalIgnoreCase);
    }

    public static void ShowResult(
        AnimationClipBatchResult result, string itemName, bool showConflicts = false)
    {
        string message =
            $"変更したクリップ：{result.ChangedClips} 個\n" +
            $"置換した{itemName}：{result.ChangedItems} 件\n";

        if (showConflicts)
        {
            message += $"置換先が既存のためスキップ：{result.Conflicts} 件\n";
        }

        message += $"対象外のクリップ：{result.SkippedClips} 個";
        EditorUtility.DisplayDialog("置換結果", message, "OK");
    }
}
