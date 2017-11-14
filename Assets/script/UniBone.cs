using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unityちゃんの動きを管理するクラス。
/// </summary>

public class UniBone : MonoBehaviour
{
    [SerializeField]
    // unityちゃんのボーン
    private GameObject[] bones;

    private Vector3[] vec;

    private Dictionary<GameObject, Vector3> unitychanBones = new Dictionary<GameObject, Vector3>();

    void Awake()
    {
        vec = new Vector3[bones.Length];
        // ボーンとそのデフォルト角度を設定
        for (int i = 0; i < bones.Length; ++i)
        {
            vec[i] = bones[i].transform.localEulerAngles;
            unitychanBones.Add(bones[i], vec[i]);
            //            Debug.Log("awake:" + bones[i].name + ":" + vec[i]);
        }
    }


    /// <summary>
    /// 回転するボーンを決定する。
    /// <param name="boneName">ボーン名</param>
    /// <param name="axis">ボーンの軸</param>
    /// <param name="radian">ボーンを動かす角度</param>
    /// <param name="time">ボーンを動かす時間</param>
    /// <param name="delay">ボーンを動かす待ち時間</param>
    /// </summary>
    public void DecideRotationBone(string boneName, string axis, float radian, float time, float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            if (bones[i].name.Equals(boneName))
            {
                RotateBone(bones[i], axis, radian, time, delay);
                return;
            }
        }
    }

    /// <summary>
    /// 回転するボーンを決定する。（複数）
    /// <param name="boneName">ボーン名</param>
    /// <param name="axis">ボーンの軸</param>
    /// <param name="radian">ボーンを動かす角度</param>
    /// <param name="time">ボーンを動かす時間</param>
    /// <param name="delay">ボーンを動かす待ち時間</param>
    /// </summary>
    public void DecideRotationBones(string boneName, string axis, float[] radian, float time, float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            if (bones[i].name.Equals(boneName))
            {
                RotateBones(bones[i], axis, radian, time, delay);
                return;
            }
        }
    }

    /// <summary>
    /// ボーンの回転を行う。
    /// <param name="boneName">ボーン名</param>
    /// <param name="axis">ボーンの軸</param>
    /// <param name="radian">ボーンを動かす角度</param>
    /// <param name="time">ボーンを動かす時間</param>
    /// <param name="delay">ボーンを動かす待ち時間</param>
    /// </summary>
    private void RotateBone(GameObject obj, string axis, float radian, float time, float delay)
    {
        if (axis.Equals("x"))
        {
            iTween.RotateTo(obj, iTween.Hash("x", unitychanBones[obj].x + radian, "islocal", true, "time", time, "delay", delay));
        }
        else if (axis.Equals("y"))
        {
            iTween.RotateTo(obj, iTween.Hash("y", unitychanBones[obj].y + radian, "islocal", true, "time", time, "delay", delay));
        }
        else if (axis.Equals("z"))
        {
            iTween.RotateTo(obj, iTween.Hash("z", unitychanBones[obj].z + radian, "islocal", true, "time", time, "delay", delay));
        }
        else
        {
            Debug.Log("axis ERROR!");
        }
    }

    /// <summary>
    /// 複数ボーンの回転を行う。
    /// <param name="boneName">ボーン名</param>
    /// <param name="axis">ボーンの軸</param>
    /// <param name="radian">ボーンを動かす角度</param>
    /// <param name="time">ボーンを動かす時間</param>
    /// <param name="delay">ボーンを動かす待ち時間</param>
    /// </summary>
    private void RotateBones(GameObject obj, string axis, float[] radians, float time, float delay)
    {
        switch (axis)
        {
            case "xy":
                iTween.RotateTo(obj, iTween.Hash("x", unitychanBones[obj].x + radians[0], "y", unitychanBones[obj].y + radians[1], "islocal", true, "time", time, "delay", delay));
                break;
            case "xz":
                iTween.RotateTo(obj, iTween.Hash("x", unitychanBones[obj].x + radians[0], "z", unitychanBones[obj].z + radians[1], "islocal", true, "time", time, "delay", delay));
                break;
            case "yz":
                iTween.RotateTo(obj, iTween.Hash("y", unitychanBones[obj].y + radians[0], "z", unitychanBones[obj].z + radians[1], "islocal", true, "time", time, "delay", delay));
                break;
            case "xyz":
                iTween.RotateTo(obj, iTween.Hash("x", unitychanBones[obj].x + radians[0], "y", unitychanBones[obj].y + radians[1], "z", unitychanBones[obj].z + radians[2], "islocal", true, "time", time, "delay", delay));
                break;
        }
    }

    /// <summary>
    /// ボーンをデフォルトに戻す。
    /// <param name="delay">ボーンを動かす待ち時間</param>
    /// </summary>
    public void SetDefaultMotion(float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            // Debug.Log("mae:" + bones[i].name + ":" + vec[i]);
            // Debug.Log("mae:" + bones[i].name + ":" + unitychanBones[bones[i]]);
            // デフォルト値に回転
            RotateBones(bones[i], "xyz", new float[3] { 0, 0, 0 }, 1.5f, delay);
            // Debug.Log("usiro:" + bones[i].name + ":" + vec[i]);
            // Debug.Log("usiro:" + bones[i].name + ":" + unitychanBones[bones[i]]);
        }
    }
}
