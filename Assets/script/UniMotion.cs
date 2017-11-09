using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Unityちゃんのモーションを設定するクラス
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
                StartCoroutine("blink");
            }
            // Unityちゃんの回転
            unitychanObj.transform.rotation = Quaternion.identity;
            unitychanObj.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.up);

            //            Debug.Log("angles:" + unitychanObj.transform.localEulerAngles);

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0)
            {
                timeLeft = 20.0f;
                defaultFace();
                defaultMotion();
                setStandbyMotion();
            }

            if (Parameter.isTalking)
            {
                timeLeft = 10.0f;
            }

            switch (motionName)
            {
                case "smile":
                    smileFace();
                    break;
                case "female":
                    femaleFace();
                    break;
                case "angry":
                    angryFace();
                    break;
                case "scornful":
                    scornfulFace();
                    break;
                case "default":
                    defaultFace();
                    break;
                case "raiseHand":
                    raiseHand();
                    break;
                case "handToMouth":
                    handToMouth();
                    break;
                case "hugOfPosture":
                    hugOfPosture();
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

    // 照れ顔
    private void femaleFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().moveBlw(20, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().moveBlw(90, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().moveEye(25, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().moveEl(75, 1, "blendShape2.EYE_SMILE1");
    }

    // デフォルトモーション
    public void defaultMotion()
    {
        Parameter.isMoving = false;
        gameObject.GetComponent<UniBone>().setDefaultMotion(0);
    }

    // 笑顔
    private void smileFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().moveEye(100, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().moveEl(100, 1, "blendShape2.EYE_SMILE1");
    }

    // 怒り顔
    private void angryFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().moveBlw(100, 1, "blendShape3.BLW_ANG1");
        gameObject.GetComponent<UniExpression>().moveBlw(50, 1, "blendShape3.BLW_ANG2");
        gameObject.GetComponent<UniExpression>().moveEye(100, 1, "blendShape2.EYE_ANG1");
        gameObject.GetComponent<UniExpression>().moveEl(100, 1, "blendShape2.EYE_ANG1");
    }

    // ジト顔
    private void scornfulFace()
    {
        Parameter.isExpressChanging = true;
        gameObject.GetComponent<UniExpression>().moveBlw(100, 1, "blendShape3.BLW_SMILE2");
        gameObject.GetComponent<UniExpression>().moveEye(35, 1, "blendShape2.EYE_DEF_C");
        gameObject.GetComponent<UniExpression>().moveEl(100, 1, "blendShape2.EYE_SMILE2");
        gameObject.GetComponent<UniExpression>().moveEl(100, 1, "blendShape2.EYE_ANG1");
        gameObject.GetComponent<UniExpression>().moveEl(35, 1, "blendShape2.EYE_DEF_C");
    }

    // まばたき
    public IEnumerator blink()
    {
        while (true)
        {
            if (Parameter.isAppearing && !Parameter.isExpressChanging)
            {
                interval = 3.0f + UnityEngine.Random.Range(-1.0f, 1.0f);
                gameObject.GetComponent<UniExpression>().moveEye(85, 5, "blendShape2.EYE_DEF_C");
                gameObject.GetComponent<UniExpression>().moveEl(85, 5, "blendShape2.EYE_DEF_C");
                yield return new WaitForSeconds(whileBlink);
                gameObject.GetComponent<UniExpression>().moveEye(0, 5, "blendShape2.EYE_DEF_C");
                gameObject.GetComponent<UniExpression>().moveEl(0, 5, "blendShape2.EYE_DEF_C");
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null;
            }
        }
    }

    // デフォルト顔
    private void defaultFace()
    {
        Parameter.isExpressChanging = false;
        gameObject.GetComponent<UniExpression>().setDefaultFace();
    }

    // モーションの設定
    public void setMotion(string motionName, float smoothness)
    {
        this.motionName = motionName;
        this.smoothness = smoothness;
        gameObject.GetComponent<UniExpression>().setDefaultFace();
    }

    // 見渡す
    private void overlooking()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_Spine", "x", -12, 2.0f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_Spine", "x", 12, 3.0f, 3);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_Spine", "x", 0, 2.0f, 9);
    }

    // ハグしよ？
    private void hugOfPosture()
    {
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_RightHand", "x", 50, 1.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_RightArm", "xz", new float[2]{10, -65}, 1.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_LeftHand", "x", -50, 1.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_LeftArm", "xz", new float[2]{-10, -65}, 1.5f, 0);
    }

    // 手をあげる
    private void raiseHand(){
        Parameter.isMoving = true;  
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_RightArm", "yz", new float[2]{120, -30}, 1.0f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_RightHand", "x", 80, 1.0f, 0);        
        smileFace();
    }

    // 手を口にあてる
    private void handToMouth(){
        Parameter.isMoving = true;
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_RightHand", "x", 90, 1.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_RightArm", "yz", new float[2]{-8, -47}, 1.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_RightForeArm", "xz", new float[2]{-20, -155}, 1.5f, 0);
        smileFace();                              
    }

    // 手を組んで伸び
    private IEnumerator streach(){
        defaultFace();
        Parameter.isMoving = true;
        Parameter.isExpressChanging = true;
        yield return new WaitForSeconds(0.05f);  
        
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_LeftArm", "yz", new float[2]{20, -155}, 2.0f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBones("Character1_RightArm", "yz", new float[2]{-20, -155}, 2.0f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_LeftHand", "y", 70, 2.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_RightHand", "y", -70, 2.5f, 0);
        gameObject.GetComponent<UniBone>().decideRotationBone("Character1_Spine", "z", 10, 2.5f, 0);        
        gameObject.GetComponent<UniBone>().setDefaultMotion(6);
        streachFace();
        yield return new WaitForSeconds(6.5f);
        defaultFace();
    }

    private void streachFace(){      
        gameObject.GetComponent<UniExpression>().moveBlw(75, 1, "blendShape3.BLW_ANG1");
        gameObject.GetComponent<UniExpression>().moveEye(80, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().moveEye(100, 1, "blendShape2.EYE_SMILE2");
        gameObject.GetComponent<UniExpression>().moveEye(10, 1, "blendShape2.EYE_DEF_C");
        gameObject.GetComponent<UniExpression>().moveEl(40, 1, "blendShape2.EYE_SMILE1");
        gameObject.GetComponent<UniExpression>().moveEl(35, 1, "blendShape2.EYE_DEF_C");
    }

    // 待機モーション(ランダム)
    private void setStandbyMotion()
    {
        int motionNum = UnityEngine.Random.Range(0, 10);

        if (motionNum >= 5)
        {
            overlooking();
        }
        else
        {
            StartCoroutine("streach");
        }
    }
}
