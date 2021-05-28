using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using V2.Enums.GUI;

namespace V2.GUI.Mouse
{
    public  class MouseAreaSelectionOnGuiDrawer:MonoBehaviour
    {
        private MouseController mouseController;
        private Texture2D selectionTexture = null;
        private Texture2D selectionBorder = null;
        // Use this for initialization
          void Start() {
            mouseController = this.GetComponent<MouseController>();
            if(selectionTexture == null) {
                selectionTexture = new Texture2D(1, 1);
                selectionTexture.SetPixels32(new Color32[] { new Color32(0, 0, 255, 30) });
                selectionTexture.alphaIsTransparency = true;
                selectionTexture.Apply();
            }
            if(selectionBorder == null) {
                selectionBorder = new Texture2D(1, 1);
                selectionBorder.SetPixels32(new Color32[] { new Color32(255, 0, 0, 100) });
                selectionBorder.Apply();
            }
        }
      
        void OnGUI() {
            var originalcolor = UnityEngine.GUI.color;
            var originalcolorback = UnityEngine.GUI.backgroundColor;
            UnityEngine.GUI.color = Color.blue;
            UnityEngine.GUI.backgroundColor = Color.blue;
            var selection = mouseController.GetRectangle();
            switch(mouseController.MouseState) {
                case MouseStates.Dragged:
                    UnityEngine.GUI.DrawTexture(selection, selectionTexture);
                    UnityEngine.GUI.DrawTexture(new Rect(selection.x, selection.y, selection.width, 1), selectionBorder);
                    UnityEngine.GUI.DrawTexture(new Rect(selection.x, selection.y, 1, selection.height), selectionBorder);
                    UnityEngine.GUI.DrawTexture(new Rect(selection.x + selection.width, selection.y, 1, selection.height), selectionBorder);
                    UnityEngine.GUI.DrawTexture(new Rect(selection.x, selection.y + selection.height, selection.width, 1), selectionBorder);

                    break;
            }
            UnityEngine.GUI.color = originalcolor;
            UnityEngine.GUI.backgroundColor = originalcolorback;
        }


    }

}