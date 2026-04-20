using System.Collections.Generic;
using UnityEngine;

public class ReadCube2x2 : MonoBehaviour
{
    public Transform tUp, tDown, tFront, tLeft, tRight, tBack;
    private List<GameObject> frontRays, backRays, leftRays, rightRays, downRays, upRays;
    
    private int layerMask = 1 << 8; 
    [SerializeField] private CubeState2x2 cubeState; // Asegúrate que el nombre coincida
    public GameObject emptyGO;
    
    void Start() 
    {
        SetRayTransforms();
    }

    public void ReadState() 
    {
        cubeState = GetComponent<CubeState2x2>();
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
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

        //ajustado a cubo 2x2
        for (float y = 0.5f; y > -1f; y -= 1f) {
            for (float x = -0.5f; x < 1f; x += 1f) {
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