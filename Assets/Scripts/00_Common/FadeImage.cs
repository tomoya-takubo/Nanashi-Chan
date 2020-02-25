using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    [HideInInspector] public bool compFadeIn = false;
    [HideInInspector] public bool compFadeOut = false;

    private Image img = null;
    private float timer = 0;
    private float wait = 0.5f;
    private bool fadeIn = false;
    private bool fadeOut = false;

    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void StartFadeIn()
    {
        //もしフェードインまたはフェードアウトか実行中なら
        if (fadeIn || fadeOut)
        {
            return; //何もしない
        }

        fadeIn = true;
        compFadeIn = false;
        timer = 0.0f;

        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void StartFadeOut()
    {
        if(fadeIn || fadeOut)
        {
            return;
        }

        fadeOut = true;
        compFadeOut = false;
        timer = 0.0f;

        img.color = new Color(1, 1, 1, 0);
        img.fillAmount = 0;
        img.raycastTarget = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            //１秒かけて幕を下ろす
            //フェードイン中
            if(timer < 1 + wait && timer > wait)
            {
                //アルファ値下げ
                img.color = new Color(1, 1, 1, 1 - timer + wait);
                //幕下げ
                img.fillAmount = 1 - timer + wait;
            }
            else if(timer >= 1 + wait)
            {
                img.color = new Color(1, 1, 1, 0);
                img.fillAmount = 0;
                img.raycastTarget = false;  //黒幕の当たり判定

                timer = 0.0f;
                fadeIn = false;
                compFadeIn = true;

            }

            timer += Time.deltaTime;
        }
        else if (fadeOut)
        {
            //フェードアウト中
            if(timer < 1)
            {
                img.color = new Color(1, 1, 1, timer);
                img.fillAmount = timer;
            }
            else if(timer >= 1)
            {
                img.color = new Color(1, 1, 1, 1);
                img.fillAmount = 1;
                img.raycastTarget = false;

                timer = 0.0f;
                fadeOut = false;
                compFadeOut = true;
            }

            timer += Time.deltaTime;
        }
    }
}
