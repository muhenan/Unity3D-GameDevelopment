# unity 3d 第四次作业



[TOC]



## 1、基本操作演练【建议做】

### 下载  Fantasy Skybox FREE， 构建自己的游戏场景

首先登陆 Assert Store 然后添加 **Fantasy Skybox FREE**，这里已经添加

![image-20201012105926842](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012105926842.png)

打开 Unity 找到我们自己的 Assert，找到  **Fantasy Skybox FREE** 然后下载

![image-20201012110127000](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012110127000.png)



这个包较大（Size: 113.72 MB (Number of files: 115)），下载可能要花点时间

下载完成后开始 import

![image-20201012110430806](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012110430806.png)

要构建自己的游戏场景，我们主要需要 天空盒 和 地形这两部分

首先我们构建自己的天空盒

首先建一个 Material，操作为 ： Assert -> Create -> Material

将这个 Material 命名为 musky 然后 在右边的 Inspector 进行如下操作

![image-20201012113607906](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012113607906.png)

选择天空盒，并选择适当的图片，这样我们就完成了天空盒的制作，呈现的结果如下：

![image-20201012113710323](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012113710323.png)

接下来我们来制作地形

首先我们创建地形，操作为： GameObject -> 3D Object -> Terrain

然后我们开始弄出一些地形，并进行一些种草种树的操作

首先弄出一些地形，这里的具体操作是右上角这个圈种的 Paint Terrain 

![image-20201012114233595](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012114233595.png)

接下来开始在这个地形上种一些树和草

![image-20201012115819932](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012115819932.png)

要在Details 这里添加草，这样才可以开始种草，这里还要注意如果摄像机离的太远，草可能会看不出来就好像没种一样，我们只要把摄像机拉近，就可以看到草了。

最终构建出的简单场景如下：

![image-20201012120207819](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012120207819.png)





### 写一个简单的总结，总结游戏对象的使用

- 游戏对象都有Active属性，Name属性，Tag属性等。每个游戏对象 (GameObject) 还包含一个变换transform组件。我们可以通过这个组件来使游戏对象改变位置，旋转和缩放。我们还可以添加不同的组件或脚本来增加游戏对象的功能。

- 游戏对象的分类

  - 常见游戏对象 
     如正方体，球体，平面等等，还有空对象，不显示却最常用。

  - Camera 摄像机 
     它是观察游戏世界的窗口，Projection属性包括正交视图与透视视图。Viewport Rect:属性是控制摄像机呈现结果对应到屏幕上的位置以及大小。屏幕坐标系：左下角是(0, 0)，右上角是(1, 1)。Depth属性是当多个摄像机同时存在时，这个参数决定了呈现的先后顺序，值越大越靠后呈现。

  - skyboxes 天空盒 
     天空是一个球状贴图，通常用 6 面贴图表示。 
     使用方法有两步，第一为在摄像机添加天空组件。Component 加入skybox组件，第二为直接拖入天空材料（Material）。

  - 光源Light 
     灯光类型（type）包括平行光（类似太阳光），聚光灯（spot），点光源（point），区域光（area，仅烘培用）。

  - 地形构造工具Terrain 

    可以通过画笔构建一些地形，并且在这些地形上可以种树种草，可以弄一些东西在这个地形，这些操作都通过画笔操作完成

  - 声音audio 
     将声音素材拖入摄像机就可以成为背景音乐。可以设置是否重复，音量等属性

  - 游戏资源库 
     从商店中查找所需资源后，import packages后就可以在resource中自由使用这些资源。

