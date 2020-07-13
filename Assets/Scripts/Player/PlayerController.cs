using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    // Body子组件的刚体组件
    Rigidbody2D BodyRB;
    // Body子组件的动画组件
    Animator BodyAnim;
    // 移动速度
    public float moveSpeed;
    // 移动的阈值，如果绝对值小于这个值就当做0处理
    public float moveThreshold;
    float xVelocity;
    float yVelocity;
    // 自身跳跃的高度
    public float jumpPower;
    // 弹簧的弹力
    public float springJumpPower;
    // 检测是否在地面上的圆的半径
    public float groundCheckRadius;
    // 检测是否在墙上的圆的半径
    public float wallCheckRadius;
    // 地面检测点
    public GameObject groundCheckPoint;
    // 墙面检测点
    public List<GameObject> wallCheckPoints;
    // 被当做是地面的层
    public LayerMask groundLayer;
    public float spikeCheckRadius;
    // 被当做是刺的层
    public LayerMask spikeLayer;
    public bool isOnGround;
    public bool isOnWall;
    public bool isDead;
    // 跳跃的次数，设定上可以二段跳
    public int jumpedTime;
    public UnityEngine.Object DustPrefabObj;
    GameObject buttomLine;

    void Start()
    {
        BodyRB=GetComponent<Rigidbody2D>();
        BodyAnim=GetComponent<Animator>();
        CheckOnGround();
        isDead=false;
        isOnWall=false;
        jumpedTime=0;
        buttomLine=GameObject.Find("ButtomLine");
    }

    void Update()
    {
        if(isDead) return;

        // 检测是否位于地面上
        CheckOnGround();

        // 检测是否位于墙面面上
        CheckOnWall();

        // 玩家行走
        Walk();   

        // 玩家跳跃
        Jump();
        
        // 检测玩家是否死亡
        CheckDead();
    }

    void CheckOnGround()
    {
        bool lst=isOnGround;
        isOnGround=Physics2D.OverlapCircle(groundCheckPoint.transform.position,groundCheckRadius,groundLayer);
        
        // 着陆或起跳时，在脚底位置产生一个灰尘特效
        if(lst!=isOnGround) 
        {
            CreateDust();
            // 再次回到地面上刷新跳跃次数
            if(isOnGround)
                jumpedTime=0;
            else 
                jumpedTime=1;
        }
    }

    void CheckOnWall()
    {
        bool tmp=false;
        foreach(GameObject it in wallCheckPoints)
            tmp|=Physics2D.OverlapCircle(it.transform.position,wallCheckRadius,groundLayer);
        isOnWall=tmp;
    }

    void Walk()
    {
        // x轴方向上的加速度
        xVelocity=Input.acceleration.x;
        // 电脑调试用
        if(Input.GetAxisRaw("Horizontal")!=0)
            xVelocity=Input.GetAxisRaw("Horizontal")/4;
        yVelocity=BodyRB.velocity.y+0.0f;

        // 防止下落速度过快导致的穿墙
        yVelocity=Mathf.Max(yVelocity,-5.0f);

        // 如果在墙面上，人物面朝的方向上就不应该有速度
        if(isOnWall&&(xVelocity>0&&transform.localScale.x==1||xVelocity<0&&transform.localScale.x==-1))
            xVelocity=0;

        // 如果加速度过小就忽略它，保持静止
        if(Mathf.Abs(xVelocity)<moveThreshold) xVelocity=0;
        BodyRB.velocity=new Vector2(xVelocity*moveSpeed,yVelocity);
        
        // 控制朝向，大于0朝右，小于0朝左，否则保持不变
        if(xVelocity<0)
            transform.localScale=new Vector3(-1,1,1);
        else if(xVelocity>0)
            transform.localScale=new Vector3(1,1,1);

        // 更新动画控制器内的x轴速度
        BodyAnim.SetFloat("xVelocity",Mathf.Abs(xVelocity));
    }

    void Jump()
    {
        if(!isOnGround&&jumpedTime>=2) return;
        if(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Began||Input.GetKeyDown(KeyCode.Space))
        {
            float newVelocityY=Mathf.Min(Mathf.Max(BodyRB.velocity.y,0f)+jumpPower,7.0f)-BodyRB.velocity.y;
            BodyRB.velocity+=new Vector2(0,newVelocityY);
            jumpedTime++;
        }
    }

    void CheckDead()
    {
        // 如果超出了屏幕范围
        if(BodyRB.position.y<=buttomLine.transform.position.y) Dead();
    }   

    private void OnTriggerStay2D(Collider2D other)
    {
        if(isDead) return;

        if(other.CompareTag("Spring")) // 弹簧碰撞
        {
            Animator anim=other.GetComponent<Animator>();
            float newVelocityY=Mathf.Min(Mathf.Max(BodyRB.velocity.y,0f)+springJumpPower,9.0f)-BodyRB.velocity.y;
            BodyRB.velocity+=new Vector2(0,newVelocityY);

            // 摄像机震动，加强反馈
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(.2f, .2f, 14, 90, false, true);
            // Ripple 特效
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            // 顺便给你手机也震一下
            Handheld.Vibrate();

            other.GetComponent<Spring>().trigger();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Spike")) // 尖刺碰撞
        {
            Dead();
        }
        else if(other.CompareTag("Broken Block")) // 踩在会坏掉的平台上
        {
            other.GetComponent<Brokenblock>().trigger();
        }
        else if(other.CompareTag("Strawberry")) // 吃草莓
        {
            other.GetComponent<Strawberry>().trigger();
            MainSceneEventManager.addScore(1000); // 加1000分
            Handheld.Vibrate();
        }
    }

    void Dead()
    {
        isDead=true;

        // 播放Dust动画
        BodyAnim.Play("Madeline Die");

        // Dust动画保持静止防止漂移
        BodyRB.bodyType=RigidbodyType2D.Static;

        Handheld.Vibrate();

        // 删除所有的子组件
        foreach(Transform child in gameObject.transform)
            Destroy(child.gameObject);
    }

    void DestroyMyself()
    {
        Destroy(gameObject);

        // 游戏结束
        MainSceneEventManager.GameOver();
    }

    void CreateDust()
    {
        Vector2 offset=new Vector2(0,-0.25f);
        GameObject go=Instantiate(DustPrefabObj,BodyRB.position+offset,Quaternion.identity) as GameObject;
        go.GetComponent<Dust>().dir=transform.localScale.x;
    }

    private void OnDrawGizmosSelected()
    {
        // 碰撞盒
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.transform.position,groundCheckRadius);
        foreach(GameObject it in wallCheckPoints)
            Gizmos.DrawWireSphere(it.transform.position,wallCheckRadius);
    }
}
