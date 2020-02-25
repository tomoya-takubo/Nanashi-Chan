using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Text heartText;
    private int oldHeartNum;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if(heartText != null && GManager.instance != null)
        {
            heartText.text = "×" + GManager.instance.heartNum.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(heartText != null && GManager.instance != null)
        {
            if(oldHeartNum != GManager.instance.heartNum)
            {
                heartText.text = "×" + GManager.instance.heartNum.ToString();
                oldHeartNum = GManager.instance.heartNum;
            }
        }
    }
}
