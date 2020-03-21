using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private string moveFloorTag = "MoveFloor";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //接地判定を返すメソッド
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            Debug.Log("isGroundEnter:" + isGroundEnter);
            Debug.Log("isGroundStay:" + isGroundStay);

            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }
        else    //EnterもStayもExitも動作しない場合、
        {
            //特に変更なし（直前のisGroundを継続）
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        //Debug.Log(isGround);
        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag || collision.tag == moveFloorTag)
        {
            //Debug.Log("collision.tag:" + collision.tag);

            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == moveFloorTag)
        {
            //Debug.Log("何かが判定に入り続けています");
            isGroundStay = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == moveFloorTag)
        {
            //Debug.Log("何かが判定をでました");
            isGroundExit = true;
        }
        
    }
}
