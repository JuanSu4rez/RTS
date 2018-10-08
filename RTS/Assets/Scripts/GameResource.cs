using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameResource 
{
    private  bool applycache;


    private string path;

    private Dictionary<string, Object> cache = new Dictionary<string, Object>();


    public GameResource(AssetTypes type)
    {
        path = type.GetPath();
        applycache = false;
    }

    public GameResource(AssetTypes type, bool applycache)
    {
        path = type.GetPath();
        if (applycache)
            cache = new Dictionary<string, Object>();
    }

    public  T Load<T>(string resourcename) where T : UnityEngine.Object
    {
        var result =  Load<T>(path, resourcename);
        
        if (applycache)
        {
            UnityEngine.Object obj = null;
            if (!cache.TryGetValue(resourcename, out obj))
            {
                T val = Load<T>(path, resourcename);
                cache.Add(resourcename, val);

                return val;
               
            }
            else
            {
                return obj as T;
            }
        }
        else
        {
            return Load<T>(path, resourcename);
        }
    }

    public static T Load<T>(AssetTypes assettype, string resourcename) where T : Object
    {
        var _type = typeof(T);
        return UnityEngine.Resources.Load(assettype.GetPath()+ resourcename, _type) as T;
    }

    public static T Load<T>(string path, string resourcename) where T : Object
    {
        var _type = typeof(T);
        return UnityEngine.Resources.Load(path + resourcename, _type) as T;
    }


}