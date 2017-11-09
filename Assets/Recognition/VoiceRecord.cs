using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 音声録音クラス
public class VoiceRecord : MonoBehaviour
{

    private AudioSource audioSource;

    // 音声録音開始
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

    // 音声録音終了
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
