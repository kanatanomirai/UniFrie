// This code was created with reference to http://posposi.blog.fc2.com/blog-entry-245.html .

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 音声クラス
public class Voice : MonoBehaviour
{

    // constant
    public readonly float RANGE_VALUE_BIT_8 = 1.0f / Mathf.Pow(2, 7);   // 1 / 128
    public readonly float RANGE_VALUE_BIT_16 = 1.0f / Mathf.Pow(2, 15); // 1 / 32768
    public const int BIT_8 = 8;
    public const int BIT_16 = 16;

    // WebGL用WAVファイルのサンプル数
    private int webGLVoiceSamples = 0;
    // 音の長さ
    private float voiceLength = 0.0f;

    public int Samples
    {
        set
        {
            this.webGLVoiceSamples = value;
        }
        get
        {
            return this.webGLVoiceSamples;
        }
    }

    public float VoiceLength
    {
        set
        {
            this.voiceLength = value;
        }
        get
        {
            return this.voiceLength;
        }
    }

    public string createSSML(string text, Dictionary<string, string> dic)
    {
        return "<?xml version=\"1.0\" encoding=\"utf-8\" ?><speak version=\"1.1\"><voice name=\"maki\"><prosody pitch=\"1.5\" rate=\"0.85\">" + text + " </prosody></voice></speak>";
    }

