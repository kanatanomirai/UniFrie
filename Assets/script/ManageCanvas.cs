using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageCanvas : MonoBehaviour
{
    private static Canvas canvas;
    void Start()
    {
        // Canvasコンポーネントを保持
        canvas = this.GetComponent<Canvas>();
    }

    /// 表示・非表示を設定する
    public static void SetActive(string name, bool b)
    {
        foreach (Transform child in canvas.transform)
        {
            // 子の要素をたどる
            if (child.name == name)
            {
                // 指定した名前と一致
                // 表示フラグを設定
                child.gameObject.SetActive(b);
                // おしまい
                return;
            }
        }
        // 指定したオブジェクト名が見つからなかった
        Debug.LogWarning("Not found objname:" + name);
    }

    /// テキストを変更する
    public static void SetText(string name, string text)
    {
        foreach (Transform child in canvas.transform)
        {
            // 子の要素をたどる
            if (child.name == name)
            {
                // 名前が一致
                foreach (Transform child2 in child.transform)
                {
                    // 孫要素をたどる
                    if (child2.name == "Text")
                    {
                        // テキストを見つけた
                        Text t = child2.GetComponent<Text>();
                        // テキスト変更
                        t.text = text;

                        // おしまい
                        return;
                    }
                }
            }
        }
        // 指定したオブジェクト名が見つからなかった
        Debug.LogWarning("Not found objname:" + name);
    }
}
