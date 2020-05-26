using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed class Utils {
    public static void ChangeColor(MeshRenderer meshRenderer, Team team) {
        meshRenderer.material.color = team.Color;
    }

    public static void ChangeColorBuildingWB(GameObject gameObject, Team team) {
        ArrayList meshes = new ArrayList();
        meshes.AddRange(gameObject.transform.GetComponentsInChildren<MeshRenderer>());
        for (int i = 0; i < meshes.Count; i++) {
            MeshRenderer temp = (MeshRenderer)meshes[i];
            if (temp.name.Contains("Col")) {
                Utils.ChangeColor(temp, team);
            }
        }
    }


    public static Vector3[] GetPoints(GameObject gor) {
        List<Vector3> aux = new List<Vector3>();
        if (gor == null)
            return aux.ToArray();

        if (GridScript.gridScript == null || GridScript.gridScript.gird == null) {
            return aux.ToArray();
        }


        if (gor.tag == "Citizen") {

            aux.AddRange(GridScript.gridScript.gird.GetSorroundPositions(gor.transform.position));
        }
        else if (gor.tag == "Military") {

            aux.AddRange(GridScript.gridScript.gird.GetSorroundPositions(gor.transform.position));

        }
        else if (gor.tag == "Building") {

            // aux.AddRange(GridScript.gridScript.gird.GetSorroundPositions(gor.transform.position));
            BoxCollider boxCollider = gor.GetComponents<BoxCollider>().FirstOrDefault(p => p.enabled == true);

            if (boxCollider != null) {

              
                var bounds = boxCollider.bounds;
             

                var boundshalfsize = bounds.size * 0.5f;
           

                var initialpoint = gor.transform.position + boundshalfsize + (GridScript.gridScript.gird.size * 0.5F);
                initialpoint.y = 0;

                int i = 0;
                for (; i < bounds.size.x ; i++) {


                    aux.Add((gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint);
                }
              
                initialpoint = (gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint;
                
                 for ( i = 0; i < bounds.size.z; i++) {


                  aux.Add((-gor.transform.forward * i * GridScript.gridScript.gird.width) + initialpoint);
                 }


                initialpoint = (-gor.transform.forward * i * GridScript.gridScript.gird.width) + initialpoint;

                 i = 0;
                for (; i < bounds.size.x; i++) {


                    aux.Add((-gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint);
                }

                initialpoint = (-gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint;


                for (i = 0; i < bounds.size.z; i++) {


                    aux.Add((gor.transform.forward * i * GridScript.gridScript.gird.width) + initialpoint);
                }







            }

        }

        else if (gor.tag == "Gold") {

            // aux.AddRange(GridScript.gridScript.gird.GetSorroundPositions(gor.transform.position));

        }

        return aux.ToArray();


    }

    public static Vector3[] GetPoints(Collider collider) {
        var pos = collider.transform.position;
        switch (collider) {
            case BoxCollider boxCollider:
                /*
             var size =    box.bounds.size;
              
             
                //pos.z = 0;
                return new Vector3[] {
                    pos + collider.transform.right * size.x,
                    pos +( -collider.transform.right) * size.x,
                    pos + collider.transform.up * size.x,
                    pos + (-collider.transform.up) * size.x
                };
                */
                List<Vector3> aux = new List<Vector3>();
                var bounds = boxCollider.bounds;


                var boundshalfsize = bounds.size * 0.5f;

                var gor = boxCollider.gameObject;
                var initialpoint = gor.transform.position + boundshalfsize + (GridScript.gridScript.gird.size * 0.5F);
                initialpoint.y = 0;

                int i = 0;
                for (; i < bounds.size.x; i++) {


                    aux.Add((-gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint);
                }

                initialpoint = (-gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint;

                for (i = 0; i < bounds.size.z; i++) {


                    aux.Add((-gor.transform.up * i * GridScript.gridScript.gird.width) + initialpoint);
                }


                initialpoint = (-gor.transform.up * i * GridScript.gridScript.gird.width) + initialpoint;

                i = 0;
                for (; i < bounds.size.x; i++) {


                    aux.Add((gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint);
                }

                initialpoint = (gor.transform.right * i * GridScript.gridScript.gird.width) + initialpoint;


                for (i = 0; i < bounds.size.z; i++) {


                    aux.Add((gor.transform.up * i * GridScript.gridScript.gird.width) + initialpoint);
                }



                return aux.ToArray();

                break;
            case CapsuleCollider capsule:

                float radius = capsule.radius * 0.7f;

                pos = collider.transform.position;
                pos.y = 0;

                var angle = 18;
                var initial  = 0;
                var reason = 360 / angle;
               aux = new List<Vector3>();

                int y = 0;
                for (y = 0; y < reason; y++) {

                    var result = Quaternion.AngleAxis(initial +(angle * y), Vector3.up) * Vector3.right;
                    aux.Add(pos+ result * capsule.radius);
                }

                return aux.ToArray();
                /*
                //pos.z = 0;
                return new Vector3[] {
                    pos + collider.transform.right * radius,

                    pos +( -collider.transform.right) * radius,
                    pos + collider.transform.up * radius,
                    pos + (-collider.transform.up) * radius,
                };
                */

                break;
        }

        return null;
    }

    internal static Vector3[] GetPointsToWait(int size, Vector3 point, Vector3 direction, Vector3 fee) {


        var result = new Vector3[size];

        for (int i = 0; i < result.Length; i++) {
            result[i] = point + direction + (fee * (i + 1));
        }


        return result;

    }

    internal static Vector3 PositionSubHalfBounsdssizeXZ(GameObject gameObject, int deltaPosition = 1) {
        Collider buildingCollider = gameObject.GetComponent<Collider>();
        Transform buildingPosition = gameObject.transform;

        Vector3 unitPosition = new Vector3();
        var v = buildingCollider.bounds.size;// buildingCollider.bounds.size.sqrMagnitude > this.transform.localScale.sqrMagnitude ? buildingCollider.bounds.size: this.transform.localScale;
        v.x /= 2;
        v.z /= 2;
        v.y = 0;
        unitPosition = (buildingPosition.position - v) - new Vector3(deltaPosition, 0, deltaPosition);

        return unitPosition;
    }
}


public sealed class UtilsCollections {
    public static bool IsNullOrEmpty<T>(T collection) where T : ICollection {
        return collection == null || collection.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(T[] Array) {
        return Array == null || Array.Length == 0;
    }
}


public sealed class UtilsMilitary {
    public static bool ValidateGameObjectStateToAttackByTrigger(GameObject gameObject) {
        bool result = true;
        var tag = gameObject.tag;
        switch (tag) {
            case "Building":
                var buildingBehaviour = gameObject.GetComponent<BuildingBehaviour>();
                result = buildingBehaviour != null && buildingBehaviour.State != BuildingStates.IsSettingOnScene;
                break;

        }


        return result;
    }
}