using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace V2.Classes
{
    public class Grid {
        public static  readonly Grid grid = new Grid(1000, 1000, 2,true);
        public static void InitGrid() {

        }
        private int rows;
        private int cols;
        public  int width;
        public  int height;
        private Vector3 size = new Vector3();
        private Vector3 initialposition = Vector3.zero;
        public Grid(int rows, int cols, int width,int height, Vector3 initalpos , bool debugird = false)  {
            this.rows = rows;
            this.cols = cols;
            this.width = width;
            this.height = height;
            this.initialposition = initalpos;
            size = new Vector3(width, 0, height);
            if(debugird) {
                DebugGrid(300);
            }
        }

        public void DebugGrid(int time = 10) {
            var _debugWidth = rows;
            var gridColor = new Color(0, 0, 0, 0.2f);
            for(int i = 0; i < rows; i++) {
                Debug.DrawLine(new Vector3(0, 0, i*width), new Vector3(_debugWidth, 0, i * width), gridColor, time);
            }
            for(int i = 0; i < cols; i++) {
                Debug.DrawLine(new Vector3(i * height, 0, 0), new Vector3(i * height, 0, _debugWidth), gridColor, time);
            }
        }

        public Grid(int rows, int cols, int width, bool debugird = false)
            :this( rows,cols,width , width,Vector3.zero, debugird)
        {
        }
        public Vector3 getWorldPositionFromXY(int x, int y) {
            return initialposition + ( new Vector3(y * width, 0, x * height) );
        }
        public Vector3 getCenteredWorldPositionFromXY(int x, int y) {
            return initialposition + new Vector3(y * width, 0, x * height) + ( size * 0.5f );
        }
        public Vector3 getCenteredWorldPositionFromVector2Int(Vector2Int vector) {
            return getCenteredWorldPositionFromXY(vector.x, vector.y);
        }
        public Vector3 getCenteredGridPositionFromWorldPosition(Vector3 vector) {
            var VX = (int)vector.x;
            var Vz = (int)vector.z;
            return initialposition + getCenteredWorldPositionFromXY(( Vz / height ), ( VX / width ));
        }

        public Vector2Int GetMatrixRowandCol(Vector3 vector) {
            var VX = (int)vector.x;
            var Vz = (int)vector.z;
            return new Vector2Int(( Vz / height ), ( VX / width ));
        }
        private static Vector2Int[] positionstoaddchache = new Vector2Int[] {

            new Vector2Int(1,1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,-1),
            new Vector2Int(1,-1),/* */
            Vector2Int.right,
            -Vector2Int.right,
            Vector2Int.up,
            -Vector2Int.up, /**/

        };
        public Vector3[] GetSorroundPositions(Vector3 vector) {
            List<Vector3> resultados = new List<Vector3>();
            var initialcoordinates = GetMatrixRowandCol(vector);
            for(int i = 0; i < positionstoaddchache.Length; i++) {
                var result = initialcoordinates + positionstoaddchache[i];
                if(
                    result.x >= 0 &&
                    result.x < rows &&
                    result.y >= 0 &&
                    result.y < cols
                    ) {
                    resultados.Add(getCenteredWorldPositionFromVector2Int(result));
                }
            }
            return resultados.ToArray();
        }

        public Vector3[] GetSorroundWorldPositions(Vector3 vector) {
            List<Vector3> resultados = new List<Vector3>();
            var initialcoordinates = vector;
            Vector3 _size = new Vector3(width, 0, height);
            for(int i = 0; i < positionstoaddchache.Length; i++) {
                var _vector = new Vector3(positionstoaddchache[i].x * width, 0, positionstoaddchache[i].y * height);
                var result = initialcoordinates + ( _vector );
                resultados.Add(result);
            }
            return resultados.ToArray();
        }
    }
}