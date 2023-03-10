using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移动的动作
public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;
    public int block;

    private CCMoveToAction() { }
    public static CCMoveToAction getAction(int block,float speed,Vector3 position)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = position;
        action.speed = speed;
        action.block = block;
        return action;
    }

    public override void Update()
    {
        if (this.transform.position == target)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
    }

    public override void Start()
    {
        Quaternion rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
        transform.rotation = rotation;
    }
}