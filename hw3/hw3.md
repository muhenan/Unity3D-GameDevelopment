# unity 3d 第三次作业



[TOC]

## 一、简答并用程序验证【建议做】



### 1. 游戏对象运动的本质是什么？

运动的本质是游戏对象通过脚本变化其（position）位置，（rotation）欧拉角，（scale）缩放。

以下这个脚本，可以让物体匀速直线往前走（同时既往前走又往左走）：

```c++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime;
        this.transform.position += Vector3.forward * Time.deltaTime;
    }
}

```

![image-20201005103712384](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005103712384.png)



### 2.请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）

**以下实现了三种方法**

#### （1）首先用一个最简单的 rigid body（刚体） 方式对物体施加一个重力效果而不用自己实现重力使物体做抛物线：

![image-20201005110437182](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005110437182.png)

在这里添加刚体组件，然后勾选重力，物体就会自己收到重力的影响

随后再给物体加一个初速度，并打印物体的位置情况，这个脚本的内容如下：

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRigid : MonoBehaviour
{

    private Vector3 speed;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime * 10;
        Debug.Log(this.transform.position);
    }
}
```

这样就完成了一个抛物线运动

为了看的清楚，这里对物体的位置和大小做了一些调整

![image-20201005111617567](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005111617567.png)

运行的效果如下：

![image-20201005111745994](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005111745994.png)

打印的位置情况如下：

![image-20201005112132135](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005112132135.png)

X轴是匀速直线运动，Y轴是重力导致的匀加速运行

#### （2）改变Transform属性

只用脚本实现即可，这个脚本的内容如下：

这里给了一个初速度，初速度在 y 轴的分量是10，在z轴的分量是5

```c#
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
```

图片难以看出具体的细微的位移，这里打印出的位置情况如下：

可以看到在 z 轴上是匀速直线运行，y 轴上刚开始是向正方向（向上）走，但速度越来越慢

![image-20201005114105008](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005114105008.png)

后来是在 y 轴上是向下的加速运动

![image-20201005114236058](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005114236058.png)

#### （3）使用Translate平移坐标，同时改变x，y方向的位置以及通过重力改变y方向的位置

同样是用一个脚本实现，实现的方法和 Transform 的内容类似，只是 transform.position 变成了 transform.Translate，脚本的内容如下：

```c#
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
```

打印的位置情况，如下， Y 轴先上升后下降，重力加速度导致的加速运动，X 轴上匀速运动

![image-20201005115329249](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005115329249.png)

![image-20201005115350737](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005115350737.png)



### 3.写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

* 首先建一个球体，用这个球体表示太阳，然后在这个对象下建 8 个球体，表示围绕太阳转动的 8 个星球
* 给这些星球分别贴上相应的图片
* 在太阳这个对象上加上一个控制所有星球转动的脚本，这个脚本的内容如下

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform neptune;
    void Start()
    {
        mercury.position = new Vector3(3, 0.5f, 0);
        venus.position = new Vector3(-5, 1, 0);
        earth.position = new Vector3(7, -0.5f, 0);
        mars.position = new Vector3(-9, 0, 0);
        jupiter.position = new Vector3(-11, -1, 0);
        saturn.position = new Vector3(13, 0.5f, 0);
        uranus.position = new Vector3(15, 1.5f, 0);
        neptune.position = new Vector3(-17, 0, 0);
    }

    void Update()
    {
        earth.RotateAround(this.transform.position, new Vector3(0, 0.99f, 0), 30 * Time.deltaTime);
        mercury.RotateAround(this.transform.position, new Vector3(0, 2.11f, 0), 47 * Time.deltaTime);
        venus.RotateAround(this.transform.position, new Vector3(0, 3.23f, 0), 35 * Time.deltaTime);
        mars.RotateAround(this.transform.position, new Vector3(0, 4.34f, 0), 24 * Time.deltaTime);
        jupiter.RotateAround(this.transform.position, new Vector3(0, 1.02f, 0), 13 * Time.deltaTime);
        saturn.RotateAround(this.transform.position, new Vector3(0, 0.98f, 0), 9 * Time.deltaTime);
        uranus.RotateAround(this.transform.position, new Vector3(0, 0.97f, 0), 6 * Time.deltaTime);
        neptune.RotateAround(this.transform.position, new Vector3(0, 0.96f, 0), 5 * Time.deltaTime);
        earth.Rotate(Vector3.up * Time.deltaTime * 250);
        mercury.Rotate(Vector3.up * Time.deltaTime * 300);
        venus.Rotate(Vector3.up * Time.deltaTime * 280);
        mars.Rotate(Vector3.up * Time.deltaTime * 220);
        jupiter.Rotate(Vector3.up * Time.deltaTime * 180);
        saturn.Rotate(Vector3.up * Time.deltaTime * 160);
        uranus.Rotate(Vector3.up * Time.deltaTime * 150);
        neptune.Rotate(Vector3.up * Time.deltaTime * 140);
    }

}
```

