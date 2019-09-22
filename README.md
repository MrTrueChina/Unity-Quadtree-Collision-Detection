# U2D四叉树碰撞检测系统</br>
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

## 简介：
Unity引擎自带一套基于物理引擎的2D碰撞检测系统，这套系统具有完备的物理功能但成本高昂无法大量使用。<br/>
本系统采用正好相反的思路，在不使用物理引擎的基础上使用传统的四叉树实现一套只具有核心功能但速度极快的碰撞检测系统。<br/>
本系统由一套碰撞器、三个接口、一个设置窗口、一个配置文件组成。<br/>
## 快速开始：
由于U3D没有提供组件挂载和移除事件，本系统基于事件委托进行使用，为简化使用提供了自动订阅功能，因此有两种使用方法：<br/>
### 使用自动订阅：
1.将碰撞器挂载到需要检测碰撞的物体上<br/>
2.在需要进行检测的物体的碰撞器组件上勾选 IsDetector<br/>
3.在需要接收事件的脚本中根据需要实现三个接口中的一个或多个<br/>
4.在接口提供的方法中写上逻辑代码<br/>

<b>警告</b>：自动订阅的原理是在 Awake 时查询物体上所有组件并将实现了接口的组件的方法进行订阅，这个订阅不会被取消。因此自动订阅只适用于需要碰撞检测的组件在物体的整个生命周期中都存在的情况，如果组件在实例化后才挂载则会无法订阅，组件中途销毁则会导致内存泄漏。
### 手动订阅：
1.将碰撞器挂载到需要检测碰撞的物体上<br/>
2.在需要进行检测的物体的碰撞器组件上勾选 IsDetector<br/>
3.根据逻辑在需要的位置订阅和取消订阅碰撞器组件的三个事件中的一个或多个<br/>

尽管三个事件可以订阅所有符合格式的方法，但仍然建议实现接口并订阅接口中的方法以保持整齐。

## 配置：
无论那种使用方式都可以通过 Tools -> Quadtree -> Quadtree Config 根据需要调整四叉树参数进行优化。<br/>
</br>
## 文件夹内容：
| 文件夹 | 内容 |
| ------------- |:-------------| 
| Assets/Quadtree | 实用版四叉树碰撞检测脚本 |
| Assets/Example | 实用版四叉树碰撞检测的演示场景和脚本 |
| Quadtree.unitypackage | 实用版四叉树碰撞检测的资源包 |
| Assets/Step | 从简单逐步复杂的代码，加了面向新人的大批量注释，如果是刚开始研究四叉树可以按照这个文件夹的顺序阅读 |
| Assets/Step/0_Basic | 最初版的四叉树，碰撞器不能移动也没有半径，就是一个个固定的点，主要用来理解碰撞检测的原理和四叉树基础的核心功能。此外还有大量的新人入门、名词解释等注释 |
| Assets/Step/1_Radius | 在0的基础上增加了碰撞器的半径，但碰撞器依然不能移动，半径也不能改变 |
| Assets/Step/2_Update | 在1的基础上增加更新功能，从这一步开始碰撞器可以移动也可以改变半径了 |
| Assets/Step/3.0_Event | 在2的基础上增加事件委托，实现类似Unity的 OnCollision 的效果，解释了什么是事件和委托，可以帮助新人理解事件委托，但不保证看完就懂 |
| Assets/Step/3.1_Action | 在3.0的基础上用 Action 代替了手写的委托，并介绍了 Action 和 Func |
| Assets/Step/4_NestedClass | 在3.1的基础上把 Leaf 和 Field 改成了 Quadtree 的内部类 |
| Asstes/Step/5_Singleton | 在4的基础上把Quadtree和QuadtreeObject合为一个脚本文件，用单例模式自动创建四叉树物体，用ScriptableObject和EditorWindow进行设置。从这一步开始不需要设置脚本执行顺序，也不需要手动创建四叉树物体 |
| Assets/Step/6_Upwards | 在5的基础上增加向上生长的功能，如果叶子存入时位置在四叉树范围以外，四叉树会自动向叶子方向生长以接住叶子 |
| ProjectSettings | Unity的ProjectSettings文件夹，里面是各种设置 |
### 注意：由于没有物理功能，四叉树的碰撞不会像Unity自带碰撞一样互相弹开，而是像触发器一样互相穿过