- 游戏对象的属性

  - activeInHierarchy——定义游戏对象在场景中是否活跃。
  - activeSelf——游戏对象本地激活状态（只读）。
  - isStatic——仅编辑器的API，用于指定游戏对象是否为静态。
  - layer——游戏对象所在的层次。
  - scene——游戏对象所属的场景。
  - tag——游戏对象的标签。
  - transform——附加到此游戏对象的Transform组件。
  -  游戏对象的构造函数：
    - GameObject——创建一个名为name的新游戏对象。
  - 游戏对象的Public方法：
    -  AddComponent——将名为className的组件类添加到游戏对象。
    - BroadcastMessage——在此游戏对象或其任何子对象中的每个MonoBehaviour上调用名为methodName的方法。
    - CompareTag——返回这个游戏对象是否带有标签的布尔值。
    - GetComponent——如果游戏对象附加了一个组件，则返回组件类型type；如果没有，则返回null。
    - SendMessage——在此游戏对象中的每个MonoBehaviour上调用名为methodName的方法。
    - SendMessageUpwards——在此游戏对象中的每个MonoBehaviour以及该行为的每个祖先上调用名为methodName的方法。
    - SetActive——根据给定的true或false值激活/停用GameObject。
  - 游戏对象的静态方法
       - CreatePrimitive——使用基本的网格渲染器和适当的碰撞器创建游戏对象。
       - Find——通过名称查找GameObject并返回这个对象。
       - FindGameObjectsWithTag——返回带有标签的活跃的游戏对象的列表，如果没有找到游戏对象返回空数组。
       - FindWithTag——返回一个带有标签的活跃的游戏对象，如果没有找到游戏对象返回null。





## 2、编程实践（二选一）

### 牧师与魔鬼 动作分离版（【2019开始的新要求】：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束）



这次的实验是在我上次的 牧师与魔鬼 的实验上进行一些升级，将动作分离，并且设计一个裁判类来判断游戏是否结束。上次的结构并不利于我们对于游戏日后的扩展和更改，本次，我们学习了建立动作管理器，将动作分离出来。我们可以建立一个动作管理器，通过场景控制器把需要移动的游戏对象、移动目标和速度等传递给动作管理器，让动作管理器去移动游戏对象，实现该动作。



本次的脚本代码是这样一个结构

![image-20201012171815331](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012171815331.png)

#### 1. 首先，SSAction是一个动作基类，里面使用virtual申明虚方法，通过重写实现多态。这样继承者就明确使用Start和Update编程游戏对象行为。它利用接口实现消息通知，避免与动作管理者直接依赖。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destory = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }
    // Use this for initialization
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
```

#### 2. 接着，通过继承SSAction这个虚基类来实现单个动作的完成CCMoveToAction类以及多个动作连续完成CCSequenceAction类。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//仅仅执行单个移动函数，通过GetSSAction来得到目的地和速度
public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }
    // Use this for initialization
    public override void Start(){}
    // Update is called once per frame
    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, 10f * Time.deltaTime);
        if (this.transform.position == target)
        {
            //waiting for destroy;
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }
}
```

#### 3. 这是标准的组合设计模式。被组合的对象和组合对象属于同一种类型。通过组合模式，我们能实现几乎满足所有越位需要、非常复杂的动作管理。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//仅仅执行多个移动函数，将动作存在队列中。通过GetSSAction来得到目的地和速度
public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1; //repeat forever
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    //  执行动作前，为每个动作注入当前动作游戏对象，并将自己作为动作事件的接收者。
    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    // 执行当前动作
    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
            sequence[start].Update();
    }
    // 收到当前动作执行完成，推下一个动作，如果完成一次循环，减次数。如完成，通知该动作的管理者。
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destory = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
            else
            {
                sequence[start].Start();
            }
        }
    }

    private void OnDestroy()
    {
        //destory
    }
}
```

#### 4. 动作事件接口（ISSActionCallback接口），这个接口是动作与动作管理者之间交流的方式，动作管理者继承并实现接口。当动作完成时，动作通过该接口通知动作管理者对象，这个动作已完成，然后管理者可以处理下一个动作。如此往复，直到所有动作都执行完成。

- 事件类型定义，使用了枚举变量
- 定义了事件处理接口，所有事件管理者都必须实现这个接口，来实现事件调度。所以，组合事件需要实现它，事件管理器也必须实现它。
- 这里还展示了语言函数默认参数的写法。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{

    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
```

