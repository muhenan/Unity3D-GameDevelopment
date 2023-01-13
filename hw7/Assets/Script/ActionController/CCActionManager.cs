using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CCActionManager : SSActionManager, SSActionCallback
{
    public SSActionEventType Complete = SSActionEventType.Completed;
    Dictionary<int,CCMoveToAction> actionList = new Dictionary<int, CCMoveToAction>();

    //跟踪的情况
    public void Tracert(GameObject p,GameObject player)
    {
        if (actionList.ContainsKey(p.GetComponent<Prop>().block)) actionList[p.GetComponent<Prop>().block].destroy = true;
        CCTracertAction action = CCTracertAction.getAction(player, 0.8f);
        addAction(p.gameObject, action, this);
    }

    //哨兵自己走的情况
    public void GoAround(GameObject p)
    {
        CCMoveToAction action = CCMoveToAction.getAction(p.GetComponent<Prop>().block,0.6f,GetNewTarget(p));
        actionList.Add(p.GetComponent<Prop>().block, action);
        addAction(p.gameObject, action, this);
    }

    //得到新的位置
    private Vector3 GetNewTarget(GameObject p)
    {
        Vector3 pos = p.transform.position;
        int block = p.GetComponent<Prop>().block;
        //在这个 block 中，各方向的长度的限制，用于后面判断生成的下一个位置是否合理
        float ZUp = 13.2f - (block / 3) * 9.65f;
        float ZDown = 5.5f - (block / 3) * 9.44f;
        float XUp = -4.7f + (block % 3) * 8.8f;
        float XDown = -13.3f + (block % 3) * 10.1f;
        Vector3 Move = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        Vector3 Next = pos + Move;
        //如果生成的新位置不在合理的范围内
        while (!(Next.x<XUp && Next.x>XDown && Next.z<ZUp && Next.z > ZDown))
        {
            Move = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Next = pos + Move;
        }
        return Next;
    }

    //停止所有哨兵的移动
    public void StopAll()
    {
        foreach(CCMoveToAction x in actionList.Values)
        {
            x.destroy = true;
        }
        actionList.Clear();
    }

    public void SSActionCallback(SSAction source)
    {
        if(actionList.ContainsKey(source.gameObject.GetComponent<Prop>().block)) actionList.Remove(source.gameObject.GetComponent<Prop>().block);
        GoAround(source.gameObject);
    }
}