using System.Text;
using UnityEditor;
using UnityEngine;

public static class AnimationClipBindingInspector
{
    [MenuItem("Tools/Log Selected Animation Clip Bindings")]
    private static void LogSelectedBindings()
    {
        var clips =
            Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);

        if (clips.Length == 0)
        {
            Debug.Log("ProjectでAnimationClipを選択してください。");
            return;
        }

        foreach (var clip in clips)
        {
            var text = new StringBuilder();
            text.AppendLine($"AnimationClip: {clip.name}");

            AppendBindings(
                text,
                "数値カーブ",
                AnimationUtility.GetCurveBindings(clip)
            );

            AppendBindings(
                text,
                "オブジェクト参照カーブ",
                AnimationUtility.GetObjectReferenceCurveBindings(clip)
            );

            Debug.Log(text.ToString(), clip);
        }
    }

    private static void AppendBindings(
        StringBuilder text,
        string category,
        EditorCurveBinding[] bindings)
    {
        text.AppendLine($"\n【{category}】");

        foreach (var binding in bindings)
        {
            text.AppendLine(
                $"Path: \"{binding.path}\" | " +
                $"Type: {binding.type?.FullName} | " +
                $"Property: {binding.propertyName}"
            );
        }
    }
}