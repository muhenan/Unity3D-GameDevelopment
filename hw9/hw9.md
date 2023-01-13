# Unity 3D 第九次作业

[TOC]

本次的作业是五选一，我选择的是第二个UI 效果制作中的 [Inventor](http://www.tasharen.com/ngui/exampleX.html) 背包系统。需要使用 UGUI 实现类似下图的效果：

![官方示例](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/6177e8b1gw1f42sxsw6drg20z80i9npm.gif)

要实现的东西有：

1. 实现背包的主要页面。
2. 背包系统中放置物品的格子和物品图片。
3. 实现鼠标拖动物体调整背包。
4. 简单的动画效果，随鼠标的移动展示一定的动画

最终实现的效果如下图所示：

![image-20201221142713027](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221142713027.png)



## 1. 设计 UI 界面

首先设置了一个天空盒：

![image-20201221145612368](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145612368.png)

![image-20201221145629592](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145629592.png)

接下来是UI界面

我们看到的整个 UI 界面主要分为以下三部分：

1. 中间的小女孩
2. 物品栏：
   1. 左侧三个代表穿戴的物品栏
   2. 右侧九个代表背包的物品栏
3. 五个物品（衣服）



### 1.1 中间的小女孩

这里用了之前作业7的巡逻兵中用到的小女孩的预设

![image-20201221143137951](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221143137951.png)

![image-20201221143203964](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221143203964.png)

我们首先把这个预设添加到游戏的场景中，然后改变其位置，使其能够出现在摄像机中适当的位置：

![image-20201221143932346](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221143932346.png)

![image-20201221143943176](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221143943176.png)

![image-20201221143956122](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221143956122.png)

调用适当的动画，使其看起来是在动而不是完全静止

![image-20201221144118064](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221144118064.png)

![img](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/15-20-46-12-21-23382.gif)



### 1.2 物品栏

首先创建画布，并在画布下创建适当的 UI Image

![image-20201221144811579](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221144811579.png)

这里的 BackPack 和 Inventory 都是画布，Bag 下面是右侧的九个物品栏，Wear下面是左边的三个物品栏。

给 BackPack 画布设置屏幕空间为摄像机，渲染摄像机为主摄像机：

![image-20201221145056663](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145056663.png)

给 Bag 和 Wear 两个组件添加组件Grid LayOut Group实现自动布局：

Bag：

![image-20201221145318156](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145318156.png)

Wear：

![image-20201221145334904](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145334904.png)

然后在这两个对象下添加 UI Image 对象，完成物品栏的设计，最后的效果如下图所示：

![image-20201221145532722](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221145532722.png)



### 1.3 物品

在 Inventory 画布下添加物品

![image-20201221150023893](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221150023893.png)

对 Items 中的每一个物品都单独设置好适当的位置，添加图片，设置 Canvas Group 组件，使其能够交互。

![image-20201221150212458](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221150212458.png)

![image-20201221150232600](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221150232600.png)

设置完成后，整 UI 界面的效果如下图所示：

![image-20201221150334232](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/image-20201221150334232.png)



## 2. 代码部分

到现在为止我们已经有了需要的所有 UI 界面组件，接下来我们通过添加代码来实现要实现的功能。

这里主要是两个代码脚本，实现了两个功能：

* FollowMouse.cs：实现所有UI组件能够跟随鼠标进行微小的移动，这样也就实现了界面的动画效果。
* DragItem.cs：用于五个物品，实现拖拽和交换的操作。



### 2.1 FollowMouse.cs

建立一个脚本获取鼠标位置然后将UI 组件的朝向设为鼠标的方向：

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    public Vector2 range = new Vector2(8f, 4f);
    Transform mTrans;
    Quaternion mStart;
    Vector2 mRot = Vector2.zero;
    void Start()
    {
        mTrans = transform;
        mStart = mTrans.localRotation;
    }
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;
        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -0.4f, 0.4f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -0.4f, 0.4f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 2f);
        //旋转方向与鼠标所在方向一致
        mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, -mRot.x * range.x, 0f);
    }
}

