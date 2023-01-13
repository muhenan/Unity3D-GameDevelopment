# Unity 3D 第八次作业



[toc]

本次作业基本要求是**三选一**

1、简单粒子制作

- 按参考资源要求，制作一个粒子系统，[参考资源](http://www.cnblogs.com/CaomaoUnity3d/p/5983730.html)
- 使用 3.3 节介绍，用代码控制使之在不同场景下效果不一样

2、完善官方的“汽车尾气”模拟

- 使用官方资源资源 Vehicle 的 car， 使用 Smoke 粒子系统模拟启动发动、运行、故障等场景效果

3、参考 http://i-remember.fr/en 这类网站，使用粒子流编程控制制作一些效果， 如“粒子光环”



我选择的作业是第三个，粒子光环

并且自己完善了一下粒子海洋这个小项目

所以这次的报告主要分为两部分：

1. 粒子光环
2. 粒子海洋



## 一、粒子光环

### 参考 http://i-remember.fr/en 的粒子光环

首先说一下基本的效果，它是两个粒子环在转动，然后环的某一部分比较亮，亮部以比粒子更快的速度在旋转，有一种“涌动”的效果。鼠标移动到中间的按钮上光环会以一种很好看的方式迅速收缩，收缩之后有一种刹车的感觉。

本次的实验的代码量比较少，主要是进行一些布局。



### 光环实现逻辑

实现的运动是一个环中每个粒子朝同一方向做圆周运动，切向的速度分成几个层次，每个层次不一样，同时每个粒子在径向做不规则的抖动。

鼠标移动到中心时整个环收缩，移开后再展开。



### 代码部分

代码只有一个脚本：

* Halo.cs

首先定义一个新的 CirclePosition 脚本，用来记录每个粒子的当前半径、角度和时间，其中时间是做游离运动需要的。

~~~c#
  public class CirclePosition {
      public float radius = 0f, angle = 0f, time = 0f;
      public CirclePosition(float radius, float angle, float time) {
          this.radius = radius;   // 半径
          this.angle = angle;     // 角度
          this.time = time;       // 时间
      }
  }
~~~

然后是一些要用到的基本的属性

  ~~~c#
    private ParticleSystem particleSys;                             // 粒子系统
    private ParticleSystem.Particle[] particleArr;                  // 粒子数组
    private CirclePosition[] circle;                                // 极坐标数组
    public int pCount = 5000;                                       // 粒子数量
    public float pSize = 0.15f;                                     // 粒子大小
    public float minRadius = 4.0f;                                  // 最小半径
    public float maxRadius = 11.0f;                                 // 最大半径
    public bool clockwise = true;                                   // 旋转方向
    public float speed = 1f;                                        // 速度
    public float pingPong = 0.01f;                                  // 游离范围
    public int tier = 10;                                           // 速度差分层数

    public Camera camera;                                           // 主摄像机
    private Ray ray;                                                // 射线
    private RaycastHit hit;
   
    private float[] before;                                         // 收缩前粒子位置
    private float[] after;                                          // 收缩后粒子位置
    public float shrinkSpeed = 5f;                                  // 粒子缩放的速度
    private bool ischange = false;                                  // 是否收缩

    public Gradient colorGradient;                                  // 颜色渐变
    private GradientAlphaKey[] alphaKeys;                           // 透明度
    private GradientColorKey[] colorKeys;                           // 颜色
  ~~~

初始化函数，在光环中间部分粒子分布多一些，边缘少一些

```c#
    void RandomlySpread() {
        // 初始化各粒子位置
        // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近
        for (int i = 0; i < pCount; ++i) {  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            // 随机每个粒子的角度
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
            // 随机每个粒子的游离起始时间
            float time = Random.Range(0.0f, 360.0f);
            circle[i] = new CirclePosition(radius, angle, time);
            before[i] = radius;
            after[i] = 0.7f * radius;
            if (after[i] < minRadius * 1.1f) {
                after[i] = Random.Range(Random.Range(minRadius, midRadius), (minRadius * 1.1f));
            }
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }
        particleSys.SetParticles(particleArr, particleArr.Length);
    }
```

改变颜色的函数：(根据时间和位置)

```c#
    void ChangeColor() {
        float colorValue;
        for (int i = 0; i < pCount; i++) {
            //改变颜色
            colorValue = (Time.realtimeSinceStartup - Mathf.Floor(Time.realtimeSinceStartup))/2;
            colorValue += circle[i].angle / 360;
            if (colorValue > 1) colorValue -= 1;
            particleArr[i].startColor = colorGradient.Evaluate(colorValue);
        }
    }
```

Update函数：

* 切向运动：让光环旋转起来，在Update里逐渐改变每个粒子地角度。同时用上差分层数tier让每一层的粒子速度都不同，`i % tier + 1`中的 “+1” 是为了不出现不运动的粒子。

~~~c#
if (clockwise) circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier);
else circle[i].angle += (i % tier + 1) * (speed / circle[i].radius / tier);
// 保证angle在0~360度
circle[i].angle = (360.0f + circle[i].angle) % 360.0f; 

