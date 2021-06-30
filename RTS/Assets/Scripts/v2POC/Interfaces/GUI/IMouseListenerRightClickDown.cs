using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace V2.Interfaces.GUI
{
    public interface IMouseListenerRightClickDown
    {
        void OnDown(PointerEventData data);
    }
}