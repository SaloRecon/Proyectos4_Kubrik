using UnityEngine;

public class SC_AstronautaRotation : MonoBehaviour
{
    [Header("Flotación")]
    public float floatAmplitude = 0.1f;
    public float floatSpeed = 1f;

    [Header("Rotación suave")]
    public Vector3 rotationAmount = new Vector3(5f, 5f, 5f);
    public float rotationSpeed = 1f;

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        initialLocalRot = transform.localRotation;
    }

    void Update()
    {
        // 🌊 Flotación LOCAL (no rompe el movimiento global)
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = initialLocalPos + new Vector3(0f, offsetY, 0f);

        // 🔄 Rotación suave tipo gravedad cero
        Quaternion rot = Quaternion.Euler(
            Mathf.Sin(Time.time * rotationSpeed) * rotationAmount.x,
            Mathf.Sin(Time.time * rotationSpeed * 0.8f) * rotationAmount.y,
            Mathf.Sin(Time.time * rotationSpeed * 1.2f) * rotationAmount.z
        );

        transform.localRotation = initialLocalRot * rot;
    }
}
