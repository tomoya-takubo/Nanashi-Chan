﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("加算スコア")]
    public int myScore;
    [Header("アイテム取得時に鳴らすSE")]
    public AudioClip itemSE;

    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == playerTag)
        {
            if (GManager.instance != null)
            {
                GManager.instance.score += myScore;
                GManager.instance.PlayerSE(itemSE);
                Destroy(this.gameObject);
            }
        }
    }
}
