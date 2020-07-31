using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePieceMoveHandler : MonoBehaviour
{
    public int moveSpeed = 1, rotateSpeed = 1;

    private const int x=0, y=1, z=2;

    private bool moving = false, rotating = false;
    private bool clockwised;
    private int axe;

    private float radiansAlreadyMoved = 0.0f, angleAlreadyRotated = 0.0f, radians;

    private Vector3 real_x_vec = Vector3.right, real_y_vec = Vector3.down, real_z_vec = Vector3.forward;

    public void rotateAxe(int _axe, bool _clockwised){
        if (!moving && !rotating){
            axe = _axe;
            clockwised = _clockwised;
            angleAlreadyRotated = 0;
            if (axe == z){
                radians = getRadians(transform.position.y, transform.position.x);
            }
            else if(axe == x){
                radians = getRadians(transform.position.z, transform.position.y);
            }
            else if(axe == y){
                radians = getRadians(transform.position.z, transform.position.x);
            }
            if (!float.IsNaN(radians)){
                moving = true;
                radiansAlreadyMoved = 0;
            }
            rotating = true;
        }
        
    }

    private float getRadians(float sCord, float cCord){
        float initialradius = Mathf.Acos(cCord/getRadius());
        if (sCord < 0){
            if (cCord < 0){
                initialradius = Mathf.PI + (Mathf.PI - initialradius);
            }
            else{
                initialradius = (2* Mathf.PI) - initialradius;
            }
        }
        return initialradius;
    }

    void Start()
    {
        
    }

    void Update()
    {
        moveHandler();
        rotateHandler();           
    }

    void moveHandler(){
        if (moving){
            Vector3 oPos = transform.position;
            int k = 1;
            if (clockwised){
                k = -1;
            }
            float radiansToMove = Mathf.PI * 2 * moveSpeed / 360;
            if ((radiansAlreadyMoved + radiansToMove) >= (Mathf.PI/2)){
                radiansToMove = (Mathf.PI/2) - radiansAlreadyMoved;
                moving = false;
            }
            radiansAlreadyMoved = radiansAlreadyMoved + radiansToMove;
            radians = radians + (radiansToMove*k);
            
            float xVal = oPos.x;
            float yVal = oPos.y;
            float zVal = oPos.z;
            float radius = getRadius();
            if (axe == z){
                xVal = Mathf.Cos(radians) * radius;
                yVal = Mathf.Sin(radians) * radius;
            }
            else if(axe == x){
                zVal = Mathf.Sin(radians) * radius;
                yVal = Mathf.Cos(radians) * radius;
            }
            else if(axe == y){
                xVal = Mathf.Cos(radians) * radius;
                zVal = Mathf.Sin(radians) * radius;
            }
            Vector3 newPos = new Vector3(xVal,yVal,zVal);
            transform.position = newPos;
        }
    }

    private float getRadius(){
        Vector3 targetPos = transform.position;
            if (axe == z){
                return Mathf.Sqrt((targetPos.x*targetPos.x) + (targetPos.y*targetPos.y)) ;
            }
            else if(axe == x){
                return Mathf.Sqrt((targetPos.z*targetPos.z) + (targetPos.y*targetPos.y)) ;
            }
            else if(axe == y){
                return Mathf.Sqrt((targetPos.z*targetPos.z) + (targetPos.x*targetPos.x)) ;
            }
            return 0.0f;
    }

    void rotateHandler(){
        if (rotating){
            float angleToRot = Mathf.Acos(Mathf.Cos(Mathf.PI * 2 * rotateSpeed / 360)) * 360 / (2*Mathf.PI);
            int j = 1;
            if (clockwised){
                j = -1;
            }
            if ((angleAlreadyRotated + angleToRot) >= 90){
                angleToRot = 90 - angleAlreadyRotated;
                attRotate(j);
                rotating = false;
            }
            angleAlreadyRotated = angleAlreadyRotated + angleToRot;
            Vector3 vec = new Vector3(0,0,0);
            if (axe == z){
                vec = real_z_vec * j;
            }
            else if(axe == x){
                vec = real_x_vec * j;
            }
            else if(axe == y){
                vec = real_y_vec * j;
            }
            transform.Rotate(vec*(angleToRot));
        }     
    }

    private void attRotate(int j){
        if (axe == z){
            Vector3 temp_x_vec = real_x_vec;
            real_x_vec = real_y_vec * -j;
            real_y_vec = temp_x_vec * (j);
        }
        else if(axe == x){
            Vector3 temp_z_vec = real_z_vec;
            real_z_vec = real_y_vec * -j;
            real_y_vec = temp_z_vec * (j);
        }
        else if(axe == y){
            Vector3 temp_z_vec = real_z_vec;
            real_z_vec = real_x_vec * (j);
            real_x_vec = temp_z_vec * (-j);
        }
    }

    public bool getMovingOrRotating(){
        return moving || rotating;
    }
}
