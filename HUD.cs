using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private static HUD instance;
    public static HUD GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public Image weaponIcon;
    public Text bulletNum;
    public Text hpNum;
    // Start is called before the first frame update
    public void UpdateWeaponUI(Sprite icon,int bullet_num)
    {
        weaponIcon.sprite = icon;
        bulletNum.text = bullet_num.ToString();
    }
    public void UpdateHpUI(int hp_num)
    {
        hpNum.text = hp_num.ToString();
    }
}
