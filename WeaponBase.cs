using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Sprite icon;//武器图标
    public Transform muzzle;//武器枪口，用于决定发射子弹的位置
    public GameObject bulletPrefab;//子弹的预制件
    public int bulletNum;
    public float bulletSpeed = 100;//子弹的初始速度
    public void OpenFire(Vector3 dir)//发射子弹的方向
    {
        if (bulletNum>0)
        {//实例化子弹出现在枪口位置上
            var bullet = GameObject.Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            bulletNum--;
        }
    }
}
