﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リップシンククラス。
/// </summary>
public class Lipsync : MonoBehaviour
{
    private GameObject obj;
    // 口のブレンドシェイプ
    private SkinnedMeshRenderer smr_mth;
    //ブレンドシェイプウェイト（初期値-1）
    int weight = -1;
    // 口パクの速さ
    int lipsyncSpeed = 7;

    void Start()
    {
        obj = GameObject.Find("MTH_DEF");
        smr_mth = obj.GetComponent<SkinnedMeshRenderer>();
    }

    void FixedUpdate()
    {
        if (Parameter.isTalking || weight > 0)
        {
            // リップシンク
            DoLipSync();
        }
    }

    /// <summary>
    /// リップシンク（口パク）を行う。
    /// </summary>
    private void DoLipSync()
    {
        if (weight < 0)
        {
            weight = 100;
        }

        smr_mth.SetBlendShapeWeight(smr_mth.sharedMesh.GetBlendShapeIndex("blendShape1.MTH_A"), weight);
        weight -= lipsyncSpeed;
    }
}
