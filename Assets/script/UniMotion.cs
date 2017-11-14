using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unityちゃんのモーションを設定するクラス。
/// </summary>
public class UniMotion : MonoBehaviour
{
    // 制御用時間
    private float timeLeft = 25.0f;

    private string motionName = "";
    private float smoothness = 1.0f;

    // まばたきする間隔
    private float interval;
    // 目を閉じてから開けるまでの間隔調整
    private float whileBlink = 0.1f;

    private GameObject unitychanObj;

    void Awake()
    {
        gameObject.AddComponent<UniExpression>();
        gameObject.AddComponent<UniBone>();
        unitychanObj = GameObject.FindWithTag("Unitychan");
    }

    void FixedUpdate()
    {
        if (Parameter.isAppearing)
        {
            if (timeLeft == 25.0f)
            {
                StartCoroutine("Blink");
            }
            // Unityちゃんの回転
            unitychanObj.transform.rotation = Quaternion.identity;
            unitychanObj.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.up);

            //            Debug.Log("angles:" + unitychanObj.transform.localEulerAngles);

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0)
            {
                timeLeft = 20.0f;
                DefaultFace();
                DefaultMotion();
                SetStandbyMotion();
            }

            if (Parameter.isTalking)
            {
                timeLeft = 10.0f;
            }

            switch (motionName)
            {
                case "smile":
                    SmileFace();
                    break;
                case "female":
                    FemaleFace();
                    break;
                case "angry":
                    AngryFace();
                    break;
                case "scornful":
                    ScornfulFace();
                    break;
                case "default":
                    DefaultFace();
                    break;
                case "raiseHand":
                    RaiseHand();
                    break;
                case "handToMouth":
                    HandToMouth();
                    break;
                case "hugOfPosture":
                    HugOfPosture();
                    break;
                default:
                    break;
            }

