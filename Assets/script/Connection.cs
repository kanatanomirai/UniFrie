using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.IO;

/// <summary>
/// 通信を行うクラス。
/// </summary>
public class Connection : MonoBehaviour
{

    private InputText it;
    private GameObject itObj;
    // デバッグ用テキスト
    private Text debugText;

    void Start()
    {
        itObj = GameObject.Find("UnitychanText");
        it = itObj.GetComponent<InputText>();
        debugText = GameObject.FindWithTag("DebugText").GetComponent<Text>();
    }

    /// <summary>
    /// APIとの通信を開始する。
    /// <param name="text">会話テキスト</param>
    /// </summary>
    public IEnumerator ConnectionStart(string text)
    {
        WWWForm form = new WWWForm();
        if (Parameter.type == InputText.TextTypes.TalkAPI)
        {
            // TalkAPIの送信ステータス
            form.AddField("apikey", Parameter.talkAPIkey);
            form.AddField("query", text);
            UnityWebRequest request = UnityWebRequest.Post("https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk", form);

            // リクエスト送信(TalkAPI)
            yield return request.Send();

            if (request.isNetworkError)
            {
                Debug.Log("error:" + request.error);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    Debug.Log("ok!");
                    it.SetText(request.downloadHandler.text, Parameter.type);
                    yield return StartCoroutine("ConvertTextToVoice", it.FinalText);
                }
                else
                {
                    Debug.Log("failed……:" + request.responseCode);
                }
            }
        }
        else if (Parameter.type == InputText.TextTypes.DocomoAPI)
        {
            // docomoAPIの送信ステータス
            string context = it.Context;
            string mode = it.Mode;

            string ourPostData = "{\"utt\":\"" + text + "\",\"context\":\"" + context + "\",\"mode\":\"" + mode + "\"}";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            // リクエストヘッダの変更
            headers.Add("Content-Type", "application/json");
            byte[] pData = System.Text.Encoding.UTF8.GetBytes(ourPostData.ToCharArray());

            WWW request = new WWW("https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue?APIKEY=" + Parameter.talkAPIkey, pData, headers);
            // リクエスト送信(docomoAPI)
            yield return request;

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.Log(request.error);
                debugText.text = request.error;
            }
            else
            {
                if (request.responseHeaders.ContainsKey("CONTENT-TYPE") &&
                    request.responseHeaders["CONTENT-TYPE"].Equals("application/json;charset=UTF-8"))
                {
                    Debug.Log("ok!");
                    // 返答を設定
                    it.SetText(request.text, Parameter.type);
                    yield return StartCoroutine("ConvertTextToVoice", it.FinalText);
                }
                else
                {
                    if (request.responseHeaders.Count > 0)
                    {
                        Debug.Log("failed……");

                        foreach (KeyValuePair<string, string> entry in request.responseHeaders)
                        {
                            Debug.Log(entry.Value + "=" + entry.Key);
                            debugText.text += entry.Value + "=" + entry.Key + "\n";
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("TEXT TYPE ERROR!");
        }
    }

    /// <summary>
    /// docomo音声合成APIでテキストを音声データに変換する。
    /// <param name="text">会話テキスト</param>
    /// </summary>
    public IEnumerator ConvertTextToVoice(string text)
    {
        string url = "https://api.apigw.smt.docomo.ne.jp/aiTalk/v1/textToSpeech?APIKEY=" + Parameter.compositionAPIkey;

        Dictionary<string, string> aiTalksParams = new Dictionary<string, string>();

        gameObject.AddComponent<Voice>();
        var postData = gameObject.GetComponent<Voice>().CreateSSML(text, aiTalksParams);
        var data = System.Text.Encoding.UTF8.GetBytes(postData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "application/ssml+xml";
        headers["Accept"] = "audio/L16";
        WWW www = new WWW(url, data, headers);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
            yield break;
        }

        AudioClip audioClip = gameObject.GetComponent<Voice>().CreateAudioClip(www.bytes, "test.wav");

        StartCoroutine(gameObject.GetComponent<Voice>().Play(audioClip, www.bytes.Length / 2));
    }
}
