using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{

    public float minToushSize = 40;

    public float minRadiusSize = 70;

    private int xFrame = 0;
    private int yFrame = 1;
    private int zFrame = 2;


    private int plusX = 1;
    private int minusX = 2;
    private int plusY = 3;
    private int minusY = 4;

    private int plusZ = 5;
    private int minusZ = 6;

    private Vector2 startPos;
    private Vector2 endPos;
    private ArrayList points = new ArrayList();
    private GameObject actualClickedPiece;

    private bool directionChosen;

    private bool capturingMove = false;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getTouchInfo();
    }

    void getTouchInfo(){
        if(Input.touchCount > 0){
            Touch touch = Input.touches[0];
            switch (touch.phase){
                case TouchPhase.Began:
                    startPos = touch.position;
                    Ray ray = Camera.main.ScreenPointToRay(startPos);
                    RaycastHit hit;
                    points = new ArrayList();
                    if(Physics.Raycast(ray, out hit)) {
                        directionChosen = false;
                        capturingMove = true;
                        actualClickedPiece = hit.collider.gameObject;
                        print(hit.transform.InverseTransformPoint(hit.point));
                        print(actualClickedPiece.transform.position);
                    }
                    break;
                case TouchPhase.Moved:
                    if (capturingMove){
                        points.Add(touch.position);
                    }
                    break;
                case TouchPhase.Ended:
                    if (capturingMove){
                        endPos = touch.position;
                        directionChosen = true;
                        capturingMove = false;
                    }
                    break;
            }   
        }
        if (directionChosen){
            int rotateAction = getTouchRotateAction();
            // print(rotateAction);
            int camSide = getCamSide();
            int camFrame = getCamFrame();
            int angleRotated = getCamRotate();
            int squareRotated = (angleRotated%180)/90;
            int inverseRotated = 0;
            if (angleRotated >= 180){
                inverseRotated = 1;
            }

            if (rotateAction == plusY){
                int rotatingAxe;
                if (squareRotated == 1){
                    rotatingAxe = 1;
                } else if(camSide == xFrame){
                    rotatingAxe = 2;
                }else{
                    rotatingAxe = 0;
                }
                // int k = 0;
                // if ((camSide + camFrame)%4 == 0){
                //     k = 2;
                // }else if ((camSide + camFrame) >= 2){
                //     k = squareRotated;
                // }
                // int clockwised = (k + inverseRotated)%2;
                int clockwised = 0;
                if (camSide + camFrame == 2){
                    clockwised = 1;
                } 
                int pieceFrame = getPieceFrame(rotatingAxe);
                executeCubeAction(rotatingAxe, clockwised != 0, pieceFrame);
            }else if (rotateAction == minusY){

            }else if (rotateAction == plusX){
                
            }else if (rotateAction == minusX){
                
            }else if (rotateAction == plusZ){
                bool _clockwised = !(camFrame != 0);
                if (camSide == yFrame){_clockwised = !_clockwised;}
                executeCubeAction(camSide, _clockwised , camFrame);
            }else if(rotateAction == minusZ){
                bool _clockwised = camFrame != 0;
                if (camSide == yFrame){_clockwised = !_clockwised;}
                executeCubeAction(camSide, _clockwised, camFrame);
            }
            directionChosen =false;
        }
    }

    private GameObject getNeighborPiece(Vector2 _adder){
        GameObject _piece = actualClickedPiece;
        Vector2 _pos = new Vector2(startPos.x,startPos.y);
        
        while ((_piece == actualClickedPiece) && (_pos.x<Screen.width) && (_pos.x>0) && (_pos.y<Screen.height) && (_pos.y>0)){
            Ray ray = Camera.main.ScreenPointToRay(_pos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) {
                _piece = hit.collider.gameObject;
            }
            _pos += _adder;
        }
        return _piece;
    }
    
    GameObject getRightPiece(){
        return getNeighborPiece(new Vector2(10, 0));
    }

    GameObject getLeftPiece(){
        return getNeighborPiece(new Vector2(-10, 0));
    }
    GameObject getUpPiece(){
        return getNeighborPiece(new Vector2(0, 10));
    }
    GameObject getDownPiece(){
        return getNeighborPiece(new Vector2(0, -10));
    }

    int getCamSide(){
        Vector3 camPos = Camera.main.transform.position;
        if (camPos.x != 0){
            return xFrame;
        }
        else if (camPos.y != 0){
            return yFrame;
        }
        else {
            return zFrame;
        }
    }

    int getCamFrame(){
        Vector3 camPos = Camera.main.transform.position;
        if (camPos.x > 0 || camPos.y > 0 || camPos.z > 0){
            return 0;
        }
        else {
            return 2;
        }
    }

    int getCamRotate(){
        Vector3 eulerRotate = Camera.main.transform.eulerAngles;
        return (int)eulerRotate.z;
    }

    void executeCubeAction(int axe, bool clockwise, int frame){
        GameObject cube = GameObject.Find("Cube");
        CubeBuilder cubeScript = cube.GetComponent<CubeBuilder>();
        cubeScript.rotate(axe, clockwise, frame);
    }

    int getTouchRotateAction(){
        Vector2 center = startPos + (endPos - startPos)/2;
        float radius = Mathf.Sqrt(Mathf.Pow(((center - startPos).x),2) + Mathf.Pow(((center - startPos).y),2));
        ArrayList angles = new ArrayList();

        bool verticalMove = true, horizontalMove = true, rotateMove = true;
        foreach (Vector2 item in points){
            if (Mathf.Abs(radius - Mathf.Sqrt(Mathf.Pow(((center - item).x),2) + Mathf.Pow(((center - item).y),2))) > minRadiusSize){
                rotateMove = false;
            }
            else{
                Vector2 tempPoint = item - center;
                float tempAngle = Mathf.Acos(tempPoint.x/Mathf.Sqrt((tempPoint.x*tempPoint.x)+(tempPoint.y*tempPoint.y)))*180/Mathf.PI;
                if (tempPoint.y < 0){
                    tempAngle = 360 - tempAngle;
                }
                angles.Add(tempAngle);
            }
            if (Mathf.Abs(startPos.x - item.x)>minToushSize){
                verticalMove = false;
            }
            if (Mathf.Abs(startPos.y - item.y)>minToushSize){
                horizontalMove = false;
            }
        }
        if (verticalMove){
            if (endPos.y > startPos.y){
                return plusY;
            }
            else{
                return minusY;
            }
        }
        if (horizontalMove){
            if(endPos.x > startPos.x){
                return plusX;
            }
            else{
                return minusX;
            }
        }
        if (rotateMove){
            int plusAngleCount = 0, minusAngleCount = 0;
            for(int i=1; i< angles.Count; i++){
                if((float)angles[i] > (float)angles[i-1]){
                    plusAngleCount += 1;
                }else{
                    minusAngleCount += 1;
                }
            }
            if (plusAngleCount > minusAngleCount){
                return plusZ;
            }
            else{
                return minusZ;
            }
        }   
        return 0;
    }

    int getPieceFrame(int rotAxe){
        CubePieceBuilder pieceScript = actualClickedPiece.GetComponent<CubePieceBuilder>();            

        if (rotAxe == 0){
            return (((int)Mathf.Round(actualClickedPiece.transform.position.x/(pieceScript.size)) -1)*(-1));
        }
        else if (rotAxe == 1){
            return (int) (Mathf.Round(actualClickedPiece.transform.position.y/(pieceScript.size)) - 1)*(-1);
        }
        else{
            return (int) (Mathf.Round(actualClickedPiece.transform.position.z/(pieceScript.size)) - 1)*(-1);
        }
    }

}
