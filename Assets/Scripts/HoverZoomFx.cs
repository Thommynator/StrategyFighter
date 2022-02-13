using UnityEngine;

public class HoverZoomFx : MonoBehaviour
{
    public float hoverZoomEffectStrength;

    private void OnMouseEnter()
    {
        if (!GameMaster.current.IsCurrentPlayerByTag(tag))
        {
            return;
        }
        transform.localScale += Vector3.one * hoverZoomEffectStrength;
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
    }
}
