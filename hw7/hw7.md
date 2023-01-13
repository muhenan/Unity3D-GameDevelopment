# Unity 3D 第七次作业



[TOC]

智能巡逻兵

- 提交要求：
- 游戏设计要求：
  - 创建一个地图和若干巡逻兵(使用动画)；
  - 每个巡逻兵走一个3~5个边的凸多边型，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算；
  - 巡逻兵碰撞到障碍物，则会自动选下一个点为目标；
  - 巡逻兵在设定范围内感知到玩家，会自动追击玩家；
  - 失去玩家目标后，继续巡逻；
  - 计分：玩家每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束；
- 程序设计要求：
  - 必须使用订阅与发布模式传消息
    - subject：OnLostGoal
    - Publisher: ?
    - Subscriber: ?
  - 工厂模式生产巡逻兵



## 代码结构

此次作业的代码结构如下：

![image-20201117101201567](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117101201567.png)

这次依然是使用了动作分离，MVC模式和工厂模式，以及新加了订阅与发表模式。



## 人物场景预设



参考网上资源和师兄博客，得到了一下的人物预设和场景

场景的预设：

![image-20201117101755805](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117101755805.png)

人物的预设：

![image-20201117101825665](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117101825665.png)

![image-20201117101855241](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117101855241.png)



## 动画设计

设计玩家和巡逻兵的动画

### 首先设置玩家的动画

这里有两个参数：

1. （bool）run
2. （Trigger）death

![image-20201117102414583](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117102414583.png)

* 当 run 设为 true时，玩家开始跑，当设为 false时，玩家无动作
* 当设置 death 时，玩家有一个倒下的动作，意味着游戏的结束

![image-20201117102749134](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117102749134.png)



### 巡逻兵的动画

这里有两个参数：

1. （bool）run
2. （Trigger）jump

![image-20201117102828541](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117102828541.png)

* 当 run 设为 true时，巡逻兵开始跑，意味着开始追玩家；当设为 false时，巡逻兵走
* 当设置 jump 时，巡逻兵有一个跳起的动作，意味着，干掉玩家，游戏结束

![image-20201117103029460](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117103029460.png)



## 给人物对象添加各种 Component

给人物对象，添加刚体，胶囊碰撞器，动画以及碰撞事件处理脚本

![image-20201117103211150](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117103211150.png)

![image-20201117103228559](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117103228559.png)



## 具体代码

![image-20201117101201567](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117101201567.png)

### Interfaces 接口

接口类声明在命名空间Interface中，UserAction类中主要为GUI和场景控制器交互的的方法，SSActionCallback中则为运动控制器的回调函数。

```cs
namespace Interfaces
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        int GetScore();
        void Restart();
        bool GetGameState();
        //移动玩家
        void MovePlayer(float translationX, float translationZ);
    }

    public enum SSActionEventType : int { Started, Completed }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}
```

这里只是声明，具体的实现由继承其中这些类的类来完成



### 游戏场景控制器 FirstSceneController

