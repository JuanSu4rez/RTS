using UnityEngine;
using System.Collections;
using V2.GUI;
using System.Linq;
using V2.Interfaces.GUI;
using UnityEngine.EventSystems;

namespace V2.GUI.Mouse.Behaviours
{
    public class Mouse3DSelectionBehaviour : MonoBehaviour, IMouseListenerUp
    {
        [SerializeField]
        private GameObject targetObject;
        private MouseController mouseController;
        // Use this for initialization
        void Start() {
            mouseController = this.GetComponent<MouseController>();
            if(!targetObject) {
                this.enabled = false;
            }
        }

        // Update is called once per frame
        void Update() {
            if(mouseController.MouseState == V2.Enums.GUI.MouseStates.Dragged) {
                Rect selection = mouseController.GetRectangle();
                var initialPosition = mouseController.GetInitialPosition();//.InitialMousePosition);
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(initialPosition);
                var results = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit? raycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                if(!raycastHitOnLand.HasValue) {
                    return;
                }
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
                /*
              Rect selection = mouseController.GetRectangle();
              Debug.Log(CameraScript.firstclick+" | "+ mouseController.InitialMousePosition+" "+Screen.height);
              //return;
              Vector3 initialPoint = UnityEngine.Camera.main.ScreenToWorldPoint(mouseController.InitialMousePosition);
              Vector3 verticalmouseproyection = UnityEngine.Camera.main.ScreenToWorldPoint(mouseController.InitialMousePosition + ( Vector2.down * selection.height ));
              Vector3 horizaontalmouseproyection = UnityEngine.Camera.main.ScreenToWorldPoint(mouseController.InitialMousePosition + ( Vector2.right * selection.width ));
              Vector3 initialPointLand = Vector3.zero;
              Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mouseController.InitialMousePosition);
              var initialPointHits = Physics.RaycastAll(ray, Mathf.Infinity);
              RaycastHit? initialPointHit = initialPointHits.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
              if(initialPointHit.HasValue) {
                  initialPointLand = initialPointHit.Value.point;
                  this.targetObject.transform.position = initialPointHit.Value.point;
                  var proyeccionvertical = mouseController.InitialMousePosition + ( Vector2.down * selection.height );
                  ray = UnityEngine.Camera.main.ScreenPointToRay(proyeccionvertical);
                  var verticalRayHits = Physics.RaycastAll(ray, Mathf.Infinity);
                  RaycastHit? verticalRayHit = verticalRayHits.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                  var proyeccionhorizontal = mouseController.InitialMousePosition + ( Vector2.right * selection.width );
                  ray = UnityEngine.Camera.main.ScreenPointToRay(proyeccionhorizontal);
                  var horizontalRayHits = Physics.RaycastAll(ray, Mathf.Infinity);
                  RaycastHit? horizontalRayHit = horizontalRayHits.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
                  if(verticalRayHit.HasValue && horizontalRayHit.HasValue) {
                      var auxpostion = ( initialPointLand - ( ( initialPointLand - verticalRayHit.Value.point ) * 0.5f ) - ( ( initialPointLand - horizontalRayHit.Value.point ) * 0.5f ) );
                      this.targetObject.transform.position = auxpostion;
                      this.targetObject.transform.localScale = new Vector3(
                          ( Mathf.Abs(( initialPointLand - verticalRayHit.Value.point ).magnitude) ),
                          1,
                         ( Mathf.Abs(( initialPointLand - horizontalRayHit.Value.point ).magnitude) )
                          );
                  }
                  */
            }
            else if(mouseController.MouseState == Enums.GUI.MouseStates._none) {

                if(this.targetObject.transform.position != (UnityEngine.Camera.main.transform.position + ( Vector3.up + -Vector3.forward ) * 100)
               ) { 
                this.targetObject.transform.position = UnityEngine.Camera.main.transform.position + (Vector3.up + -Vector3.forward) * 100;
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

    }
}