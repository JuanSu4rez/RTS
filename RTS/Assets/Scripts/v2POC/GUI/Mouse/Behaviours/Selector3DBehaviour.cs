using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using V2.Enums;

namespace V2.GUI.Mouse.Behaviours
{
    public class Selector3DBehaviour : MonoBehaviour
    {
        private List<GameObject> selection = new List<GameObject>();
        public List<GameObject> Selection { get => selection; }
        public MouseController MouseController { get; set; }
        int lastselection = 0;
        // Use this for initialization
        void Start() {
        }
        // Update is called once per frame
        void Update() {
            if(lastselection != selection.Count) 
            {
                lastselection = selection.Count;
                Debug.Log("selection " + lastselection);
            }
        }
        void OnCollisionEnter(Collision col) {
            if(MouseController.MouseState != Enums.GUI.MouseStates.Dragged) {
                return;
            }
            bool add = false;
            switch(col.gameObject.tag) {
                case "Citizen":
                    add = true;
                    break;
                case "Military":
                    add = true;
                    break;
            }
            if(add) {
                selection.Add(col.gameObject);
                var iselectable = col.gameObject.GetComponent<V2.Interfaces.ISelectable>();
                if(iselectable != null)
                    iselectable.IsSelected = true;
            }
        }
        void OnCollisionExit(Collision col) {
            if(MouseController.MouseState != Enums.GUI.MouseStates.Dragged) {
                return;
            }
            bool add = false;
            switch(col.gameObject.tag) {
                case "Citizen":
                    add = true;
                    break;
                case "Military":
                    add = true;
                    break;
            }
            if(add) {
                selection.Remove(col.gameObject);
                var iselectable = col.gameObject.GetComponent<V2.Interfaces.ISelectable>();
                if(iselectable != null)
                    iselectable.IsSelected = false;
            }
        }
        public void ClearSelection() {
            foreach(var gameObject in selection) {
                var iselectable = gameObject.GetComponent<V2.Interfaces.ISelectable>();
                if(iselectable != null)
                    iselectable.IsSelected = false;
            }
            selection.Clear();
        }
        public KindOfSelection GetKindOfSelection() {
            return KindOfSelection.Citizens;
        }
    }
}