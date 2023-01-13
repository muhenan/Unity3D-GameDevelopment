# unity 3d 第五次作业

[TOC]

## 编写一个简单的鼠标打飞碟（Hit UFO）游戏



- 游戏内容要求：
  1. 游戏有 n 个 round，每个 round 都包括10 次 trial；
  2. 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
  3. 每个 trial 的飞碟有随机性，总体难度随 round 上升；
  4. 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
- 游戏的要求：
  - 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
  - 近可能使用前面 MVC 结构实现人机交互与游戏模型分离



这里是我设置的 n 为 3，即游戏为三回合

### 给场景加一个天空盒

首先为了使游戏更加好看，我们选择给场景加一个天空盒

我从 Assets Store 下载了一个名为 Customizable Skybox 的天空盒，然后导入到了新建的项目

![image-20201028140601921](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028140601921.png)

随后我选择在场景上添加一个天空盒

选择 **Window -> Rendering -> Lighting** ，然后选择到进行如下操作添加天空盒

![image-20201028140805325](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028140805325.png)



### 创建飞碟预设

创建三个飞碟预设，即做成类似飞碟的样子，三个飞碟预设的不同点在于 Scale 的 x，z 属性不同，通过这种方式来控制三个飞碟的大小不同

![image-20201028141212242](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028141212242.png)

![image-20201028141224801](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028141224801.png)



这里三个飞碟预设的区别仅为大小不同



### 编写一个简单的自定义 Component （选做）



这里选择做一个自定义的 Component ，来控制飞碟的：

* 类型号
* 分值
* 颜色

并把这个 自定义的 Component （即这个代码脚本）分别挂载到三个飞碟上，并进行一些不同的赋值

![image-20201028141925970](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028141925970.png)

例如我们把第二种飞碟设为类型号为2，分值为2，颜色为绿色

最后做成预设

这个自定义 Component 脚本代码如下：

#### Disk

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour {
    public int type = 1;
    public int score = 1;                               
    public Color color = Color.white;                    
}
```



### 其他代码编写

首先代码文件的结构如下（其中 Disk.cs 已经解说过）

![image-20201028142946206](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028142946206.png)



其中 SequenceAction，SSAction，SSActionManager 三个脚本属于这种结构的通用代码，具体作用在上一次作业中已有解说，代码内容也没有太多更改，基本是复用上次的代码。所以这里不再过多解说和粘贴代码。



#### FlyActionManager

飞碟的动作管理类，当场景控制器需要发射飞碟时就调用DiskFly使飞碟飞行。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : SSActionManager {
    public DiskFlyAction fly;  
    public FirstController scene_controller;           

    protected void Start() {
        scene_controller = (FirstController)SSDirector.GetInstance().CurrentScenceController;
        scene_controller.action_manager = this;     
    }

    //飞碟飞行
    public void DiskFly(GameObject disk, float angle, float power) {
        int lor = 1;
        if (disk.transform.position.x > 0) lor = -1;
        fly = DiskFlyAction.GetSSAction(lor, angle, power);
        this.RunAction(disk, fly, this);
    }
}
```



#### DiskFlyAction

