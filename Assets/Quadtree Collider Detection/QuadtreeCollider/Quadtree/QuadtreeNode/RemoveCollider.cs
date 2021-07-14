﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MtC.Tools.QuadtreeCollider
{
    // 从树中移除碰撞器的部分
    internal partial class QuadtreeNode
    {
        /// <summary>
        /// 从当前节点中移除指定碰撞器
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        internal OperationResult RemoveColliderFromSelf(QuadtreeCollider collider)
        {
            bool listResult = colliders.Remove(collider);

            if (listResult)
            {
                // 创建操作成功的返回对象
                OperationResult result = new OperationResult(true);

                // 映射表值为 null 表示碰撞器不属于任何节点
                result.CollidersToNodes.Add(collider, null);

                return result;
            }
            else
            {
                // 返回移除失败，移除失败不改变映射表，直接返回失败即可
                return new OperationResult(false);
            }
        }

        /*
         * UNDONE: 合并逻辑和分割有很大不同，合并逻辑在末梢是不可能进行的，实际上进行合并逻辑的是树枝，当然这是一个向上递归的逻辑，他可以从末梢开始调用
         * 
         * 此外合并逻辑并不是在每次移除的时候都需要进行
         * 在一般的移除节点逻辑中，节点移除后就可以进行合并
         * 但在位置更新时是不可以的，如果在位置更新时发生了合并。触发合并的时候必然在更新末梢节点，合并会导致树枝成为树梢，这就导致之前树枝的更新错误，他应该按照树梢的方式更新
         * 但在位置更新时又是必须进行合并的，否则很可能导致很多节点应该合并却没有合并，因此最好的办法是在一个节点更新完毕后再进行合并
         * 
         * 按照逻辑来说，如果一个节点需要合并，那么它的所有子节点也应该合并，那么在从下到上不漏合并的情况下，一个需要合并的节点需要统计碰撞器数量时只需要找他的四个子节点即可
         * 那么可以写一个递归的统计数量方法，虽然是递归的，但实际上大约只需要查一层，不会有很大的性能损失，还能应对意外的合并不完整的情况
         * 
         * 那么现在看来，需要的是一个合并单个节点的方法，之后是一个基于这个方法的向上递归直到不能合并的方法，这两个方法都需要返回映射表的变更
         */

        /// <summary>
        /// 如果当前节点达到了合并条件，返回 true
        /// </summary>
        /// <returns></returns>
        private bool Needmerge()
        {
            // 有子节点，且节点中的碰撞器总数小于最小碰撞器总数，就是达到了合并条件
            return
                HaveChildren()
                && GetColliderNumbers() < QuadtreeConfig.MinSideLength;
        }

        /// <summary>
        /// 获取当前节点及所有子节点的总计碰撞器数量
        /// </summary>
        /// <returns></returns>
        private int GetColliderNumbers()
        {
            if (HaveChildren())
            {
                // 有子节点，子节点的碰撞器数量和就是这个节点的碰撞器数量
                int number = 0;
                children.ForEach(child => number += child.GetColliderNumbers());
                return number;
            }
            else
            {
                // 没有子节点，返回碰撞器列表数量
                return colliders.Count;
            }
        }

        /// <summary>
        /// 合并当前节点
        /// </summary>
        /// <returns></returns>
        private OperationResult Merge()
        {
            // 合并节点必然成功
            OperationResult result = new OperationResult(true);

            // UNDONE：递归把所有子节点的碰撞器移除并记录

            // 把子节点列表抛弃掉
            children = null;

            // UNDONE：把移除的碰撞器全部存入到当前节点中并记录

            // 返回记录
            return result;
        }
    }
}
