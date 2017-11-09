using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Unityちゃんの動きを管理するクラス
public class UniBone : MonoBehaviour
{
    // unityちゃんのボーン
    [SerializeField]
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


    // 回転するボーンを決定
    public void decideRotationBone(string boneName, string axis, float radian, float time, float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            if (bones[i].name.Equals(boneName))
            {
                rotateBone(bones[i], axis, radian, time, delay);
                return;
            }
        }
    }

    // 回転するボーンを決定（複数）
    public void decideRotationBones(string boneName, string axis, float[] radian, float time, float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            if (bones[i].name.Equals(boneName))
            {
                rotateBones(bones[i], axis, radian, time, delay);
                return;
            }
        }
    }

    // ボーンの回転を行う
    private void rotateBone(GameObject obj, string axis, float radian, float time, float delay)
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

    // 複数ボーンの回転を行う
    private void rotateBones(GameObject obj, string axis, float[] radians, float time, float delay)
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

    // デフォルトに戻す
    public void setDefaultMotion(float delay)
    {
        for (int i = 0; i < bones.Length; ++i)
        {
            // Debug.Log("mae:" + bones[i].name + ":" + vec[i]);
            // Debug.Log("mae:" + bones[i].name + ":" + unitychanBones[bones[i]]);
            // デフォルト値に回転
            rotateBones(bones[i], "xyz", new float[3] { 0, 0, 0 }, 1.5f, delay);
            // Debug.Log("usiro:" + bones[i].name + ":" + vec[i]);
            // Debug.Log("usiro:" + bones[i].name + ":" + unitychanBones[bones[i]]);
        }
    }
}
