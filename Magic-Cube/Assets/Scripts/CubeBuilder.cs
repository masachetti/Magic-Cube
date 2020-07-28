using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeBuilder : MonoBehaviour
{
    public float size = 10;
    public int borderSize = 10;

    public Color forwardColor = Color.magenta, backColor = Color.red, rightColor = Color.blue, leftColor = Color.green, upColor = Color.white, downColor = Color.yellow;


    private bool alreadyCreated;

    private GameObject[] pieces = new GameObject[27];

    private GameObject template;

    private const int x = 0, y=1, z=2;

    

    void Start()
    {
        template = Resources.Load("Prefabs/CubePiece") as GameObject;
        createPieces();
        setUpPieces();
        setUpFacesColors();
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

    void setUpFacesColors(){
        setUpPiecesColor(x, 0, rightColor);
        setUpPiecesColor(x, 2, leftColor);
        setUpPiecesColor(y, 0, upColor);
        setUpPiecesColor(y, 2, downColor);
        setUpPiecesColor(z, 0, forwardColor);
        setUpPiecesColor(z, 2, backColor);
    }

    void setUpPiecesColor(int axe, int frame, Color faceColor){
        List<GameObject> framePieces = getFramePieces(axe, frame);
        bool reverse = (frame == 2);
        foreach (var item in framePieces){
            CubePieceBuilder pieceScpt = item.GetComponent<CubePieceBuilder>();
            if (axe == x){
                pieceScpt.xColor = faceColor;
                pieceScpt.reverseX = reverse;
            }
            else if (axe == y){
                pieceScpt.yColor = faceColor;
                pieceScpt.reverseY = reverse;
            }
            else if (axe == z){
                pieceScpt.zColor = faceColor;
                pieceScpt.reverseZ = reverse;
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
