using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace V2.Interfaces.GUI
{
    public interface IMouseListenerRightClickUp
    {
        void OnUp(PointerEventData data);
    }
}