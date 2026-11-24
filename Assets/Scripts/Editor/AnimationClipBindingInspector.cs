using System.Text;
using UnityEditor;
using UnityEngine;

public static class AnimationClipBindingInspector
{
    [MenuItem("Tools/Log Selected Animation Clip Bindings")]
    private static void LogSelectedBindings()
    {
        var clips =
            AnimationClipToolUtility.GetSelectedClips();

        if (clips.Length == 0)
        {
            Debug.Log("ProjectでAnimationClipを選択してください。");
            return;
        }

        foreach (var clip in clips)
        {
            var text = new StringBuilder();
            text.AppendLine($"AnimationClip: {clip.name}");

            var bindings = AnimationClipBindingUtility.GetBindings(clip);
            AppendBindings(text, "数値カーブ", bindings, isObjectReference: false);
            AppendBindings(text, "オブジェクト参照カーブ", bindings, isObjectReference: true);

            Debug.Log(text.ToString(), clip);
        }
    }

    private static void AppendBindings(
        StringBuilder text,
        string category,
        EditorCurveBinding[] bindings,
        bool isObjectReference)
    {
        text.AppendLine($"\n【{category}】");

        foreach (var binding in bindings)
        {
            if (binding.isPPtrCurve != isObjectReference)
                continue;

            text.AppendLine(
                $"Path: \"{binding.path}\" | " +
                $"Type: {binding.type?.FullName} | " +
                $"Property: {binding.propertyName}"
            );
        }
    }
}
