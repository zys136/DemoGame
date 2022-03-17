using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    public float Pitch { get; private set; }
    //沿着水平轴旋转(x轴),
    public float Yaw { get; private set; }
    //沿着竖直轴旋转(y轴),
    public float mouseSensitivity = 5;//自己设置鼠标灵敏度
    public float cameraRotatingSpeed = 80;
    public float cameraYSpeed = 5;
    
    private Transform _camera;

    [SerializeField]
    private AnimationCurve _armLengthCurve;//摄影机臂长的曲线
    [SerializeField]
    private Transform _target;
    private void Awake()
    {
        _camera = transform.GetChild(0);//获取到第一个子物体
    }
    // Start is called before the first frame update
    void Start()
    {
        InitCamera();
    }

    public void InitCamera()
    {//初始化相机将position拿过来
        //_target = target;
        transform.position = _target.position;
    }
    // Update is called once per frame
    void Update()
    {
        UpdataRotation();
        UpdatePosition();
        UpdateArmLength();
    }
    private void UpdataRotation()
    {//这里值得注意的是，鼠标对应的轴的数值是表示位移距离，而手柄则是速度大小(
     //不推摇杆速度是0，推满了则是1也因此手柄的速度是有上限的，拉满就不能再增加速度了)
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;//相当于水平发生旋转
        Yaw += Input.GetAxis("CameraRateX") * cameraRotatingSpeed * Time.deltaTime;//适配手柄

        Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;//这里因为是+=符号，所以需要在设置里面把反向勾选上
        Pitch += Input.GetAxis("CameraRateY") * cameraRotatingSpeed * Time.deltaTime;//适配手柄
        //这里要注意的是Pitch是向上看和向下看，因此角度只能为(-90，90)
        Pitch = Mathf.Clamp(Pitch, -90, 90);
        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);//三个数字分别为x轴y轴z轴

    }
    private void UpdatePosition()
    {//transform.position = _target.position//上下左右严格跟随就这么使用，但是如果想要做到其他特殊效果就还需要改动一下

        Vector3 position = _target.position;//因为_target为函数外的数值，
        //因此为了降低性能损耗，内设一个position，用其替代_targrt.position
        /*float newY = Mathf.Lerp(transform.position.y, _target.position.y, Time.deltaTime * cameraYSpeed);
        //(a,b,t)让Y轴视角(上下移动)有差值(类似于荒野之息)，差值从自己当前a到目标b，大小为t
        transform.position = new Vector3(_target.position.x, newY, _target.position.z);
        */
        float newY = Mathf.Lerp(transform.position.y, position.y, Time.deltaTime * cameraYSpeed);
        //(a,b,t)让Y轴视角(上下移动)有差值(类似于荒野之息)，差值从自己当前a到目标b，大小为t
        transform.position = new Vector3(position.x, newY, position.z);

    }

    private void UpdateArmLength()
    {//摄影机臂长应该是一个曲线，而一个曲线是有x轴y轴的
     //_armLengthCurve.Evaluate(Pitch);//这个曲线的输入值(x轴)为Pitch，返回值为长度(y轴)
        _camera.localPosition = new Vector3(0, 0, _armLengthCurve.Evaluate(Pitch) * -1);
    }
}
