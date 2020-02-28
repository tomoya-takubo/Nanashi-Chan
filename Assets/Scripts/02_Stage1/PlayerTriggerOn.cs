using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerOn : MonoBehaviour
{
    [Header("プレイヤーの下から何％が範囲内に入って入ればOKとみなすか")]
    public float playerOnRate;

    private string playerTag = "Player";
    private BoxCollider2D col;
    private bool isOn = false;
    private bool callFixed = false;
    private bool isEnter, isStay, isExit;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        if(col == null)
        {
            Debug.Log("ボックスコライダーがついていません");
        }
    }

    /// <summary>
    /// プレイヤーが上に乗っているかどうか
    /// </summary>
    /// <returns></returns>
    public bool isPlayerOn()
    {
        if(isEnter || isStay)
        {
            isOn = true;
        }
        else if(isExit)
        {
            isOn = false;
        }

        return isOn;
    }

    void FixedUpdate()
    {
        callFixed = true;
    }

    private void LateUpdate()
    {
        if (callFixed)
        {
            //フラグを元に戻す
            isEnter = false;
            isStay = false;
            isExit = false;
            callFixed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isEnter = JudgePlayerOn(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isStay = JudgePlayerOn(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isExit = true;
        }
    }

    private bool JudgePlayerOn(Collider2D collision)
    {
        //踏みつけ判定になる高さ
        float stepOnHeight = (collision.bounds.size.y * (playerOnRate / 100f));
        //踏みつけ判定のワールド座標
        float judgePos = collision.transform.position.y - (collision.bounds.size.y / 2f) + stepOnHeight;

        //プレイヤーの下から指定した％分上の範囲内にいるとき乗っているとみなします
        if(judgePos > transform.position.y - (col.size.y / 2))
        {
            return true;
        }

        return false;
    }
}
