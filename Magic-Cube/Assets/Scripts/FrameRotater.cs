using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRotater : MonoBehaviour
{
    public int speed = 5;

    private bool rotating = false;
    private GameObject frameObject;
    private List<GameObject> listOfPieces;
    private float angleAlreadyRotated;
    private Vector3 rotateAxe;

    void Update()
    {
        updateRotate();
        checkFinishRotate();
    }

    private void updateRotate(){
        if (rotating){
            float angleToRot = Mathf.Acos(Mathf.Cos(Mathf.PI * 2 * speed / 360)) * 360 / (2*Mathf.PI);
            if ((angleAlreadyRotated + angleToRot) >= 90){
                angleToRot = 90 - angleAlreadyRotated;
            }
            angleAlreadyRotated = angleAlreadyRotated + angleToRot;
            frameObject.transform.Rotate(rotateAxe*(angleToRot));
        } 
    }

    private void checkFinishRotate(){
        if (angleAlreadyRotated >= 90){
            rotating = false;
            finishRotate();
        }
    }

    private void finishRotate(){
        foreach (var item in listOfPieces){
            item.transform.parent = transform;
        }
        Destroy(frameObject);
    }

    public void rotate(Vector3 axe, int frame){
        if (!rotating){
            listOfPieces = getFramePieces(axe, frame);

            frameObject = new GameObject("Temp Object");
            frameObject.transform.parent = transform;
            foreach (var item in listOfPieces)
            {
                item.transform.parent = frameObject.transform;
            }
            rotating = true;
            angleAlreadyRotated = 0;
            rotateAxe = axe;
        }        
    }

    List<GameObject> getFramePieces(Vector3 axe, int frame){
        float size = GetComponent<CubeBuilder>().size;
        float majorSize = (size*3/2)-(size*0.1f);
        float minorSize = (size/2)-(size*0.1f);

        Vector3 overlapCenter,overlapSize,temp;

        overlapSize = new Vector3(Mathf.Abs(axe.x),Mathf.Abs(axe.y),Mathf.Abs(axe.z));
        temp = (overlapSize - Vector3.one)*-1;
        overlapSize = (overlapSize*minorSize) + (temp * majorSize);
        overlapCenter = axe*size;

        Collider[] hitColliders = Physics.OverlapBox(overlapCenter, overlapSize);
        List<GameObject> retval = new List<GameObject>();
        foreach (var item in hitColliders)
        {
            if (item.gameObject.CompareTag("CubePiece")){
                retval.Add(item.gameObject);
            }
        }
        return retval;
    }
}