#### 5. 然后实现动作管理基类SSActionManager类 ，它来管理不同的动作，将动作放在链表中等待执行或等待删除。还有能从ISSActionCallback接口中得知动作是否完成，是否需要销毁。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSActionManager : MonoBehaviour,ISSActionCallback
{
    public Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destory)
                waitingDelete.Add(ac.GetInstanceID());
            else if (ac.enable)
                ac.Update();
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        action.destory = false;
        action.enable = true;
        waitingAdd.Add(action);
        action.Start();
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null)
    {
    }
}
```

#### 6. 继承SSActionManager基类实现动作管理者CCActionManager。场景控制器就可以调用该类的方法实现不同游戏对象的移动，如通过moveBoatAction来实现船的移动，仅仅需要单个动作，利用CCAction类；而通过moveCharacterAction实现角色的移动，需要两个动作，所以需要CCSequenceAction类。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class CCActionManager : SSActionManager
{
    public CCMoveToAction ccmoveBoat;     //移动船
    public CCSequenceAction ccmoveCharacter;     //移动角色
    public FirstController sceneController;

    protected new void Start()
    {
        sceneController = (FirstController)Director.getInstance().sceneController;
        sceneController.actionManager = this;    
    }
    public void moveBoatAction(GameObject boat, Vector3 target, float speed)
    {
        ccmoveBoat = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, ccmoveBoat, this);
    }

    public void moveCharacterAction(GameObject character, Vector3 middle_pos, Vector3 end_pos, float speed)
    {
        SSAction action1 = CCMoveToAction.GetSSAction(middle_pos, speed);
        SSAction action2 = CCMoveToAction.GetSSAction(end_pos, speed);
        ccmoveCharacter = CCSequenceAction.GetSSAction(1, 0, new List<SSAction> { action1, action2 }); //1表示做一次动作，0表示从初始action1开始
        this.RunAction(character, ccmoveCharacter, this);
    }
}
```

