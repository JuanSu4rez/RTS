using UnityEngine;
using UnityEditor;
namespace V2.Structs
{
    public struct RectangleProjectedVetexes  {
        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomLeft;
        public Vector3 BottomRight;

        public void SetYToZero() {
            TopLeft.y = 0;
            TopRight.y = 0;
            BottomLeft.y = 0;
            BottomRight.y = 0;
        }
    }
}
