using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zako1 : MonoBehaviour
{
    #region
    [Header("加算スコア")] public int myScore;   //自分を踏んだらこれだけ得点が入るよ
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接地判定")] public EnemyCollisionCheck checkCollision;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;
    #endregion

    #region
    private Rigidbody2D rb = null;      //RB取得用
    private SpriteRenderer sr = null;   //レンダラー取得用
    private Animator anim = null;
    private bool rightTleftF = false;
    private ObjectCollision oc = null;  //接触状態
    private BoxCollider2D col = null;
    private bool isDead = false;        //生死判定
    private float deadTimer = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oc.playerStepOn)
        {
            
            if (sr.isVisible || nonVisibleAct)
            {
                //壁に衝突した際に反転判定をONにする
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }

                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                //Debug.Log("写ってる！");
                rb.velocity = new Vector2(xVector * speed, -gravity);
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }
        }
        else
        {
            if (!isDead)
            {
                anim.Play("enemy_dead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                //anim.SetBool("walk", false);

                if(GManager.instance != null)
                {
                    GManager.instance.PlayerSE(deadSE);
                    GManager.instance.score += myScore;
                }
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
                if (deadTimer > 3.0f)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    deadTimer += Time.deltaTime;
                }
            }
        }
    }
}
