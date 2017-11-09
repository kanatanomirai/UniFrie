using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 音声認識通信クラス
public class RecConnection : MonoBehaviour
{
    public IEnumerator StartRecognition(byte[] voiceData)
    {
        Debug.Log("recognition!");
        byte[] boundary = UnityWebRequest.GenerateBoundary();
        // 最初〜音声データの前までのデータ
        byte[] preVoiceData;
        // 最後の締めのバウンダリ文字列
        byte[] lastBoundary;
        // リクエスト
        string requestData = "";
        int typeSize = System.Runtime.InteropServices.Marshal.SizeOf(voiceData.GetType().GetElementType());

        string boundaryStr = System.Text.Encoding.ASCII.GetString(boundary);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "multipart/form-data; boundary=" + boundaryStr;

        requestData += "--" + boundaryStr + "\r\n";
        requestData += "Content-Disposition: form-data; name=\"v\"\r\n\r\n" + "on\r\n";

        requestData += "--" + boundaryStr + "\r\n";
        requestData += "Content-Disposition: form-data; name=\"a\"; filename=\"test.wav\"\r\n";
        requestData += "Content-Type: application/octet-stream\r\n\r\n";

        preVoiceData = System.Text.Encoding.ASCII.GetBytes(requestData);

        lastBoundary = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundaryStr + "--\r\n");

        // byte配列に音声データを代入
        byte[] data = new byte[preVoiceData.Length + voiceData.Length + lastBoundary.Length];
        System.Buffer.BlockCopy(preVoiceData, 0, data, 0, preVoiceData.Length * typeSize);
        System.Buffer.BlockCopy(voiceData, 0, data, preVoiceData.Length * typeSize, voiceData.Length * typeSize);
        System.Buffer.BlockCopy(lastBoundary, 0, data, preVoiceData.Length * typeSize + voiceData.Length * typeSize, lastBoundary.Length * typeSize);

        gameObject.AddComponent<RecognitionResult>();
        // 音声認識APIに音声データを送信
        WWW request = new WWW("https://api.apigw.smt.docomo.ne.jp/amiVoice/v1/recognize?APIKEY=" + Parameter.recognizeAPIkey, data, headers);
        yield return request;
        Debug.Log(System.Text.RegularExpressions.Regex.Unescape(request.text));

        string resultRecText = gameObject.GetComponent<RecognitionResult>().getRecognitionText(System.Text.RegularExpressions.Regex.Unescape(request.text));
        Connection c = GameObject.Find("UnitychanText").GetComponent<Connection>();
        StartCoroutine(c.ConnectionStart(resultRecText));
    }
}
