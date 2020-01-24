using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クラスの概要をここに書くことができるよ！
/// </summary>
public class Title : MonoBehaviour
{
    private bool firstPush = false;
    
    /// <summary>
    /// ボタンを押すとシーンが変わるよ！
    /// unity上のボタンで登録済み（クラス間での参照ではないので１行下の参照にカウントされないため記載します
    /// </summary>
    public void PressStart()
    {
        Debug.Log("Press Start!");

        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //ここに次のシーンへいく命令を書く

            //
            firstPush = true;
        }
    }
}
