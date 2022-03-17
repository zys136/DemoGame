using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private float mouseX, mouseY;
    public float mouseSensitivity;
    public float xRotation;
    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseY;//这里原本用的是 -= ，因为设置的关系会反方向移动，所以改为+=
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        player.Rotate(Vector3.up * mouseX);//up为一个向上的三维变量，类比于(0,1,0)
        //transform.localRotation = Quaternion.Euler(-mouseY, 0, 0);//这里会出现鼠标上下移动视角一卡一卡的问题，这是因为getaxis函数会返回-1——1之间的值，鼠标不动就会自动返回到1
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //localRotation就可以只移动自己而不移动主物体

    }
}
