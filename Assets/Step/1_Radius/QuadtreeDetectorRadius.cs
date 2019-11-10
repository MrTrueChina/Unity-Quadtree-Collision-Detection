﻿using UnityEngine;

namespace MtC.Tools.Quadtree.Example.Step1Radius
{
    public class QuadtreeDetectorRadius : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        float _radius;

        private void OnDrawGizmos()
        {
            DrawRadius();
            DrawCollision();
        }

        void DrawRadius()
        {
            Gizmos.color = Color.yellow * 0.8f;
            MyGizmos.DrawCircle(transform.position, _radius, 60);
        }

        void DrawCollision()
        {
            foreach (GameObject collider in QuadtreeObjectRadius.CheckCollision(transform.position, _radius))
                Gizmos.DrawLine(transform.position, collider.transform.position);
        }
    }
}
