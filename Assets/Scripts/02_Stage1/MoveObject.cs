﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("移動経路")]
    public GameObject[] movePoint;
    [Header("速さ")]
    public float speed = 1.0f;

    private Rigidbody2D rb;
    private int noPoint = 0;
    private bool returnPoint = false;

    private Vector2 oldPos = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(movePoint != null && movePoint.Length > 0 && rb != null)
        {
            rb.position = movePoint[0].transform.position;
            oldPos = rb.position;
        }
    }

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

    private void FixedUpdate()
    {
        if(movePoint != null && movePoint.Length > 1 && rb != null)
        {
            //通常進行
            if (!returnPoint)
            {
                int nextPoint = noPoint + 1;

                //目標ポイントとの誤差がわずかになるまで移動
                if(Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);

                }
                //「次のポイント」を一つ進める
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    ++noPoint;

                    //現在地が配列の最後だった場合
                    if(noPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            //折り返し進行
            else
            {
                int nextPoint = noPoint - 1;

                //目標ポイントとの誤差がわずかになるまでに移動
                if(Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                //次のポイントを１つ戻す
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    --noPoint;

                    //現在地が配列の最初だった場合
                    if(noPoint <= 0)
                    {
                        returnPoint = false;

                    }
                }

            }

            myVelocity = (rb.position - oldPos) / Time.deltaTime;
            oldPos = rb.position;

        }
    }

}
