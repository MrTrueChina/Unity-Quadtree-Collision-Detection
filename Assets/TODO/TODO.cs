﻿//TODO：
//这个.cs用于记录可能出现的方案和对应的灵感

/*
 *  Quadtree:
 *    核心问题：
 *      更新时刻：
 *        Unity自带的物理碰撞检测以物理更新频率为基准进行检测（FixedUpdate），这是为了保证物理系统的准确性而选择的方案
 *        四叉树碰撞检测就是通过削减物理部分来在速度上超越自带物理碰撞，如此可以考虑将四叉树的碰撞改到Update之中
 *      碰撞检测发出方：
 *        发出方有两种方案：在碰撞器发出 / 在四叉树发出
 *        如果在碰撞器发出，不需要任何额外设计，缺点是更新和检测的顺序无法保证
 *        如果在四叉树发出，优点是检测可以保证在更新完成后进行，缺点是需要遍历整棵树寻找检测器或维护一个检测器列表
 *        可以考虑在接口中增加是否检测碰撞的设置方法，以此更新四叉树中的检测器列表
 *      分割时刻：
 *        分割时刻有两种方案：在存入碰撞器时分割 / 在更新时分割
 *        首先分割之前都需要先更新碰撞器位置，否则会发生分割前有碰撞器移出节点范围导致分割后碰撞器无法正常下发给子节点的bug
 *        如果使用存入时分割则会带来一个问题：存入前的更新导致碰撞器存入另一个节点，导致另一个节点也进行更新，另一个节点更新的同时又有一个碰撞器进入了这个节点导致了这个节点的更新，两个节点互相触发更新导致大量多余更新
 *        这个问题可以通过先把所有节点取出来，等分割完毕后再存入树的方式解决
 *        如果使用更新时分割则不会产生这个问题，但对应的会带来新的问题：每次更新都会造成一次分割判断，绝大多数都会是false
 *        可以考虑使用总碰撞器数量小于分割临界值就不向下递归的方式减小运算量消耗，代价是多维护一个变量
 *        无论哪种分割时机都会带来多余的检测，问题核心在于两个哪个消耗小一些
 *      节点合并:
 *        是否需要增加子节点合并的方法以在不需要大量子节点的情况下减少检测消耗
 *        如果要增加合并功能，有三种方案：合并时子节点删除 / 合并时子节点保留可复用属性入池，不可重复属性通过存值或重新创建的方式补充 / 合并时子节点依然在父节点内，节点添加一个是否是树梢的字段
 *        三者在内存占用上逐渐增加，但在速度上逐渐增快。其中中间方案的不可复用属性中最复杂的是Rect，创建和赋值哪个更快就是关键
 *        合并和分割的检测时间也有讨论空间，一个节点能发生合并必然不需要分割，需要进行分割必然不能够合并，但一旦处理不好可能造成逻辑的复杂化
 *      半径更新时刻：
 *        存入移除时更新 / 每帧更新一次
 *        如果存入时更新，从下向上更新，到了父级最大半径大于自己的最大半径时就结束，调用过程性能好，但检测次数多、逻辑较复杂
 *        如果每帧更新，最大的问题是如何避免四个节点半径从小到大导致连续四次更新，所幸每帧更新是从上向下更新，可以先更新全部子节点再决定自己要不要更新。最大缺点是每一帧都要将全部节点调用一次
 *      移除时刻：
 *        移除时移除 / 更新时移除
 *        移除时移除最大的优势就是逻辑简单，但缺点是移除时可能碰撞器已经进入到其他节点之中，导致根据位置移除无效，需要全树遍历移除。如果一个更新的时间里出现两次全树移除，基本就可以认为性能上与更新时移除拉开了距离
 *        更新时移除可以节约计算量，但需要一个新的列表来保存需要移除的节点
 *      事件订阅问题:
 *        碰撞事件无论检测发起方是谁，必然要由检测器向其他组件传达，为了减轻耦合应该使用面向接口的开发方式，实现碰撞检测接口来达到检测效果
 *        直接通过检测器在四叉树中添加订阅？
 *        通过订阅查看订阅脚本是否存在？
 *        不直接订阅而是在检测器中维护一个脚本应用List用于检测检测器是否存在？
 *      反向生长实现方式：
 *        两种方式：在每个节点内做中转 / 用一个包装类做中转
 *    数据问题:
 *      Transform:
 *        Transform获取到的所有属性都要是世界空间的，不能用相对空间的
 *    设计问题：
 *      更新:
 *        是否可以使用责任链完成在更新时的所有操作？
 *        是否需要使用命令模式对各个操作进行分离？
 */
