using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Unityちゃんの表情を管理するクラス
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

    // 目の動き
    public void moveEye(float endWeight, int coefficient, string blendShapeName)
    {
        float currentWeight = smr_eye.GetBlendShapeWeight(smr_eye.sharedMesh.GetBlendShapeIndex(blendShapeName));
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
            currentWeight = updateCurrentWeight(f, addFlag, currentWeight);
            smr_eye.SetBlendShapeWeight(smr_eye.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    // まつ毛の動き
    public void moveEl(float endWeight, int coefficient, string blendShapeName)
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
            currentWeight = updateCurrentWeight(f, addFlag, currentWeight);
            // 更新したブレンドシェイプウェイトを設定
            smr_el.SetBlendShapeWeight(smr_el.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    // 眉の動き
    public void moveBlw(float endWeight, int coefficient, string blendShapeName)
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
            currentWeight = updateCurrentWeight(f, addFlag, currentWeight);
            smr_blw.SetBlendShapeWeight(smr_blw.sharedMesh.GetBlendShapeIndex(blendShapeName), currentWeight);
        }
    }

    // ブレンドシェイプのウェイト更新
    private float updateCurrentWeight(float f, bool addFlag, float currentWeight)
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

    // デフォルトの表情にする
    public void setDefaultFace()
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
