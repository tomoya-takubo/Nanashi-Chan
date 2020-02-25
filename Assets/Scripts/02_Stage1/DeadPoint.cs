using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPoint : MonoBehaviour
{
    [Header("ステージコントローラー")] public StageCtrl ctrl;

    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == playerTag)
        {
            if(GManager.instance != null && ctrl != null)
            {
                GManager.instance.SubHeratNum();

                if (!GManager.instance.isGameOver)
                {
                    ctrl.PlayerSetContinuePoint();
                }
            }
            else
            {
                Debug.Log("設定が足りません");
            }
        }
    }
}