![image-20201005124400373](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005124400373.png)

* 为了方便观看，给每个星球都加上轨迹这个组件，具体内容如下

![image-20201005124519062](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005124519062.png)



最终的运行结果如下：



![image-20201005124109330](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005124109330.png)



## 二、编程实践



### 列出游戏中提及的事物（Objects）

* 魔鬼
* 牧师
* 河流
* 船
* 左岸
* 右岸



### 用表格列出玩家动作表（规则表），注意，动作越少越好

| 规则名称         | 条件                               |
| ---------------- | ---------------------------------- |
| 牧师或魔鬼上左岸 | 船已靠左岸，船上有牧师或魔鬼       |
| 牧师或魔鬼上右岸 | 船已靠右岸，船上有牧师或魔鬼       |
| 魔鬼上船         | 岸上有魔鬼，船未载满两人           |
| 牧师上船         | 岸上有牧师，船未载满两人           |
| 船只开动         | 船上有人                           |
| 击杀规则         | 一边的牧师数量少于魔鬼的数量       |
| 游戏胜利规则     | 所有角色从左岸到达右岸，且全部存活 |
| 游戏失败规则     | 牧师被击杀                         |



### 请将游戏中对象做成预制

![image-20201005132639173](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005132639173.png)



### **使用MVC结构编程**

按照下图的课件架构图编程

![image-20201005133008105](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005133008105.png)

分别写了四个脚本：

* UserGUI
* ClickGUI
* FirstControl
* Moveable



#### UserGUI

生成基础的 UI 界面

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
        if (status == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 65, 100, 50), "Gameover!", style2);   
        }
        else if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 65, 100, 50), "You win!", style2);  
        }
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 15;
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 20, 100, 50), "Restart", buttonStyle))
        {
            status = 0;
            action.restart();
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 80, 100, 50), "Rule", buttonStyle))
        {
            show = true;
        }
        if (show)
        {
            GUI.Label(new Rect(Screen.width / 2 + 70, 20, 100, 100), "游戏规则：\n白色正方体为牧师，黑色球体为魔鬼。\n" +
            "船只最多能容纳两人，有人在船上才可开船\n" +
            "当某一岸恶魔数量大于牧师数量，游戏失败！\n" +
            "牧师与恶魔全部渡过河流，游戏胜利！\n", style);
        }
    }
}
```



#### FirstControl

加载一些对象，对输赢做出判断，还有 restart , stop 函数

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class FirstController : MonoBehaviour, SceneController, UserAction {
    Vector3 water_pos = new Vector3(0, 0.5f, 0);
    Vector3 bac_pos = new Vector3(0, 0, 10);

    public CoastController leftCoast;
    public CoastController rightCoast;
    public BoatController boat;

    private MyCharacterController[] characters = null;
    private UserGUI userGUI = null;
    public bool flag_stop = false;

    void Awake()
    {
        Director director = Director.getInstance();
        director.sceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyCharacterController[6];
        load();
        flag_stop = false;
    }

    public void load()
    {
        GameObject water = Instantiate(Resources.Load("Perfabs/water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
        GameObject bac = Instantiate(Resources.Load("Perfabs/background", typeof(GameObject)), bac_pos, Quaternion.identity, null) as GameObject;
        bac.name = "background";
        water.name = "water";
        leftCoast = new CoastController("left");
        rightCoast = new CoastController("right");
        boat = new BoatController();
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
    public void moveBoat()
    {
        if (boat.isEmpty())
            return;
        boat.boat_move();
        userGUI.status = check_game_over();
    }
    int check_game_over()
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
    }

    public void characterIsClicked(MyCharacterController character)
    {
        //角色要上岸
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
            character.movePosition(coast.getEmptyPosition());
            character.Oncoast(coast);
            coast.getOnCoast(character);
        }
        // 角色要上船
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
            character.movePosition(boat.getEmptyPos());
            character.Onboat(boat);
            boat.GetOnBoat(character);
        }
        userGUI.status = check_game_over();
    }
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
    public bool stop()
    {
        if(check_game_over() != 0)
            return true;
        return false;
    }
}
```



