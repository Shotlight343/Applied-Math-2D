using System;
using UnityEngine;

public class AABBCollider : MonoBehaviour
{
    public event Action<AABBCollider> Collided;

    [SerializeField] private float _length;
    [SerializeField] private float _width;
    [SerializeField] private bool _isSolid = true;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    public float Width => _width;
    public float Length => _length;
    public bool IsSolid => _isSolid;

    private void Start()
    {
        UpdateBounds();

        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Unregister(this);
        }
    }

    private void Update()
    {
        UpdateBounds();

        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.blueViolet);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.blueViolet);
    }

    private void UpdateBounds()
    {
        var pos = transform.position;

        max = new Vector2(pos.x + _width * .5f, pos.y + _length * .5f);
        min = new Vector2(pos.x - _width * .5f, pos.y - _length * .5f);
    }

    public void OnCollision(AABBCollider other)
    {
        Debug.Log($"Hit {other.name}");
        Collided?.Invoke(other);
    }
}