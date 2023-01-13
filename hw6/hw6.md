# Unity 3d 第六次作业

[TOC]

## 改进飞碟（Hit UFO）游戏：



- 游戏内容要求：
  1. 按 *adapter模式* 设计图修改飞碟游戏
  2. 使它同时支持物理运动与运动学（变换）运动



这个作业的内容，主要是改进上次的打飞碟游戏，使其能够支持物理运动与运动学（变换）运动的切换，我们只需要在之前实验的基础上进行一些改动即可。

代码方面主要的改动是，在上次作业的基础上增加使用物理引擎的动作管理类，并在场景控制器中对两个动作管理类进行选择即可。



**这里天空盒的设计，与上次相同，这里不再赘述。**



飞碟预设的设计在上次的基础上增加了刚体 Component

![image-20201110212902981](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201110212902981.png)



首先，若要使用物理引擎，我们需要使用刚体（Rigidbody）组件，刚体组件不能通过Update()函数来刷新，Update()的调用速率默认是60次/秒，受到机器性能和被渲染物体的影响，但是物理引擎渲染是一个固定的时间，是可以设置的。

在Edit->ProjectSetting->Time:

![image-20201110213147716](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201110213147716.png)

对于刚体的更新我们需要用FixedUpdate()函数来实现。



下面的代码的修改：


- 在SSActionManager.cs中，加上FixedUpdate，使动作管理类可以适配物理引擎：

  ~~~c#
  //对于字典中每一个pair，看是执行还是删除
  foreach (KeyValuePair<int, SSAction> kv in actions) {
      SSAction ac = kv.Value;
      if (ac.destroy) {
          waitingDelete.Add(ac.GetInstanceID());
      }
      else if (ac.enable) {
          ac.Update();
          ac.FixedUpdate(); //-----适配物理引擎
      }
  }
  ~~~

  

- 在SSAction.cs中同样加上FixedUpdate()。

  ~~~C#
  public virtual void Update() {
      throw new System.NotImplementedException();
  }
  //-----适配物理引擎
  public virtual void FixedUpdate() {
      throw new System.NotImplementedException();
  }
  ~~~

  

- PhysisDiskFlyAction.cs

  与DiskFlyAction大同小异，将所有操作都放进FixedUpdate()，当飞碟落到屏幕外时将飞碟的速度清空。Start()中对飞碟施加一个冲击力（Impulse）。

  ~~~c#
  public class PhysisDiskFlyAction : SSAction {
      private Vector3 start_vector;                              
      public float power;
      private PhysisDiskFlyAction() { }
      public static PhysisDiskFlyAction GetSSAction(int lor, float power) {
          PhysisDiskFlyAction action = CreateInstance<PhysisDiskFlyAction>();
          if (lor == -1) action.start_vector = Vector3.left * power;
          else action.start_vector = Vector3.right * power;
          action.power = power;
          return action;
      }
  
      public override void Update() { }
  
      public override void FixedUpdate() {
          if (transform.position.y <= -10f) {
              gameobject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
              this.destroy = true;
              this.callback.SSActionEvent(this);
          }
      }
  
      public override void Start() {
          gameobject.GetComponent<Rigidbody>().AddForce(start_vector*3, ForceMode.Impulse);
      }
  }
  ~~~

  

- FlyActionManager.cs

  在动作管理器中加上使用物理引擎的动作并重载DiskFly函数，当不使用物理引擎时启用isKinematic运动学变换，当使用物理引擎时关闭运动学变换：

  ~~~c#
  //飞碟飞行
  public void DiskFly(GameObject disk, float angle, float power) {
      disk.GetComponent<Rigidbody>().isKinematic = true;
      int lor = 1;
      if (disk.transform.position.x > 0) lor = -1;
      fly = DiskFlyAction.GetSSAction(lor, angle, power);
      this.RunAction(disk, fly, this);
  }
  
  public void DiskFly(GameObject disk, float power) {
      disk.GetComponent<Rigidbody>().isKinematic = false;
      int lor = 1;
      if (disk.transform.position.x > 0) lor = -1;
      fly_ph = PhysisDiskFlyAction.GetSSAction(lor, power);
      this.RunAction(disk, fly_ph, this);
  }
  ~~~

  

- 在FirstController中可切换：

  首先增加了一个 bool 变量，可以用来从外部控制是否使用物理引擎：

  ```c#
      public bool UsePhysicalEngine;
  ```

  我们根据这个变量的情况来进行切换即可：

  ~~~c#
          if(UsePhysicalEngine){
              action_manager.DiskFly(disk, power);
          }else{
              action_manager.DiskFly(disk, angle, power);
          }
  ~~~

* 其他代码基本无变动，详情可见仓库中的代码（**Hit UFO v2 文件夹**）。



在外部通过如下勾选的情况来切换是否使用物理引擎：

![image-20201110213912066](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201110213912066.png)

本次的改动主要是充分的了解了物理引擎，在开发的过程中，也确实发现使用物理引擎更加简单，不要很多复杂的运动学公式。

一些运行截图：

![image-20201111001037630](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111001037630.png)

![image-20201111001113227](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111001113227.png)

![image-20201111001151479](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111001151479.png)

![image-20201111001217486](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111001217486.png)



***



## 打靶游戏（**可选作业**）：

游戏内容要求：

