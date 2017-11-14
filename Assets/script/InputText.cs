using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// テキストを管理するクラス。
/// </summary>
public class InputText : MonoBehaviour
{

    private Text text;
    private UniMotion motion;
    // テキストの長さ
    private int textLength = 0;
    // 会話コンテキスト
    private string context;
    // 会話モード
    private string mode;
    // 最終的なテキスト
    private string finalText;

    private string[] angryText = new string[10];
    private string[] reluxText = new string[10];

    /// <summary>
    /// テキストの種類。
    /// </summary>
    public enum TextTypes
    {
        TalkAPI,
        DocomoAPI,
        StartText,
        ExpressionText
    }

    void Start()
    {
        text = GameObject.FindWithTag("Text").GetComponent<Text>();
        gameObject.AddComponent<Connection>();
        motion = GameObject.FindWithTag("Unitychan").GetComponent<UniMotion>();
        // 初期化
        Init();
    }

    /// <summary>
    /// テキストの長さを設定するプロパティ。
    /// </summary>
    public int TextLength
    {
        set
        {
            // テキストの長さを設定
            this.textLength = value;
        }
        get
        {
            // テキストの長さを取得
            return this.textLength;
        }
    }

    /// <summary>
    /// 会話継続コンテキストを設定するプロパティ。
    /// </summary>
    public string Context
    {
        set
        {
            // 会話継続コンテキストを設定
            this.context = value;
        }
        get
        {
            // 会話継続コンテキストを取得
            return this.context;
        }
    }

    /// <summary>
    /// 会話モードを設定するプロパティ。
    /// </summary>
    public string Mode
    {
        set
        {
            // 会話モードを設定
            this.mode = value;
        }
        get
        {
            // 会話モードを取得
            return this.mode;
        }
    }

    /// <summary>
    /// 最終的な会話テキストを設定するプロパティ。
    /// </summary>
    public string FinalText
    {
        set
        {
            // 最終会話テキストを設定
            this.finalText = value;
        }
        get
        {
            // 最終会話テキストを取得
            return this.finalText;
        }
    }

    /// <summary>
    /// 会話テキストの設定を行う。
    /// <param name="comText">会話テキスト</param>
    /// <param name="type">テキストタイプ</param>
    /// </summary>
    public void SetText(string comText, TextTypes type)
    {

        Match matchedObject;
        int group = 0;

        // UTF-8にする
        string utfText = Regex.Unescape(comText);
        Debug.Log(utfText);
        // 初期化
        matchedObject = Regex.Match(utfText, "");

        switch (type)
        {
            case TextTypes.TalkAPI:
                // TalkAPIの正規表現
                matchedObject = Regex.Match(utfText, "reply\":(.*\").*");
                group = 1;
                FinalText = matchedObject.Groups[group].Value.Replace("\"", "").Trim();
                break;
            case TextTypes.DocomoAPI:
                // docomoAPIの正規表現
                matchedObject = Regex.Match(utfText, "utt\":(.*\").*(.*\"yomi).*(.*mode\":).*(\".*\").*(,\"da*).*(.*\"context\":).*(\".*\")");
                group = 1;
                FinalText = matchedObject.Groups[group].Value.Replace("\"", "").Trim();
                break;
            case TextTypes.StartText:
            case TextTypes.ExpressionText:
                FinalText = utfText;
                break;
            default:
                FinalText = "テキストタイプエラー。開発者に問い合わせてください。";
                break;
        }

        FinalText = FixMessage(FinalText);
        // 最終的なテキスト
        text.text = FinalText;
        Debug.Log(text.text);
        TextLength = FinalText.Length;
        if (type == TextTypes.DocomoAPI)
        {
            Context = matchedObject.Groups[7].Value.Replace("\"", "").Trim();
            Mode = matchedObject.Groups[4].Value.Replace("\"", "").Trim();
        }

        if (type != TextTypes.ExpressionText)
        {
            SetMotionOnText();
        }
    }

    /// <summary>
    /// 会話テキストの調整を行う。
    /// <param name="comText">会話テキスト</param>
    /// </summary>
    private string FixMessage(string comText)
    {
        if (comText.Contains("名前はゼロ"))
        {
            comText = "私の名前はUnityちゃんだよ。よろしくね！";
        }
        return comText;
    }

