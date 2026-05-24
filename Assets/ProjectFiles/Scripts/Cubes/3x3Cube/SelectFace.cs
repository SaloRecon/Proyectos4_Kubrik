using System.Collections.Generic;
using System.Xml;
using ProjectFiles.Scripts.Cubes._3x3Cube;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    [SerializeField] private CubeState cubeState;
    [SerializeField] private ReadCube readCube;
    int layerMask = 1 << 8;
    
    private bool dragWait = false;
    private Vector3 mouseDownPos;
    private GameObject hitFace;
    private List<GameObject> outerFaceClicked;
    private Vector3 hitLocalPos;
    private string faceAxis;
    private float dragThreshold = 5f;
    
    void Start()
    {
        cubeState = GetComponent<CubeState>();
        readCube = GetComponent<ReadCube>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating 
                                        && CubeState.started && AutoShuffle.started && !AutoShuffle.shuffling)
        {
            readCube.ReadState();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                outerFaceClicked = GetOuterFace(face);

                if (outerFaceClicked != null)
                {
                    hitLocalPos = transform.InverseTransformPoint(hit.point);
                    faceAxis = GetFaceAxis(outerFaceClicked);
                    mouseDownPos = Input.mousePosition;
                    dragWait = true;
                }
            }
        }
        //si cruzo mi umbral de drag, detecto si es horizonatal o vertical y en base a eso elijo la cara a rotar
        if (dragWait && Input.GetMouseButton(0))
        {
            Vector3 dragDelta = Input.mousePosition - mouseDownPos;
            if (dragDelta.magnitude >= dragThreshold)
            {
                dragWait = false;
                bool dragHorizontal = Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y);
                List<GameObject> sliceToRotate = PickSlice(outerFaceClicked, faceAxis, hitLocalPos, dragHorizontal);

                if (sliceToRotate != null)
                {
                    cubeState.PickUp(sliceToRotate);
                    PivotRotation pr = sliceToRotate[4].transform.parent.GetComponent<PivotRotation>();
                    if (pr != null) pr.Rotate(sliceToRotate);
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) dragWait = false;
    }
    
    private List<GameObject> GetOuterFace(GameObject face)
    {
        List<List<GameObject>> outerFaces = new List<List<GameObject>>()
        {
            cubeState.up, cubeState.down,
            cubeState.left, cubeState.right,
            cubeState.front, cubeState.back
        };
        foreach (var side in outerFaces)
            if (side.Contains(face)) return side;
        return null;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private PivotRotation GetPivotRotation(List<GameObject> side)
    {
        if (side == cubeState.front) return FindPivotByAxis(Vector3.right,   -1f); // F(-1,0,0)
        if (side == cubeState.back)  return FindPivotByAxis(Vector3.right,    1f); // B(1,0,0)
        if (side == cubeState.right) return FindPivotByAxis(Vector3.forward, -1f); // R(0,0,-1)
        if (side == cubeState.left)  return FindPivotByAxis(Vector3.forward,  1f); // L(0,0,1)
        if (side == cubeState.up)    return FindPivotByAxis(Vector3.up,       1f); // U(0,1,0)
        if (side == cubeState.down)  return FindPivotByAxis(Vector3.up,      -1f); // D(0,-1,0)

        return null;
    }

    private PivotRotation FindPivotByAxis(Vector3 axis, float sign)
    {
        //busca en los hijos del GO el más cercano al pivote (*1 porque los pivotes están en +- 1)
        Vector3 targetPos = axis * sign;
        foreach (Transform child in transform)
        {
            if (Vector3.Distance(child.localPosition, targetPos) < 0.1f)
            {
                PivotRotation pr = child.GetComponent<PivotRotation>();
                if (pr != null) return pr;
            }
        }
        return null;
    }
    private string GetFaceAxis(List<GameObject> face)
    {
        //devuelvo en forma de string el eje perpendicular
        if (face == cubeState.up    || face == cubeState.down)  return "y";
        if (face == cubeState.left  || face == cubeState.right) return "z";
        if (face == cubeState.front || face == cubeState.back)  return "x";
        return "y";
    }
    
    private List<GameObject> PickSlice(List<GameObject> outerFace, string axis, Vector3 localHit, bool dragHorizontal)
    {
        if (axis == "y")
        {
            float coord = dragHorizontal ? localHit.z : localHit.x;
            string sliceAxis = dragHorizontal ? "z" : "x";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
        else if (axis == "z")
        {
            float coord = dragHorizontal ? localHit.x : localHit.y;
            string sliceAxis = dragHorizontal ? "x" : "y";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
        else
        {
            float coord = dragHorizontal ? localHit.z : localHit.y;
            string sliceAxis = dragHorizontal ? "z" : "y";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
    }
    
    private List<GameObject> GetSliceByCoord(string sliceAxis, float coord, List<GameObject> outerFace)
    {
        //las piezas del 3x3 están en -1, 0, 1
        float rounded = Mathf.Round(coord);

        if (sliceAxis == "y")
        {
            if (Mathf.Approximately(rounded,  1f)) return cubeState.up;
            if (Mathf.Approximately(rounded,  0f)) return cubeState.up; 
            if (Mathf.Approximately(rounded, -1f)) return cubeState.down;
        }
        else if (sliceAxis == "z")
        {
            if (Mathf.Approximately(rounded,  1f)) return cubeState.left;
            if (Mathf.Approximately(rounded,  0f)) return cubeState.right; 
            if (Mathf.Approximately(rounded, -1f)) return cubeState.right;
        }
        else if (sliceAxis == "x")
        {
            if (Mathf.Approximately(rounded, -1f)) return cubeState.front;
            if (Mathf.Approximately(rounded,  0f)) return cubeState.front; 
            if (Mathf.Approximately(rounded,  1f)) return cubeState.back;
        }

        return outerFace;
    }
}
