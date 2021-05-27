using UnityEngine;
using System.Collections;

namespace V2.GUI
{
    public class MousePlaneBehaviour: MonoBehaviour
    {
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        void OnMouseDown() {
            //Debug.Log("OnMouseDown "+ Input.GetMouseButtonDown(0)+" "+ Input.GetMouseButtonDown(1)+" "+ Input.GetMouseButtonDown(2));
            if(Input.GetMouseButtonDown(0)) Debug.Log("Pressed left click.");
            if(Input.GetMouseButtonDown(1)) Debug.Log("Pressed right click.");
            if(Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
        }// OnMouseDown is called when the user has pressed the mouse button while over the Collider.
        void OnMouseDrag() {
            //Debug.Log("OnMouseDrag");
        }// OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.
        void OnMouseEnter() {
            // Debug.Log("OnMouseEnter");
        }// Called when the mouse enters the Collider.
        void OnMouseExit() {
            // Debug.Log("OnMouseExit");
        }// Called when the mouse is not any longer over the Collider.
        void OnMouseOver() {
           // Debug.Log("OnMouseOver");
        }// Called every frame while the mouse is over the Collider.
        void OnMouseUp() {
            // Debug.Log("OnMouseUp");
        }// OnMouseUp is called when the user has released the mouse button.
        void OnMouseUpAsButton() {
            // Debug.Log("OnMouseUpAsButton");
        }// OnMouseUpAsButton is only called when the mouse is released over the same Collider as it was pressed.

    }

}