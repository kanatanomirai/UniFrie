using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// さまざまなパラメータを管理するクラス
public class Parameter : MonoBehaviour
{
    // Unityちゃんが動いているか
    public static bool isMoving;
    // 表情が変わっているか
    public static bool isExpressChanging;
    // 話し中か
    public static bool isTalking;
    // Unityちゃんが現れているか
    public static bool isAppearing;

    // 音声認識APIのAPIキー
    public static readonly string recognizeAPIkey = "";
    // 雑談対話APIのAPIキー
    public static readonly string talkAPIkey = "";
    // 音声合成APIのAPIキー
    public static readonly string compositionAPIkey = "";
    // API種類(TalkAPI、DocomoAPI)
    public static readonly InputText.TextTypes type = InputText.TextTypes.DocomoAPI;


    private Text debugText;

    void Start()
    {
        debugText = GameObject.FindWithTag("DebugText").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        debugText.text = "おしゃべり：" + System.Convert.ToString(isTalking) + "\nひょうじょう：" + System.Convert.ToString(isExpressChanging)
        + "\nうごき：" + System.Convert.ToString(isMoving) + "\nいる：" + System.Convert.ToString(isAppearing);
    }

}
