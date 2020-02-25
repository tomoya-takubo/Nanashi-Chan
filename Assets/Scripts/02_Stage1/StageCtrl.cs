using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject GameOverObj;
    [Header("フェード")] public FadeImage fade;

    [Header("ゲームオーバー時に鳴らすSE")] public AudioClip gameOverSE;

    private Player p;
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            GameOverObj.SetActive(false);

            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<Player>();
            if (p == null)
            {
                Debug.Log("プレイヤーが設定されていません");
                Destroy(this);
            }
        }
        else
        {
            Debug.Log("ステージコントローラーの設定が足りてません");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー
        if(GManager.instance.isGameOver && !doGameOver)
        {
            GameOverObj.SetActive(true);
            GManager.instance.PlayerSE(gameOverSE);
            doGameOver = true;
        }
        //プレイヤーがダメージを受けた
        else if (p.IsDownAnimEnd())
        {
            PlayerSetContinuePoint();
        }

        //ステージを切り替える
        if(fade != null && startFade)
        {
            if (fade.compFadeOut)
            {
                GManager.instance.stageNum = nextStageNum;
                SceneManager.LoadScene("stage" + nextStageNum);
            }
        }

    }

    /// <summary>
    /// プレイヤーをコンティニューポイントへ移動する
    /// </summary>
    public void PlayerSetContinuePoint()
    {
        playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
        p.ContinuePlayer();

        /***
        p.anim.Play("player_stand");
        p.isDown = false;
        ***/
    }

    /// <summary>
    /// 最初から始める
    /// </summary>
    public void Retry()
    {
        GManager.instance.RetryGame();
        ChangeScene(1);
    }


    public void ChangeScene(int num)
    {
        if(fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
}
