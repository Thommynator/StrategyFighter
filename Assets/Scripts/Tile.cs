using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public LayerMask obstacleLayer;
    public Color highlightColor;

    private bool isReachable;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        if (isReachable && GameMaster.current.selectedUnit != null)
        {
            GameMaster.current.selectedUnit.MoveTo(transform.position);
        }
    }

    public bool IsClear()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (collider != null)
        {
            return false;
        }
        return true;
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
        isReachable = true;
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white;
        isReachable = false;
    }

}
