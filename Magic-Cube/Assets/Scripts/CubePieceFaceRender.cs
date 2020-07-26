using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubePieceFaceRender : MonoBehaviour
{
    public Color borderColor, fillColor;
    public float size = 10;
    public int borderSize = 10;
    public Vector3 center = Vector3.zero;

    private Color[] colors;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    struct square{
        public Vector3[] Vertices;
        public int[] Triangles;
        public Color[] Colors;
    }  

    void Start()
    {
        UpdateMesh();
    }

    void Update()
    {
        UpdateMesh();
    }

    public void UpdateMesh() { 
        createMesh();

        if (size < 1)
        {
            size = 1;
        }
        
        vertices = new Vector3[0];
        colors = new Color[0];
        triangles = new int[0];
        mesh.Clear();

        square temp = buildFace();

        mesh.vertices = temp.Vertices;
        mesh.colors = temp.Colors;
        mesh.triangles = temp.Triangles;


        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    private void createMesh(){
        if (mesh == null){
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "CubePiece Mesh";
            mesh.hideFlags = HideFlags.HideAndDontSave;
        }  
    }

    private square buildFace(){
        float borderLen = size * (float) borderSize/200;

        Vector3 leftVertice = center + new Vector3(-size/2, -size/2,0);
        Vector3 rightVertice = center + new Vector3(size/2, size/2,0);
        Vector3 centerSqrVertice = rightVertice + new Vector3(-borderLen, -borderLen, 0);

        square borderLeft = buildRectangle(leftVertice, borderLen, size, borderColor);
        square borderRight = buildRectangle(rightVertice, -borderLen, -size, borderColor);
        square borderTop = buildRectangle(rightVertice, -size, -borderLen, borderColor);
        square borderBot = buildRectangle(leftVertice, size, borderLen, borderColor);

        float centerSize = -(size-(borderLen*2));
        square centerSqr = buildRectangle(centerSqrVertice, centerSize, centerSize, fillColor);

        square retval = concatenate(borderLeft,borderRight,borderTop,borderBot,centerSqr);

        return retval;        
    }

    private square buildRectangle(Vector3 originPoint, float xLen, float yLen, Color fillColor){
        square retval = new square();
        Vector3[] tempvertices = new Vector3[4];
        
        tempvertices[0] = originPoint;
        tempvertices[1] = originPoint + new Vector3(xLen,0,0);
        tempvertices[2] = originPoint + new Vector3(0,yLen,0);
        tempvertices[3] = originPoint + new Vector3(xLen,yLen,0);
        
        retval.Colors = new Color[]{fillColor,fillColor,fillColor,fillColor};
        retval.Triangles = new int[]{0,1,3,0,2,3};
        retval.Vertices = tempvertices;
        return retval;
    }

    private square concatenate(params square[] sqrs){
        square retval = new square();

        int vLen = 0, tLen = 0, cLen = 0;
        foreach (var item in sqrs){
            vLen += item.Vertices.Length;
            tLen += item.Triangles.Length;
            cLen += item.Colors.Length;
        } 

        retval.Colors = new Color[cLen];
        retval.Triangles = new int[tLen];
        retval.Vertices = new Vector3[vLen];

        int vIndex = 0, tIndex = 0, cIndex = 0;
        foreach (var item in sqrs){
            item.Vertices.CopyTo(retval.Vertices, vIndex);
            item.Colors.CopyTo(retval.Colors, cIndex);

            for (int i = 0; i < item.Triangles.Length; i++){
                item.Triangles[i] = item.Triangles[i] + vIndex;
            }

            item.Triangles.CopyTo(retval.Triangles, tIndex);
            vIndex += item.Vertices.Length;
            tIndex += item.Triangles.Length;
            cIndex += item.Colors.Length;
        } 
        return retval;
    }
}
