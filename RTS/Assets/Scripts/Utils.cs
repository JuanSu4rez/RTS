using UnityEngine;
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