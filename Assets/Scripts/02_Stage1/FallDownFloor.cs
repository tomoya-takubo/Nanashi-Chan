﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownFloor : MonoBehaviour
{
    #region//パブリック変数
    [Header("スプライトがあるオブジェクト")]
    public GameObject spriteObj;
    [Header("プレイヤーの判定をするスクリプト")]
    public PlayerTriggerOn trigger;
    [Header("振動幅")]
    public float vibrationWidth = 0.05f;
    [Header("振動速度")]
    public float vibrationSpeed = 30.0f;
    [Header("落ちるまでの時間")]
    public float fallTime = 1.0f;
    [Header("落ちていく速度")]
    public float fallSpeed = 10.0f;
    [Header("落ちてから戻ってくる時間")]
    public float returnTime = 5.0f;
    #endregion

    #region//プライベート変数
    private bool isOn;
    private bool isFall;
    private bool isReturn;
    private Vector3 spriteDefaultPos;
    private Vector3 floorDefaultPos;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private Vector2 fallVelocity;
    private SpriteRenderer sr;
    private float timer = 0.0f;
    private float fallingTimer = 0.0f;
    private float returnTimer = 0.0f;
    private float blinkTimer = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        if(spriteObj != null && trigger != null && col != null && rb != null)
        {
            spriteDefaultPos = spriteObj.transform.position;
            fallVelocity = new Vector2(0, -fallSpeed);
            floorDefaultPos = gameObject.transform.position;
            sr = spriteObj.GetComponent<SpriteRenderer>();

            if(sr == null)
            {
                Debug.Log("fallDownFloor インスペクターに設定し忘れがあります");
                Destroy(this);
            }
        }
        else
        {
            Debug.Log("fallDownFloor インスペクターに設定し忘れがあります");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが１回でも乗ったらフラグをオンに
        if (trigger.isPlayerOn())
        {
            isOn = true;
        }

        //プレイヤーがのってから落ちるまでの間
        if(isOn && !isFall)
        {
            //振動する
            spriteObj.transform.position = spriteDefaultPos + new Vector3(Mathf.Sin(timer * vibrationSpeed) * vibrationWidth, 0, 0);

            //一定時間たったら落ちる
            if(timer > fallTime)
            {
                isFall = true;
            }

            timer += Time.deltaTime;
        }

        //一定時間たつと点滅して戻ってくる
        if (isReturn)
        {
            //点滅　ついてるときに戻る
            if(blinkTimer > 0.2f)
            {
                sr.enabled = true;
                blinkTimer = 0.0f;
            }
            //点滅　消えているとき
            else if(blinkTimer > 0.1f)
            {
                sr.enabled = false;
            }
            //点滅　ついているとき
            else
            {
                sr.enabled = true;
            }

            //1秒経ったら点滅終わり
            if(returnTimer > 1.0f)
            {
                isReturn = false;
                blinkTimer = 0f;
                returnTimer = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTimer += Time.deltaTime;
                returnTimer += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        //落下中
        if (isFall)
        {
            rb.velocity = fallVelocity;

            //一定時間たつと元の位置に戻る
            if(fallingTimer > fallTime)
            {
                isReturn = true;
                transform.position = floorDefaultPos;
                rb.velocity = Vector2.zero;
                isFall = false;
                timer = 0.0f;
                fallingTimer = 0.0f;
            }
            else
            {
                fallingTimer += Time.deltaTime;
                isOn = false;
            }
        }
    }
}
