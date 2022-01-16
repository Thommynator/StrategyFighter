using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
