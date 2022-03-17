using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItem : MonoBehaviour
{
    public GameObject WeaponPrefab;//用于保存掉落的武器是什么
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RobotPlayer.GetInstance().AddWeapon(WeaponPrefab);
            Destroy(this.gameObject);
        }
    }
}
