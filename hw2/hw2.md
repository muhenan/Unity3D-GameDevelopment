# unity 3d 第二次作业


## 一、简答题

### 1.解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系



区别：

**游戏对象（GameObjects）**：Unity场景中所有实体的基类，可直接出现在游戏场景中，是资源整合的具体表现；对象一般有玩家，敌人，游戏场景，摄像机等虚拟父类，这些父类没有实例化，而他们的子类实例化并包含了这些游戏对象，我们可以对这些对象进行操作。即游戏中的那些实体。

**资源（Assets）**：资源可以是我们自定义或下载下来的素材，可以被多个对象使用，有些资源可以做为模板并实例化为对象。资源文件夹（Asset）通常包含脚本，预设，场景，声音等。即实现对象的一些辅助的东西。



联系：

游戏对象是资源的具体表现，可通过生成预设的方式变为资源。资源可被游戏对象使用，其中的预设可实例化为游戏对象。

简单来说就是，游戏对象是那些实际的类，他们构成游戏场景，而资源是一些辅助的东西，通常包含脚本，预设，场景，声音等，两者相辅相成，共同构成完整的游戏。



### 2.下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）

首先来到 unity Asset Store 的网站 https://assetstore.unity.com/ ，下载游戏案例

![image-20200921180140803](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200921180140803.png)

这里我选择下载的是  Low Poly Road Pack 游戏

![image-20200922084246765](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922084246765.png)

随后我在unity中打开游戏

首先我们先来看它的 Assets 组织情况

![image-20200922084451690](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922084451690.png)

可以看到，同一类的资源被放到了同一个文件夹里，如 图片，预设，模型，素材，场景等‘

接着来看它的对象的情况，同样是层次结构

![image-20200922084603033](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922084603033.png)

主要由 Map 和 House 两个游戏对象的大类构成游戏

![image-20200922084744971](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922084744971.png)

### 3.编写一个代码，使用 debug 语句来验证 [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) 基本行为或事件触发的条件

- 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
- 常用事件包括 OnGUI() OnDisable() OnEnable()

