using System.Collections.Generic;
using UnityEngine;

public class ReadCube4x4 : MonoBehaviour
{
    public Transform tUp, tDown, tFront, tLeft, tRight, tBack;
    private List<GameObject> frontRays, backRays, leftRays, rightRays, downRays, upRays;
    
    private int layerMask = 1 << 8; 
    [SerializeField] private CubeState4x4 cubeState4x4; 
    public GameObject emptyGO;
    void Start() 
    {
        SetRayTransforms();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ReadState() 
    {
        cubeState4x4 = GetComponent<CubeState4x4>();
        
        //leer caras exteriores
        cubeState4x4.up = ReadFace(upRays, tUp);
        cubeState4x4.down = ReadFace(downRays, tDown);
        cubeState4x4.front = ReadFace(frontRays, tFront);
        cubeState4x4.back = ReadFace(backRays, tBack);
        cubeState4x4.left = ReadFace(leftRays, tLeft);
        cubeState4x4.right = ReadFace(rightRays, tRight);
        
        //capas internas
        cubeState4x4.up1 = FindIntFace(0.5f, "y");
        cubeState4x4.up2 = FindIntFace(-0.5f, "y");
        cubeState4x4.left1 = FindIntFace(0.5f, "z");
        cubeState4x4.left2 = FindIntFace(-0.5f, "z");
        cubeState4x4.front1 = FindIntFace(-0.5f, "x");
        cubeState4x4.front2 = FindIntFace(0.5f, "x");
        
    }

    List<GameObject> FindIntFace(float coordinate, string axis)
    {
        List<GameObject> face = new List<GameObject>();
        
        //busca en todas las piezas
        foreach (Transform piece in transform.Find("Pieces"))
        {
            bool match = false;
            if (axis == "y") match = Mathf.Abs(piece.localPosition.y - coordinate) < 0.1f; 
            if (axis == "x") match = Mathf.Abs(piece.localPosition.x - coordinate) < 0.1f; 
            if (axis == "z") match = Mathf.Abs(piece.localPosition.z - coordinate) < 0.1f;

            if (match)
            {
                foreach (Transform child in piece)
                {
                    if (child.gameObject.layer == 8)
                        face.Add(child.gameObject);
                }   
            }
        }
        return face;
    }
    
    private void SetRayTransforms() 
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }
    
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction) 
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        for (float y = 1.5f; y >= -1.5f; y -= 1f) {
            for (float x = -1.5f; x <= 1.5f; x += 1f) {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                    rayTransform.localPosition.y + y,
                    rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform) 
    {
        List<GameObject> facesHit = new List<GameObject>();
        foreach (GameObject rayStart in rayStarts) 
        {
            RaycastHit hit;
            if (Physics.Raycast(rayStart.transform.position, rayTransform.forward, out hit, Mathf.Infinity, layerMask)) 
            {
                facesHit.Add(hit.collider.gameObject);
            }
        }
        return facesHit;
    }
}
