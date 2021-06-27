using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using V2.Enums;
using V2.Interfaces;
using System;

namespace V2.Utils
{
    public static class SurroundingPoints
    {
        public static Vector3[] GetPoints(ref GameObject go, ref IUnitController controller) {
            List<Vector3> points = new List<Vector3>();
            if(go == null || controller == null || V2.Classes.Grid.grid == null) {
                return points.ToArray();
            }
            switch(controller.KindOfEntity) {
                case KindsOfEntities.Animal:
                    CalculateAnimalPoints(ref points, ref go, ref controller);
                    return points.ToArray();
                    break;
                case KindsOfEntities.Citizen:
                    CalculateHumanPoints(ref points, ref go, ref controller);
                    return points.ToArray();
                    break;
                case KindsOfEntities.Militar:
                    CalculateHumanPoints(ref points, ref go, ref controller);
                    return points.ToArray();
                    break;
            }
            switch(controller.EntityType) {
                case EntityType.GoldMine:
                    GetPointsByCollider(ref points, ref go, ref controller);
                    break;
                case EntityType.Three:
                    break;
                case EntityType.House:
                    GetPointsByCollider(ref points, ref go, ref controller);
                    break;
            }
            return points.ToArray();
        }

        private static void CalculateHumanPoints(ref List<Vector3> points, ref GameObject go, ref IUnitController controller) {
            points.AddRange(V2.Classes.Grid.grid.GetSorroundPositions(go.transform.position));
        }
        private static void CalculateAnimalPoints(ref List<Vector3> points, ref GameObject go, ref IUnitController controller) {
            points.AddRange(V2.Classes.Grid.grid.GetSorroundPositions(go.transform.position));
        }

        private static void GetPointsByCollider(ref List<Vector3> points, ref GameObject go, ref IUnitController controller) {
            var collider = go.GetComponent<Collider>();

            switch(collider) {
                case BoxCollider boxCollider:
                    CalculateByBoxCollider(ref points, ref go, ref controller, boxCollider);
                    break;
                case CapsuleCollider capsule:
                    CalculateByCapsuleCollider(ref points, ref go, ref controller, capsule);
                    break;
            }
        }
        private static void CalculateByBoxCollider(ref List<Vector3> points, ref GameObject go, ref IUnitController controller, BoxCollider boxCollider) {
            var bounds = boxCollider.bounds;
            var size = bounds.size;
            var boundshalfsize = size * 1f;
            var initialpoint = go.transform.position;
            initialpoint = go.transform.position + ( bounds.size * 0.5f ) +
                ( ( Vector3.forward + Vector3.right ) * V2.Classes.Grid.grid.width * 0.5f );
            //Debug.DrawLine(initialpoint, initialpoint + Vector3.up * 10, Color.cyan,1000);
            //todo this depends on th y of the object
            initialpoint.y = 0;
            initialpoint =  V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(initialpoint);
            int i = 0;
            Vector3 right = Vector3.right;
            Vector3 up = Vector3.forward;
            var sizeX = (bounds.size.x + V2.Classes.Grid.grid.width) / V2.Classes.Grid.grid.width;
            var sizeZ = (bounds.size.z + V2.Classes.Grid.grid.width) / V2.Classes.Grid.grid.width;
            //Debug.DrawLine(initialpoint+ -Vector3.right * sizeX ,( initialpoint + -Vector3.right * sizeX ) + Vector3.up * 10, Color.cyan, 1000);
            for(; i < sizeX; i++) {
                points.Add(( -right * i * V2.Classes.Grid.grid.width ) + initialpoint);
            }
            initialpoint = ( -right * i * V2.Classes.Grid.grid.width ) + initialpoint;
            for(i = 0; i < sizeZ; i++) {
                points.Add(( -up * i * V2.Classes.Grid.grid.width ) + initialpoint);
            }
            initialpoint = ( -up * i * V2.Classes.Grid.grid.width ) + initialpoint;
            i = 0;
            for(; i < sizeX; i++) {
                points.Add(( right * i * V2.Classes.Grid.grid.width ) + initialpoint);
            }
            initialpoint = ( right * i * V2.Classes.Grid.grid.width ) + initialpoint;
            for(i = 0; i < sizeZ; i++) {
                points.Add(( up * i * V2.Classes.Grid.grid.width ) + initialpoint);
            }
        }
        private static void CalculateByCapsuleCollider(ref List<Vector3> points, ref GameObject go, ref IUnitController controller, CapsuleCollider capsule) {
            float radius = capsule.radius * 0.7f;
            var initialpoint = go.transform.position;
            initialpoint.y = 0;
            var angle = 18;
            var initial = 0;
            var reason = 360 / angle;
            points = new List<Vector3>();
            int y = 0;
            for(y = 0; y < reason; y++) {
                var result = Quaternion.AngleAxis(initial + ( angle * y ), Vector3.up) * Vector3.right;
                points.Add(initialpoint + result * capsule.radius);
            }

        }
    }
}