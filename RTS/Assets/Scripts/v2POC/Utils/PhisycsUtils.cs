using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using V2.Enums;
using V2.Interfaces;
using System;

namespace V2.Utils
{
    public static class PhisycsUtils
    {
        public static RaycastHit? GetRaycastHitFromPoint( Vector2 position ,  int layerMask) {
            layerMask = 1 << layerMask;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(position);
            var results = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
            return results.FirstOrDefault();
        }
    }
}