    /// <summary>
    /// 表情による返答を設定する。
    /// <param name="expression">表情</param>
    /// </summary>
    public void SetExpressionMessage(string expression)
    {
        SetText(GetExpressionMessage(expression), TextTypes.ExpressionText);
        StartCoroutine(gameObject.GetComponent<Connection>().ConvertTextToVoice(FinalText));
        // 触ったら会話はいったんリセット
        Context = "";
        Mode = "";
    }

    /// <summary>
    /// 表情による返答を取得する。
    /// <param name="expression">表情</param>
    /// </summary>
    private string GetExpressionMessage(string expression)
    {
        // 怒り
        if (expression.Equals("angry"))
        {
            return angryText[UnityEngine.Random.Range(0, 10)];
        }
        // 笑顔
        else if (expression.Equals("relux"))
        {
            return reluxText[UnityEngine.Random.Range(0, 10)];
        }
        Debug.Log("error!");
        return "";
    }

    /// <summary>
    /// 表情テキストを設定する。
    /// </summary>
    private void ExpressionText()
    {
        // 身体触ったテキスト
        angryText[0] = "やめてよっ 変態！";
        angryText[1] = "きゃっ　今カラダ触らなかった？";
        angryText[2] = "触らないでよっ！";
        angryText[3] = "こういうのが趣味なの！？";
        angryText[4] = "次触ったら許さないから！";
        angryText[5] = "んっ！";
        angryText[6] = "殴るよ？";
        angryText[7] = "きゃっ　今カラダ触らなかった？";
        angryText[8] = "こういうのが趣味なの！？";
        angryText[9] = "んっ！";

        // 頭なでたテキスト
        reluxText[0] = "もっと頭なでてほしいな……";
        reluxText[1] = "んぅ……";
        reluxText[2] = "えへへ……";
        reluxText[3] = "うれしい……";
        reluxText[4] = "んぅ……";
        reluxText[5] = "えへへ……";
        reluxText[6] = "もっと頭なでてほしいな……";
        reluxText[7] = "うれしい……";
        reluxText[8] = "えへへ……";
        reluxText[9] = "もっと頭なでてほしいな……";
    }

    /// <summary>
    /// 始めに喋るテキストを設定する。
    /// </summary>
    private void StartText()
    {
        SetText(SetStartText(DateTime.Now), TextTypes.StartText);
        StartCoroutine(gameObject.GetComponent<Connection>().ConvertTextToVoice(FinalText));
    }

    /// <summary>
    /// 時間帯による、始めに喋るテキストを設定する。
    /// <param name="time">時刻</param>
    /// </summary>
    private string SetStartText(DateTime time)
    {
        // 時間によって変わる
        switch (time.Hour)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                return "ねむいなー……";
            case 5:
            case 6:
                return "おはようございます";
            case 7:
                return "朝ごはんは食べましたか？";
            case 8:
            case 9:
                return "おはようございます";
            case 10:
            case 11:
                return "こんにちは";
            case 12:
                return "お昼ごはんは食べましたか？";
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                return "こんにちは";
            case 18:
            case 19:
            case 20:
            case 21:
                return "こんばんは";
            case 22:
            case 23:
                return "そろそろ寝る時間ですよ？";
            default:
                return "時間に関するエラーが発生したので開発者に問い合わせてください";
        }
    }

    /// <summary>
    /// 初期化を行う。
    /// </summary>
    private void Init()
    {
        Context = "";
        Mode = "";
        FinalText = "";
        ExpressionText();
        //        startText();
    }
    /// <summary>
    ///  テキストによって、表情やモーションを変える。
    /// </summary>
    private void SetMotionOnText()
    {
        int motionNum = UnityEngine.Random.Range(0, 10);

        switch (motionNum)
        {
            case 2:
            case 6:
                motion.SetMotion("scornful", 1);
                break;
            case 8:
                motion.SetMotion("smile", 1);
                break;
            default:
                motionNum = UnityEngine.Random.Range(0, 10);

                motion.DefaultMotion();

                switch (motionNum)
                {
                    case 2:
                        motion.SetMotion("hugOfPosture", 1);
                        break;
                    case 5:
                        motion.SetMotion("raiseHand", 1);
                        break;
                    case 8:
                        motion.SetMotion("handToMouth", 1);
                        break;
                    default:
                        motion.SetMotion("default", 1);
                        break;
                }
                break;
        }
    }
}
