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

        var clips = AnimationClipToolUtility.GetSelectedClips();
        EditorGUILayout.LabelField("選択中のクリップ数", clips.Length.ToString());
        EditorGUILayout.HelpBox(
            "Projectウィンドウで対象のAnimationClipを複数選択してください。\n" +
            "Old Pathと完全一致するパスを置換します。対象は .anim ファイルです。\n" +
            "New Pathを空にするとルートを対象にします。\n" +
            "変更先に同じType・Propertyのカーブがある場合はスキップします。",
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
        var result = AnimationClipBatchProcessor.Run(
            clips, "Replace Animation Paths",
            clip => AnimationClipBindingUtility.CreateReplacement(
                clip,
                binding => binding.path == oldP,
                binding =>
                {
                    binding.path = newP;
                    return binding;
                }));

        AnimationClipToolUtility.ShowResult(result, "カーブ", showConflicts: true);
    }
}
