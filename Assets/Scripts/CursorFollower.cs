using UnityEngine;
using UnityEngine.UI;

public class CursorFollower : MonoBehaviour
{
    [SerializeField] Sprite defaultCursorImage;

    void Start()
    {
        Cursor.visible = false;
        ResetCursorImage();
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
