using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer : BaseRobot
{
    static RobotPlayer instance;
    public static RobotPlayer GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public List<WeaponBase> Weapons;//玩家的武器背包
    public Transform Hand;
    public float WalkSpeed = 10;
    //
    private WeaponBase CurWeapon;//当前武器
    private int CurWeaponIdx = 0;//当前武器索引
    //
    private Animator animator;     //取到角色当前Animator组件
    private CharacterController cc;//取到角色当前CharacterController组件


    public float Gravity = -9.8f;
    private Vector3 Velocity = Vector3.zero;
    public float JumpHeight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        CurWeapon = Weapons[CurWeaponIdx];
    }

    // Update is called once per frame
    void Update()
    {
        FireModele();
        Movement();
        //是否开火
        if (Input.GetButton("Fire1"))
        //if (Input.GetMouseButton(0))
        {
            //Debug.Log("鼠标左键按下，即将调用Shoot()函数！！");
            Shoot();
        }
        //switch weapon武器切换功能
        float f = Input.GetAxis("Mouse ScrollWheel");//根据鼠标滚轮切换武器
        if (f > 0) { NextWeapon(1); }
        else if (f < 0) { NextWeapon(1); }
        //UI界面
        HUD.GetInstance().UpdateHpUI(hp);
        HUD.GetInstance().UpdateWeaponUI(CurWeapon.icon,CurWeapon.bulletNum);
    }

    public void FireModele()
    {
          
    }

    public void Movement()
    {
        var trans = Camera.main.transform;//玩家的移动方向和camera相关联
        var forward = Vector3.ProjectOnPlane(trans.forward, Vector3.up);
        //向前的向量forward是通过 相机 向前的向量在水平面上的投影得到的
        var right = Vector3.ProjectOnPlane(trans.right, Vector3.up);//向右
        //只需要两个垂直向量就可以描述运动方向了
        //movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveDirection = v * forward + h * right;

        Velocity.y += Gravity * Time.deltaTime;
        cc.Move(Velocity * Time.deltaTime);

        if (cc.isGrounded && Velocity.y < 0)
        {
            Velocity.y = 0;
        }
        //关于跳跃，这里有一个固定公式：velocity = 根号下(JumpHeight*-2*Gravity)
        //JumpHeight:跳跃高度；Gravity:重力
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            Velocity.y += Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }

        cc.Move(moveDirection.normalized * WalkSpeed * Time.deltaTime);
        animator.SetFloat("Speed", cc.velocity.magnitude / WalkSpeed);
        //由于speed是0-1间规范化的数值，因此采用CharacterControler里面的速度
        //值除以WalkSpeed，这样当角色速度超过走路速度，就过渡到跑步状态
        //rotate旋转功能
        //var r = GetAimPoint();
        //RotateToTarget(r);//朝向鼠标指向的方向
        RotateToTarget(trans.forward);//朝向相机的前方(会导致人物身体前倾或者后倾，需要调节一下人物的上半身和下半身)
    }

    public void AddWeapon(GameObject weapon)
    {
        //先判断当前武器是否已经在背包里面了
        for(int i=0;i<Weapons.Count; i++)
        {
            if(Weapons[i].gameObject.name==weapon.name)
            {//不增加武器但是可以增加武器数目
                Weapons[i].bulletNum += 15;
                return;
            }
        }
        //如果不在背包的话
        var new_weapon = GameObject.Instantiate(weapon, Hand);
        new_weapon.name = weapon.name;
        new_weapon.transform.localPosition = CurWeapon.transform.localPosition;
        Weapons.Add(new_weapon.GetComponent<WeaponBase>());
        NextWeapon(Weapons.Count - 1 - CurWeaponIdx);
        //得到新武器并且加入背包后，将当前角色武器切换成新武器
    }
    public void NextWeapon(int step)//更换武器
    {
        var idx = (CurWeaponIdx + step + Weapons.Count) % Weapons.Count;//当前武器下标进行相应变换
        CurWeapon.gameObject.SetActive(false);
        CurWeapon = Weapons[idx];
        CurWeapon.gameObject.SetActive(true);
        CurWeaponIdx = idx;
        //设定当前武器为非激活状态然后重新赋值为新的武器,然后将新的武器设置为
        //激活状态,最后保存当前武器的下标******这里可能有问题，没有算进去负数*****
    }
    public Vector3 GetAimPoint()//将方向调整为鼠标指向的方向
    //具体原理为鼠标从屏幕向三位游戏空间投射一个射线，与地面floor做一个检测，如果成功，
    //就说明鼠标投射的射线成功投射到地面了，这种情况下就可以通过向量相减的方式计算出
    //从角色位置出发，到鼠标投射到地面上的点之间的向量
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit flootHit;
        if(Physics.Raycast(camRay,out flootHit, 100.0f, LayerMask.GetMask("Floor")))
        {
            Vector3 playerToMouse = flootHit.point - transform.position;
            playerToMouse.y = 0;
            return playerToMouse;
        }
        return Vector3.zero;
    }
    public void RotateToTarget(Vector3 rot)
    {
        transform.LookAt(rot + transform.position);
    }
    public void Shoot()
    {
        //Debug.Log("Shoot函数正被调用");
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("idle"))
        //if (!(animator.GetCurrentAnimatorStateInfo(1).IsName("shoot")))
        //上面俩等价的，我这里将idle写成了Idle，所以才会出错！！！！！！以后要记得名字大小写啊啊啊啊啊！！！
        {
            //Debug.Log("此时动画层1是idle状态，此时将Shoot状态置为1");
            animator.SetBool("Shoot", true);
            //Debug.Log("Shoot状态置为1");
        }
    }
    public override void OpenFire()
    {
        base.OpenFire();//在老师视频p3中可以知道在动画中添加了OpenFire的Event
        //
        //Debug.Log("OpenFire!!");
        //
        CurWeapon.OpenFire(transform.forward);
    }
}