游戏场景控制器FirstSceneController类继承了接口ISceneController和UserAction，并且在其中实现了接口声明的函数。场景控制器还是订阅者，在初始化时将自身相应的事件处理函数提交给消息处理器，在相应事件发生时被自动调用。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstSceneController : MonoBehaviour, ISceneController, UserAction
{
    GameObject player = null;
    PropFactory PF;
    int score = 0;
    int PlayerArea = 4;
    bool gameState = false;
    Dictionary<int, GameObject> allProp = null;
    CCActionManager CCManager = null;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        PF = PropFactory.PF;
        if(CCManager == null) CCManager = gameObject.AddComponent<CCActionManager>();
        if (player == null && allProp == null)
        {
            //加载要用到的人物和场景
            Instantiate(Resources.Load<GameObject>("Prefabs/Plane"), new Vector3(0, 0, 0), Quaternion.identity);
            player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            allProp = PF.GetProp();
        }
        if (player.GetComponent<Rigidbody>())
        {
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called once per frame
	void Update () {
        //防止碰撞带来的移动
        if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
        {
            player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
        }
        if (player.transform.position.y <= 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
    }

    void OnEnable()
    {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }

    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }

    public void LoadResources()
    {
        
    }

    public int GetScore()
    {
        return score;
    }

    public void Restart()
    {
        //重新开始时，player的动画状态
        player.GetComponent<Animator>().Play("New State");
        PF.StopPatrol();
        gameState = true;
        score = 0;
        player.transform.position = new Vector3(0, 0, 0);
        allProp[PlayerArea].GetComponent<Prop>().follow_player = true;
        CCManager.Tracert(allProp[PlayerArea], player);
        foreach (GameObject x in allProp.Values)
        {
            if (!x.GetComponent<Prop>().follow_player)
            {
                CCManager.GoAround(x);
            }
        }
    }

    public bool GetGameState()
    {
        return gameState;
    }
    public void SetPlayerArea(int x)
    {
        if (PlayerArea != x && gameState)
        {
            allProp[PlayerArea].GetComponent<Animator>().SetBool("run", false);
            allProp[PlayerArea].GetComponent<Prop>().follow_player = false;
            PlayerArea = x;
        }
    }

    void AddScore()
    {
        if (gameState)
        {
            ++score;
            allProp[PlayerArea].GetComponent<Prop>().follow_player = true;
            CCManager.Tracert(allProp[PlayerArea], player);
            allProp[PlayerArea].GetComponent<Animator>().SetBool("run", true);
        }
    }

    void Gameover()
    {
        CCManager.StopAll();
        allProp[PlayerArea].GetComponent<Prop>().follow_player = false;
        player.GetComponent<Animator>().SetTrigger("death");
        allProp[PlayerArea].GetComponent<Animator>().SetTrigger("jump");
        gameState = false;
    }

    //玩家移动
    public void MovePlayer(float translationX, float translationZ)
    {
        if (gameState&&player!=null)
        {
            if (translationX != 0 || translationZ != 0)
            {
                player.GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("run", false);
            }
            //移动和旋转
            player.transform.Translate(0, 0, translationZ * 4f * Time.deltaTime);
            player.transform.Rotate(0, translationX * 50f * Time.deltaTime, 0);
        }
    }
}
```

其中一些具体函数，关于玩家和巡逻兵的控制，在后面的代码中有详细解说。



### InterfaceGUI 图形界面

主要是实现显示分数和计时，并且在游戏结束的时候显示开始按钮以重开游戏。

其中在第一次开始时会显示 Start，之后的重新开始会显示 Restart。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.UI;

public class InterfaceGUI : MonoBehaviour {
    UserAction UserActionController;
    ISceneController SceneController;
    public GameObject t;
    bool ss = false;
    float S;
    int times = 0;
    // Use this for initialization
    void Start () {
        UserActionController = SSDirector.getInstance().currentScenceController as UserAction;
        SceneController = SSDirector.getInstance().currentScenceController as ISceneController;
        S = Time.time;
    }

    private void OnGUI()
    {
        if(!ss) S = Time.time;
        GUI.Label(new Rect(Screen.width -160, 30, 150, 30),"Score: " + UserActionController.GetScore().ToString() + "  Time:  " + ((int)(Time.time - S)).ToString());
        if (ss)
        {
            if (!UserActionController.GetGameState())
            {
                ss = false;
            }
        }
        else
        {
            if(times == 0){
                if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 50), "Start"))
                {
                    ss = true;
                    SceneController.LoadResources();
                    S = Time.time;
                    UserActionController.Restart();
                    times++;
                }
            }else{
                if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 50), "Restart"))
                {
                    ss = true;
                    SceneController.LoadResources();
                    S = Time.time;
                    UserActionController.Restart();
                    times++;
                }
            }
        }
    }

    private void Update()
    {
        //获取方向键的偏移量
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        //移动玩家
        UserActionController.MovePlayer(translationX, translationZ);
    }
}

```



### Singleton / SSDirector

这两个代码和之前的情况一样，这里不再赘述



### AreaCollide 区域碰撞

有游戏对象进入区域时，判断进入区域的对象是否为玩家“Player”。如果是玩家，区域将调用**事件管理器**发布玩家进入新区域的事件。

```cs
public class AreaCollide : MonoBehaviour
{
    public int sign = 0;
    FirstSceneController sceneController;
    private void Start()
    {
        sceneController = SSDirector.getInstance().currentScenceController as FirstSceneController;
    }
    void OnTriggerEnter(Collider collider)
    {
        //标记玩家进入自己的区域
        if (collider.gameObject.tag == "Player")
        {
            sceneController.SetPlayerArea(sign);
            GameEventManager.Instance.PlayerEscape();
        }
    }
}
```



### PlayerCollide 巡逻兵碰撞

当巡逻兵发生碰撞时，判断碰撞对象是否为玩家。如果是玩家，调用**事件管理器**发表游戏结束的消息。

```cs
public class PlayerCollide : MonoBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        //当玩家与侦察兵相撞
        if (other.gameObject.tag == "Player")
        {
            GameEventManager.Instance.PlayerGameover();
        }
    }
}

```



### GameEventManager 游戏事件管理器

游戏事件管理器是订阅与发布模式中的中继者，消息的订阅者通过与管理器中相应的事件委托绑定，在管理器相应的函数被发布者调用（也就是发布者发布相应消息时），订阅者绑定的相应事件处理函数也会被调用。订阅与发布模式实现了一部分消息的发布者和订阅者之间的解耦，让发布者和订阅者不必产生直接联系。

```cs
public class GameEventManager
{
    public static GameEventManager Instance = new GameEventManager();
    //计分委托
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;
    //游戏结束委托
    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;

    private GameEventManager() { }

    //玩家逃脱进入新区域
    public void PlayerEscape()
    {
        if (ScoreChange != null)
        {
            ScoreChange();
        }
    }
    //玩家被捕，游戏结束
    public void PlayerGameover()
    {
        if (GameoverChange != null)
        {
            GameoverChange();
        }
    }
}
```



### 巡逻兵的 Prop Component

这个用于对巡逻兵进行一些简单的设置

两个变量：

* block，在哪个区域
* follow_player，是否正在跟随玩家

然后是一些基础设置并防止碰撞时弹起

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
    public int block;                      //标志巡逻兵的区域
    public bool follow_player = false;    //玩家进入区域时是否跟随玩家的标志  

    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        //防止碰撞发生后的旋转
        //保证姿态
        if (this.gameObject.transform.localEulerAngles.x != 0 || gameObject.transform.localEulerAngles.z != 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);
        }
        //防止跳起来
        if (gameObject.transform.position.y != 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
    }
}

