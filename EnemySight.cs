using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float fov = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;//最后一次看到玩家的位置
    public static Vector3 resetPos = Vector3.back;
    private GameObject player;
    private SphereCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        personalLastSighting = resetPos;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;
            Vector3 direction = other.transform.position - transform.position;
            //玩家和视野球所在地人连线为方向向量
            float angle = Vector3.Angle(direction, transform.forward);
            //如果没有超过视野范围，则进一步检查，否则说明敌人看不到玩家
            if (angle < fov * 0.5f)
            {//进一步检查过程中
                RaycastHit hit;
                if(Physics.Raycast(transform.position + transform.up,direction.normalized,out hit, col.radius))
                {//使用射线检查的方式，如果敌人和玩家之间没有任何障碍物，
                 //则最终看到玩家，此时需要记录玩家的位置
                    if(hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        personalLastSighting = player.transform.position;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {//离开检测范围
        playerInSight = false;
    }
}
