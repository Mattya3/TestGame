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
            AnimationClipToolUtility.GetSelectedClips();

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
        var result = AnimationClipBatchProcessor.Run(
            clips, "Replace Animation Event Function Names", CreateChange);

        AnimationClipToolUtility.ShowResult(result, "イベント");
    }

    private AnimationClipChange CreateChange(AnimationClip clip)
    {
        var events = AnimationUtility.GetAnimationEvents(clip);
        int replacedCount = 0;

        foreach (AnimationEvent animationEvent in events)
        {
            if (animationEvent.functionName != _oldName)
                continue;

            animationEvent.functionName = _newName;
            replacedCount++;
        }

        return new AnimationClipChange(replacedCount, 0,
            () => AnimationUtility.SetAnimationEvents(clip, events));
    }
}
