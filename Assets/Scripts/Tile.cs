using UnityEngine;
using MoreMountains.Feedbacks;
using System.Linq;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public LayerMask obstacleLayer;
    public Color highlightColor;
    private bool isReachable;
    private bool isCreateable;
    public MMFeedbacks wiggleTileFeedbacks;

    void Awake()
    {
        wiggleTileFeedbacks = GetComponent<MMFeedbacks>();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void OnMouseDown()
    {
        if (isReachable && GameMaster.current.selectedUnit != null)
        {
            GameMaster.current.selectedUnit.MoveTo(transform.position);
        }
        else if (isCreateable)
        {
            Unit item = Instantiate(GameMaster.current.GetAndPayForShopSelectedItem(), new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            GameMaster.current.ResetTiles();
            if (item.TryGetComponent<Unit>(out Unit unit))
            {
                unit.PlaceUnit();
                WiggleNeighborTiles(2.1f);
            }
        }
    }

    private void WiggleNeighborTiles(float distanceRange)
    {
        IEnumerable<Tile> neighborTiles = FindObjectsOfType<Tile>().Where(tile => tile.transform.position.ManhattenDistanceTo(this.transform.position) <= distanceRange);
        foreach (Tile tile in neighborTiles)
        {
            tile.wiggleTileFeedbacks.PlayFeedbacks();
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

    public void HighlightReachable()
    {
        spriteRenderer.color = highlightColor;
        isReachable = true;
    }

    public void HighlightCreateable()
    {
        spriteRenderer.color = highlightColor;
        isCreateable = true;
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white;
        isReachable = false;
        isCreateable = false;
    }

}
