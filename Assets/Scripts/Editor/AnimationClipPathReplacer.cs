using UnityEngine;
using UnityEditor;

public class AnimationClipPathReplacer : EditorWindow
{
    private AnimationClip clip;
    private string oldPath = "";
    private string newPath = "";

    [MenuItem("Tools/Animation Clip Path Replacer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipPathReplacer>("Anim Path Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Clip Path Replacer", EditorStyles.boldLabel);

        clip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", clip, typeof(AnimationClip), false);
        oldPath = EditorGUILayout.TextField("Old Path", oldPath);
        newPath = EditorGUILayout.TextField("New Path", newPath);

        if (GUILayout.Button("Replace Paths"))
        {
            if (clip == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select an AnimationClip.", "OK");
                return;
            }
            ReplacePaths(clip, oldPath, newPath);
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

        // すべてのカーブを取得
        var bindings = AnimationUtility.GetCurveBindings(targetClip);
        foreach (var binding in bindings)
        {
            if (binding.path == oldP)
            {
                var curve = AnimationUtility.GetEditorCurve(targetClip, binding);

                // 新しいバインディングを作成
                var newBinding = binding;
                newBinding.path = newP;

                // 古いカーブを削除して新しいカーブを追加
                AnimationUtility.SetEditorCurve(targetClip, binding, null);
                AnimationUtility.SetEditorCurve(targetClip, newBinding, curve);
            }
        }

        EditorUtility.SetDirty(targetClip);
        AssetDatabase.SaveAssets();
        Debug.Log($"Replaced path \"{oldP}\" → \"{newP}\" in {targetClip.name}");
    }
}