```

将这个代码拖拽到 Inventory 这个画布上，即使得整个画布都能够跟随鼠标方向来旋转。

效果如下图所示：

![img](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/15-17-52-12-21-22814.gif)



### 2.2 DragItem.cs

首先这里有三个与拖拽有关的接口IBeginDragHandler, IDragHandler, IEndDragHandler，实现的拖拽类必须继承这些接口，然后再具体实现。

主要是函数有三个：

1. OnBeginDrag
2. OnDrag
3. OnEndDrag

* 首先是一些变量的声明：

```c#
    private Transform myTransform;
    private RectTransform myRectTransform;

    // 用于event trigger对自身检测的开关
    private CanvasGroup canvasGroup;

    // 拖拽操作前的有效位置，拖拽到有效位置时更新
    public Vector3 originalPosition;

    // 记录上一帧所在物品格子
    private GameObject lastEnter = null;

    // 记录上一帧所在物品格子的正常颜色
    private Color lastEnterNormalColor;

    // 拖拽至新的物品格子时，该物品格子的高亮颜色
    private Color highLightColor = Color.green;
```

* Start 函数，对一些变量进行赋值：

```c#
    void Start()
    {
        myTransform = this.transform;
        myRectTransform = this.transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = myTransform.position;
    }
```

* OnBeginDrag函数在开始拖动时调用，设置显示优先级以及记录起点位置。

```c#
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;//让event trigger忽略自身，这样才可以让event trigger检测到它下面一层的对象,如包裹或物品格子等
        lastEnter = eventData.pointerEnter;
        lastEnterNormalColor = lastEnter.GetComponent<Image>().color;
        originalPosition = myTransform.position;//拖拽前记录起始位置
        gameObject.transform.SetAsLastSibling();//保证当前操作的对象能够优先渲染，即不会被其它对象遮挡住
    }
```

* OnDrag函数实现了Image对象的拖拽效果，实现当前指着的格子的高亮。

```c#
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(myRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            myRectTransform.position = globalMousePos;
        }
        GameObject curEnter = eventData.pointerEnter;
        bool inItemGrid = EnterItemGrid(curEnter);
        if (inItemGrid)
        {
            Image img = curEnter.GetComponent<Image>();
            lastEnter.GetComponent<Image>().color = lastEnterNormalColor;
            if (lastEnter != curEnter)
            {
                lastEnter.GetComponent<Image>().color = lastEnterNormalColor;
                lastEnter = curEnter;//记录当前物品格子以供下一帧调用
            }
            //当前格子设置高亮
            img.color = highLightColor;
        }
    }
```

* OnEndDrag函数则是最后停止拖拽时进行判断，分为拖拽到了方格位置，另一个物品对象上和空位置三种情况。分别进行判断处理。
  * 空位置 -> 物品回到最初的位置。
  * 方格位置 -> 物品的位置变成这个格子的位置，这个格子的位置恢复正常
  * 另一个物品对象上 -> 实现交换操作

```c#
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject curEnter = eventData.pointerEnter;
        //拖拽到的空区域中（如包裹外），恢复原位
        if (curEnter == null)
        {
            myTransform.position = originalPosition;
        }
        else
        {
            //移动至物品格子上
            if (curEnter.name == "UI_ItemGrid")
            {
                myTransform.position = curEnter.transform.position;
                originalPosition = myTransform.position;
                curEnter.GetComponent<Image>().color = lastEnterNormalColor;//当前格子恢复正常颜色
            }
            else
            {
                //移动至包裹中的其它物品上
                if (curEnter.name == eventData.pointerDrag.name && curEnter != eventData.pointerDrag)
                {
                    Vector3 targetPostion = curEnter.transform.position;
                    curEnter.transform.position = originalPosition;
                    myTransform.position = targetPostion;
                    originalPosition = myTransform.position;
                }
                else//拖拽至其它对象上面（包裹上的其它区域）
                {
                    myTransform.position = originalPosition;
                }
            }
        }
        lastEnter.GetComponent<Image>().color = lastEnterNormalColor;//上一帧的格子恢复正常颜色
        canvasGroup.blocksRaycasts = true;//确保event trigger下次能检测到当前对象
    }
```

* 一个通用函数 EnterItemGrid ，用来判断鼠标指针是否指向包裹中的物品格子

```c#
    // 判断鼠标指针是否指向包裹中的物品格子
    // <param name="go">鼠标指向的对象</param>
    bool EnterItemGrid(GameObject go)
    {
        if (go == null)
        {
            return false;
        }
        return go.name == "UI_ItemGrid";
    }
```

把这个 DragItem.cs 脚本拖拽到物品 Image 上即可。



## 3. 最终效果

最终的运行结果如下面的动图所示：

![img](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw9/img/15-49-4-12-21-28927.gif)