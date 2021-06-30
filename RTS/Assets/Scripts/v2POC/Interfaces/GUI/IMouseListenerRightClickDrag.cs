using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace V2.Interfaces.GUI
{
    public interface IMouseListenerRightClickDrag
    {
        void OnDrag(PointerEventData data);
    }
}