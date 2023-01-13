using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaTransform : MonoBehaviour
{
    public float Gravity = -10;//这个代表重力加速度

    private Vector3 MoveSpeed;//初速度向量

    private Vector3 GritySpeed = Vector3.zero;//重力的速度向量，t时为0

    private float dTime;//已经过去的时间

    // Use this for initialization

    void Start()

    {
        
        //初速度
        MoveSpeed = new Vector3(0, 10, 5) ;

    }

    // Update is called once per frame

    void FixedUpdate()

    {

        //重力加速度带来的速度改变
        //v = at ;
        GritySpeed.y = Gravity * (dTime += Time.fixedDeltaTime);

        //位移

        transform.position += (MoveSpeed + GritySpeed) * Time.fixedDeltaTime;
        Debug.Log(this.transform.position);
    }

}
