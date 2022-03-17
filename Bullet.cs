using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power = 10;//子弹的威力
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("角色受到攻击！");
            other.GetComponent<RobotPlayer>().GetDamage(power);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("敌人受到攻击！");
            other.GetComponent<RobotEnemy>().GetDamage(power);
        }
        Destroy(this.gameObject);
    }
}
