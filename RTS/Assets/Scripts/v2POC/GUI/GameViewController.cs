using UnityEngine;
using System.Collections;
namespace V2.GUI
{
    public class GameViewController : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image MainPanel;
        [SerializeField]
        private UnityEngine.UI.Image OptionsPanel;
        // Use this for initialization
        void Start() {
            V2.Classes.Grid.InitGrid();
            V2.Classes.Entities.EntityManger.Init();
        }
        // Update is called once per frame
        void Update() {

        }
    }
}