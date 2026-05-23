using UnityEngine;

public class MouseAestheticChange : MonoBehaviour
{
    public Texture2D cursorTexture;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Vector2 hotspot = new Vector2(0, 0);

        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
