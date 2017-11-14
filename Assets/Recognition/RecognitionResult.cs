using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 認識結果を管理するクラス。
/// </summary>
public class RecognitionResult : MonoBehaviour
{
    public string utteranceid;
    public string text;
    public string code;
    public string message;
    public string results;

    /// <summary>
    /// 認識結果を取得する。
    /// <param name="jsonStr">JSON形式の文字列</param>
    /// </summary>
    public string GetRecognitionText(string jsonStr)
    {
        RecognitionResult result = this;
        JsonUtility.FromJsonOverwrite(jsonStr, result);

        // 認識結果の読点、句読点を消す（しりとり時の認識改善）
        char[] removeChars = new char[] { '、', '。' };

        foreach (char c in removeChars)
        {
            result.text = result.text.Replace(c.ToString(), "");
        }

        Debug.Log("認識結果:" + result.text);
        return result.text;
    }
}
