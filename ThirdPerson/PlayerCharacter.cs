using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//本脚本负责执行接受输入并演算，之所以将本脚本和CharacterMovement脚本分开，
//是为了方便AI的使用，让AI输入向量到CharacterMovement脚本
public class PlayerCharacter : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    [SerializeField]
    //一方面希望_photographer私有,一方面希望外面能给他赋值就可以使用SerializeField将它序列化
    private Photographer _photographer;
    [SerializeField]
    private Transform _followingTarget;
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();

        //_photographer.InitCamera(_followingTarget);
/*这里值得注意的是，也可以直接将角色本身的Transform传递进去，但是会将相机中心固定
为角色中心，而某些游戏主视角应当是从眼睛部位开始的，因此重新创建一个Transform，让
我们从unity里面传入一个物体(这个物体可以在任何部位)，使用它的Transform就可以避免这个问题*/
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementInput();
    }
    private void UpdateMovementInput()
    {//用_photographer.Yaw来代表正方向，因为绕着竖直轴旋转的角度，也就可以代表目前朝向的前方是哪
        Quaternion rot = Quaternion.Euler(0, _photographer.Yaw, 0);//用四元数的欧拉角(x:,y:,z:)来控制旋转  
        //一个向量乘以四元数相当于将这个向量旋转多少度，因此rot*Vector3.forward*Input.GetAxis("Vertical")这个公式代表当前旋转量下的正前方向量
        //同样的用Vector3.right*Input.GetAxis("Horizontal")来代表右方的向量
        _characterMovement.SetMovementInput(rot * Vector3.forward * Input.GetAxis("Vertical") + 
                                            rot * Vector3.right * Input.GetAxis("Horizontal"));
        //_characterMovement.SetMovementInput(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        //三个参数代表x,y,z三个轴,这里输入会导致行走不按照相机走，而是只按照默认xz坐标走
    }
}
