using UnityEngine;
using System.Collections;
using V2.GUI;
using System.Linq;
using V2.Interfaces.GUI;
using UnityEngine.EventSystems;
using V2.Behaviours;
using System;

namespace V2.GUI.Mouse.Behaviours
{
    public class Mouse3DSelectionBehaviour : MonoBehaviour, IMouseListenerDown, IMouseListenerUp
    {
        [SerializeField]
        private GameObject targetObject;
        private MouseController mouseController;
        private Selector3DBehaviour selector3DBehaviour;
        // Use this for initialization
        void Start() {
            mouseController = this.GetComponent<MouseController>();
            if(!targetObject) {
                this.enabled = false;
                return;
            }
            selector3DBehaviour = targetObject.GetComponent<Selector3DBehaviour>();
            if(!selector3DBehaviour) {
                this.enabled = false;
                return;
            }
            selector3DBehaviour.MouseController = mouseController;
            mouseController.listenerDown = this;
        }

        // Update is called once per frame
        void Update() {
            if(mouseController.MouseState == V2.Enums.GUI.MouseStates.Dragged) {
                Rect selection = mouseController.GetRectangle();
                var initialPosition = mouseController.GetInitialPosition();//.InitialMousePosition);
                //REFACTOR Ray hit from 2d
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(initialPosition);
                var results = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit? raycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                if(!raycastHitOnLand.HasValue) {
                    return;
                }
                //
                Vector2 verticalProyection2D = initialPosition + selection.height * Vector2.down;
                Vector3 verticalProyection3D = UnityEngine.Camera.main.ScreenToWorldPoint(verticalProyection2D);
                ray = UnityEngine.Camera.main.ScreenPointToRay(verticalProyection2D);
                results = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit? heightRaycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                if(!heightRaycastHitOnLand.HasValue) {
                    return;
                }
                Debug.DrawLine(heightRaycastHitOnLand.Value.point, heightRaycastHitOnLand.Value.point + new Vector3(0, 120, 0), Color.blue);

                Vector2 HorizontalProyection2D = initialPosition + selection.width * Vector2.right;
                Vector3 HorizontalProyection3D = UnityEngine.Camera.main.ScreenToWorldPoint(HorizontalProyection2D);
                ray = UnityEngine.Camera.main.ScreenPointToRay(HorizontalProyection2D);
                results = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit? widthRaycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                if(!widthRaycastHitOnLand.HasValue) {
                    return;
                }
                var finalPosition = ( raycastHitOnLand.Value.point -
                    ( ( raycastHitOnLand.Value.point - widthRaycastHitOnLand.Value.point ) * 0.5f ) -
                    ( ( raycastHitOnLand.Value.point - heightRaycastHitOnLand.Value.point ) * 0.5f ) );
                this.targetObject.transform.position = finalPosition;
                var localscale_y = ( Mathf.Abs(( raycastHitOnLand.Value.point - widthRaycastHitOnLand.Value.point ).magnitude) );
                var localscale_x = Mathf.Abs(( raycastHitOnLand.Value.point - heightRaycastHitOnLand.Value.point ).magnitude);
                this.targetObject.transform.localScale = new Vector3(localscale_x, 1, localscale_y);
            }
            else if(mouseController.MouseState == Enums.GUI.MouseStates._none) {

                if(this.targetObject.transform.position != ( UnityEngine.Camera.main.transform.position + ( Vector3.up + -Vector3.forward ) * 100 )
               ) {
                    this.targetObject.transform.position = UnityEngine.Camera.main.transform.position + ( Vector3.up + -Vector3.forward ) * 100;
                    this.targetObject.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        public void OnUp(PointerEventData data) {
            switch(mouseController.MouseState) {
                case Enums.GUI.MouseStates.Pressed:
                    //send a ray to select one unit
                    break;

            }
        }
        public void OnDown(PointerEventData data) {
            if(data.button == PointerEventData.InputButton.Left
                && selector3DBehaviour.Selection.Count > 0) {
                if(ProvisionalTaskWalkingAsignation())
                    return;
            }
            selector3DBehaviour.ClearSelection();
        }

        private bool ProvisionalTaskWalkingAsignation() {
            //REFACTOR Ray hit from 2d
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            var results = Physics.RaycastAll(ray, Mathf.Infinity);
            RaycastHit? raycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
            if(!raycastHitOnLand.HasValue) {
                return false;
            }
            //
            //if palyer hit in the land
            var selection = selector3DBehaviour.Selection;
            var destiny = V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(raycastHitOnLand.Value.point);
            var sorroundpoins = V2.Classes.Grid.grid.GetSorroundPositions(raycastHitOnLand.Value.point);
            int i = 0;
            int assigned = 0;
            foreach(var selected in selection) {
                var controller = selected.GetComponent<Controllers.UnitController>();
                if(controller) {
                    if(i != 0) {
                        destiny =  sorroundpoins[(i - 1) % sorroundpoins.Length];
                    }
                    controller.AssingTask(new V2.Tasks.Unit.MoveTaskWithAI(selected, destiny));
                    assigned++;
                }
                i++;
            }
            return assigned>0;
        }
    }
}