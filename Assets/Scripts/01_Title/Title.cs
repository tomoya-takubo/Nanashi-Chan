using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// クラスの概要をここに書くことができるよ！
/// </summary>
public class Title : MonoBehaviour
{
    [Header("フェード")]
    public FadeImage fade;
    [Header("ゲームスタート時に鳴らすSE")]
    public AudioClip startSE;

    private bool firstPush = false;

    private void Update()
    {
        if (fade.compFadeOut)
        {
            //Debug.Log("Go Next Scene!");

            //ここに次のシーンへいく命令を書く
            SceneManager.LoadScene("stage1");
        }
    }

    /// <summary>
    /// ボタンを押すとシーンが変わるよ！
    /// unity上のボタンで登録済み（クラス間での参照ではないので１行下の参照にカウントされないため記載します
    /// </summary>
    public void PressStart()
    {
        Debug.Log("Press Start!");

        if (!firstPush)
        {
            if(fade != null)
            {
                GManager.instance.PlayerSE(startSE);
                fade.StartFadeOut();
                firstPush = true;
            }
        }
    }
}
