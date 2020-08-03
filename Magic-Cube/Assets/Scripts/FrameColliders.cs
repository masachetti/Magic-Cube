using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameColliders : MonoBehaviour
{
    void Start()
    {
        createColliders();
    }

    private void createColliders(){
        List<Vector3> faces = new List<Vector3>();
        faces.Add(Vector3.forward);
        faces.Add(Vector3.up);
        faces.Add(Vector3.right);
        foreach (var item in faces)
        {
            foreach (var item2 in faces)
            {
                if (item != item2){
                    createFaceFrameColliders(item, item2);
                    createFaceFrameColliders(item*-1, item2);
                }
            }
        }
    }

    private void createFaceFrameColliders(Vector3 face, Vector3 frame){
        float size = GetComponent<CubeBuilder>().size;

        for (int i = -1; i < 2; i++)
        {
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            Vector3 inverseFace = new Vector3((face.x + 1)%2,(face.y + 1)%2,(face.z + 1)%2);
            box.size = (inverseFace + (inverseFace - frame)*2)*size;
            box.center = (frame * i*size) + face*size*3/2;

        }
    }
}
