using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public int score = 0;
    public int stageNum = 8;
    public int continueNum = 0;
    public int heartNum;
    public bool isGameOver = false;
    public int defaultHeartNum = 3;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機を１つ増やす
    /// </summary>
    public void AddHeartNum()
    {
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }

    /// <summary>
    /// 残機を１つ減らす
    /// </summary>
    public void SubHeratNum()
    {
        if(heartNum > 0)
        {
            --heartNum;
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    /// 最初から始めるときの処理
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        heartNum = defaultHeartNum;
        score = 0;
        stageNum = 1;
        continueNum = 0;

    }

    public void PlayerSE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }
}
