using UnityEngine;

[RequireComponent(typeof(AABBCollider))]
public class Spike : MonoBehaviour
{
    private AABBCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<AABBCollider>();
    }

    private void OnEnable()
    {
        _collider.Collided += OnCollided;
    }

    private void OnDisable()
    {
        _collider.Collided -= OnCollided;
    }

    private void OnCollided(AABBCollider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.Respawn();
        }
    }
}
