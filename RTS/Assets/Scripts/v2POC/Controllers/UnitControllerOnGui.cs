using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace V2.Controllers
{
    public partial class UnitController{
        private Texture2D textureHealt;
        private Texture2D textureCurrentHealt;
        // Update is called once per frame
        void OnGUI() {

            if(this.IsAlive() &&
                IsSelected) {
                Vector3 screenPos = UnityEngine.Camera.main.WorldToScreenPoint(this.transform.position);
                Vector2 v2 = new Vector2(screenPos.x, Screen.height - screenPos.y);
                UnityEngine.GUI.contentColor = Color.cyan;
                drawHealthGUIBar();
            }
        }

        public void drawHealthGUIBar() {
            if(textureCurrentHealt == null) {
                textureHealt = new Texture2D(1, 1);
                textureHealt.SetPixel(0, 0, Color.red);
                textureHealt.Apply();
                textureCurrentHealt = new Texture2D(1, 1);
                textureCurrentHealt.SetPixel(0, 0, Color.green);
                textureCurrentHealt.Apply();
            }
            var sqwidth = 100/2;
            var sqheight = 10 / 2;
            Vector3 screenPos = UnityEngine.Camera.main.WorldToScreenPoint(this.transform.position);
            screenPos += new Vector3(-( sqwidth / 2 ), 30);
            UnityEngine.GUI.DrawTexture(new Rect(screenPos.x, Screen.height - screenPos.y-10, sqwidth, 2), textureHealt);
            var rect = new Rect(new Vector2(screenPos.x, Screen.height - screenPos.y-10), new Vector2(sqwidth *(CurrentHealth / 100), 2));
            UnityEngine.GUI.DrawTexture(rect, textureCurrentHealt);
        }
    }
}