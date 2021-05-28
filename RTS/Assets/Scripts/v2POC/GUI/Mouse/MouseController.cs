using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using V2.Enums.GUI;
using System.Linq;
using V2.Interfaces.GUI;

namespace V2.GUI.Mouse
{
    public  class MouseController : MonoBehaviour    {
        public IMouseListenerDown listenerDown{ get; set; }
        public IMouseListenerDrag listenerDrag { get; set; }
        public IMouseListenerUp listenerDrop { get; set; }
        private MouseStates mouseState = MouseStates._none;
        private Vector2 initialMousePosition;
        public Vector2 InitialMousePosition { get => initialMousePosition; }
        private Vector2 lastMousePosition;
        public Vector2 LastMousePosition { get => lastMousePosition; }
        public MouseStates MouseState { get => mouseState; }
        private Rect mouseArea = new Rect();
        // Use this for initialization
        void Start() {
            enabled = false;
            this.gameObject.AddComponent<EventTrigger>();
            var triggerObject = this.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry_0 = new EventTrigger.Entry();
            entry_0.eventID = EventTriggerType.PointerDown;
            entry_0.callback.AddListener((data) => { _PointerDown((PointerEventData)data); });
            triggerObject.triggers.Add(entry_0);
            entry_0 = new EventTrigger.Entry();
            entry_0.eventID = EventTriggerType.Drag;
            entry_0.callback.AddListener((data) => { _MouseDrag((PointerEventData)data); });
            triggerObject.triggers.Add(entry_0);
            entry_0 = new EventTrigger.Entry();
            entry_0.eventID = EventTriggerType.PointerUp;
            entry_0.callback.AddListener((data) => { _PointerUp((PointerEventData)data); });
            triggerObject.triggers.Add(entry_0);
        }

        internal Rect GetRectangle() {
            Vector2 selectionStart = new Vector2();
            Vector2 selectionEnd = selectionStart;
            selectionStart.x = initialMousePosition.x <= lastMousePosition.x ? initialMousePosition.x : lastMousePosition.x;
            selectionStart.y = initialMousePosition.y >= lastMousePosition.y ? Screen.height - initialMousePosition.y : Screen.height - lastMousePosition.y;
            selectionEnd.x = initialMousePosition.x <= lastMousePosition.x ? lastMousePosition.x : initialMousePosition.x;
            selectionEnd.y = initialMousePosition.y <= lastMousePosition.y ? Screen.height - initialMousePosition.y : Screen.height - lastMousePosition.y;
            float width = selectionEnd.x - selectionStart.x;
            float heigth = selectionEnd.y - selectionStart.y;
            mouseArea.Set(selectionStart.x,selectionStart.y, width, heigth);
            return mouseArea;
        }

        internal Vector2 GetInitialPosition() {
            Vector2 selectionStart = new Vector2();
            Vector2 selectionEnd = selectionStart;
            selectionStart.x = initialMousePosition.x <= lastMousePosition.x ? initialMousePosition.x : lastMousePosition.x;
            selectionStart.y = initialMousePosition.y >= lastMousePosition.y ?  initialMousePosition.y :  lastMousePosition.y;
            selectionEnd.x = initialMousePosition.x <= lastMousePosition.x ? lastMousePosition.x : initialMousePosition.x;
            selectionEnd.y = initialMousePosition.y <= lastMousePosition.y ?  initialMousePosition.y :  lastMousePosition.y;
            return selectionStart;
        }
        private void _PointerDown(PointerEventData data) {
           //Debug.Log("_PointerDown");
            mouseState = MouseStates.Pressed;
            initialMousePosition = data.position;
            if(listenerDown != null) {
                listenerDown.OnDown(data);
            }
        }
        private void _MouseDrag(PointerEventData data) {
           //Debug.Log("_MouseDrag");
            mouseState = MouseStates.Dragged;
            lastMousePosition = data.position;
            if(listenerDrag != null) {
                listenerDrag.OnDrag(data);
            }
        }
        private void _PointerUp(PointerEventData data) {
            switch(mouseState) {
                case MouseStates._none:
                    break;
                case MouseStates.Pressed:
                    break;
                case MouseStates.Dragged:
                    break;
            }
            if(listenerDrop != null) {
                listenerDrop.OnUp(data);
            }
            mouseState = MouseStates._none;
            initialMousePosition = Vector2.zero;
            lastMousePosition = Vector2.zero;
        }
    }

}