飞碟飞行动作类，通过位置变换和角度变换模拟飞碟的飞行。当飞碟的高度在摄像机观察范围之下时则动作停止。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFlyAction : SSAction {
    public float gravity = -5;                                 //向下的加速度
    private Vector3 start_vector;                              //初速度向量
    private Vector3 gravity_vector = Vector3.zero;             //加速度的向量，初始时为0
    private Vector3 current_angle = Vector3.zero;              //当前时间的欧拉角
    private float time;                                        //已经过去的时间

    private DiskFlyAction() { }
    public static DiskFlyAction GetSSAction(int lor, float angle, float power) {
        //初始化物体将要运动的初速度向量
        DiskFlyAction action = CreateInstance<DiskFlyAction>();
        if (lor == -1) {
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else {
            action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update() {
        //计算物体的向下的速度,v=at
        time += Time.fixedDeltaTime;
        gravity_vector.y = gravity * time;

        //位移模拟
        transform.position += (start_vector + gravity_vector) * Time.fixedDeltaTime;
        current_angle.z = Mathf.Atan((start_vector.y + gravity_vector.y) / start_vector.x) * Mathf.Rad2Deg;
        transform.eulerAngles = current_angle;

        //如果物体y坐标小于-10，动作就做完了
        if (this.transform.position.y < -10) {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }

    public override void Start() { }
}
```



#### DiskFactory

维护两个列表，一个是正在使用的飞碟，一个是空闲飞碟。当场景控制器需要获取一个飞碟时，先在空闲列表中寻找可用的空闲飞碟，如果找不到就根据预制重新实例化一个飞碟。回收飞碟的逻辑为遍历使用列表，当有飞碟已经完成了所有动作，即位置在摄像机之下，则回收。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    private List<Disk> used = new List<Disk>();
    private List<Disk> free = new List<Disk>();

    public GameObject GetDisk(int type) {
        GameObject disk_prefab = null;
        //寻找空闲飞碟,如果无空闲飞碟则重新实例化飞碟
        if (free.Count>0) {
            for(int i = 0; i < free.Count; i++) {
                if (free[i].type == type) {
                    disk_prefab = free[i].gameObject;
                    free.Remove(free[i]);
                    break;
                }
            }     
        }

        if(disk_prefab == null) {
            if(type == 1) {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk1"),
                new Vector3(0, -10f, 0), Quaternion.identity);
            }
            else if (type == 2) {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk2"),
                new Vector3(0, -10f, 0), Quaternion.identity);
            }
            else {
                disk_prefab = Instantiate(
                Resources.Load<GameObject>("Prefabs/disk3"),
                new Vector3(0, -10f, 0), Quaternion.identity);
            }

            disk_prefab.GetComponent<Renderer>().material.color = disk_prefab.GetComponent<Disk>().color;
        }

        used.Add(disk_prefab.GetComponent<Disk>());
        disk_prefab.SetActive(true);
        return disk_prefab;
    }

    public void FreeDisk() {
        for(int i=0; i<used.Count; i++) {
            if (used[i].gameObject.transform.position.y <= -10f) {
                free.Add(used[i]);
                used.Remove(used[i]);
            }
        }          
    }

    public void Reset() {
        FreeDisk();
    }
}

```



#### ScoreRecorder

一个非常简单的类，来进行一些记录分数的操作，还包括 初始化，获取分数，和 reset 操作

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*记录分数*/
public class ScoreRecorder : MonoBehaviour {
    private float score;
    void Start () {
        score = 0;
    }
    public void Record(GameObject disk) {
        score += disk.GetComponent<Disk>().score;
    }
    public float GetScore() {
        return score;
    }
    public void Reset() {
        score = 0;
    }
}

```



#### FirstController

场景控制器，游戏开始之后，设置一个定时器，每隔一定时间从飞碟工厂中获取一个飞碟并发射，检测用户点击发送的射线是否与飞碟发生碰撞，有则通知记分员加分并且通知工厂回收飞碟。

Update函数每一帧检测鼠标点击，并根据round调整规则。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public FlyActionManager action_manager;
    public DiskFactory disk_factory;
    public UserGUI user_gui;
    public ScoreRecorder score_recorder;
    private int round = 1;                                                  
    private int trial = 0;
    private float speed = 1f;                                             
    private bool running = false;

    void Start () {
        SSDirector director = SSDirector.GetInstance();     
        director.CurrentScenceController = this;
        disk_factory = Singleton<DiskFactory>.Instance;
        score_recorder = Singleton<ScoreRecorder>.Instance;
        action_manager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
    }

    int count = 0;
	void Update () {
        if(running) {
            count++;
            if (Input.GetButtonDown("Fire1")) {
                //Debug.Log("sdfsdfdf");
                Vector3 pos = Input.mousePosition;
                Hit(pos);
            }
            switch (round) {
                case 1: {
                        if (count >= 150) {
                            count = 0;
                            SendDisk(1);
                            trial += 1;
                            if (trial == 10) {
                                round += 1;
                                trial = 0;
                            }
                        }
                        break;
                    }
                case 2: {
                        if (count >= 100) {
                            count = 0;
                            if (trial % 2 == 0) SendDisk(1);
                            else SendDisk(2);
                            trial += 1;
                            if (trial == 10) {
                                round += 1;
                                trial = 0;
                            }
                        }
                        break;
                    }
                case 3: {
                        if (count >= 50) {
                            count = 0;
                            if (trial % 3 == 0) SendDisk(1);
                            else if(trial % 3 == 1) SendDisk(2);
                            else SendDisk(3);
                            trial += 1;
                            if (trial == 10) {
                                running = false;
                            }
                        }
                        break;
                    }
                default:break;
            } 
            disk_factory.FreeDisk();
        }
    }

    public void LoadResources() {
        disk_factory.GetDisk(round);
        disk_factory.FreeDisk();
    }

    private void SendDisk(int type) {

        //从工厂中拿一个飞碟
        GameObject disk = disk_factory.GetDisk(type);

        //飞碟位置
        float ran_y = 0;
        float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
 
        //飞碟初始所受的力和角度
        float power = 0;
        float angle = 0;
        if (type == 1) {
            ran_y = Random.Range(1f, 5f);
            power = Random.Range(5f, 7f);
            angle = Random.Range(25f,30f);
        }
        else if (type == 2) {
            ran_y = Random.Range(2f, 3f);
            power = Random.Range(10f, 12f);
            angle = Random.Range(15f, 17f);
        }
        else {
            ran_y = Random.Range(5f, 6f);
            power = Random.Range(15f, 20f);
            angle = Random.Range(10f, 12f);
        }
        disk.transform.position = new Vector3(ran_x*16f, ran_y, 0);
        action_manager.DiskFly(disk, angle, power);
    }

    public void Hit(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<Disk>() != null) {
                score_recorder.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -10, 0);
            }
        }
    }

    public float GetScore() {
        return score_recorder.GetScore();
    }
    public int GetRound() {
        return round;
    }
    public int GetTrial() {
        return trial;
    }
    //重新开始
    public void ReStart() {
        running = true;
        score_recorder.Reset();
        disk_factory.Reset();
        round = 1;
        trial = 1;
        speed = 2f;
    }
    //游戏结束
    public void GameOver() {
        running = false;
    }
}

```

重要函数的具体解说：

SendDisk：SendDisk从工厂中拿飞碟并根据种类设置发射参数，然后调用动作管理器执行动作。

Hit：Hit函数检测射线与飞碟是否碰撞，如碰撞则计分并回收飞碟。



#### UserGUI

进行一些图形界面的布局和操作，根据用户行为调用相关函数

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    
    //每个GUI的style
    GUIStyle bold_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    GUIStyle over_style = new GUIStyle();
    private bool game_start = false;

    void Start () {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
	
	void OnGUI () {
        bold_style.normal.textColor = new Color(1, 0, 0);
        bold_style.fontSize = 16;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        over_style.normal.textColor = new Color(1, 0, 0);
        over_style.fontSize = 25;

        if (game_start) {

            GUI.Label(new Rect(Screen.width - 150, 5, 200, 50), "Scores:"+ action.GetScore().ToString(), text_style);
            GUI.Label(new Rect(100, 5, 50, 50), "Round:" + action.GetRound().ToString(), text_style);
            GUI.Label(new Rect(180, 5, 50, 50), "Trial:" + action.GetTrial().ToString(), text_style);

            if (action.GetRound() == 3 && action.GetTrial() == 10) {
                GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 - 250, 100, 100), "Game Over", over_style);
                GUI.Label(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 200, 50, 50), "Your scores are:" + action.GetScore().ToString(), text_style);
                if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.height / 2 - 150, 100, 50), "Restart")) {
                    action.ReStart();
                    return;
                }
                action.GameOver();
            }
        }
        else {
            GUI.Label(new Rect(Screen.width / 2 - 50, 100, 100, 100), "Hit UFO", over_style);
            GUI.Label(new Rect(Screen.width / 2 - 50, 150, 100, 100), "Click UFO", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, 200, 100, 50), "Game starts")) {
                game_start = true;
                action.ReStart();
            }
        }
    }
   
}

```



#### Interface

根据交互的需要定义函数

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController {
    void LoadResources();                                  
}

public interface IUserAction {
    void Hit(Vector3 pos);
    float GetScore();
    int GetRound();
    int GetTrial();
    void GameOver();
    void ReStart();
}
public enum SSActionEventType : int { Started, Competeted }
public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
```



#### Singleton

定义单例类

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) {
                    Debug.LogError("An instance of " + typeof(T)
                        + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
```



#### SSDirector

定义导演类

```c#
public class SSDirector : System.Object {
    private static SSDirector _instance;
    public ISceneController CurrentScenceController { get; set; }
    public static SSDirector GetInstance() {
        if (_instance == null) {
            _instance = new SSDirector();
        }
        return _instance;
    }
}
```



### 运行情况如下，项目中有视频演示，也可观看视频，视频在 gitee 项目中

![image-20201028150846447](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028150846447.png)

![image-20201028150859168](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028150859168.png)

![image-20201028150942911](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028150942911.png)

![image-20201028151000892](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw5/img/image-20201028151000892.png)



视频为 hw5.mp4