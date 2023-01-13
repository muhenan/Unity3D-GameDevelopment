# 3D游戏第三次作业



### 作业三仓库说明：

* 脚本均在 code 文件夹中
* 博客中用到的图片在 img 文件夹中
* 博客为 hw3.md
* 用来运行的牧师与魔鬼的 Priests and Devils 文件夹下的 Assert 文件夹
* 用来运行太阳系的 Sun 文件下的 Assert 文件夹



### 如果要查看代码：

游戏对象运动的本质是什么？ 对应脚本是 Move.cs

抛物线运动对应的三个脚本：

* ParabolaRigid.cs
* ParabolaTransform.cs
* ParabolaTranslate.cs

太阳系对应的脚本： SolarSystem.cs

牧师与魔鬼对应的脚本：

* UserGUI.cs
* ClickGUI.cs
* FirstController.cs
* Moveable.cs



### 运行方式：

游戏对象运动和抛物线运动的运行方式比较简单，可以参考博客，基本就是建一个 3D 的 GameObject，然后把相应脚本拖到其身上，运行即可。



太阳系的运行：在 Sun 这个文件夹中，包含了这个项目的 Assert 文件；新建一个 项目，然后把用 Sun 文件夹中的 Assert 文件夹覆盖掉新建的项目的 Assert 文件，然后将 Scenes 中的 SampleScene.unity 拖进左边的 Hierarchy 一栏，运行即可。



牧师与恶魔：在 Priests and Devils 这个文件夹中，包含了这个项目的 Assert 文件；新建一个 项目，然后把用 Sun 文件夹中的 Assert 文件夹覆盖掉新建的项目的 Assert 文件，然后将 Scenes 中的 SampleScene.unity 拖进左边的 Hierarchy 一栏，运行即可。

