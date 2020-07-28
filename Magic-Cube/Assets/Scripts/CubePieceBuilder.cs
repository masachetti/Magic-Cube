using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubePieceBuilder : MonoBehaviour
{
    public Color xColor, yColor, zColor;
    public Color borderColor;
    public float size = 10;
    public int borderSize = 10;
 

    void Start()
    {
        formatPiece();
    }

    void Update()
    {
        formatPiece();
    }

    public void formatPiece() { 

        if (size < 1)
        {
            size = 1;
        }
        
        GameObject back = transform.Find("Back").gameObject;
        GameObject up = transform.Find("Up").gameObject;
        GameObject forward = transform.Find("Forward").gameObject;
        GameObject down = transform.Find("Down").gameObject;
        GameObject right = transform.Find("Right").gameObject;
        GameObject left = transform.Find("Left").gameObject;

        back.transform.localPosition = Vector3.back * size/2;
        up.transform.localPosition = Vector3.up * size/2;
        forward.transform.localPosition = Vector3.forward * size/2;
        down.transform.localPosition = Vector3.down * size/2;
        right.transform.localPosition = Vector3.right * size/2;
        left.transform.localPosition = Vector3.left * size/2;

        up.transform.eulerAngles = Vector3.right * 90;
        down.transform.eulerAngles = Vector3.right * 90;
        right.transform.eulerAngles = Vector3.up * 90;
        left.transform.eulerAngles = Vector3.up * 90;

        CubePieceFaceRender backF = back.GetComponent<CubePieceFaceRender>();
        backF.size = size;
        backF.borderSize = borderSize;
        backF.borderColor = borderColor;
        backF.fillColor = zColor;
        CubePieceFaceRender upF = up.GetComponent<CubePieceFaceRender>();
        upF.size = size;
        upF.borderSize = borderSize;
        upF.borderColor = borderColor;
        upF.fillColor = yColor;
        CubePieceFaceRender forwardF = forward.GetComponent<CubePieceFaceRender>();
        forwardF.size = size;
        forwardF.borderSize = borderSize;
        forwardF.borderColor = borderColor;
        forwardF.fillColor = Color.black;
        CubePieceFaceRender downF = down.GetComponent<CubePieceFaceRender>();
        downF.size = size;
        downF.borderSize = borderSize;
        downF.borderColor = borderColor;
        downF.fillColor = Color.black;
        CubePieceFaceRender rightF = right.GetComponent<CubePieceFaceRender>();
        rightF.size = size;
        rightF.borderSize = borderSize;
        rightF.borderColor = borderColor;
        rightF.fillColor = Color.black;
        CubePieceFaceRender leftF = left.GetComponent<CubePieceFaceRender>();
        leftF.size = size;
        leftF.borderSize = borderSize;
        leftF.borderColor = borderColor;
        leftF.fillColor = xColor;

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = Vector3.one * size;

    }
}
