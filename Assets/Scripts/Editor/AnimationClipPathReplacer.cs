using System;
using UnityEditor;
using UnityEngine;

public class AnimationClipPathReplacer : EditorWindow
{
    private AnimationClip _clip;
    private string _oldPath = "";
    private string _newPath = "";

    [MenuItem("Tools/Animation Clip Path Replacer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipPathReplacer>("Anim Path Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Clip Path Replacer", EditorStyles.boldLabel);

        _clip = (AnimationClip)
            EditorGUILayout.ObjectField("Animation Clip", _clip, typeof(AnimationClip), false);
        _oldPath = EditorGUILayout.TextField("Old Path", _oldPath);
        _newPath = EditorGUILayout.TextField("New Path", _newPath);

        if (GUILayout.Button("Replace Paths"))
        {
            if (_clip == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select an AnimationClip.", "OK");
                return;
            }
            ReplacePaths(_clip, _oldPath, _newPath);
        }
    }

    private void ReplacePaths(AnimationClip targetClip, string oldP, string newP)
    {
        if (string.IsNullOrEmpty(oldP))
        {
            EditorUtility.DisplayDialog("Error", "Old path cannot be empty.", "OK");
            return;
        }

        Undo.RecordObject(targetClip, "Replace Animation Paths");

        // --- 数値カーブ（Transform, Renderer など） ---
        var curveBindings = AnimationUtility.GetCurveBindings(targetClip);
        ReplaceBindingsGeneric(
            targetClip,
            curveBindings,
            oldP,
            newP,
            AnimationUtility.GetEditorCurve,
            AnimationUtility.SetEditorCurve
        );

        // --- オブジェクト参照カーブ（Sprite, Material など） ---
        var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(targetClip);
        ReplaceBindingsGeneric(
            targetClip,
            objectBindings,
            oldP,
            newP,
            AnimationUtility.GetObjectReferenceCurve,
            AnimationUtility.SetObjectReferenceCurve
        );

        EditorUtility.SetDirty(targetClip);
        AssetDatabase.SaveAssets();
        Debug.Log($"Replaced path \"{oldP}\" → \"{newP}\" in {targetClip.name}");
    }

    private void ReplaceBindingsGeneric<T>(
        AnimationClip targetClip,
        EditorCurveBinding[] bindings,
        string oldP,
        string newP,
        Func<AnimationClip, EditorCurveBinding, T> getter,
        Action<AnimationClip, EditorCurveBinding, T> setter
    )
    {
        foreach (var binding in bindings)
        {
            if (binding.path != oldP)
                continue;
            T data = getter(targetClip, binding);
            var newBinding = binding;
            newBinding.path = newP;

            setter(targetClip, binding, default);
            setter(targetClip, newBinding, data);
        }
    }
}
