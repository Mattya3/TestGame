using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Bindingの取得、照合、競合判定、カーブ移動を共通化する。
internal static class AnimationClipBindingUtility
{
    public static EditorCurveBinding[] GetBindings(AnimationClip clip)
    {
        var numeric = AnimationUtility.GetCurveBindings(clip);
        var objects = AnimationUtility.GetObjectReferenceCurveBindings(clip);
        var bindings = new EditorCurveBinding[numeric.Length + objects.Length];
        Array.Copy(numeric, bindings, numeric.Length);
        Array.Copy(objects, 0, bindings, numeric.Length, objects.Length);
        return bindings;
    }

    // 置換先の競合はカーブ種別を問わず、Path・Type・Propertyで判断する。
    public static bool HasSameTarget(EditorCurveBinding a, EditorCurveBinding b)
    {
        return a.path == b.path && a.type == b.type &&
               a.propertyName == b.propertyName;
    }

    public static bool IsSameSource(EditorCurveBinding a, EditorCurveBinding b)
    {
        return HasSameTarget(a, b) &&
               a.isPPtrCurve == b.isPPtrCurve &&
               a.isDiscreteCurve == b.isDiscreteCurve;
    }

    public static string GetCurveKind(EditorCurveBinding binding)
    {
        if (binding.isPPtrCurve)
            return "オブジェクト参照";

        return binding.isDiscreteCurve ? "離散値" : "数値";
    }

    public static AnimationClipChange CreateReplacement(
        AnimationClip clip,
        Predicate<EditorCurveBinding> isSource,
        Func<EditorCurveBinding, EditorCurveBinding> getDestination)
    {
        var bindings = GetBindings(clip);
        var destinations = new List<EditorCurveBinding>();
        var moves = new List<Action>();
        int conflicts = 0;

        foreach (var source in bindings)
        {
            if (!isSource(source))
                continue;

            var destination = getDestination(source);
            if (HasSameTarget(source, destination))
                continue;

            if (Array.Exists(bindings, existing => HasSameTarget(existing, destination)) ||
                destinations.Exists(planned => HasSameTarget(planned, destination)))
            {
                conflicts++;
                continue;
            }

            destinations.Add(destination);
            moves.Add(CreateCurveMove(clip, source, destination));
        }

        return new AnimationClipChange(moves.Count, conflicts, () =>
        {
            foreach (Action move in moves)
            {
                move();
            }
        });
    }

    private static Action CreateCurveMove(
        AnimationClip clip, EditorCurveBinding source, EditorCurveBinding destination)
    {
        // 計画作成時にデータを取得し、適用時にはカーブ種別などの情報を保持する。
        if (source.isPPtrCurve)
        {
            var keys = AnimationUtility.GetObjectReferenceCurve(clip, source);
            return () =>
            {
                AnimationUtility.SetObjectReferenceCurve(clip, source, null);
                AnimationUtility.SetObjectReferenceCurve(clip, destination, keys);
            };
        }

        var curve = AnimationUtility.GetEditorCurve(clip, source);
        return () =>
        {
            AnimationUtility.SetEditorCurve(clip, source, null);
            AnimationUtility.SetEditorCurve(clip, destination, curve);
        };
    }
}
