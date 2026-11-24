using System;
using UnityEditor;
using UnityEngine;

public class AnimationEventFunctionNameReplacer : EditorWindow
{
    [SerializeField] private string _oldName = "";
    [SerializeField] private string _newName = "";

    [MenuItem("Tools/Animation Event Function Name Replacer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationEventFunctionNameReplacer>(
            "Event Name Replacer"
        );
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label(
            "Animation Event Function Name Replacer",
            EditorStyles.boldLabel
        );

        _oldName = EditorGUILayout.TextField(
            "Old Function Name", _oldName
        );

        _newName = EditorGUILayout.TextField(
            "New Function Name", _newName
        );

        AnimationClip[] clips =
            Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("選択中のクリップ数", clips.Length.ToString());

        EditorGUILayout.HelpBox(
            "Projectウィンドウで複数のAnimationClipを選択してください。\n" +
            "名前が完全一致する全イベントを置換します。\n" +
            "独立した .anim ファイルのみ対象です。",
            MessageType.Info
        );

        using (new EditorGUI.DisabledScope(
            clips.Length == 0 ||
            string.IsNullOrWhiteSpace(_oldName) ||
            string.IsNullOrWhiteSpace(_newName) ||
            _oldName == _newName))
        {
            if (GUILayout.Button("Replace Function Names"))
            {
                ReplaceFunctionNames(clips);
            }
        }
    }

    private void ReplaceFunctionNames(AnimationClip[] clips)
    {
        int changedClipCount = 0;
        int changedEventCount = 0;
        int skippedClipCount = 0;

        // 今回の一括置換を1回のUndoで戻せるようにする。
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Replace Animation Event Function Names");

        try
        {
            foreach (AnimationClip clip in clips)
            {
                string assetPath = AssetDatabase.GetAssetPath(clip);

                // FBXなどに含まれるクリップは対象外。
                if (!assetPath.EndsWith(
                    ".anim", StringComparison.OrdinalIgnoreCase))
                {
                    skippedClipCount++;
                    continue;
                }

                AnimationEvent[] events =
                    AnimationUtility.GetAnimationEvents(clip);

                int replacedCount = 0;

                foreach (AnimationEvent animationEvent in events)
                {
                    if (animationEvent.functionName != _oldName)
                        continue;

                    animationEvent.functionName = _newName;
                    replacedCount++;
                }

                if (replacedCount == 0)
                    continue;

                Undo.RecordObject(
                    clip, "Replace Animation Event Function Names"
                );

                AnimationUtility.SetAnimationEvents(clip, events);
                EditorUtility.SetDirty(clip);

                changedClipCount++;
                changedEventCount += replacedCount;
            }

            if (changedClipCount > 0)
            {
                AssetDatabase.SaveAssets();
            }
        }
        finally
        {
            Undo.CollapseUndoOperations(undoGroup);
        }

        EditorUtility.DisplayDialog(
            "置換結果",
            $"変更したクリップ：{changedClipCount} 個\n" +
            $"置換したイベント：{changedEventCount} 件\n" +
            $"対象外のクリップ：{skippedClipCount} 個",
            "OK"
        );
    }
}