1. 靶对象为 5 环，按环计分；
2. 箭对象，射中后要插在靶上
   - **增强要求**：射中后，箭对象产生颤抖效果，到下一次射击 或 1秒以后
3. 游戏仅一轮，无限 trials；
   - **增强要求**：添加一个风向和强度标志，提高难度



首先通过借助一些网上的现有材料来做出一些预设：

![image-20201111004206415](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111004206415.png)

箭要加上刚体 Component

![image-20201111004240605](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111004240605.png)



然后就是代码部分，这里我们是用的物理引擎。

代码的结构如下：

![image-20201111005343351](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111005343351.png)



### FirstSenceController

主要的内容在FirstSenceController中：

首先是Update函数，使用LookAt函数将弓箭转向鼠标的方向，如果没有箭则从工厂中取出一支箭放在弓上，检测鼠标左键按下则射箭。

```c#
 void Update () {
     if(game_start) {
         Vector3 mpos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
         if (Input.GetButtonDown("Fire1")) {
             Shoot(mpos * 15 );
         }
         if (arrow == null) {
             arrow = arrow_factory.GetArrow();
             arrow.transform.position = bow.transform.position;
             arrow.gameObject.SetActive(true);
             arrow.GetComponent<Rigidbody>().isKinematic = true;
         }
         bow.transform.LookAt(mpos * 30);
         arrow.transform.LookAt(mpos * 30);
         arrow_factory.FreeArrow();
     }
 }

```

Shoot函数：通知动作管理器根据初始的力和风力射箭，并且设置新的更强的风向。

```c#
 public void Shoot(Vector3 force) {
     if (arrow != null) {
         arrow.GetComponent<Rigidbody>().isKinematic = false;
         action_manager.ArrowFly(arrow, wind, force);
         child_camera.GetComponent<ChildCamera>().StartShow();
         arrow = null;
         CreateWind();
         round++;
     }
 }

```

CreateWind函数：随机生成不同的风

```c#
    public void CreateWind() {
        float wind_directX = ((Random.Range(-10, 10) > 0) ? 1 : -1) * round;
        float wind_directY = ((Random.Range(-10, 10) > 0) ? 1 : -1) * round;
        Debug.Log(wind_directX);
        wind = new Vector3(wind_directX, wind_directY, 0);

        string Horizontal = "", Vertical = "", level = "";
        if (wind_directX > 0) {
            Horizontal = "西";
        } else if (wind_directX <= 0) {
            Horizontal = "东";
        }
        if (wind_directY > 0) {
            Vertical = "南";
        } else if (wind_directY <= 0) {
            Vertical = "北";
        }
        level = round.ToString();
        wind_name = Horizontal + Vertical + "风" + " " + level;
    }
```



### 箭飞行的动作 ArrowFlyAction

箭飞行的动作很简单，给一个冲击力和风力即可。

```c#
public class ArrowFlyAction : SSAction {
    public Vector3 _force;
    public Vector3 _wind;

    private ArrowFlyAction() { }
    public static ArrowFlyAction GetSSAction(Vector3 wind, Vector3 force) {
        ArrowFlyAction action = CreateInstance<ArrowFlyAction>();
        action._force = force;
        action._wind = wind;
        return action;
    }
    public override void Start() {
        gameobject.GetComponent<Rigidbody>().AddForce(_force, ForceMode.Impulse);
        gameobject.GetComponent<Rigidbody>().AddForce(_wind);
    }

    public override void Update() {}

    public override void FixedUpdate(){
        if (transform.position.y < -30) {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}

```



### CollisionDetection

检测碰撞，将其挂在靶子的每一环上，使用OnTriggerEnter函数检测碰撞，碰撞之后将箭头隐藏并且将箭设置为运动学停止其运动。

```c#
public class CollisionDetection : MonoBehaviour {
    public FirstSceneController scene_controller;
    public ScoreRecorder recorder;

    void Start() {
        scene_controller = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
        recorder = Singleton<ScoreRecorder>.Instance;
    }

    void OnTriggerEnter(Collider arrow_head) {
        Transform arrow = arrow_head.gameObject.transform.parent;
        if (arrow == null) return;
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        arrow_head.gameObject.SetActive(false);
        recorder.Record(this.gameObject);
    }
}

```



### ChildCamera

用来控制副摄像机的显示情况

```c#
public class ChildCamera : MonoBehaviour
{   
    public bool isShow = false;                   //是否显示副摄像机
    public float leftTime;                        //显示时间

    void Update()
    {
        if (isShow)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                this.gameObject.SetActive(false);
                isShow = false;
            }
        }
    }

    public void StartShow()
    {
        this.gameObject.SetActive(true);
        isShow = true;
        leftTime = 2f;
    }
}
```



### RingData

一个来的类，来表示击中得分

```c#
public class RingData : MonoBehaviour {
    public int score = 1;                               //射击此环得分
}
```



除以上几个类以外，其他的类都类似改进后的 打飞碟的代码。



运行截图

![image-20201111010745073](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw6/img/image-20201111010745073.png)



## 总结

总体而言，这两个小的实验都使用了物理引擎，通过这两个实验，我对物理引擎的使用和刚体的一些知识有了更深刻的认识，这种方法大大简化了物体的运动，只要给其对应的输入即可，然后便可以按刚体的运动规律运动，这种写法在模拟实际的物体时非常有效，是一个有力的方案。

第二个实验也让我更加清楚了两个摄像机具体是怎么操作，更好的显示我们的游戏。