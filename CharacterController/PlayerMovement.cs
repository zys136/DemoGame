using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float Speed = 10f;
    public float RotateSpeed = 1f;
    public float Gravity = -9.8f;
    private Vector3 Velocity = Vector3.zero;
    public float JumpHeight = 3f;
    public float mouseSensitivity = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLikeWow();
        //MoveLikeTopDown();
    }
    private void MoveLikeWow()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var rotate = Input.GetAxis("Mouse X");
        var move = transform.forward * Speed * vertical * Time.deltaTime + transform.right * Speed * horizontal * Time.deltaTime;
        controller.Move(move);
        
        Velocity.y += Gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);

        if (controller.isGrounded && Velocity.y < 0)
        {
            Velocity.y = 0;
        }
        //关于跳跃，这里有一个固定公式：velocity = 根号下(JumpHeight*-2*Gravity)
        //JumpHeight:跳跃高度；Gravity:重力
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            Velocity.y += Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }

        //transform.Rotate(Vector3.up, horizontal * RotateSpeed);//a,d键控制旋转
        transform.Rotate(Vector3.up, rotate * mouseSensitivity);//鼠标控制旋转
    }
    private void MoveLikeTopDown()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontal, 0, vertical).normalized;
        var move = direction * Speed * Time.deltaTime;
        controller.Move(move);

        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var point = Input.mousePosition - playerScreenPoint;
        var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)//ctrl+shift+m可以打开函数界面
    {//用作角色碰撞到其他物体产生的效果，因为角色本身没有rigidbody
        if (hit.transform.CompareTag("Moved"))
        {//假如物体具有Moved的tag就会产生效果
            hit.rigidbody.AddForce(transform.forward * Speed);
        }
    }
}