float theta = circle[i].angle / 180 * Mathf.PI;
particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta)); 
~~~

* 径向不规则运动：让每个粒子不那么死板，随机抖动，用了Mathf类的PingPong方法。

~~~C#
// 粒子在半径方向上游离
circle[i].time += Time.deltaTime;
circle[i].radius += Mathf.PingPong(circle[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;
~~~

​	这里的pingpong是游离范围，最后`- pingpong / 2.0f`是为了radius有增有减。

* 径向随鼠标收缩运动：

  这里添加了一个空对象，然后在这个对象上添加 Sphere Collider 组件用来检测鼠标的进出，稍后的布局部分有详细解说

~~~C#
/*增加成员*/
private float[] before;                                         // 收缩前粒子位置
private float[] after;                                          // 收缩后粒子位置
public float shrinkSpeed = 5f;                                  // 粒子缩放的速度
private bool ischange = false;                                  // 是否收缩

/*初始化RandomlySpread*/
before[i] = radius;
after[i] = 0.7f * radius;
if (after[i] < minRadius * 1.1f) {
    after[i] = Random.Range(Random.Range(minRadius, midRadius), (minRadius * 1.1f));
}

/*更新Update*/
if (ischange) {
    // 开始收缩
    if (circle[i].radius > after[i]) {
        circle[i].radius -= shrinkSpeed * (circle[i].radius / after[i]) * Time.deltaTime;
    }
}
else {
    // 开始还原
    if (circle[i].radius < before[i]) {
        circle[i].radius += shrinkSpeed * (before[i] / circle[i].radius) * Time.deltaTime;
    }
}
~~~



### 布局结构

![image-20201130004554985](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130004554985.png)

* halo 是一个空对象
  * sensor 只带一个 Sphere Collider 组件，来检测鼠标的进出，然后发生相应的动作
  * outer  带 Particle System 和 代码脚本，形成外圈光环
  * inner 带 Particle System 和 代码脚本，形成内圈光环



sensor

![image-20201130004844798](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130004844798.png)

![image-20201130004911128](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130004911128.png)

outer

它发射粒子的形式如图所示

![image-20201130004955236](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130004955236.png)

一些组件和参数如下

![image-20201130005044248](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005044248.png)

![image-20201130005104316](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005104316.png)

![image-20201130005123538](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005123538.png)

挂载了代码脚本，一些初始化的变量如下：

![image-20201130005155964](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005155964.png)

inner 的情况与 outer类似，但参数有些变化，这里不再展示



### 运行结果

最后的运行结果如下图

正常运行的情况，由于粒子运动情况的变化，可以看到类似涌动的现象

![image-20201130005447251](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005447251.png)

鼠标放到中间后压缩的情况：

![image-20201130005553231](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130005553231.png)

在鼠标进出的过程中可以看到粒子压缩与展开的运动情况





## 二、粒子海洋

### 代码部分

进行了一些初始化，设置了粒子的运动情况，并且有规律的设置了粒子的起伏情况，也会伴随一些噪声。

```c#
using UnityEngine;
using System.Collections;

public class ParticleSea : MonoBehaviour {

    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;

    public int seaResolution = 100;
    public float spacing = 0.3f;
    public float noiseScale = 0.05f;
    public float heightScale = 3f;
    private float perlinNoiseAnimX = 0.01f;
    private float perlinNoiseAnimY = 0.01f;

    public Gradient colorGradient;

    [System.Obsolete]
    void Start() {
        particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution];
        particleSystem.maxParticles = seaResolution * seaResolution;
        particleSystem.Emit(seaResolution * seaResolution);
        particleSystem.GetParticles(particlesArray);
    }

    void Update() {
        for (int i = 0; i < seaResolution; i++) {
            for (int j = 0; j < seaResolution; j++) {
                float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY);
                particlesArray[i * seaResolution + j].startColor = colorGradient.Evaluate(zPos);
                particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos * heightScale, j * spacing);
                
            }
        }

        perlinNoiseAnimX += 0.01f;
        perlinNoiseAnimY += 0.01f;

        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

}
```



### 结构布局

添加一个粒子系统的对象，并且在这个对象上挂在代码脚本

![image-20201130011702983](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130011702983.png)

![image-20201130011734503](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130011734503.png)

对粒子系统进行一些设置：

![image-20201130011912335](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130011912335.png)

注意这里的形状

![image-20201130011943215](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130011943215.png)

![image-20201130012005009](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130012005009.png)

![image-20201130012019216](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130012019216.png)

挂载代码脚本并进行一些变量的初始化

![image-20201130012046502](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130012046502.png)

最后的运行结果如下

![image-20201130012127810](https://gitee.com/mu-he-nan/Unity3D/raw/master/hw8/img/image-20201130012127810.png)