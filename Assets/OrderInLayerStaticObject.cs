using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OrderInLayerStaticObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset = 0;
    private void Start()
    {
        UpdateSortOrder();
    }
    [ContextMenu("UpdateSortOrder")]
    public void UpdateSortOrder()
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }
}
