using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unityちゃんの表情を管理するクラス。
/// </summary>
public class UniExpression : MonoBehaviour
{

    private GameObject obj;
    // 眉のブレンドシェイプ
    private SkinnedMeshRenderer smr_blw;
    // 目のブレンドシェイプ
    private SkinnedMeshRenderer smr_eye;
    // まつ毛のブレンドシェイプ
    private SkinnedMeshRenderer smr_el;

    void Awake()
    {
        obj = GameObject.Find("EYE_DEF");
        smr_eye = obj.GetComponent<SkinnedMeshRenderer>();
        obj = GameObject.Find("EL_DEF");
        smr_el = obj.GetComponent<SkinnedMeshRenderer>();
        obj = GameObject.Find("BLW_DEF");
        smr_blw = obj.GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// 目を動かす。
    /// <param name="weight">目のシェイプのウェイト</param>
    /// <param name="coefficient">係数</param>
    /// <param name="blendShapeName">シェイプ名</param>
    /// </summary>
    public void MoveEye(float weight, int coefficient, string blendShapeName)
    {
        float currentWeight = smr_eye.GetBlendShapeWeight(smr_eye.sharedMesh.GetBlendShapeIndex(blendShapeName));
        float f;
        bool addFlag;

        if (coefficient == 0)
        {
            Debug.Log("Divide-by-zero!!!");
            return;
        }

        if (currentWeight < weight)
        {
            f = weight / coefficient;
            addFlag = true;
        }
        else
        {
            f = currentWeight / coefficient;
            addFlag = false;
        }

        for (int i = 0; i < coefficient; ++i)
        {
            currentWeight = UpdateCurrentWeight(f, addFlag, currentWeight);
            smr_eye.SetBlendShapeWeight(smr_eye.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    /// <summary>
    /// まつ毛を動かす。
    /// <param name="weight">まつ毛のシェイプのウェイト</param>
    /// <param name="coefficient">係数</param>
    /// <param name="blendShapeName">シェイプ名</param>
    /// </summary>
    public void MoveEl(float endWeight, int coefficient, string blendShapeName)
    {
        // 現在のブレンドシェイプウェイトを取得
        float currentWeight = smr_el.GetBlendShapeWeight(smr_el.sharedMesh.GetBlendShapeIndex(blendShapeName));
        float f;
        bool addFlag;

        if (coefficient == 0)
        {
            Debug.Log("Divide-by-zero!!!");
            return;
        }

        if (currentWeight < endWeight)
        {
            f = endWeight / coefficient;
            addFlag = true;
        }
        else
        {
            f = currentWeight / coefficient;
            addFlag = false;
        }

        for (int i = 0; i < coefficient; ++i)
        {
            // 現在のブレンドシェイプウェイトを更新
            currentWeight = UpdateCurrentWeight(f, addFlag, currentWeight);
            // 更新したブレンドシェイプウェイトを設定
            smr_el.SetBlendShapeWeight(smr_el.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    /// <summary>
    /// 眉を動かす。
    /// <param name="weight">眉のシェイプのウェイト</param>
    /// <param name="coefficient">係数</param>
    /// <param name="blendShapeName">シェイプ名</param>
    /// </summary>
    public void MoveBlw(float endWeight, int coefficient, string blendShapeName)
    {
        float currentWeight = smr_blw.GetBlendShapeWeight(smr_blw.sharedMesh.GetBlendShapeIndex(blendShapeName));
        float f;
        bool addFlag;

        if (coefficient == 0)
        {
            Debug.Log("Divide-by-zero!!!");
            return;
        }

        if (currentWeight < endWeight)
        {
            f = endWeight / coefficient;
            addFlag = true;
        }
        else
        {
            f = currentWeight / coefficient;
            addFlag = false;
        }

        for (int i = 0; i < coefficient; ++i)
        {
            currentWeight = UpdateCurrentWeight(f, addFlag, currentWeight);
            smr_blw.SetBlendShapeWeight(smr_blw.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    /// <summary>
    /// シェイプのウェイトを更新する。
    /// <param name="weight">更新量</param>
    /// <param name="addFlag">加算するか減算するかのフラグ</param>
    /// <param name="currentWeight">現在のウェイト</param>
    /// </summary>
    private float UpdateCurrentWeight(float f, bool addFlag, float currentWeight)
    {
        if (addFlag)
        {
            currentWeight += f;
        }
        else
        {
            currentWeight -= f;
        }

        return currentWeight;
    }

    /// <summary>
    /// デフォルトの表情にする。
    /// </summary>
    public void SetDefaultFace()
    {
        for (int i = 0; i < smr_eye.sharedMesh.blendShapeCount; ++i)
        {
            smr_eye.SetBlendShapeWeight(i, 0);
        }
        for (int i = 0; i < smr_el.sharedMesh.blendShapeCount; ++i)
        {
            smr_el.SetBlendShapeWeight(i, 0);
        }
        for (int i = 0; i < smr_blw.sharedMesh.blendShapeCount; ++i)
        {
            smr_blw.SetBlendShapeWeight(i, 0);
        }
    }
}
