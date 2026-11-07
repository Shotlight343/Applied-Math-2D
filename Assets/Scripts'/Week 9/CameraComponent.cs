using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    [SerializeField] private float focalLength = 10f;
    [SerializeField] private Vector2 vanishingPointValue = Vector2.zero;

    public static float focalLenth = 10f;
    public static Vector2 vanishingPoint = Vector2.zero;

    private void Awake()
    {
        Apply();
    }

    private void OnValidate()
    {
        Apply();
    }

    private void Apply()
    {
        focalLenth = focalLength;
        vanishingPoint = vanishingPointValue;
    }
}