我们写一个脚本，在每个函数运行时，都让其输出相应的内容，来看运行情况

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    void Awake()
    {
        Debug.Log("Awake!");
    }
    void Start()
    {
        Debug.Log("Start!");
    }

    void Update()
    {
        Debug.Log("Update!");
    }
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate!");
    }
    void LateUpdate()
    {
        Debug.Log("LateUpdate!");
    }
    void Reset()
    {
        Debug.Log("Reset!");
    }
    void OnGUI()
    {
        Debug.Log("onGUI!");
    }
    void OnDisable()
    {
        Debug.Log("onDisable!");
    }
    void OnDestroy()
    {
        Debug.Log("onDestroy!");
    }
    void OnEnable()
    {
        Debug.Log("OnEnable");
    }
}
```

把这个脚本拖拽到一个 GameObject 上，运行后结果如下：

![image-20200922085756493](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922085756493.png)

![image-20200922085936281](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922085936281.png)

这里看到这些函数的执行顺序是：

Awake ->OnEable-> Start -> FixedUpdate-> Update  -> LateUpdate ->OnGUI ->OnDisable ->OnDestroy

下面来解释一下这几个函数的具体作用：

- **Awake：** 用于在游戏开始之前初始化变量或游戏状态。在脚本整个生命周期内它仅被调用一次。Awake在所有对象被初始化之后调用。
- **OnEnable：** 在脚本被载入后调用。
- **Start：** 仅在Update函数第一次被调用前调用。Start在behaviour的生命周期中只被调用一次。
- **FixedUpdate：** 固定帧更新，更新频率默认为0.02s。
- **Update：** 正常帧更新，用于更新逻辑，每一帧都执行。FixedUpdate比较适用于物理引擎的计算，因为是跟每帧渲染有关。Update就比较适合做控制。
- **LateUpdate：** 在所有Update函数调用后被调用，和fixedupdate一样都是每一帧都被调用执行，这可用于调整脚本执行顺序。
- **OnGUI：** 在渲染和处理GUI事件时调用。这意味着OnGUI也是每帧执行一次。
- **Reset：** 在用户点击检视面板的Reset按钮或者首次添加该组件时被调用。
- **OnDisable：** 当物体被销毁时 OnDisable将被调用，并且可用于任意清理代码。脚本被卸载时，OnDisable将被调用。
- **OnDestroy：** 当MonoBehaviour将被销毁时，这个函数被调用。OnDestroy只会在预先已经被激活的游戏物体上被调用。

### 4. 查找脚本手册，了解 [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)，Transform，Component 对象

#### 分别翻译官方对三个对象的描述（Description）

GameObjects：The fundamental object in Unity scenes. A Scene contains the environments and menus of your game. Think of each unique Scene file as a unique level. In each Scene, you place your environments, obstacles, and decorations, essentially designing and building your game in pieces. 

游戏对象是场景中的基本对象。场景包含游戏的环境和菜单。将每个唯一的场景文件视为独特个体，则在每一个场景中，放置环境、障碍物和装饰物的过程就是在以碎片的方式构建游戏场景。



 Transform：The Transform component determines the Position, Rotation, and Scale of each object in the scene.

变换组件决定了每个对象在场景中的位置，比例和旋转，每个对象都有一个变换组件。



Component：Base class for everything attached to GameObjects.

组件是一切附加到游戏对象上的东西的基类。

#### 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件

- 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。
- 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。

还是打开我刚刚下载的那个项目

![image-20200922091308802](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922091308802.png)

这个 Highway Join Highway 2 的对象是 GameObject，第一个选择框是 activeSelf 属性，后面下拉栏是isStatic属性，接下来是Layer、Tag、Transform属性。

下面的Transform属性，Position为 -580 0 -820，Rotation都为0，Scale 为 2 2 2。

 Highway Join Highway 2 的组件除了Transform，还有Mesh Filter、Box Colider、Mesh Renderer和Materials。

#### 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）

![image-20200922123548220](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922123548220.png)

### 5. 资源预设（Prefabs）与 对象克隆 (clone)

#### 预设（Prefabs）有什么好处？

预设可以当作是一个在代码未运行就创建好的一个物体，我们可以在代码里重复引用它，这样相当于实现了物体的多用（与我们写代码的重用很相似）。好处就是我们可以通过一个预设实例化多个对象，能够反复重用，让实例化也变得更容易。把一些基础的游戏对象组合起来做成预设，也便于我们快速开发游戏。预设有利于资源的重用，使用预设能够避免多次重复地构建对象。

#### 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？

预设不是真实的实例化的对象，通过预设进行实例化。当利用预设来创建一个新的对象时，这个对象相当于是从预设克隆而来的。预设和对象克隆的不同点在于，当修改预设时，由它实例化产生的多个对象也会跟着变化；修改克隆的对象时则不会发生这种变化，因为克隆得到的对象是相互独立的，彼此之间不会影响。

#### 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象

首先制作一个table预设

![image-20200922142849345](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922142849345.png)

编写一段实例化的代码

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBeh : MonoBehaviour
{

    public GameObject obj; //传入的预设

    // Use this for initialization
    void Start()
    {
        //参数一：是预设 参数二：实例化预设的坐标  参数三：实例化预设的旋转角度
        GameObject instance = (GameObject)Instantiate(obj, transform.position, transform.rotation);
        instance.name = "a table";
        //这里 transform.position，transform.rotation分别代表的是相机和坐标和 旋转角度
    }

    // Update is called once per frame
    void Update()
    {

    }
}
```

把这个脚本放在摄像机上，然后不要忘了添加预设

![image-20200922143825584](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922143825584.png)

运行以后，可以看到，在摄像机的位置我们实例化了预设

![image-20200922144952466](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922144952466.png)



## 二、 编程实践，小游戏



在这个模块，我选择做井字棋这个小游戏：

这里我的实现方法比较简单，首先建一个空白的 GameObject 命名为 main，然后创建一个脚本 NewBehaviourScript ，然后将 逻辑 和 UI 层的内容直接全部写在这个脚本中，随后将这个脚本拖拽到 main 这个游戏对象上，然后运行即可。

