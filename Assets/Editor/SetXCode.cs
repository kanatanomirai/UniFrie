using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

/// <summary>
/// Xcodeの設定を行うクラス。
/// </summary>
static class SetXCode{

    /// <summary>
    /// プロジェクトのビルド後に修正を加える。
    /// </summary>
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget platform, string projectPath)
    {
        if (platform != BuildTarget.iOS) return;

        EditPlist(projectPath);
    }

    /// <summary>
    /// .plistファイルの編集を行う。
    /// </summary>
    static void EditPlist(string projectPath)
    {
        var plistPath = Path.Combine(projectPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // マイクへのアクセスが必要な場合には plist に用途を書かないといけない
        // see https://developer.apple.com/library/content/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html#//apple_ref/doc/uid/TP40009251-SW25
        plist.root.SetString("NSMicrophoneUsageDescription", "音声認識に使用します");
        plist.WriteToFile(plistPath);
    }
}
