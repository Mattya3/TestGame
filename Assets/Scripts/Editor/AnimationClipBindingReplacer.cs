using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationClipBindingReplacer : EditorWindow
{
    private EditorCurveBinding _source;
    private bool _hasSource;

    private string _newPath = "";
    private string _newProperty = "";
    private Type _newType;

    private readonly List<Type> _types = new List<Type>();
    private string[] _typeLabels;
    private Vector2 _scroll;

    [MenuItem("Tools/Animation Clip Binding Replacer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipBindingReplacer>("Binding Replacer");
    }

    private void OnEnable()
    {
        minSize = new Vector2(550, 500);

        _types.Clear();
        _types.Add(typeof(GameObject));

        foreach (Type type in TypeCache.GetTypesDerivedFrom<Component>())
        {
            if (!type.IsAbstract && !type.ContainsGenericParameters)
            {
                _types.Add(type);
            }
        }

        UpdateTypeLabels();
    }

    private void UpdateTypeLabels()
    {
        _types.Sort((a, b) =>
            string.CompareOrdinal(a.AssemblyQualifiedName, b.AssemblyQualifiedName));

        _typeLabels = _types.ConvertAll(type =>
            $"{type.FullName} ({type.Assembly.GetName().Name})"
        ).ToArray();
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnProjectChange()
    {
        Repaint();
    }

    private static bool IsEditableClip(AnimationClip clip)
    {
        return AssetDatabase.GetAssetPath(clip).EndsWith(
            ".anim", StringComparison.OrdinalIgnoreCase
        );
    }

    private static string CurveKind(EditorCurveBinding binding)
    {
        if (binding.isPPtrCurve)
            return "オブジェクト参照";

        return binding.isDiscreteCurve ? "離散値" : "数値";
    }

    private void OnGUI()
    {
        AnimationClip[] clips =
            Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);

        GUILayout.Label("変更元", EditorStyles.boldLabel);

        // 変更元は一覧から選択する。
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.TextField(
                "Old Path", _hasSource ? _source.path : ""
            );

            EditorGUILayout.TextField(
                "Old Type",
                _hasSource ? _source.type?.FullName ?? "(Missing Type)" : ""
            );

            EditorGUILayout.TextField(
                "Old Property", _hasSource ? _source.propertyName : ""
            );
        }

        if (_hasSource)
        {
            EditorGUILayout.LabelField("カーブの種類", CurveKind(_source));
        }

        EditorGUILayout.Space();
        GUILayout.Label("変更先", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(!_hasSource))
        {
            _newPath = EditorGUILayout.TextField("New Path", _newPath);

            int currentIndex = _types.IndexOf(_newType);
            int selectedIndex = EditorGUILayout.Popup(
                "New Type", currentIndex, _typeLabels
            );

            if (selectedIndex >= 0 && selectedIndex < _types.Count)
            {
                _newType = _types[selectedIndex];
            }

            _newProperty = EditorGUILayout.TextField(
                "New Property", _newProperty
            );
        }

        bool unchanged =
            _hasSource &&
            _source.path == _newPath &&
            _source.type == _newType &&
            _source.propertyName == _newProperty;

        using (new EditorGUI.DisabledScope(
            !_hasSource ||
            _source.type == null ||
            _newType == null ||
            string.IsNullOrWhiteSpace(_newProperty) ||
            clips.Length == 0 ||
            unchanged))
        {
            if (GUILayout.Button("選択中のクリップを一括置換"))
            {
                ReplaceSelected(clips);
                Repaint();
                GUIUtility.ExitGUI();
            }
        }

        EditorGUILayout.Space();
        GUILayout.Label(
            $"プロパティ一覧：選択中 {clips.Length} クリップ",
            EditorStyles.boldLabel
        );

        EditorGUILayout.HelpBox(
            "Projectで対象の .anim ファイルを選択してください。\n" +
            "下のプロパティをクリックすると変更元と変更先の初期値が入ります。\n" +
            "パスの空欄はルートです。New Propertyには内部名を指定してください。",
            MessageType.Info
        );

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        foreach (AnimationClip clip in clips)
        {
            if (!IsEditableClip(clip))
                continue;

            EditorGUILayout.LabelField(clip.name, EditorStyles.boldLabel);

            DrawBindings(AnimationUtility.GetCurveBindings(clip));
            DrawBindings(
                AnimationUtility.GetObjectReferenceCurveBindings(clip)
            );

            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawBindings(EditorCurveBinding[] bindings)
    {
        foreach (EditorCurveBinding binding in bindings)
        {
            string path = string.IsNullOrEmpty(binding.path)
                ? "(Root)"
                : binding.path;

            string typeName = binding.type?.FullName ?? "(Missing Type)";

            string label =
                $"{binding.propertyName}\n" +
                $"Path: {path} | Type: {typeName} | {CurveKind(binding)}";

            if (GUILayout.Button(label, GUILayout.MinHeight(38)))
            {
                _source = binding;
                _hasSource = true;

                // 変更先を同じ内容で初期化し、必要な項目だけ編集する。
                _newPath = binding.path;
                _newProperty = binding.propertyName;
                _newType = binding.type;

                // 一覧の型が候補にない場合も保持できるよう追加する。
                if (_newType != null && !_types.Contains(_newType))
                {
                    _types.Add(_newType);
                    UpdateTypeLabels();
                }
            }
        }
    }

    private bool IsSource(EditorCurveBinding binding)
    {
        return binding.path == _source.path &&
               binding.type == _source.type &&
               binding.propertyName == _source.propertyName &&
               binding.isPPtrCurve == _source.isPPtrCurve &&
               binding.isDiscreteCurve == _source.isDiscreteCurve;
    }

    private bool IsDestination(EditorCurveBinding binding)
    {
        // カーブの種類が違っていても同じ宛先は競合とする。
        return binding.path == _newPath &&
               binding.type == _newType &&
               binding.propertyName == _newProperty;
    }

    private void ReplaceSelected(AnimationClip[] clips)
    {
        int changedClips = 0;
        int conflicts = 0;
        int skippedClips = 0;

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Replace Animation Bindings");

        try
        {
            foreach (AnimationClip clip in clips)
            {
                if (!IsEditableClip(clip))
                {
                    skippedClips++;
                    continue;
                }

                var numericBindings =
                    AnimationUtility.GetCurveBindings(clip);

                var objectBindings =
                    AnimationUtility.GetObjectReferenceCurveBindings(clip);

                var sourceBindings = _source.isPPtrCurve
                    ? objectBindings
                    : numericBindings;

                int sourceIndex = Array.FindIndex(sourceBindings, IsSource);

                if (sourceIndex < 0)
                    continue;

                if (Array.Exists(numericBindings, IsDestination) ||
                    Array.Exists(objectBindings, IsDestination))
                {
                    conflicts++;
                    continue;
                }

                var oldBinding = sourceBindings[sourceIndex];

                // 元のカーブ種別などの情報は保持する。
                var newBinding = oldBinding;
                newBinding.path = _newPath;
                newBinding.type = _newType;
                newBinding.propertyName = _newProperty;

                Undo.RecordObject(clip, "Replace Animation Bindings");

                if (oldBinding.isPPtrCurve)
                {
                    var keys = AnimationUtility.GetObjectReferenceCurve(
                        clip, oldBinding
                    );

                    AnimationUtility.SetObjectReferenceCurve(
                        clip, oldBinding, null
                    );

                    AnimationUtility.SetObjectReferenceCurve(
                        clip, newBinding, keys
                    );
                }
                else
                {
                    var curve = AnimationUtility.GetEditorCurve(
                        clip, oldBinding
                    );

                    AnimationUtility.SetEditorCurve(
                        clip, oldBinding, null
                    );

                    AnimationUtility.SetEditorCurve(
                        clip, newBinding, curve
                    );
                }

                EditorUtility.SetDirty(clip);
                changedClips++;
            }

            if (changedClips > 0)
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
            $"置換したクリップ／カーブ：{changedClips} 件\n" +
            $"置換先が既存のためスキップ：{conflicts} 件\n" +
            $"対象外のクリップ：{skippedClips} 個",
            "OK"
        );
    }
}