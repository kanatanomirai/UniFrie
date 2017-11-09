using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.IO;

// 通信クラス
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

    //通信開始
    public IEnumerator ConnectionStart(string str)
    {
        WWWForm form = new WWWForm();
        if (Parameter.type == InputText.TextTypes.TalkAPI)
        {
            // TalkAPIの送信ステータス
            form.AddField("apikey", Parameter.talkAPIkey);
            form.AddField("query", str);
            UnityWebRequest request = UnityWebRequest.Post("https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk", form);

            // リクエスト送信(TalkAPI)
            yield return request.Send();

            if (request.isError)
            {
                Debug.Log("error:" + request.error);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    Debug.Log("ok!");
                    it.setText(request.downloadHandler.text, Parameter.type);
                    yield return StartCoroutine("convertTextToVoice", it.FinalText);
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

            string ourPostData = "{\"utt\":\"" + str + "\",\"context\":\"" + context + "\",\"mode\":\"" + mode + "\"}";

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
                    it.setText(request.text, Parameter.type);
                    yield return StartCoroutine("convertTextToVoice", it.FinalText);
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

    // docomo音声合成API
    public IEnumerator convertTextToVoice(string text)
    {
        string url = "https://api.apigw.smt.docomo.ne.jp/aiTalk/v1/textToSpeech?APIKEY=" + Parameter.compositionAPIkey;

        Dictionary<string, string> aiTalksParams = new Dictionary<string, string>();
  
        gameObject.AddComponent<Voice>();
        var postData = gameObject.GetComponent<Voice>().createSSML(text, aiTalksParams);
        var data = System.Text.Encoding.UTF8.GetBytes(postData);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "application/ssml+xml";
        headers["Accept"] = "audio/L16";
        headers["Content-Length"] = data.Length.ToString();
        WWW www = new WWW(url, data, headers);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
            yield break;
        }

        AudioClip audioClip = gameObject.GetComponent<Voice>().createAudioClip(www.bytes, "test.wav");

        StartCoroutine(gameObject.GetComponent<Voice>().Play(audioClip, www.bytes.Length / 2));
    }
}
