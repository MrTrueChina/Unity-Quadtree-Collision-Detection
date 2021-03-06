﻿using UnityEngine;

namespace MtC.Tools.QuadtreeCollider
{
    /// <summary>
    /// 四叉树配置文件
    /// </summary>
    public class QuadtreeConfig : ScriptableObject
    {
        /// <summary>
        /// 四叉树配置资源文件的文件名
        /// </summary>
        public const string CONFIG_OBJECT_NAME = "Quadtree Config";

        private static QuadtreeConfig Config
        {
            get
            {
                if (config != null)
                    return config;
                else
                    config = Resources.Load<QuadtreeConfig>(CONFIG_OBJECT_NAME);
                return config;
            }
        }
        private static QuadtreeConfig config;

        /// <summary>
        /// 一个节点里的碰撞器数量上限，超过上限后进行分割
        /// </summary>
        public static int MaxCollidersNumber
        {
            get { return Config.maxCollidersNumber; }
        }
        [SerializeField]
        [Header("单个节点的直属碰撞器数量上限，超过这个数量则判断为需要分割")]
        private int maxCollidersNumber = 10;

        /// <summary>
        /// 单个节点的最短边的最小长度，当任意一个边的长度小于这个长度时，无论碰撞器数量，不再进行分割
        /// </summary>
        public static float MinSideLength
        {
            get { return Config.minSideLength; }
        }
        [SerializeField]
        [Header("单个节点的最短边长，节点任意一条边边长小于这个值则不能再进行分割")]
        private float minSideLength = 10; // 这个值用于应对过度分割导致树深度过大性能反而下降的情况，同时可以避免大量碰撞器位置完全相同导致的无限分割

        /// <summary>
        /// 一个节点里的碰撞器数量上限，超过上限后进行分割
        /// </summary>
        public static int MinCollidersNumber
        {
            get { return Config.minCollidersNumber; }
        }
        [SerializeField]
        [Header("单个父级节点的所有子节点的碰撞器数量和下限，低于这个数量则判断为需要合并子节点到这个节点")]
        private int minCollidersNumber = 5;

        /// <summary>
        /// 初始根节点范围
        /// </summary>
        public static Rect StartArea
        {
            get { return Config.startArea; }
        }
        [SerializeField]
        [Header("四叉树创建时的范围")]
        private Rect startArea = new Rect(-1, -1, 1922, 1082);

        private void OnValidate()
        {
            // 最大碰撞器数量必须大于0，否则会无限分割
            if(maxCollidersNumber < 1)
            {
                maxCollidersNumber = 1;
            }

            // 最小边长必须大于0，否则一旦所有碰撞器聚集到一个位置将会发生无限分割
            if(minSideLength < 0)
            {
                minSideLength = 0.1f;
            }

            // 最小碰撞器数量必须小于最大碰撞器数量，否则会发生合并分割来回跳
            if (minCollidersNumber >= maxCollidersNumber)
            {
                minCollidersNumber = maxCollidersNumber - 1;
            }
        }
    }
}
