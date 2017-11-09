using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Unityちゃんにタッチしたときの挙動設定クラス
public class UniTouch : MonoBehaviour
{

    private InputText it;
    private GameObject obj;

    void Start()
    {
        gameObject.AddComponent<UniMotion>();
        obj = GameObject.Find("UnitychanText");
        it = obj.GetComponent<InputText>();
    }

    // Unityちゃんにタッチ
    public void OnMouseDown()
    {
        // 初期化
        string expression = "";
        // オブジェクトがcanvasの上にある場合は反応させない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // タッチした箇所で分岐
        if (transform.tag.Equals("Unitychan_body"))
        {
            expression = "angry";
            // 怒り顔をさせる
            gameObject.GetComponent<UniMotion>().setMotion("angry",1);
        }
        else if (transform.tag.Equals("Unitychan_hair"))
        {
            
            expression = "relux";
            // いい顔をさせる
            gameObject.GetComponent<UniMotion>().setMotion("female",1);
        }
        // 返答を設定
        it.setExpressionMessage(expression);
        
    }
}
