using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class OrderInLayerTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TilemapRenderer tilemapRenderer;
    [SerializeField] private int offset = 0;
    private void Start()
    {
        UpdateSortOrder();
    }
    [ContextMenu("UpdateSortOrder")]
    public void UpdateSortOrder()
    {
        if (!tilemap || !tilemapRenderer)
        {
            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
        }
        tilemap.CompressBounds();

        BoundsInt bounds = tilemap.cellBounds;


        tilemapRenderer.sortingOrder = -(int)(bounds.min.y* 100) + offset;
    }
}