#### 7. Judge 类，用来判断是否结束，这个类会在 FirstControl 中被 调用

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class Judge : MonoBehaviour
{
    public CoastController leftCoast;
    public CoastController rightCoast;
    public BoatController boat;

    public Judge(CoastController c1, CoastController c2, BoatController b)
    {
        leftCoast = c1;
        rightCoast = c2;
        boat = b;
    }


    public int Check()
    {
        int left_priest = 0, left_devil = 0, right_priest = 0, right_devil = 0;
        int[] fromCount = leftCoast.getCharacterNum();
        int[] toCount = rightCoast.getCharacterNum();
        left_priest += fromCount[0];
        left_devil += fromCount[1];
        right_priest += toCount[0];
        right_devil += toCount[1];
        //获胜条件
        if (right_priest + right_devil == 6)
            return 1;
        int[] boatCount = boat.getCharacterNum();
        //统计左右两岸的牧师与恶魔的数量
        if (!boat.get_is_left())
        {
            right_priest += boatCount[0];
            right_devil += boatCount[1];
        }
        else
        {
            left_priest += boatCount[0];
            left_devil += boatCount[1];
        }
        //游戏失败条件
        if ((left_priest < left_devil && left_priest > 0) || (right_priest < right_devil && right_priest > 0))
        {
            return -1;
        }
        return 0;           //游戏继续
    }
}
```

#### 8. 改动 FirstController类调用动作管理器的函数实现动作，并利用 Judge 来判断是否结束

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class FirstController : MonoBehaviour, SceneController, UserAction {
    Vector3 water_pos = new Vector3(0, 0.5f, 0);

    public CoastController leftCoast;
    public CoastController rightCoast;
    public BoatController boat;

    //新添 Judge 类
    public Judge judge;
        

    private MyCharacterController[] characters = null;
    private UserGUI userGUI = null;
    public bool flag_stop = false;
    
    Vector3 target;
    public float speed = 2.0f;

    public CCActionManager actionManager;
    //得到唯一的实例
    void Awake()
    {
        Director director = Director.getInstance();
        director.sceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyCharacterController[6];
        load();
        flag_stop = false;
    }
    //初始化游戏资源，如角色，船等等
    public void load()
    {
        GameObject water = Instantiate(Resources.Load("Perfabs/water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
        water.name = "water";
        leftCoast = new CoastController("left");
        rightCoast = new CoastController("right");
        boat = new BoatController();
        judge = new Judge(leftCoast, rightCoast, boat);
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController character = new MyCharacterController("priest");
            character.setPosition(leftCoast.getEmptyPosition());
            character.Oncoast(leftCoast);
            leftCoast.getOnCoast(character);
            characters[i] = character;
            character.setName("priest" + i);
        }
        for (int i = 0; i < 3; i++)
        {
            MyCharacterController character = new MyCharacterController("devil");
            character.setPosition(leftCoast.getEmptyPosition());
            character.Oncoast(leftCoast);
            leftCoast.getOnCoast(character);
            characters[i + 3] = character;
            character.setName("devil" + i);
        }
    }
    //判断游戏胜负
    /*int check_game_over()
    {   
        int left_priest = 0, left_devil = 0, right_priest = 0, right_devil = 0;
        int[] fromCount = leftCoast.getCharacterNum();
        int[] toCount = rightCoast.getCharacterNum();
        left_priest += fromCount[0];
        left_devil += fromCount[1];
        right_priest += toCount[0];
        right_devil += toCount[1];
        //获胜条件
        if (right_priest + right_devil == 6)      
            return 1;
        int[] boatCount = boat.getCharacterNum();
        //统计左右两岸的牧师与恶魔的数量
        if (!boat.get_is_left())
        {   
            right_priest += boatCount[0];
            right_devil += boatCount[1];
        }
        else
        {        
            left_priest += boatCount[0];
            left_devil += boatCount[1];
        }
        //游戏失败条件
        if ((left_priest < left_devil && left_priest > 0)|| (right_priest < right_devil && right_priest > 0))
        {       
            return -1;
        }
        return 0;           //游戏继续
    }*/
    //重置函数
    public void restart()
    {
        boat.reset();
        leftCoast.reset();
        rightCoast.reset();
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].reset();
        }
    }
    //游戏结束后，不能再点击产生交互信息
    public bool stop()
    {
        if(judge.Check() != 0)
            return true;
        return false;
    }
    //动作
    public void moveBoat()                  //移动船
    {
        if (boat.isEmpty()) return;
        actionManager.moveBoatAction(boat.getBoat(), boat.BoatMoveToPosition(), speed);   //调用动作管理器中的移动船函数，原先为调用moveable函数
        userGUI.status = judge.Check();
    }

    public void characterIsClicked(MyCharacterController character)    //移动角色
    {
        if (userGUI.status != 0) return;
        if (character.getis_onboat())
        {
            CoastController coast;
            if (!boat.get_is_left())
            {
                coast = rightCoast;
            }
            else
            {
                coast = leftCoast;
            }
            boat.GetOffBoat(character.getName());
            Vector3 end_pos = coast.getEmptyPosition();                                         
            Vector3 middle_pos = new Vector3(character.getGameObject().transform.position.x, end_pos.y, end_pos.z);  
            actionManager.moveCharacterAction(character.getGameObject(), middle_pos, end_pos, speed);  //调用动作管理器中的移动角色函数，原先为调用moveable函数
            character.Oncoast(coast);
            coast.getOnCoast(character);
        }
        else
        {
            CoastController coast = character.getcoastController();
            // 船上已有两人
            if (boat.getEmptyIndex() == -1)
            {
                return;
            }
            // 船与角色并不在同一边岸
            if (coast.get_is_right() == boat.get_is_left())
                return;

            coast.getOffCoast(character.getName());
            Vector3 end_pos = boat.getEmptyPos();                                             
            Vector3 middle_pos = new Vector3(end_pos.x, character.getGameObject().transform.position.y, end_pos.z); 
            actionManager.moveCharacterAction(character.getGameObject(), middle_pos, end_pos, character.speed);  //调用动作管理器中的移动角色函数，原先为调用moveable函数
            character.Onboat(boat);
            boat.GetOnBoat(character);
        }
        userGUI.status = judge.Check();
    }
}
```


#### 9. 一个实现船和角色相关函数和实现一些通用函数的 baseCode 脚本