```



### PropFactory 巡逻兵工厂

主要是用于产生多个巡逻兵，并设置其相应的位置。

停止巡逻兵即将他们恢复原位。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropFactory {
    
    public static PropFactory PF = new PropFactory();
    private Dictionary<int, GameObject> used = new Dictionary<int, GameObject>();//used是用来保存正在使用的巡逻兵 

    private PropFactory()
    {
    }

    int[] pos_x = { -7, 1, 7 };
    int[] pos_z = { 8, 2, -8 };
    public Dictionary<int, GameObject> GetProp()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject newProp = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Prop"));//获取预制的游戏对象
                //给 刚生成的游戏对象 添加 Prop 这个 Component
                newProp.AddComponent<Prop>();
                //设置其位置
                newProp.transform.position = new Vector3(pos_x[j], 0, pos_z[i]);
                //所在的分区
                newProp.GetComponent<Prop>().block = i * 3 + j;
                newProp.SetActive(true);
                //添加到使用过的 Prop里面
                used.Add(i*3+j, newProp);
            }
        }
        return used;
    }

    public void StopPatrol()
    {
        //停止所有巡逻兵
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                //设为原位
                used[i * 3 + j].transform.position = new Vector3(pos_x[j], 0, pos_z[i]);
            }
        }
    }
}

```



### SSAction

与之前的基本相同，这里不再赘述，主要的内容不在这里，而在继承它的其他动作



### CCMoveToAction 移动动作

控制巡逻兵移动的动作

```c#
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
```



### CCTracertAction 追踪动作

控制巡逻兵追踪的动作

```c#
public class CCTracertAction : SSAction
{
    public GameObject target;
    public float speed;

    private CCTracertAction() { }
    public static CCTracertAction getAction(GameObject target, float speed)
    {
        CCTracertAction action = ScriptableObject.CreateInstance<CCTracertAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - gameObject.transform.position, Vector3.up);
        gameObject.transform.rotation = rotation;
        if (gameObject.GetComponent<Prop>().follow_player == false||transform.position == target.transform.position)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }

    public override void Start()
    {

    }
}
```

* 每次 update，都以一定的小的距离 speed * Time.deltaTime 向 target（即玩家的方向走），这样就完成了追踪的操作。



### SSActionManager 动作管理器

与之前项目的情况大致相同。



### CCActionManager 动作管理器

场记通过动作管理器CCActionManager管理对象的移动，CCActionManager实现了追踪Tracert，巡逻GoAround方法，并通过回调函数来循环执行巡逻动作或者在追踪结束时继续巡逻动作。

```c#
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
```

* Tracert 进行跟踪
* GoAround 巡逻兵在不跟踪的状态下，自己移动的情况
  * GetNewTarget 得到下一个要走到的新位置
* StopAll 停止所有哨兵



## 运行截图

![image-20201117111706369](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117111706369.png)

![image-20201117111742915](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117111742915.png)

![image-20201117111835594](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw7/img/image-20201117111835594.png)



## 总结



* 首先通过这次的实验，又一次使用动作分离，MVC模式和工厂模式，也让我更加熟悉了这些设计模式，感觉很多游戏的设计都有规律可寻，用固定的模式去做，就会变得很有逻辑。更加熟悉了这些动作模式。
* 设置了动画，更加了解了动画控制器具体是怎么控制，那些参数的意义，和在代码中完成对各种动画的控制。通过这次的动画设计，更加清楚了那些组件的具体意义。
* 学习并新加入了订阅与发表模式。
* 更加熟悉了 Callback 的具体意义

