using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaTranslate : MonoBehaviour
{
    public float g = -10;//重力加速度
    private Vector3 speed;//初速度向量
    private Vector3 Gravity;//重力向量
    void Start()
    {
        Gravity = Vector3.zero;//重力初始速度为0
        speed = new Vector3(10, 10, 0);
    }
    private float dTime = 0;
    // Update is called once per frame
    void FixedUpdate()
    {

        Gravity.y = g * (dTime += Time.fixedDeltaTime);//v=at
                                                       
        //Translate
        transform.Translate(speed * Time.fixedDeltaTime);
        transform.Translate(Gravity * Time.fixedDeltaTime);
        Debug.Log(this.transform.position);
    }

}
