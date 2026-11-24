using System;
using UnityEditor;
using UnityEngine;

public class AnimationClipPathReplacer : EditorWindow
{
    [SerializeField] private string _oldPath = "";
    [SerializeField] private string _newPath = "";

    [MenuItem("Tools/Animation Clip Path Replacer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipPathReplacer>("Anim Path Replacer");
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Clip Path Replacer", EditorStyles.boldLabel);
        _oldPath = EditorGUILayout.TextField("Old Path", _oldPath);
        _newPath = EditorGUILayout.TextField("New Path", _newPath);

        var clips = Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);
        EditorGUILayout.LabelField("選択中のクリップ数", clips.Length.ToString());
        EditorGUILayout.HelpBox(
            "Projectウィンドウで対象のAnimationClipを複数選択してください。\n" +
            "Old Pathと完全一致するパスを置換します。対象は .anim ファイルです。\n" +
            "New Pathを空にするとルートを対象にします。",
            MessageType.Info
        );

        using (new EditorGUI.DisabledScope(
            clips.Length == 0 || string.IsNullOrEmpty(_oldPath) || _oldPath == _newPath))
        {
            if (GUILayout.Button("Replace Paths"))
            {
                ReplacePaths(clips, _oldPath, _newPath);
            }
        }
    }

    private void ReplacePaths(AnimationClip[] clips, string oldP, string newP)
    {
        int changedClipCount = 0;
        int changedBindingCount = 0;
        int skippedClipCount = 0;

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Replace Animation Paths");

        try
        {
            foreach (var targetClip in clips)
            {
                string assetPath = AssetDatabase.GetAssetPath(targetClip);
                if (!assetPath.EndsWith(".anim", StringComparison.OrdinalIgnoreCase))
                {
                    skippedClipCount++;
                    continue;
                }

                var curveBindings = AnimationUtility.GetCurveBindings(targetClip);
                var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(targetClip);
                if (!Array.Exists(curveBindings, binding => binding.path == oldP) &&
                    !Array.Exists(objectBindings, binding => binding.path == oldP))
                {
                    continue;
                }

                Undo.RecordObject(targetClip, "Replace Animation Paths");

                // 数値カーブ（Transform, Rendererなど）
                changedBindingCount += ReplaceBindingsGeneric(
                    targetClip, curveBindings, oldP, newP,
                    AnimationUtility.GetEditorCurve,
                    AnimationUtility.SetEditorCurve
                );

                // オブジェクト参照カーブ（Sprite, Materialなど）
                changedBindingCount += ReplaceBindingsGeneric(
                    targetClip, objectBindings, oldP, newP,
                    AnimationUtility.GetObjectReferenceCurve,
                    AnimationUtility.SetObjectReferenceCurve
                );

                EditorUtility.SetDirty(targetClip);
                changedClipCount++;
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
            $"置換したカーブ：{changedBindingCount} 件\n" +
            $"対象外のクリップ：{skippedClipCount} 個",
            "OK"
        );
    }

    private int ReplaceBindingsGeneric<T>(
        AnimationClip targetClip,
        EditorCurveBinding[] bindings,
        string oldP,
        string newP,
        Func<AnimationClip, EditorCurveBinding, T> getter,
        Action<AnimationClip, EditorCurveBinding, T> setter
    )
    {
        int replacedCount = 0;
        foreach (var binding in bindings)
        {
            if (binding.path != oldP)
                continue;

            T data = getter(targetClip, binding);
            var newBinding = binding;
            newBinding.path = newP;

            setter(targetClip, binding, default);
            setter(targetClip, newBinding, data);
            replacedCount++;
        }
        return replacedCount;
    }
}
