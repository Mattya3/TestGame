using System;
using UnityEditor;
using UnityEngine;

// 変更計画を作成してからUndoを記録し、実際の変更を適用する。
internal sealed class AnimationClipChange
{
    public int ChangedItems { get; }
    public int Conflicts { get; }
    public Action Apply { get; }

    public AnimationClipChange(int changedItems, int conflicts, Action apply)
    {
        ChangedItems = changedItems;
        Conflicts = conflicts;
        Apply = apply;
    }
}

internal sealed class AnimationClipBatchResult
{
    public int ChangedClips { get; internal set; }
    public int ChangedItems { get; internal set; }
    public int Conflicts { get; internal set; }
    public int SkippedClips { get; internal set; }
}

// 一括処理のUndo、Dirty管理、保存、件数集計を共通化する。
internal static class AnimationClipBatchProcessor
{
    public static AnimationClipBatchResult Run(
        AnimationClip[] clips,
        string undoName,
        Func<AnimationClip, AnimationClipChange> createChange)
    {
        var result = new AnimationClipBatchResult();
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName(undoName);

        try
        {
            foreach (AnimationClip clip in clips)
            {
                if (!AnimationClipToolUtility.IsEditableClip(clip))
                {
                    result.SkippedClips++;
                    continue;
                }

                AnimationClipChange change = createChange(clip);
                result.Conflicts += change.Conflicts;
                if (change.ChangedItems == 0)
                    continue;

                Undo.RecordObject(clip, undoName);
                change.Apply();
                EditorUtility.SetDirty(clip);

                result.ChangedClips++;
                result.ChangedItems += change.ChangedItems;
            }

            if (result.ChangedClips > 0)
            {
                AssetDatabase.SaveAssets();
            }
        }
        finally
        {
            Undo.CollapseUndoOperations(undoGroup);
        }

        return result;
    }
}
