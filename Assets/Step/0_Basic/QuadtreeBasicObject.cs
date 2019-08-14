﻿/*
 *  四叉树物体脚本，使用时挂载在需要进行四叉树碰撞检测的场景里的物体上并写上需要的参数。
 *  
 *  注意这个脚本的执行顺序必须在 QuadtreeBasicCollider 前面，设置方式是 Edit -> Project Settings -> Script Excution Order，点"+"选择要设置执行顺序的脚本，上下拖动，越靠上执行越早
 */

using UnityEngine;

public class QuadtreeBasicObject : MonoBehaviour
{
    [SerializeField]                //加了这个的变量可以不写public就在 Inspector 面板展示，如果有变量需要在 Inspector 面板展示但又不需要public则应该用[SerializeField]
    float _top = 0;
    [SerializeField]
    float _right = 0;
    [SerializeField]
    float _bottom = 50;
    [SerializeField]
    float _left = 100;
    [SerializeField]
    int _maxLeafsNumber = 50;
    [SerializeField]
    float _minSideLength;

    static QuadtreeBasic<GameObject> _quadtree;     //static方法必须要static变量，所以这里设为static



    private void Awake()
    {
        _quadtree = new QuadtreeBasic<GameObject>(_top, _right, _bottom, _left, _maxLeafsNumber, _minSideLength);
    }



    /*
     *  存入。移除。检测三个方法都是 static ，因为static方法可以通过类名调用，这样就省去了每个检测器都要 Find 一次四叉树物体的工作
     */
    public static void SetLeaf(QuadtreeBasicLeaf<GameObject> leaf)
    {
        _quadtree.SetLeaf(leaf);
    }

    public static void RemoveLeaf(QuadtreeBasicLeaf<GameObject> leaf)
    {
        _quadtree.RemoveLeaf(leaf);
    }

    public static GameObject[] CheckCollision(Vector2 checkPosition, float radius)
    {
        return _quadtree.CheckCollision(checkPosition, radius);
    }



    /*
     *  OnDrawGizmos：Unity自带方法之一，在绘制Gizmo的时候调用，一般来说只要在Scene面板上做了操作就会绘制Gizmo
     *  Gizmo是个很难解释的词汇，它直译叫“小工具”，Unity自带碰撞器的体积的线是Gizmo，移动、缩放。旋转物体的那几个工具同样是Gizmo，就是直译里说的“工具”
     *  Gizmo在Unity里是一次次的“绘制”出来的，在绘制Unity自带Gizmo的同时也可以通过 OnDrawGizmo 自己设定需要绘制的Gizmo
     *  一般来说Gizmo只能在 Scene 面板里看到，在 Game 面板里看不到，因为这是为了方便开发才写的工具，在实际游戏过程里是没有用的
     *  
     *  虽然不会写Gizmo也不会对功能有什么影响，但会写Gizmo可以让开发过程更舒服
     */
    private void OnDrawGizmos()
    {
        Vector3 upperRight = new Vector3(_right, _top, transform.position.z);
        Vector3 lowerRight = new Vector3(_right, _bottom, transform.position.z);
        Vector3 lowerLeft = new Vector3(_left, _bottom, transform.position.z);
        Vector3 upperLeft = new Vector3(_left, _top, transform.position.z);

        Gizmos.color = Color.red * 0.8f;            //Gizmos.color：绘制Gizmo的颜色

        Gizmos.DrawLine(upperRight, lowerRight);
        Gizmos.DrawLine(lowerRight, lowerLeft);
        Gizmos.DrawLine(lowerLeft, upperLeft);
        Gizmos.DrawLine(upperLeft, upperRight);
    }



    /*
     *  OnValidate：Unity自带方法之一，当 Inspector 面板的数值变化时调用，一般用来限制数据的调整，防止误操作导致的bug
     */
    private void OnValidate()
    {
        if (_top < _bottom)
            _top = _bottom;
        if (_right < _left)
            _right = _left;
        if (_maxLeafsNumber < 1)
            _maxLeafsNumber = 1;
        if (_minSideLength < 0.001f)
            _minSideLength = 0.001f;
    }
}
