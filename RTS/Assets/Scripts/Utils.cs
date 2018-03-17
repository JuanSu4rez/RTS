using UnityEngine;
using UnityEditor;
using System.Collections;

public sealed class Utils : ScriptableObject
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