    private byte[] convertBytesEndian(byte[] bytes)
    {
        byte[] newBytes = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i += 2)
        {
            newBytes[i] = bytes[i + 1];
            newBytes[i + 1] = bytes[i];
        }
        // 44byte付加したnewBytes
        newBytes = addWAVHeader(newBytes);
        return newBytes;
    }

    private byte[] addWAVHeader(byte[] bytes)
    {
        byte[] header = new byte[44];
        // サンプリングレート
        long longSampleRate = 16000;
        // チャンネル数
        int channels = 1;
        int bits = 16;
        // データ速度
        long byteRate = longSampleRate * (bits / 8) * channels;
        long dataLength = bytes.Length;
        long totalDataLen = dataLength + 36;
        // 最終的なWAVファイルのバイナリ
        byte[] finalWAVBytes = new byte[bytes.Length + header.Length];
        int typeSize = System.Runtime.InteropServices.Marshal.SizeOf(bytes.GetType().GetElementType());

        header[0] = convertByte("R");
        header[1] = convertByte("I");
        header[2] = convertByte("F");
        header[3] = convertByte("F");
        header[4] = (byte)(totalDataLen & 0xff);
        header[5] = (byte)((totalDataLen >> 8) & 0xff);
        header[6] = (byte)((totalDataLen >> 16) & 0xff);
        header[7] = (byte)((totalDataLen >> 24) & 0xff);
        header[8] = convertByte("W");
        header[9] = convertByte("A");
        header[10] = convertByte("V");
        header[11] = convertByte("E");
        header[12] = convertByte("f");
        header[13] = convertByte("m");
        header[14] = convertByte("t");
        header[15] = convertByte(" ");
        header[16] = 16;
        header[17] = 0;
        header[18] = 0;
        header[19] = 0;
        header[20] = 1;
        header[21] = 0;
        header[22] = (byte)channels;
        header[23] = 0;
        header[24] = (byte)(longSampleRate & 0xff);
        header[25] = (byte)((longSampleRate >> 8) & 0xff);
        header[26] = (byte)((longSampleRate >> 16) & 0xff);
        header[27] = (byte)((longSampleRate >> 24) & 0xff);
        header[28] = (byte)(byteRate & 0xff);
        header[29] = (byte)((byteRate >> 8) & 0xff);
        header[30] = (byte)((byteRate >> 16) & 0xff);
        header[31] = (byte)((byteRate >> 24) & 0xff);
        header[32] = (byte)((bits / 8) * channels);
        header[33] = 0;
        header[34] = (byte)bits;
        header[35] = 0;
        header[36] = convertByte("d");
        header[37] = convertByte("a");
        header[38] = convertByte("t");
        header[39] = convertByte("a");
        header[40] = (byte)(dataLength & 0xff);
        header[41] = (byte)((dataLength >> 8) & 0xff);
        header[42] = (byte)((dataLength >> 16) & 0xff);
        header[43] = (byte)((dataLength >> 24) & 0xff);

        System.Buffer.BlockCopy(header, 0, finalWAVBytes, 0, header.Length * typeSize);
        System.Buffer.BlockCopy(bytes, 0, finalWAVBytes, header.Length * typeSize, bytes.Length * typeSize);

        return finalWAVBytes;
    }

    private byte convertByte(string str)
    {
        return System.Text.Encoding.UTF8.GetBytes(str)[0];
    }

    public AudioClip createAudioClip(byte[] bytes, string name)
    {
        byte[] wavBytes = new byte[bytes.Length + 44];
        wavBytes = convertBytesEndian(bytes);
        Samples = bytes.Length / 2;
        return Create(name, wavBytes, 44, 16, Samples, 1, 16000, false, false);
    }

    //---------------------------------------------------------------------------
    // create AudioClip by binary raw data
    //---------------------------------------------------------------------------
    // name				: 名前
    // raw_data			: バイナリデータ
    // wav_buf_idx		: WAVデータのindex 44
    // bit_per_sample 	: サンプルあたりのビット数 (bit/sample) 16
    // samples			: サンプル(波形データ/2)
    // channels			: チャンネル数 1
    // frequency		: サンプリングレート 16000
    private AudioClip Create(
        string name,
        byte[] raw_data,
        int wav_buf_idx,
        int bit_per_sample,
        int samples,
        int channels,
        int frequency,
        bool is3D,
        bool isStream
    )
    {
        // convert to ranged_raw_data from byte_raw_data
        float[] ranged_raw_data = CreateRangedRawData(raw_data, wav_buf_idx, samples, channels, bit_per_sample);

        // create clip and set
        return Create(name, ranged_raw_data, samples, channels, frequency, is3D, isStream);
    }

    //---------------------------------------------------------------------------
    // create AudioClip by ranged raw data
    //---------------------------------------------------------------------------
    private AudioClip Create(
        string name,
        float[] ranged_data,
        int samples,
        int channels,
        int frequency,
        bool is3D,
        bool isStream
    )
    {
        // Debug.Log("prevous Create samples=" + samples);
        // Debug.Log("prevous Create channels=" + channels);
        // Debug.Log("prevous Create frequency=" + frequency);
        AudioClip clip = AudioClip.Create(name, samples, channels, frequency, isStream);
        // Debug.Log("after Create samples=" + clip.samples);
        // Debug.Log("after channels=" + clip.channels);
        // Debug.Log("after frequency=" + clip.frequency);
        // set data to clip
        clip.SetData(ranged_data, 0);

        VoiceLength = clip.length;
//        Debug.Log("音長さ:" + VoiceLength);
        return clip;
    }

    //---------------------------------------------------------------------------
    // create rawdata( ranged 0.0 - 1.0 ) from binary wav data
    //---------------------------------------------------------------------------
    private float[] CreateRangedRawData(byte[] byte_data, int wav_buf_idx, int samples, int channels, int bit_per_sample)
    {
        float[] ranged_rawdata = new float[samples * channels];

        int step_byte = bit_per_sample / BIT_8;
        int now_idx = wav_buf_idx;

        for (int i = 0; i < (samples * channels); ++i)
        {
            ranged_rawdata[i] = convertByteToFloatData(byte_data, now_idx, bit_per_sample);

            now_idx += step_byte;
        }

        return ranged_rawdata;
    }

    //---------------------------------------------------------------------------
    // convert byte data to float data
    //---------------------------------------------------------------------------
    private float convertByteToFloatData(byte[] byte_data, int idx, int bit_per_sample)
    {
        float float_data = 0.0f;

        switch (bit_per_sample)
        {
            case BIT_8:
                {
                    float_data = ((int)byte_data[idx] - 0x80) * RANGE_VALUE_BIT_8;
                }
                break;
            case BIT_16:
                {
                    short sample_data = System.BitConverter.ToInt16(byte_data, idx);
                    float_data = sample_data * RANGE_VALUE_BIT_16;
                }
                break;
        }

        return float_data;
    }

    public IEnumerator Play(AudioClip clip, int samples)
    {
        Parameter.isMoving = false;
        Parameter.isTalking = true;

        AudioSource audio = gameObject.AddComponent<AudioSource>();
        float[] rawData = new float[samples * clip.channels];
        clip.GetData(rawData, 0);

        audio.clip = clip;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length - 0.5f);

        Parameter.isTalking = false;
    }
}
