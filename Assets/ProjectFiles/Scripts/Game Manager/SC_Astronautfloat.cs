using UnityEngine;
using System.Collections;

public class SC_Astronautfloat : MonoBehaviour
{
 public float speed = 1.5f;
    public float respawnDelay = 3f;

    private Vector3 direction;
    private Camera cam;
    private bool isRespawning = false;
    private float fixedZ;

    void Start()
    {
        cam = Camera.main;
        fixedZ = transform.position.z;
        Respawn();
    }

    void Update()
    {
        if (isRespawning) return;

        // Movimiento 2D
        transform.position += new Vector3(direction.x, direction.y, 0f) * speed * Time.deltaTime;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        if (viewPos.x < -0.1f || viewPos.x > 1.1f || viewPos.y < -0.1f || viewPos.y > 1.1f)
        {
            StartCoroutine(RespawnDelay());
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);
    }

    IEnumerator RespawnDelay()
    {
        isRespawning = true;

        // 🔴 SOLO ocultar visualmente (NO desactivar objeto)
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
            r.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        Respawn();

        foreach (var r in renderers)
            r.enabled = true;

        isRespawning = false;
    }

    void Respawn()
    {
        int side = Random.Range(0, 4);
        Vector3 viewPos = Vector3.zero;

        float depth = Mathf.Abs(fixedZ - cam.transform.position.z);

        switch (side)
        {
            case 0:
                viewPos = new Vector3(0f, Random.value, depth);
                direction = Vector3.right;
                break;

            case 1:
                viewPos = new Vector3(1f, Random.value, depth);
                direction = Vector3.left;
                break;

            case 2:
                viewPos = new Vector3(Random.value, 0f, depth);
                direction = Vector3.up;
                break;

            case 3:
                viewPos = new Vector3(Random.value, 1f, depth);
                direction = Vector3.down;
                break;
        }

        direction += new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.3f, 0.3f),
            0f
        );

        direction.Normalize();

        Vector3 worldPos = cam.ViewportToWorldPoint(viewPos);
        worldPos.z = fixedZ;

        transform.position = worldPos;
    }
}