```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


namespace Com.Mygame
{
    //Director 控制唯一一个实例
    public class Director : System.Object
    {
        private static Director D_instance;
        public SceneController sceneController { get; set; }

        public static Director getInstance()
        {
            if (D_instance == null)
                D_instance = new Director();
            return D_instance;
        }
    }

    //场景控制器，被FirstController类所继承来加载资源预设
    public interface SceneController
    {
        void load();
    }

    //门面模式，FirstController实现该接口来与用户交互
    public interface UserAction
    {
        void moveBoat();
        void characterIsClicked(MyCharacterController c_controller);
        void restart();
        bool stop();
    }
    //角色的类
    public class MyCharacterController
    {
        //只读数据，不希望通过Inspector中改变角色
        readonly GameObject character;
        //readonly Moveable moveable;
        readonly ClickGUI clickGUI;
        readonly bool is_priest;
        public float speed = 200;
        //可改变
        bool is_onboat;
        CoastController coastController;
        //通过字符串构造角色函数
        public MyCharacterController(string c_str)
        {
            if(c_str == "priest")
            {
                is_priest = true;
                character = Object.Instantiate(Resources.Load("Perfabs/priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            }
            else if(c_str == "devil")
            {
                is_priest = false;
                character = Object.Instantiate(Resources.Load("Perfabs/devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            }
            //moveable = character.AddComponent(typeof(Moveable)) as Moveable;
            clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickGUI.setController(this);
        }
        //角色上船函数
        public void Onboat(BoatController boatController)
        {
            coastController = null; //离开岸边
            character.transform.parent = boatController.getBoat().transform;
            is_onboat = true;
        }
        //角色上岸函数
        public void Oncoast(CoastController temp)
        {
            coastController = temp;
            character.transform.parent = null;
            is_onboat = false;
        }
        //重置函数，恢复现场
        public void reset()
        {
            //moveable.reset();
            coastController = (Director.getInstance().sceneController as FirstController).leftCoast;
            Oncoast(coastController);
            setPosition(coastController.getEmptyPosition());
            coastController.getOnCoast(this);
        }
        //各种get，set函数
        public void setName(string name)
        {
            character.name = name;
        }
        public GameObject getGameObject()
        {
            return character;
        }
        public string getName()
        {
            return character.name;
        }
        public void setPosition(Vector3 position)
        {
            character.transform.position = position;
        }
        public Vector3 getPosition()
        {
            return character.transform.position;
        }
        /*public void movePosition(Vector3 position)
        {
            moveable.setDestination(position);
        }*/
        public bool getType() //true -> priest; false -> devil
        {
            return is_priest;
        }
        public bool getis_onboat()
        {
            return is_onboat;
        }
        public CoastController getcoastController()
        {
            return coastController;
        }
    }
    //船的类
    public class BoatController
    {
        //只读数据，不希望通过Inspector中改变船的位置
        readonly GameObject boat;
        public float speed = 200;
        //readonly Moveable moveable;
        readonly Vector3 right_pos = new Vector3(4, 1, 0);
        readonly Vector3 left_pos = new Vector3(-4, 1, 0);
        readonly Vector3[] start_pos;
        readonly Vector3[] end_pos;
        //判断船是否向左岸走
        bool is_left;
        //船上的角色最多只有两个
        MyCharacterController[] passenger = new MyCharacterController[2];
        //船的构造函数
        public BoatController()
        {
            is_left = true;
            end_pos =  new Vector3[] { new Vector3(3F, 2F, 0), new Vector3(4.5F, 2F, 0) };
            start_pos = new Vector3[] { new Vector3(-4.5F, 2F, 0), new Vector3(-3F, 2F, 0) };
            boat = Object.Instantiate(Resources.Load("Perfabs/boat", typeof(GameObject)), left_pos, Quaternion.identity, null) as GameObject;
            boat.name = "boat";
            //moveable = boat.AddComponent(typeof(Moveable)) as Moveable;
            boat.AddComponent(typeof(ClickGUI));
        }
        //判断船是否为空
        public bool isEmpty()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if(passenger[i] != null)
                    return false;
            }
            return true;
        }
        //查找船上的空位
        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null) return i;
            }
            return -1;
        }
        //查找船上空位的位置
        public Vector3 getEmptyPos()
        {
            int index = getEmptyIndex();
            if (is_left)
                return start_pos[index];
            else
                return end_pos[index];
        }
        //船的移动函数，动作分离版改变,获得目的地
        public Vector3 BoatMoveToPosition()                   
        {
            is_left = !is_left;
            if (is_left == true)
            {
                return left_pos;
            }
            else
            {
                return right_pos;
            }
        }
        //上船函数
        public void GetOnBoat(MyCharacterController charactercontroller)
        {
            int index = getEmptyIndex();
            if(index != -1)
                passenger[index] = charactercontroller;
        }
        //上岸函数
        public MyCharacterController GetOffBoat(string name)
        {
            for(int i = 0; i < passenger.Length; i++)
            {
                if(passenger[i] != null && passenger[i].getName() == name)
                {
                    MyCharacterController mycharacter = passenger[i];
                    passenger[i] = null;
                    return mycharacter;
                }
            }
            return null;
        }
        //重置函数
        public void reset()
        {
            //moveable.reset();
            is_left = true;
            boat.transform.position = left_pos;
            passenger = new MyCharacterController[2];
        }
        //各种get和set函数
        public bool get_is_left()
        {
            return is_left;
        }
        public GameObject getBoat()
        {
            return boat;
        }
        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for(int i = 0; i < passenger.Length; i++)
            {
                if(passenger[i] != null)
                {
                    if(passenger[i].getType() == true)
                    {
                        count[0]++;
                    }
                    else
                    {
                        count[1]++;
                    }
                }
            }
            return count;
        }
    }
    //岸的类
    public class CoastController
    {
        //只读数据，不希望通过Inspector中改变左右岸的位置
        readonly GameObject coast;
        readonly Vector3 right_pos = new Vector3(10, 1, 0);
        readonly Vector3 left_pos = new Vector3(-10, 1, 0);
        //角色在岸上的位置
        readonly Vector3[] positions;
        //岸是否在右边
        readonly bool is_right;

        MyCharacterController[] passenger;

        public CoastController(string pos)
        {
            positions = new Vector3[] {new Vector3(6.5F,2.6F,0), new Vector3(7.7F,2.6F,0), new Vector3(8.9F,2.6F,0),
                new Vector3(10.1F,2.6F,0), new Vector3(11.3F,2.6F,0), new Vector3(12.5F,2.6F,0)};
            passenger = new MyCharacterController[6];
            if (pos == "right")
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/coast", typeof(GameObject)), right_pos, Quaternion.identity, null) as GameObject;
                coast.name = "right";
                is_right = true;
            }
            else if (pos == "left")
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/coast", typeof(GameObject)), left_pos, Quaternion.identity, null) as GameObject;
                coast.name = "left";
                is_right = false;
            }
        }
        //获得空位函数
        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
        //获得空位位置函数
        public Vector3 getEmptyPosition()
        {
            Vector3 pos = positions[getEmptyIndex()];
            if (is_right == false)
                pos.x *= -1;
            return pos;
        }
        //上岸函数
        public void getOnCoast (MyCharacterController mycharacter)
        {
            passenger[getEmptyIndex()] = mycharacter;
        }
        //离岸函数
        public MyCharacterController getOffCoast(string name)
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if(passenger[i] != null && passenger[i].getName() == name)
                {
                    MyCharacterController mycharacter = passenger[i];
                    passenger[i] = null;
                    return mycharacter;
                }
            }
            return null;
        }
        //重置函数
        public void reset()
        {
            passenger = new MyCharacterController[6];
        }
        //各种get和set函数
        public bool get_is_right()
        {
            return is_right;
        }
        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for(int i = 0; i < passenger.Length; i++)
            {
                if(passenger[i] != null)
                {
                    if(passenger[i].getType() == true)
                    {
                        count[0]++;
                    }
                    else
                    {
                        count[1]++;
                    }
                }
            }
            return count;
        }
    }
}
```



