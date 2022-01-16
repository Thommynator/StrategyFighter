using UnityEngine;

public class HoverZoomFx : MonoBehaviour
{
    public float hoverZoomEffectStrength;

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverZoomEffectStrength;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverZoomEffectStrength;
    }
}
