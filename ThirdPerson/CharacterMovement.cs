using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用于控制角色移动。不仅可以给玩家用，也可以给npc用
[RequireComponent(typeof(Rigidbody))]//代表这个脚本一定需要依赖于rigidbody组件，
//此时如果将本脚本拖入游戏物体中，会直接给游戏物体添加rigidbody组件
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3 CurrentInput { get; private set; }
    public float MaxWalkSpeed = 5;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()//unity是推荐将物理系统写入FixedUpdate()里面的
    {
        _rigidbody.MovePosition(_rigidbody.position + CurrentInput * MaxWalkSpeed * Time.fixedDeltaTime);
        //fixeddeltatime也是deltatime，只是固定的时间间隔
        //MovePosition()函数是输入移动的目标位置，如果移动到目标位置会发生碰撞那么就移动不了
    }
    public void SetMovementInput(Vector3 input)
    {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
        //在这里我们统一规范化向量的大小为0-1之间，因此用ClampMagnitude函数将input限制在1以内
    }
}
