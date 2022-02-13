using UnityEngine;
using UnityEngine.UI;

public class CursorFollower : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] Sprite defaultCursorImage;

    void Start()
    {
        Cursor.visible = false;
        ResetCursorImage();
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ResetCursorImage()
    {
        GetComponent<Image>().sprite = defaultCursorImage;
    }

    public void SetCursorSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
}
