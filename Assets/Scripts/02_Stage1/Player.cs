using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクタで設定する
    [Header("移動速度")]
    public float speed;                 //スピード値決定（インスペクタ上から〇）
    [Header("重力")] 
    public float gravity;               //重力
    [Header("ジャンプ速度")] 
    public float jumpSpeed;             //ジャンプ力
    [Header("ジャンプする高さ")] 
    public float jumpHeight;            //高さ制限
    [Header("ジャンプする長さ")] 
    public float jumpLimitTime;         //ジャンプ制限時間
    [Header("接地判定")] 
    public GroundCheck ground;        //接地判定
    [Header("天井判定")] 
    public GroundCheck head;          //頭ぶつけた判定
    [Header("ダッシュの速さ表現")] 
    public AnimationCurve dashCurve;    //ダッシュ重み関数
    [Header("ジャンプの速さ表現")]
    public AnimationCurve jumpCurve;    //ジャンプ重み関数
    [Header("踏みつけ判定の高さの割合")]
    public float stepOnRate;            //踏みつけ位置あそび

    
    [Header("ジャンプする時に鳴らすSE")]
    public AudioClip jumpSE;
    [Header("やられた時に鳴らすSE")]
    public AudioClip downSE;
    [Header("コンティニュー時に鳴らすSE")]
    public AudioClip continueSE;

    #endregion

    #region//プライベート変数
    private Animator anim = null;      //Animator型の変数を用意
    private Rigidbody2D rb  = null;         //rigidbody2dインスタンス取得用の変数を定義
    private CapsuleCollider2D capcol = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isRun = false;             //ダッシュ中判別
    private bool isDown = false;
    private bool isOtherJump = false;
    private float jumpPos = 0.0f;
    private float otherJumpHeight = 0.0f;
    //private float jumpTime = 0.0f;        //ジャンプ時間格納
    private float dashTime, jumpTime;       //経過時間（ダッシュ・ジャンプ）
    private float beforeKey;                //ダッシュ反転判別用
    private string enemyTag = "Enemy";      //敵タグ判別用
    private string moveFloorTag = "MoveFloor";
    private bool isContinue = false;
    private float continueTime, blinkTime;
    private SpriteRenderer sr = null;
    private MoveObject moveObj;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   //Animatorコンポーネントを取得（⇒run変数を制御）
        rb = GetComponent<Rigidbody2D>();  //Rigdbody2Dコンポーネントを取得
        capcol = GetComponent<CapsuleCollider2D>();

        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (isContinue)
        {
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;

            }
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;

            }
            else
            {
                sr.enabled = true;

            }

            //1秒経ったら点滅終わり
            if (continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;

            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;

            }
        }
    }

    //物理演算するごとにアップグレードされる
    void FixedUpdate()
    {
        if (!isDown && !GManager.instance.isGameOver)
        {
            //接地判定を得る
            isGround = ground.IsGround();

            //各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            //アニメーション適用
            SetAnimation();

            //anim.SetBool("jump", isJump);
            //anim.SetBool("ground", isGround);

            //移動速度を設定
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null)
            {
                addVelocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;
        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
        }
        
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>y軸の速さ</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        if (isGround)
        {
            //Debug.Log("isGround");
            if (verticalKey > 0 && jumpTime < jumpLimitTime)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプ直前のy方向の位置を取得
                isJump = true;
                jumpTime = 0.0f;

                GManager.instance.PlayerSE(jumpSE);
            }
            else
            {
                isJump = false;
            }
        }
        else if (isOtherJump)
        {
            //Debug.Log("isOtherJump");
            //現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
            if (jumpPos + otherJumpHeight > transform.position.y && jumpTime < jumpLimitTime && !head.IsGround())
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;

            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        else if (isJump)
        {
            //Debug.Log("isJump");
            if (verticalKey > 0 && transform.position.y < jumpPos + jumpHeight && jumpTime <
                jumpLimitTime && !head.IsGround())
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime); //ジャンプ
        }
        return ySpeed;
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");  //横入力を取得
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //anim.SetBool("run", true);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //anim.SetBool("run", true);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = -speed;
        }
        else
        {
            //anim.SetBool("run", false);
            isRun = false;
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        //Debug.Log(xSpeed);

        //ダッシュ旋回時経過時間リセット
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }

        beforeKey = horizontalKey;
        //Debug.Log("前："+horizontalKey);
        //（ジャンプ・ダッシュ）重み付与
        xSpeed *= dashCurve.Evaluate(dashTime);   //ダッシュ
        //beforeKey = horizontalKey;              //不要（記載ミス？）
        //Debug.Log("後：" + horizontalKey);      //不要（記載ミス？）

        return xSpeed;
    }

    /// <summary>
    /// アニメーション設定
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jump",isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }


    /// <summary>
    /// ダウンアニメーションが終わっているかどうか
    /// </summary>
    /// <returns>終了しているかどうか</returns>
    public bool IsDownAnimEnd()
    {
        if(isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            Debug.Log("現在の状態： " + currentState.ToString());

            if (currentState.IsName("player_knockdown"))    //文字列として出すために⇒.IsName()
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        isDown = false;
        anim.Play("player_stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;

        GManager.instance.PlayerSE(continueSE);

    }

    #region//敵接触
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == enemyTag)
        {
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in col.contacts)
            {
                if (p.point.y < judgePos)
                {
                    //もう一度跳ねる
                    ObjectCollision o = col.gameObject.GetComponent<ObjectCollision>();
                    if (o != null)
                    {
                        otherJumpHeight = o.boundHeight;
                        o.playerStepOn = true;
                        isOtherJump = true;
                        isJump = false;
                        jumpTime = 0.0f;
                    }
                    else
                    {
                        Debug.Log("ObjectCollisionがついてないよ！");
                    }
                }
                else
                {
                    //ダウンする
                    anim.Play("player_knockdown");
                    isDown = true;
                    GManager.instance.SubHeratNum();

                    GManager.instance.PlayerSE(downSE);

                    break;
                }
            }
        }

    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.collider.tag == moveFloorTag)
        {
            //動く床から離れた
            moveObj = null;
        }
    }
    #endregion


}
