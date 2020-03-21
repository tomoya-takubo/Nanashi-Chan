using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    [Header("コンティニュー番号")] public int continueNum;
    [Header("音")] public AudioClip se;
    [Header("プレイヤー判定")] public PlayerTriggerOn trigger;
    [Header("スピード")] public float speed = 3.0f;
    [Header("動く幅")] public float moveDis = 3.0f;

    private bool on = false;
    private float kakudo = 0.0f;
    private Vector3 defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        if(trigger == null)
        {
            Debug.Log("インスペクターの設定が足りません");
            Destroy(this);
        }

        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがコンティニューポイントの範囲内に入った
        if(trigger.isPlayerOn() && !on)
        {
            //コンティニュー位置をゲームマネージャーに知らせる
            GManager.instance.continueNum = continueNum;
            //通過時に音を鳴らす
            GManager.instance.PlayerSE(se);
            //通貨フラグを立てる
            on = true;
        }

        //プレイヤー通貨直後からのコンティニューポイントのふるまいを記述
        if (on)
        {
            if(kakudo < 180.0f)
            {
                //sinカーブで振動させる
                transform.position = defaultPos + Vector3.up * moveDis * Mathf.Sin(kakudo * Mathf.Deg2Rad);

                //途中からちっちゃくなる
                if(kakudo > 90.0f)
                {
                    transform.localScale = Vector3.one * (1 - ((kakudo - 90.0f) / 90.0f));
                }
                kakudo += 180.0f * Time.deltaTime * speed;
            }
            else
            {
                gameObject.SetActive(false);
                on = false;

            }
        }
    }
}