代码如下：

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GUISkin guiSkin;
    private int turn = 1;
    int[][] space = new int[3][] { new int[3], new int[3], new int[3] };
    // Use this for initialization
    void Start()
    {
        Debug.Log("start game!");
    }
    private void reset()
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                space[i][j] = 0;
            }
        }
        Debug.Log("reset is running");
    }
    //0表示未分出胜负，1表示玩家O获胜，-1表示玩家X获胜
    private int gameOver()
    {
        //横线
        for (int i = 0; i < 3; i++)
        {
            if (space[i][0] != 0 && space[i][0] == space[i][1] && space[i][1] == space[i][2])
            {
                return space[i][0];
            }
        }
        //纵线
        for (int i = 0; i < 3; i++)
        {
            if (space[0][i] != 0 && space[0][i] == space[1][i] && space[1][i] == space[2][i])
            {
                return space[0][i];
            }
        }
        //斜线
        if (space[1][1] != 0 && space[0][0] == space[1][1] && space[1][1] == space[2][2])
        {
            return space[1][1];
        }
        else if (space[1][1] != 0 && space[2][0] == space[1][1] && space[1][1] == space[0][2])
        {
            return space[1][1];
        }
        return 0; //尚未分出胜负
    }
    void OnGUI()
    {
        GUI.skin = guiSkin;
        GUI.Box(new Rect(200, 30, 320, 370), "Tic-Tac-Toe");
        if (GUI.Button(new Rect(310, 365, 100, 35), "Reset"))
            reset();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (space[i][j] == 1)
                {
                    GUI.Button(new Rect(210 + i * 100, 60 + j * 100, 100, 100), "O");
                }
                else if (space[i][j] == 2)
                {
                    GUI.Button(new Rect(210 + i * 100, 60 + j * 100, 100, 100), "X");
                }

                if (GUI.Button(new Rect(210 + i * 100, 60 + j * 100, 100, 100), ""))
                {
                    if (gameOver() == 0)
                    {
                        if (turn == 1)
                        {
                            space[i][j] = 1;
                            turn = 2;
                        }
                        else
                        {
                            space[i][j] = 2;
                            turn = 1;
                        }
                    }
                }
            }
        }
        if (gameOver() == 0)
        {
            GUI.Box(new Rect(310, 0, 100, 27), "Draw!");
        }
        else if (gameOver() == 1)
        {
            GUI.Box(new Rect(310, 0, 100, 27), "O win!");
        }
        else if (gameOver() == 2)
        {
            GUI.Box(new Rect(310, 0, 100, 27), "X win!");
        }
    }
}
```

实现主要功能的几个函数

* void reset()：用于将 turn 置成1，记录九宫格情况的 space 都置为0
* int gameOver()：用于判断比赛胜负，0表示未分出胜负，1表示玩家O获胜，-1表示玩家X获胜；具体的逻辑是判断横线，纵线，斜线会不会出现三个一样的情况，如果出现就返回相应的代表数字，如果最后未分胜负，则返回0
* OnGUI()：构建 UI 界面，构成整体的 Box 和各个 Button，在点击空白的 Button的时候（在游戏未结束的情况下），根据此时 turn 的情况，将这个位置对应的 space 进行相应的赋值，再根据 space 的值，将 Button 从空白变成 O 或者 X，最后判断游戏是否结束。

阅读 API 文档进行编程

![image-20200922200853920](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922200853920.png)

运行情况如下：

![image-20200922201043810](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922201043810.png)

![image-20200922201107816](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw2/img/image-20200922201107816.png)



## 三、思考题【选做】

- 微软 XNA 引擎的 Game 对象屏蔽了游戏循环的细节，并使用一组虚方法让继承者完成它们，我们称这种设计为“模板方法模式”。

  - 为什么是“模板方法”模式而不是“策略模式”呢？

  模板方法定义了一个过程或算法中的核心步骤，而对于子步骤或是子方法则是使用继承或接口在子类中实现，从而可以通过不改变整体而对一些子步骤进行重构；策略模式则是指对象具备某个行为，但是在不同的场景中，该行为有不同的实现算法。对于微软的XNA引擎，其屏蔽了游戏循环的细节，并使用一组虚方法由继承者完成，目的就是使得可以在不改变代码基本结构的情况下将一些具体模块交由继承者具体实现，这明显更符合“模板方法”，而非“策略模式”。

- 将游戏对象组成树型结构，每个节点都是游戏对象（或数）。

  - 尝试解释组合模式（Composite Pattern / 一种设计模式）。

  组合模式允许用户将对象组合成树形结构表现“整体-部分”的层次结构，使得客户以一致的方式处理单个对象以及对象的组合，组合模式实现的关键地方是单个对象与复合对象必须实现相同的接口，这就是组合模式能够将组合对象和简单对象进行一致处理的原因。 

  - 使用 BroadcastMessage() 方法，向子对象发送消息。你能写出 BroadcastMessage() 的伪代码吗?

  子类对象方法：

  ```c#
  	void test() {
           print("Hello!");
   	}
  ```

  父类对象方法：

  ```c#
   void Start () {
           this.BroadcastMessage("test");
   }
  ```

- 一个游戏对象用许多部件描述不同方面的特征。我们设计坦克（Tank）游戏对象不是继承于GameObject对象，而是 GameObject 添加一组行为部件（Component）。

  - 这是什么设计模式？

    Decorator模式

  - 为什么不用继承设计特殊的游戏对象？

  Decorator模式允许向一个现有的对象添加新的功能，同时又不改变其结构。使用多重继承容易使代码变得不易理解，层次结构变得很复杂。因此，我们应该可以使用更直接的模式，直接添加额外的东西。