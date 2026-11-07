using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    public bool hasHit;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void Update()
    {
        var depth = Mathf.Max(CameraComponent.focalLenth + itemPosition.z, 0.01f);
        var perspective = CameraComponent.focalLenth / depth;
        transform.localScale = Vector3.one * perspective;

        var projected = new Vector3(itemPosition.x, itemPosition.y, 0f) * perspective;
        var vanishing = new Vector3(CameraComponent.vanishingPoint.x, CameraComponent.vanishingPoint.y, 0f);
        transform.position = projected + vanishing;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-itemPosition.z * 100f);
        }
    }

    private Color GetRandomColor()
    {
        var rRand = Random.Range(0f, 1f);
        var gRand = Random.Range(0f, 1f);
        var bRand = Random.Range(0f, 1f);
        return new Color(rRand, gRand, bRand);
    }
}