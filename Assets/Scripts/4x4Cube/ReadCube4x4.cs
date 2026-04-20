using System.Collections.Generic;
using UnityEngine;

public class ReadCube4x4 : MonoBehaviour
{
    public Transform tUp, tDown, tFront, tLeft, tRight, tBack;
    private List<GameObject> frontRays, backRays, leftRays, rightRays, downRays, upRays;
    
    private int layerMask = 1 << 8; 
    [SerializeField] private CubeState4x4 cubeState4x4; // Asegúrate que el nombre coincida
    public GameObject emptyGO;
    
    void Start() 
    {
        SetRayTransforms();
    }

    public void ReadState() 
    {
        cubeState4x4 = GetComponent<CubeState4x4>();
        cubeState4x4.up = ReadFace(upRays, tUp);
        cubeState4x4.down = ReadFace(downRays, tDown);
        cubeState4x4.front = ReadFace(frontRays, tFront);
        cubeState4x4.back = ReadFace(backRays, tBack);
        cubeState4x4.left = ReadFace(leftRays, tLeft);
        cubeState4x4.right = ReadFace(rightRays, tRight);
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

        for (float y = 1f; y > -2f; y -= 2f) {
            for (float x = -1f; x < 2f; x += 2f) {
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