#### Moveable

一些控制游戏中事件的函数，包括一些对这些对象的具体操作

```c#
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
        readonly Moveable moveable;
        readonly ClickGUI clickGUI;
        readonly bool is_priest;
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
            moveable = character.AddComponent(typeof(Moveable)) as Moveable;
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
            moveable.reset();
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
        public void movePosition(Vector3 position)
        {
            moveable.setDestination(position);
        }
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
        readonly Moveable moveable;
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
            moveable = boat.AddComponent(typeof(Moveable)) as Moveable;
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
        //船的移动函数，通过调用moveable中的setDestination函数
        public void boat_move()
        {
            if (is_left)
            {
                is_left = false;
                moveable.setDestination(right_pos);
            }
            else
            {
                is_left = true;
                moveable.setDestination(left_pos);
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
            moveable.reset();
            if(is_left == false)
            {
                boat_move();
            }
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
    //移动函数的类
    public class Moveable : MonoBehaviour
    {
        public float speed = 20;
        int status;  // 0->not moving, 1->moving to boat, 2->moving to dest
        Vector3 dest;
        Vector3 boat;
        void Update()
        {
            if(status == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, boat, speed * Time.deltaTime);
                if(transform.position == boat)
                    status = 2;
            }
            else if(status == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                if (transform.position == dest)
                    status = 0;
            }
        }
        //设置目的地
        public void setDestination(Vector3 pos)
        {
            dest = boat = pos;
            if (pos.y < transform.position.y)      
            {       
                boat.y = transform.position.y;
            }
            else if(pos.y > transform.position.y)
            {                               
                boat.x = transform.position.x;
            }
            status = 1;
        }
        public void reset()
        {
            status = 0;
        }
    }
}
```



#### ClickGUI

鼠标点击触发不同事件时，调用相应函数

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



运行情况如下

![image-20201005133829115](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005133829115.png)



其中摄像机的位置情况如下

![image-20201005134059541](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw3/img/image-20201005134059541.png)



## 三、思考题【选做】



使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等



对 Rotate 的拓展

```c#
void Rotate(Transform t, Vector3 axis, float angle)
{
  var rot = Quaternion.AngleAxis(angle, axis); // 获取并生成四元数

  // 进行变换
  t.position *= rot;
  t.rotation *= rot;
}
```



对 RotateAround 的拓展

```c#
void RotateAround(Transform t, Vector3 center, Vector3 axis, float angle)
{
  var position = t.position;
  var rot = Quaternion.AngleAxis(angle, axis);

  var direction = position - center;  // 两者插值，用于下一步进行变换
  direction = rot * direction;        // 方向
  t.position = center + direction;    // 得到新的位置
  t.rotation *= rot;                  // 新的旋转
}
```

