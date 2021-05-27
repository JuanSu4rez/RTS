using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using V2.Enums.GUI;

namespace V2.GUI
{
    public partial class MouseController : MonoBehaviour    {
        private MouseStates mouseState = MouseStates._none;
        private Vector2 initialMousePosition;
        public Vector2 InitialMousePosition { get => initialMousePosition; }
        private Vector2 lastMousePosition;
        public Vector2 LastMousePosition { get => lastMousePosition; }
        public MouseStates MouseState { get => mouseState; }
        private Rect rectangle = new Rect();
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
            rectangle.Set(selectionStart.x,selectionStart.y, width, heigth);
            return rectangle;
        }

        // Update is called once per frame
        void Update() {

        }
        private void _PointerDown(PointerEventData data) {
           //Debug.Log("_PointerDown");
            mouseState = MouseStates.Pressed;
            initialMousePosition = data.position;
        }
        private void _MouseDrag(PointerEventData data) {
           //Debug.Log("_MouseDrag");
            mouseState = MouseStates.Dragged;
            lastMousePosition = data.position;
        }
        private void _PointerUp(PointerEventData data) {
           //Debug.Log("_PointerUp");
            switch(mouseState) {
                case MouseStates._none:
                    break;
                case MouseStates.Pressed:
                   //Debug.Log("Mouse was only pressed.");
                    break;
                case MouseStates.Dragged:
                   //Debug.Log("Mouse was Pressed and Dragged.");
                    break;
            }
            mouseState = MouseStates._none;
            initialMousePosition = Vector2.zero;
            lastMousePosition = Vector2.zero;
        }
    }

}