using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UGUIを使うので

public class Score : MonoBehaviour
{
    private Text scoreText;
    private int oldScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(scoreText != null && GManager.instance != null)
        {
            scoreText.text = "Score " + GManager.instance.score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreText != null && GManager.instance != null)
        {
            //Updateで毎フレームTextの中身を変更するのはよろしくないので
            if (oldScore != GManager.instance.score)
            {
                scoreText.text = "Score " + GManager.instance.score.ToString();
                oldScore = GManager.instance.score;
            }
        }
    }
}
