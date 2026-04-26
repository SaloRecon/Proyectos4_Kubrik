using UnityEngine;

public class SC_EstheticRotation : MonoBehaviour
{
    [Header("Rotación libre")]
    public Vector3 rotationSpeed = new Vector3(5f, 8f, 3f); // MÁS lento

    [Header("Movimiento flotante")]
    public float movementAmplitude = 0.3f;  // menos desplazamiento
    public float movementSpeed = 0.15f;     // MUCHO más lento

    private Vector3 startPos;
    private float offsetX;
    private float offsetY;
    private float offsetZ;

    void Start()
    {
        startPos = transform.position;

        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);
        offsetZ = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Rotación lenta y continua
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Movimiento suave tipo deriva espacial
        float x = Mathf.PerlinNoise(Time.time * movementSpeed + offsetX, 0f) - 0.5f;
        float y = Mathf.PerlinNoise(0f, Time.time * movementSpeed + offsetY) - 0.5f;
        float z = Mathf.PerlinNoise(Time.time * movementSpeed + offsetZ, Time.time * movementSpeed) - 0.5f;

        Vector3 floatOffset = new Vector3(x, y, z) * movementAmplitude;

        transform.position = startPos + floatOffset;
    }
}