            --smoothness;
            if (smoothness <= 0.0)
            {
                motionName = "";
            }
        }
    }

    /// <summary>
    /// テレ顔にする。
    /// </summary>
    private void FemaleFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().MoveBlw(20, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().MoveBlw(90, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().MoveEye(25, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().MoveEl(75, 1, "blendShape2.EYE_SMILE1");
    }

    /// <summary>
    /// デフォルトモーションにする。
    /// </summary>
    public void DefaultMotion()
    {
        Parameter.isMoving = false;
        gameObject.GetComponent<UniBone>().SetDefaultMotion(0);
    }

    /// <summary>
    /// 笑顔にする。
    /// </summary>
    private void SmileFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().MoveEye(100, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().MoveEl(100, 1, "blendShape2.EYE_SMILE1");
    }

    /// <summary>
    /// 怒り顔にする。
    /// </summary>
    private void AngryFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().MoveBlw(100, 1, "blendShape3.BLW_ANG1");
        gameObject.GetComponent<UniExpression>().MoveBlw(50, 1, "blendShape3.BLW_ANG2");
        gameObject.GetComponent<UniExpression>().MoveEye(100, 1, "blendShape2.EYE_ANG1");
        gameObject.GetComponent<UniExpression>().MoveEl(100, 1, "blendShape2.EYE_ANG1");
    }

    /// <summary>
    /// ジト顔にする。
    /// </summary>
    private void ScornfulFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().MoveBlw(100, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().MoveEye(35, 1, "blendShape2.EYE_DEF_C");
        gameObject.GetComponent<UniExpression>().MoveEl(100, 1, "blendShape2.EYE_SMILE2");
        gameObject.GetComponent<UniExpression>().MoveEl(100, 1, "blendShape2.EYE_ANG1");
        gameObject.GetComponent<UniExpression>().MoveEl(35, 1, "blendShape2.EYE_DEF_C");
    }

    /// <summary>
    /// まばたきをする。
    /// </summary>
    public IEnumerator Blink()
    {
        while (true)
        {
            if (Parameter.isAppearing && !Parameter.isExpressChanging)
            {
                interval = 3.0f + UnityEngine.Random.Range(-1.0f, 1.0f);
                gameObject.GetComponent<UniExpression>().MoveEye(85, 5, "blendShape2.EYE_DEF_C");
                gameObject.GetComponent<UniExpression>().MoveEl(85, 5, "blendShape2.EYE_DEF_C");
                yield return new WaitForSeconds(whileBlink);
                gameObject.GetComponent<UniExpression>().MoveEye(0, 5, "blendShape2.EYE_DEF_C");
                gameObject.GetComponent<UniExpression>().MoveEl(0, 5, "blendShape2.EYE_DEF_C");
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// デフォルトの顔にする。
    /// </summary>
    private void DefaultFace()
    {
        Parameter.isExpressChanging = false;
        gameObject.GetComponent<UniExpression>().SetDefaultFace();
    }

    /// <summary>
    /// モーション設定を行う。
    /// </summary>
    public void SetMotion(string motionName, float smoothness)
    {
        this.motionName = motionName;
        this.smoothness = smoothness;
        gameObject.GetComponent<UniExpression>().SetDefaultFace();
    }

    /// <summary>
    /// 見渡すモーションをする。
    /// </summary>
    private void Overlooking()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_Spine", "x", -12, 2.0f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_Spine", "x", 12, 3.0f, 3);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_Spine", "x", 0, 2.0f, 9);
    }

    /// <summary>
    /// 手を前に出すモーションをする。
    /// </summary>
    private void HugOfPosture()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_RightHand", "x", 50, 1.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_RightArm", "xz", new float[2] { 10, -65 }, 1.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_LeftHand", "x", -50, 1.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_LeftArm", "xz", new float[2] { -10, -65 }, 1.5f, 0);
    }

    /// <summary>
    /// 手をあげるモーションをする。
    /// </summary>
    private void RaiseHand()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_RightArm", "yz", new float[2] { 120, -30 }, 1.0f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_RightHand", "x", 80, 1.0f, 0);
        SmileFace();
    }

    /// <summary>
    /// 手を口にあてるモーションをする。
    /// </summary>
    private void HandToMouth()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_RightHand", "x", 90, 1.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_RightArm", "yz", new float[2] { -8, -47 }, 1.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_RightForeArm", "xz", new float[2] { -20, -155 }, 1.5f, 0);
        SmileFace();
    }

    /// <summary>
    /// 手を組んで伸びをするモーションをする。
    /// </summary>
    private IEnumerator Streach()
    {
        DefaultFace();
        Parameter.isMoving = true;
        Parameter.isExpressChanging = true;
        yield return new WaitForSeconds(0.05f);

        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_LeftArm", "yz", new float[2] { 20, -155 }, 2.0f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBones("Character1_RightArm", "yz", new float[2] { -20, -155 }, 2.0f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_LeftHand", "y", 70, 2.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_RightHand", "y", -70, 2.5f, 0);
        gameObject.GetComponent<UniBone>().DecideRotationBone("Character1_Spine", "z", 10, 2.5f, 0);
        gameObject.GetComponent<UniBone>().SetDefaultMotion(6);
        StreachFace();
        yield return new WaitForSeconds(6.5f);
        DefaultFace();
    }

    /// <summary>
    /// 手を組んで伸びをするモーション用の顔にする。
    /// </summary>
    private void StreachFace()
    {
        gameObject.GetComponent<UniExpression>().MoveBlw(75, 1, "blendShape3.BLW_ANG1");
        gameObject.GetComponent<UniExpression>().MoveEye(80, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().MoveEye(100, 1, "blendShape2.EYE_SMILE2");
        gameObject.GetComponent<UniExpression>().MoveEye(10, 1, "blendShape2.EYE_DEF_C");
        gameObject.GetComponent<UniExpression>().MoveEl(40, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().MoveEl(35, 1, "blendShape2.EYE_DEF_C");
    }

    /// <summary>
    /// 待機モーションを設定する。
    /// </summary>
    private void SetStandbyMotion()
    {
        int motionNum = UnityEngine.Random.Range(0, 10);

        if (motionNum >= 5)
        {
            Overlooking();
        }
        else
        {
            StartCoroutine("Streach");
        }
    }
}