以下是生成 UI 界面的两个脚本



#### 10. ClickGUI.cs

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class ClickGUI : MonoBehaviour {
    UserAction action;
    MyCharacterController character;
	// 得到唯一的实例
	void Start () {
        action = Director.getInstance().sceneController as UserAction;
    }
    //鼠标点击触发不同事件
    void OnMouseDown()
    {
        if (action.stop())
            return;
        if (gameObject.name == "boat")
        {
            action.moveBoat();
        }
        else
        {
            action.characterIsClicked(character);
        }
    }
    //设置角色控制器
    public void setController(MyCharacterController characterCtrl)
    {
        character = characterCtrl;
    }
}
```



#### 11. UserGUI.cs

这个脚本相比之前做了一些改动，UI界面稍微改变了下，显示的内容更加丰富

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class UserGUI : MonoBehaviour {
    private UserAction action;
    public int status = 0; // -1表示失败，1表示成功
    GUIStyle style;
    GUIStyle style2;
    GUIStyle buttonStyle;
    public bool show = false;

    void Start()
    {
        action = Director.getInstance().sceneController as UserAction;
        style = new GUIStyle();
        style.fontSize = 15;
        style.alignment = TextAnchor.MiddleLeft;
        style2 = new GUIStyle();
        style2.fontSize = 30;
        style2.alignment = TextAnchor.MiddleCenter;
    }

    void OnGUI()
    {
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 15;
        if (status == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 65, 100, 50), "Gameover!", style2);   
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 20, 300, 50), "Game is over , Click to Restart", buttonStyle))
            {
                status = 0;
                action.restart();
                status = 0;
            }
        }
        else if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 65, 100, 50), "You win!", style2); 
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 20, 300, 50), "Game is over , Click to Restart", buttonStyle))
            {
                status = 0;
                action.restart();
                status = 0;
                //Debug.Log("Restart" + status);
            }
        }
        
        if (GUI.Button(new Rect(Screen.width / 2 + 200, 80, 130, 50), "Stop and restart", buttonStyle))
        {
            status = 0;
            action.restart();
            status = 0;
            //Debug.Log("Restart" + status);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 80, 100, 50), "Rule", buttonStyle))
        {
            show = true;
        }
        if (show)
        {
            GUI.Label(new Rect(Screen.width / 2 - 380, 20, 100, 100), "游戏规则：\n白色正方体为牧师，黑色球体为魔鬼。\n" +
            "船只最多能容纳两人，有人在船上才可开船\n" +
            "当某一岸恶魔数量大于牧师数量，游戏失败！\n" +
            "牧师与恶魔全部渡过河流，游戏胜利！\n", style);
        }
        //Debug.Log(status);
    }
}
```



