using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotEnemy : BaseRobot
{
    private NavMeshAgent nav;
    private Animator animator;
    private EnemySight enemySight;
    private EnemySight weaponSight;
    public WeaponBase CurWeapon;
    private GameObject player;
    //Patrolling巡逻相关
    public float patrolSpeed = 5;
    public int wayPointIndex;         //当前路径点索引
    public float patrolWaitTime = 1f; //巡逻到路径点之后的等待时间
    public Transform patrolWayPoints; //用来保存所有的巡逻路径点
    private float patrolTimer;        //巡逻到路径点后的计时器
    //Chasing追逐相关
    public float chaseSpeed = 8;
    public float chaseWaitTime = 5f;
    private float chaseTimer;
    private void Awake()
    {
        enemySight = transform.Find("EnemySight").GetComponent<EnemySight>();
        weaponSight = transform.Find("WeaponSight").GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Shooting()
    {//敌人站定，然后旋转到指定射击方向******这里可以添加角色运动方向从而增加射击准度
        nav.isStopped = true;
        nav.speed = 0;
        //
        Vector3 lookPos = player.transform.position;
        lookPos.y = transform.position.y;
        Vector3 targetDir = lookPos - transform.position;
        float step = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0);
        transform.rotation = Quaternion.LookRotation(newDir);
        //
        animator.SetBool("Shoot", true);//切换状态机为shoot状态
    }

    public override void OpenFire()
    {
        base.OpenFire();
        CurWeapon.OpenFire(transform.forward);
    }
    // Update is called once per frame
    void Update()
    {//写敌人的逻辑
        if (!RobotPlayer.GetInstance().IsAlive()) return;//角色死亡不做动作
        if (weaponSight.playerInSight)
        {
            //shoot
            Shooting();
        }
        else if (enemySight.playerInSight)
        {
            //chase
            Chasing();
        }
        else
        {
            //patrol
            Patrolling();
        }
        animator.SetFloat("Speed", nav.speed / chaseSpeed);
    }

    public void Chasing()
    {
        nav.isStopped = false;//切换为可运动状态
        nav.speed = chaseSpeed;
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        //向着最后一次看见玩家的方向冲刺
        if (sightingDeltaPos.sqrMagnitude > 4)
        {
            nav.destination = enemySight.personalLastSighting;
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                chaseTimer += Time.deltaTime;
                //敌人观察一下直到计时器满，否则的话追击计时器会清零
                if (chaseTimer>=chaseWaitTime)
                {
                    chaseTimer = 0;
                    nav.speed = 0;
                }
                else
                {
                    chaseTimer = 0;
                }
            }
        }
    }
    public void Patrolling()
    {
        nav.isStopped = false;//切换为可活动状态
        nav.speed = patrolSpeed;//速度变为巡逻速度
        if (nav.remainingDistance <= nav.stoppingDistance)
        //判断是否到了当前巡逻的目标路径点
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {//计时等待结束后，将下一个路径点的下标设置为巡逻路径点的下标
                if (wayPointIndex == patrolWayPoints.childCount - 1)
                {
                    wayPointIndex++;
                }
                patrolTimer = 0;//归零等待计时器
            }
        }
        else//没有到达路径点，处于巡逻中途
        {
            patrolTimer = 0;
        }
        //始终将巡逻目标点设置为当前路径点的位置
        nav.destination = patrolWayPoints.GetChild(wayPointIndex).position;
    }
}
