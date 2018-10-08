using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public CameraMovementType cameraMovementType = CameraMovementType.Type1;

    Vector3 cameraPosition;

    public float VelocityScale = 10f;


    public int AreaScaleW = 8;
    public int AreaScaleH = 8;

    public bool DebugAreas;

    // Use this for initialization
    void Start()
    {
        cameraPosition = gameObject.transform.position;

        //Debug.Log("Total de la pantalla " + Screen.width);
        //Debug.Log("Tercio de la pantalla " + Screen.width / 3);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            return;
        }

        switch (cameraMovementType)
        {
            case CameraMovementType.Type1:
                MovementTypeOne();

                break;
            case CameraMovementType.Type2:
                MovementTypeTwo();

                break;
        }




    }


    private void MovementTypeOne()
    {
        Vector2 mousePosition = Input.mousePosition;

        //Mouse Correction included to use cotains method of the Rect class
        //So the conditions to move vertically the camera were swapped
        mousePosition.y = Screen.height - Input.mousePosition.y;

        var width = CameraScript.gameareas.GameArea.width;
        var height = CameraScript.gameareas.GameArea.height;

        if (!CameraScript.gameareas.GameArea.Contains(mousePosition))
            return;

        //Debug.Log("mousePosition.x " + mousePosition.x);

        if (mousePosition.x < width / AreaScaleW)
        {
            gameObject.transform.position = new Vector3(--cameraPosition.x, cameraPosition.y, ++cameraPosition.z);
        }
        if (mousePosition.x > width - width / AreaScaleW)
        {
            gameObject.transform.position = new Vector3(++cameraPosition.x, cameraPosition.y, --cameraPosition.z);
        }
        if (mousePosition.y < height / AreaScaleH)
        {
            //this condition used to be on mouse position greater than height - height / AreaScale
            gameObject.transform.position = new Vector3(++cameraPosition.x, cameraPosition.y, ++cameraPosition.z);
        }
        if (mousePosition.y > height - height / AreaScaleH)
        {
            //this condition used to be on mouse position less than  height / AreaScale
            gameObject.transform.position = new Vector3(--cameraPosition.x, cameraPosition.y, --cameraPosition.z);
        }
    }

    Vector3 result = Vector3.zero;
    Vector3 distance = Vector3.zero;
    Vector3 centerScreen = Vector3.zero;
    Vector3 mousePosition = Vector3.zero;
    bool MouseInGameArea;

    private void MovementTypeTwo()
    {
        mousePosition = Input.mousePosition;

        //Mouse Correction included to use cotains method of the Rect class
        //So the conditions to move vertically the camera were swapped
        mousePosition.y = Screen.height - Input.mousePosition.y;

        centerScreen = new Vector2(CameraScript.gameareas.GameArea.width / 2, CameraScript.gameareas.GameArea.height / 2);

        distance = (centerScreen - mousePosition);
        result = distance.normalized;

        var width = CameraScript.gameareas.GameArea.width;
        var height = CameraScript.gameareas.GameArea.height;


        MouseInGameArea = CameraScript.gameareas.GameArea.Contains(mousePosition);
        if (!MouseInGameArea)
            return;

        //Debug.Log("mousePosition.x " + mousePosition.x);



        if (mousePosition.x < width / AreaScaleW
            || mousePosition.x > width - width / AreaScaleW
            || mousePosition.y < height / AreaScaleH
            || mousePosition.y > height - height / AreaScaleH
            )
        {
            //Inversion due to camera rotation
            result.x = result.x * -1;
            result.z = result.y;
            result.y = 0;

            var finalpos  = gameObject.transform.position + (result * (VelocityScale * Time.deltaTime));
           // finalpos.y = gameObject.transform.position.y;
            gameObject.transform.position = finalpos;
        }

    }


    void OnGUI()
    {
        if (DebugAreas)
        {
            var originalcolor = GUI.color;

            var originalbackcolor = GUI.backgroundColor;

            var width = CameraScript.gameareas.GameArea.width;
            var height = CameraScript.gameareas.GameArea.height;

          
            var _width = width / AreaScaleW;

     
            var _height =  height / AreaScaleH;

            var auxColor = Color.blue;
            auxColor.a = 0.5f;

            GUI.backgroundColor = auxColor;

            GUI.Button(new Rect(0, 0, width, _height), "");
            GUI.Button(new Rect(0, 0, _width, height), "");
            GUI.Button(new Rect(0, height-_height, width, _height), "");
            GUI.Button(new Rect(width - _width, 0, _width, height), "");


            auxColor = Color.white;
            auxColor.a = 0.5f;

            GUI.backgroundColor = auxColor;

            GUI.Label(new Rect(width/2,height/2, 100, 100), "A");



            GUI.color = originalcolor;

            GUI.backgroundColor = originalbackcolor;

            if (cameraMovementType == CameraMovementType.Type2) {

                Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (result* 10));
                GUI.Label(new Rect(Screen.width / 2 + 10, Screen.height / 2 + 10, Screen.width, 500), cameraMovementType + " " + MouseInGameArea + " " + centerScreen + " " + mousePosition + " " + distance + " " + result);
            }
               

        }



    }

}
