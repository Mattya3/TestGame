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

    private void OnGUI()
    {
        AnimationClip[] clips =
            AnimationClipToolUtility.GetSelectedClips();

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
            EditorGUILayout.LabelField("カーブの種類", AnimationClipBindingUtility.GetCurveKind(_source));
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
            if (!AnimationClipToolUtility.IsEditableClip(clip))
                continue;

            EditorGUILayout.LabelField(clip.name, EditorStyles.boldLabel);

            DrawBindings(AnimationClipBindingUtility.GetBindings(clip));

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
                $"Path: {path} | Type: {typeName} | {AnimationClipBindingUtility.GetCurveKind(binding)}";

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

    private void ReplaceSelected(AnimationClip[] clips)
    {
        var result = AnimationClipBatchProcessor.Run(
            clips, "Replace Animation Bindings",
            clip => AnimationClipBindingUtility.CreateReplacement(
                clip,
                binding => AnimationClipBindingUtility.IsSameSource(binding, _source),
                binding =>
                {
                    binding.path = _newPath;
                    binding.type = _newType;
                    binding.propertyName = _newProperty;
                    return binding;
                }));

        AnimationClipToolUtility.ShowResult(result, "カーブ", showConflicts: true);
    }
}