在这次的实验中我们学以致用，添加了一个天空盒，让界面变得更加漂亮。

![image-20201012173934448](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012173934448.png)

运行情况如下，项目中有视频演示，也可观看视频

![image-20201012174034184](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012174034184.png)

![image-20201012174057681](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012174057681.png)





## 3、材料与渲染联系【可选】



### Standard Shader 自然场景渲染器

- 阅读官方 [Standard Shader](https://docs.unity3d.com/Manual/shader-StandardShader.html) 手册 。
- 选择合适内容，如 [Albedo Color and Transparency](https://docs.unity3d.com/Manual/StandardShaderMaterialParameterAlbedoColor.html)，寻找合适素材，用博客展示相关效果的呈现

具体操作：

1. 首先建一个 3D GameObject 这里建了一个 Cylinder
2. 建一个 Material 
3. 改变 Material 的颜色

![image-20201012213809778](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012213809778.png)

4. 将这个 Material 拖到 我们创建的 Cylinder 上，结果如下：

![image-20201012214220138](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012214220138.png)

5. 把 Rendering Mode 设成 Transparent ，然后改变 颜色中 A 的属性，即可看但透明度的变化

![image-20201012214352168](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012214352168.png)

![image-20201012214410577](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012214410577.png)

![image-20201012214419352](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012214419352.png)



### 声音

- 阅读官方 [Audio](https://docs.unity3d.com/Manual/Audio.html) 手册
- 用博客给出游戏中利用 Reverb Zones 呈现车辆穿过隧道的声效的案例

具体操作如下

1. 首先建一个名为 main 的空对象
2. 从 Assert Store 下载一个 音频的资源

![image-20201012221125026](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012221125026.png)

3. 给这个空的 main 对象添加 Audio Source，添加 AudioClip，并且勾选 Loop

![image-20201012221404676](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012221404676.png)

4. 添加 Audio Reverb Zone 组件， Reverb Preset 选择 Cave，即可呈现车辆穿过隧道的声效

![image-20201012221612230](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw4/img/image-20201012221612230.png)