using UnityEngine;
using System.Collections;
using V2.GUI;
using System.Linq;
using V2.Interfaces.GUI;
using UnityEngine.EventSystems;
using V2.Behaviours;
using System;
using System.Collections.Generic;
using V2.Structs;
using V2.Utils;
using V2.Constans;

namespace V2.GUI.Mouse.Behaviours
{
    public class Mouse3DSelectionBehaviour : MonoBehaviour, IMouseListenerLeftClickDown, IMouseListenerLeftClickUp, IMouseListenerLeftClickDrag, IMouseListenerRightClickDown
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
            mouseController.mouseListenerLeftClickDown = this;
            mouseController.mouseListenerLeftClickUp = this;
            mouseController.mouseListenerLeftClickDrag = this;
            mouseController.mouseListenerRightClickDown = this;
        }
        // Update is called once per frame
        void Update() {
         
        }
        #region mouse_events 
        void IMouseListenerRightClickDown.OnDown(PointerEventData data) {
            //Debug.Log("Right click down.");
            if(selector3DBehaviour.Selection.Count > 0) {
                ProvisionalTaskWalkingAsignation()
            }
        }
        public void OnDown(PointerEventData data) {
            //Debug.Log("Left click down.");
            selector3DBehaviour.ClearSelection();
        }
        public void OnDrag(PointerEventData data) {
            //Debug.Log("Left click drag.");
            CaculateCubeSelection();
        }
        public void OnUp(PointerEventData data) {
            //Debug.Log("Left click up.");
            switch(mouseController.MouseState) {
                case Enums.GUI.MouseStates.Pressed:
                    //send a ray to select one unit
                    break;
                case Enums.GUI.MouseStates.Dragged:
                    //send a ray to select one unit
                    HideCubeSelection();
                    //if there is no selection validate two rays
                    break;
            }
        }
        #endregion
        #region Cube selection
        private void CaculateCubeSelection() {
            var screenRectVertexes = this.mouseController.GetScreenRectVertexes();
            RectangleProjectedVetexes rectangleProjectedVetexes = new RectangleProjectedVetexes();
            if(TryGetProjectedPoints(screenRectVertexes, ref rectangleProjectedVetexes)) {
                AssingCubeSelectionPositionAndScale(ref rectangleProjectedVetexes);
            }
        }
        private bool TryGetProjectedPoints(RectangleVetexes rectangleVetexes, ref RectangleProjectedVetexes projectedVetexes ) {
           var topLeftHit =  PhisycsUtils.GetRaycastHitFromPoint(rectangleVetexes.TopLeft, Layers.LayerLand);
            if(!topLeftHit.HasValue)
                return false;
            var bottomLeftHit = PhisycsUtils.GetRaycastHitFromPoint(rectangleVetexes.BottomLeft, Layers.LayerLand);
            if(!bottomLeftHit.HasValue)
                return false;
            var topRightHit = PhisycsUtils.GetRaycastHitFromPoint(rectangleVetexes.TopRight, Layers.LayerLand);
            if(!topRightHit.HasValue)
                return false;
            projectedVetexes.TopLeft = topLeftHit.Value.point;
            projectedVetexes.BottomLeft = bottomLeftHit.Value.point;
            projectedVetexes.TopRight = topRightHit.Value.point;
            projectedVetexes.SetYToZero();
            return true;
        }
        private void AssingCubeSelectionPositionAndScale(ref RectangleProjectedVetexes rectangleProjectedVetexes) {
            var finalPosition = ( rectangleProjectedVetexes.TopLeft -
                ( ( rectangleProjectedVetexes.TopLeft - rectangleProjectedVetexes.TopRight ) * 0.5f ) -
                ( ( rectangleProjectedVetexes.TopLeft - rectangleProjectedVetexes.BottomLeft ) * 0.5f ) );
            this.targetObject.transform.position = finalPosition;

            var localscale_y = Mathf.Abs(( rectangleProjectedVetexes.TopLeft - rectangleProjectedVetexes.TopRight ).magnitude);
            var localscale_x = Mathf.Abs(( rectangleProjectedVetexes.TopLeft - rectangleProjectedVetexes.BottomLeft ).magnitude);
            this.targetObject.transform.localScale = new Vector3(localscale_x, 1, localscale_y);
        }
        private void HideCubeSelection() {
            if(this.targetObject.transform.position != ( UnityEngine.Camera.main.transform.position + ( Vector3.up + -Vector3.forward ) * 100 )
            ) {
                this.targetObject.transform.position = UnityEngine.Camera.main.transform.position + ( Vector3.up + -Vector3.forward ) * 100;
                this.targetObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        #endregion
        #region taskasignation
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
            List<Vector3> destinyPoints = new List<Vector3>();
            destinyPoints.Add(destiny);
            destinyPoints.AddRange(sorroundpoins);
            int i = 0;
            int assigned = 0;

            foreach(var selected in selection) {
                var controller = selected.GetComponent<Controllers.UnitController>();
                if(controller) {
                    destiny = destinyPoints[i % destinyPoints.Count];
                    controller.AssingTask(new V2.Tasks.Unit.MoveTaskWithAI(selected, destiny));
                    assigned++;
                }
                i++;
            }
            return assigned > 0;
        }
        #endregion
    }
}