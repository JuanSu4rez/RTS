using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections.Generic;

public class Grid {


    private int[,] matrix;

    public int[,] Matrix {
        get {
            return matrix;
        }
    }

    private int rows;
    private int cols;
    private int width;
    private int height;


    private Vector3 size = new Vector3();
    private Vector3 initialposition = Vector3.zero;

    public Grid(int rows, int cols, int width, Vector3 initalpos, bool debugird = false) : this(rows, cols, width, width, initalpos, debugird) {

       
       

    }


    public Grid(int rows, int cols, int width, int height, Vector3 initalpos  ,bool debugird = false) {

        initialposition = initalpos;
        this.rows = rows;
        this.cols = cols;
        this.width = width;
        this.height = height;

        size = new Vector3(width,0, this.height);

        matrix = new int[rows, cols];

        int index = 0;
        for (int i = 0; i < matrix.GetLength(0); i++) {

            for (int j = 0; j < matrix.GetLength(1); j++) {

                matrix[i, j] = index;
                index++;

            }


        }

        if (debugird)
        {

            float timeduration = 1000;
            var color = Color.black;

           // Debug.DrawLine(Vector3.zero, new Vector3(60,10,52), color, timeduration);

            for (int x = 0; x < matrix.GetLength(0); x++) {

                for (int y = 0; y < matrix.GetLength(1); y++) {

                  
                    Debug.DrawLine(getPosition(x, y), getPosition(x * width, y + 1), color, timeduration);

                    Debug.DrawLine(getPosition(x, y), getPosition(x+1, y* height ), color, timeduration);


                    Debug.DrawLine(getPosition(x, y), getGridWorldPosition(x, y ), Color.red, timeduration);

                }


            }

        }

    }


    public Vector3 getPosition(int x, int y) {


        return initialposition + (new Vector3(y*width, 0 , x* height));
    }


    public Vector3 getGridWorldPosition(int x, int y) {


        return initialposition+ new Vector3(y * width, 0, x * height)+ (size * 0.5f);
    }

    public Vector3 getGridWorldPosition(Vector2Int vector) {


        return getGridWorldPosition(vector.x, vector.y);
    }

    public Vector3 getGridPositionFromWorldPosition(Vector3 vector ) {

      
        var VX = (int)vector.x;
        var Vz = (int)vector.z;

        return initialposition +getGridWorldPosition( (Vz / height), (VX / width));

    }

    public Vector2Int GetMatrixRowandCol(Vector3 vector) {


        var VX = (int)vector.x;
        var Vz = (int)vector.z;

        return  new Vector2Int((Vz / height), (VX / width));

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
        

          var initialcoordinates =  GetMatrixRowandCol(vector);

        for (int i = 0; i < positionstoaddchache.Length; i++) {

            var result = initialcoordinates + positionstoaddchache[i];

            if (
                result.x >= 0 &&
                result.x < matrix.GetLength(1) &&
                result.y >= 0 &&
                result.y < matrix.GetLength(0)
                ) {

                resultados.Add(getGridWorldPosition(result));
            }


        }


        return resultados.ToArray() ;

    }


    public Vector3[] GetSorroundWorldPositions(Vector3 vector) {

        List<Vector3> resultados = new List<Vector3>();


        var initialcoordinates = vector;
        Vector3  _size = new Vector3(width, 0 ,height);


        for (int i = 0; i < positionstoaddchache.Length; i++) {

            var _vector = new Vector3(positionstoaddchache[i].x* width, 0, positionstoaddchache[i].y * height);
      
            var result = initialcoordinates + (_vector );


        

                resultados.Add(result);
          


        }


        return resultados.ToArray();

    }




    //metodo que convierta las coordenadas del mouse en el mundo y luego se transforme a get world position





}