using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{    
    public float radius = 50;
    public int moveSpeed = 1, rotateSpeed = 1;

    private bool moving, rotating, movingClockwised;

    private float radiansAlreadyMoved = 0.0f, realRadians;

    private int right = 1;
    private int left = 2;
    private int top = 3;
    private int down = 4;

    private float anglesAlreadyRotated = 0.0f;

    private Vector3 rotatingAxe;
    private Vector3 targetPosition;
    private char movingAxe;

    // Start is called before the first frame update
    void Start()
    {
        resetCamera();
        resetFlags();
    }
    void resetCamera(){
        transform.position= new Vector3(0.0f, 0.0f, -radius);
    }
    void resetFlags(){
        moving = false;
        rotating = false;
    }
    public void moveRight(){
        moveBySide(right);
    }
    public void moveLeft(){
        moveBySide(left);
    }
    public void moveUp(){
        moveBySide(top); 
    }
    public void moveDown(){
        moveBySide(down);
    }
    // Update is called once per frame
    void Update()
    {
        if (moving){
            movingHandler();
        }
        if (rotating){
            rotatingHandler();
        }  
    }

    void movingHandler(){
        int k = 1;
        if (movingClockwised){
            k = -1;
        }
        float radiansToMove = Mathf.PI * 2 * moveSpeed / 360;
        if ((radiansAlreadyMoved + radiansToMove) >= (Mathf.PI/2)){
            radiansToMove = (Mathf.PI/2) - radiansAlreadyMoved;
            moving = false;
        }
        radiansAlreadyMoved = radiansAlreadyMoved + radiansToMove;
        realRadians = realRadians + (radiansToMove*k);

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        if (movingAxe == 'z'){
            x = Mathf.Cos(realRadians) * radius;
            y = Mathf.Sin(realRadians) * radius;
        }
        else if(movingAxe == 'x'){
            y = Mathf.Sin(realRadians) * radius;
            z = Mathf.Cos(realRadians) * radius;
        }
        else if(movingAxe == 'y'){
            x = Mathf.Cos(realRadians) * radius;
            z = Mathf.Sin(realRadians) * radius;
        }

        if (moving){
            transform.position = new Vector3(x,y,z);        
        }
        else{
            transform.position = targetPosition;
        }
    }

    void rotatingHandler(){
        float angleToRot = rotateSpeed;
        if (anglesAlreadyRotated + rotateSpeed >=90){
            rotating = false;
            angleToRot = 90 - anglesAlreadyRotated;
        }
        transform.Rotate(rotatingAxe * angleToRot);
        anglesAlreadyRotated += angleToRot;
        
    }

    void moveBySide(int side){
        if (!moving && !rotating){
            anglesAlreadyRotated = 0.0f;
            radiansAlreadyMoved = 0.0f;
            if (side == right){
                rotatingAxe = Vector3.down;
            }
            else if(side == left){
                rotatingAxe = Vector3.up;
            }
            else if(side == top){
                rotatingAxe = Vector3.right;
            }
            else{
                rotatingAxe = Vector3.left;
            }
           

            transform.Rotate(rotatingAxe*90);
            Vector3 tempAngle = transform.eulerAngles;
            transform.Rotate(-rotatingAxe*90);
            targetPosition = getNewPositionByRotate(tempAngle);

            setupMoving(transform.position, targetPosition);            

            rotating = true;      
            moving = true;        
        }
    }

    Vector3 getNewPositionByRotate(Vector3 newRotate){
        float x = newRotate.x;
        float y = newRotate.y;
        float z = newRotate.z;

        float posX = 0.0f, posY = 0.0f, posZ = 0.0f;
        if (Mathf.Abs(x) < 5){
            if (Mathf.Abs(y-270) < 5){
                posX = radius;
            }
            else if (Mathf.Abs(y-90) <5){
                posX = -radius;
            }
            if(Mathf.Abs(y)<5){
                posZ = -radius;
            }
            else if(Mathf.Abs(y-180)<5){
                posZ = radius;
            }
        }
        if (Mathf.Abs(z) < 5){
            if (Mathf.Abs(x-270) < 5){
                posY = -radius;
            }
            else if (Mathf.Abs(x-90) <5){
                posY = radius;
            }
        }
        return new Vector3(posX,posY,posZ);
    }

    void setupMoving(Vector3 init, Vector3 end){
        targetPosition = end;
        if (init.x == 0 && end.x == 0){
            movingAxe = 'x';
            if (init.y == 0){
                movingClockwised = !(Mathf.Abs((init.z + end.y)) > 0);
                realRadians = Mathf.PI* (1-(init.z/radius))/2;
            }
            if (init.z == 0){
                movingClockwised = (Mathf.Abs((init.y + end.z)) > 0);
                realRadians = Mathf.PI* ((init.y/radius))/2;
            }
        }
        if (init.y == 0 && end.y == 0){
            movingAxe = 'y';
            if (init.x == 0){
                movingClockwised = (Mathf.Abs(init.z + end.x) > 0);
                realRadians = Mathf.PI* (2-(init.z/radius))/2;
            }
            if (init.z == 0){
                movingClockwised = !(Mathf.Abs(init.x + end.z) > 0);
                realRadians = Mathf.PI* (1-(init.x/radius))/2;
            }
        }
        if (init.z == 0 && end.z == 0){
            movingAxe = 'z';
            if (init.x == 0){
                movingClockwised = (Mathf.Abs(init.y + end.x) > 0);
                realRadians = Mathf.PI* (2-(init.y/radius))/2;
            }
            if (init.y == 0){
                movingClockwised = !(Mathf.Abs(init.x + end.y) > 0);
                realRadians = Mathf.PI* (1-(init.x/radius))/2;
            }
        }
    }

}