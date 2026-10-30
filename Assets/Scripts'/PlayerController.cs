using UnityEngine;

[RequireComponent(typeof(AABBCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpSpeed = 15f;
    [SerializeField] private float _gravity = 40f;

    private const float Skin = 0.001f;

    private AABBCollider _collider;
    private Vector2 _velocity;
    private bool _isGrounded;
    private Vector3 _spawnPosition;

    private void Awake()
    {
        _collider = GetComponent<AABBCollider>();
        _spawnPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = _spawnPosition;
        _velocity = Vector2.zero;
    }

    private void Update()
    {
        _velocity.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = _jumpSpeed;
        }

        _velocity.y -= _gravity * Time.deltaTime;

        MoveX(_velocity.x * Time.deltaTime);
        MoveY(_velocity.y * Time.deltaTime);
    }

    private void MoveX(float delta)
    {
        if (delta == 0f || CollisionManager.Instance == null) return;

        Vector3 pos = transform.position;
        pos.x += delta;

        var colliders = CollisionManager.Instance.Colliders;
        float halfWidth = _collider.Width * 0.5f;

        for (int i = 0; i < colliders.Count; i++)
        {
            var other = colliders[i];
            if (!Blocks(other) || !Overlaps(pos, other)) continue;

            pos.x = delta > 0f
                ? other.min.x - halfWidth - Skin
                : other.max.x + halfWidth + Skin;
        }

        transform.position = pos;
    }

    private void MoveY(float delta)
    {
        _isGrounded = false;
        if (CollisionManager.Instance == null) return;

        Vector3 pos = transform.position;
        pos.y += delta;

        var colliders = CollisionManager.Instance.Colliders;
        float halfLength = _collider.Length * 0.5f;

        for (int i = 0; i < colliders.Count; i++)
        {
            var other = colliders[i];
            if (!Blocks(other) || !Overlaps(pos, other)) continue;

            if (delta < 0f)
            {
                pos.y = other.max.y + halfLength + Skin;
                _isGrounded = true;
            }
            else
            {
                pos.y = other.min.y - halfLength - Skin;
            }
            _velocity.y = 0f;
        }

        transform.position = pos;
    }

    private bool Blocks(AABBCollider other)
    {
        return other != _collider && other.IsSolid;
    }

    private bool Overlaps(Vector3 pos, AABBCollider other)
    {
        float halfWidth = _collider.Width * 0.5f;
        float halfLength = _collider.Length * 0.5f;

        return pos.x - halfWidth < other.max.x && pos.x + halfWidth > other.min.x &&
               pos.y - halfLength < other.max.y && pos.y + halfLength > other.min.y;
    }
}