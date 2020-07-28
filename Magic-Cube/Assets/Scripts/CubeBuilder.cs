using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeBuilder : MonoBehaviour
{
    public float size = 10;
    public int borderSize = 10;


    private bool alreadyCreated;

    private GameObject[] pieces = new GameObject[27];

    private GameObject template;

    private const int x = 0, y=1, z=2;

    

    void Start()
    {
        template = Resources.Load("Prefabs/CubePiece") as GameObject;
        createPieces();
        setUpPieces();
        foreach (var item in getFramePieces(x,2))
        {
            print(item.transform.position);
        }
    }

    void Update()
    {
        setUpPieces();
    }

    void createPieces(){
        if (!checkAlreadyCreated()){
            for (int i=0; i<27; i++){
                pieces[i] = createPiece();
            }
        }
        else{
            for (int i = 0; i < transform.childCount; i++)
            {
                pieces[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    bool checkAlreadyCreated(){
        if (transform.childCount != 27){
            int c = transform.childCount;
            for (int i = 0; i < c; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            return false;
        }
        return true;
    }

    private GameObject createPiece(){
        GameObject retval = (GameObject)GameObject.Instantiate(template, new Vector3(0, 0, 0), Quaternion.identity);
        retval.transform.parent = transform;
        
        return retval;
    }

    void setUpPieces(){
        int m = 0;
        for (int i=-1; i<2; i++){
            for (int j=-1; j<2; j++){
                for (int k = -1; k<2; k++)
                {
                    pieces[m].transform.position = new Vector3(i*size, j*size, k*size);
                    CubePieceBuilder pieceScript = pieces[m].GetComponent<CubePieceBuilder>();
                    pieceScript.size = size;
                    pieceScript.borderSize = borderSize;
                    //pieceScript.moveSpeed = speed;
                    //pieceScript.rotateSpeed = speed;
                    m++;
                }
            }
        }       
    }

    List<GameObject> getFramePieces(int axe, int frame){
        float majorSize = (size*3/2)-(size*0.1f);
        float minorSize = (size/2)-(size*0.1f);

        Vector3 overlapCenter,overlapSize;
        if (axe == x){
            overlapCenter = Vector3.right;
            overlapSize = new Vector3(minorSize, majorSize, majorSize);

        }
        else if (axe == y){
            overlapCenter = Vector3.up;
            overlapSize = new Vector3(majorSize, minorSize, majorSize);
        }
        else{
            overlapCenter = Vector3.forward;
            overlapSize = new Vector3(majorSize, majorSize, minorSize);
        }
        overlapCenter *= (1-frame)*size;

        Collider[] hitColliders = Physics.OverlapBox(overlapCenter, overlapSize);
        List<GameObject> retval = new List<GameObject>();
        foreach (var item in hitColliders)
        {
            retval.Add(item.gameObject);
        }
        return retval;
    }
}
