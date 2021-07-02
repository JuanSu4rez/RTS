using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using V2.Enums;

namespace V2.GUI.Mouse.Behaviours
{
    public class Selector3DBehaviour : MonoBehaviour
    {
        private HashSet<GameObject> selection = new HashSet<GameObject>();
        public HashSet<GameObject> Selection { get => selection; }
        public MouseController MouseController { get; set; }
       //todo for now
        public Enums.TeamsList mainTeam = TeamsList.TeamOne;
        int lastselection = 0;
        // Use this for initialization
        void Start() {
        }
        // Update is called once per frame
        void Update() {

            if(lastselection > 0 && selection.Count == 0) {
                int a = 0;
            }

            lastselection = selection.Count;

        }
        void OnCollisionEnter(Collision col) {
            if(MouseController.MouseState != Enums.GUI.MouseStates.Dragged) 
                return;
            Interfaces.ISelectable selectable = null;
            if(IsValidSelection(col.gameObject, ref selectable)) {
                selection.Add(col.gameObject);
                selectable.IsSelected = true;
            }
        }
        void OnCollisionExit(Collision col) {
            if(MouseController.MouseState != Enums.GUI.MouseStates.Dragged) 
                return;
                Interfaces.ISelectable selectable = null;
            if(IsValidSelection(col.gameObject, ref selectable)) {
                selection.Remove(col.gameObject);
                selectable.IsSelected = false;
                Debug.Log("OnCollisionExit IsDraggingMouse removed" );
            }
        }
        private bool IsValidSelection(GameObject gameObject ,ref Interfaces.ISelectable selectable) {
            bool flag = false;
            selectable = gameObject.GetComponent<Interfaces.ISelectable>();
            switch(gameObject.tag) {
                case Constans.Tags.Citizen:
                    flag = true;
                    break;
                case Constans.Tags.Military:
                    flag = true;
                    break;
            }
            return flag && selectable != null;
        }
        public void ClearSelection() {
            Debug.Log("ClearSelection "+selection.Count);
            foreach(var gameObject in selection) {
                var iselectable = gameObject.GetComponent<V2.Interfaces.ISelectable>();
                if(iselectable != null)
                   iselectable.IsSelected = false;
            }
            selection.Clear();
            Debug.Log("ClearSelection " + selection.Count);
        }
        public KindOfSelection GetKindOfSelection() {
            if(selection.Count == 0)
                return KindOfSelection._none;
            int CitizensCount = 0;
            int MilitariesCount = 0;
            foreach(var gameObject in selection) {
                switch(gameObject.tag) {
                    case Constans.Tags.Citizen:
                        CitizensCount++;
                        break;
                    case Constans.Tags.Military:
                        MilitariesCount++;
                        break;
                }
                if(CitizensCount > 0 && MilitariesCount > 0)
                    return KindOfSelection.Mixed;
            }
            if(CitizensCount > 0)
                return KindOfSelection.Citizens;
            return KindOfSelection.Militaries;
        }
        public void AddSelectedUnit(ref GameObject gameObject) {
            if(gameObject == null)
                return;
            Selection.Add(gameObject);
            var iSelectable = gameObject.GetComponent<V2.Interfaces.ISelectable>();
            if(iSelectable != null)
                iSelectable.IsSelected = true;
        }
    }
}