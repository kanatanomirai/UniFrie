using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音声を録音するクラス。
/// </summary>
public class VoiceRecord : MonoBehaviour
{

    private AudioSource audioSource;

    /// <summary>
    /// 音声録音を開始する。
    /// </summary>
    public void OnClickRec()
    {
        audioSource = GameObject.Find("SimpleUI").GetComponent<AudioSource>();
        Debug.Log("rec start!");
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 9, 16000);
    }

    /// <summary>
    /// 音声録音を終了する。
    /// </summary>
    public void StopRec()
    {
        audioSource = GameObject.Find("SimpleUI").GetComponent<AudioSource>();
        Debug.Log("rec end!");
        Microphone.End(Microphone.devices[0]);
        SavWav sw = new SavWav();
        byte[] voiceData = sw.GetVoiceBinaryData(audioSource.clip);
        gameObject.AddComponent<RecConnection>();
        StartCoroutine(gameObject.GetComponent<RecConnection>().StartRecognition(voiceData));
    }
}
