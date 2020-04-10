﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public sealed class Utils
{
    public static void ChangeColor(MeshRenderer meshRenderer, Team team) {
        meshRenderer.material.color = team.Color;
    }    

    public static void ChangeColorBuildingWB(GameObject gameObject, Team team) {        
        ArrayList meshes = new ArrayList();
        meshes.AddRange(gameObject.transform.GetComponentsInChildren<MeshRenderer>());
        for (int i = 0; i < meshes.Count; i++){
            MeshRenderer temp = (MeshRenderer)meshes[i];
            if (temp.name.Contains("Col"))
            {
                Utils.ChangeColor(temp, team);
            }
        }
    }


    public static Vector3[] GetPoints(Collider collider)
    {
        var pos = collider.transform.position;
        switch (collider)
        {
            case BoxCollider box:
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




                break;
            case CapsuleCollider capsule:

                float radius = capsule.radius* 0.7f;

                pos = collider.transform.position;
                //pos.z = 0;
                return new Vector3[] {
                    pos + collider.transform.right * radius,

                    pos +( -collider.transform.right) * radius,
                    pos + collider.transform.up * radius,
                    pos + (-collider.transform.up) * radius,
                };

                break;
        }

        return null;
    }

    internal static Vector3[] GetPointsToWait(int numbers, Vector3 point, Vector3 direction, Vector3 fee)
    {




        var result = new Vector3[numbers];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = point + direction + (fee * (i + 1));
        }


        return result;

    }
}


public sealed class UtilsCollections
{
    public static bool IsNullOrEmpty<T>(T collection)where T: ICollection
    {
        return collection == null || collection.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(T[] Array) 
    {
        return Array == null || Array.Length == 0;
    }
}


public sealed class UtilsMilitary
{
    public static bool ValidateGameObjectStateToAttackByTrigger(GameObject gameObject)
    {
        bool result = true;
        var tag = gameObject.tag;
        switch (tag)
        {
            case "Building":
               var buildingBehaviour =  gameObject.GetComponent<BuildingBehaviour>();
                result = buildingBehaviour != null && buildingBehaviour.State != BuildingStates.IsSettingOnScene;
                break;

        }


        return result;
    }
}