using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RunnerPlayer : MonoBehaviour
{
    public float laneWidth = 3f;
    public float groundY = -4f;
    public float laneSmoothTime = 0.12f;
    public float jumpVelocity = 10f;
    public float gravity = 28f;
    public float maxHealth = 100f;
    public float damagePerHit = 5f;
    public float regenPerSecond = 1f;

    public Vector3 playerPosition;
    public float health;

    private int lane = 1;
    private float velocityX;
    private float jumpSpeed;
    private float height;
    private bool isGrounded = true;

    private void Start()
    {
        health = maxHealth;
        playerPosition = new Vector3(LaneToX(lane), groundY, 0f);
        transform.position = playerPosition;
    }

    private void Update()
    {
        var dt = Time.deltaTime;

        if (LeftPressed())
        {
            lane = Mathf.Clamp(lane - 1, 0, 2);
        }
        if (RightPressed())
        {
            lane = Mathf.Clamp(lane + 1, 0, 2);
        }
        if (JumpPressed() && isGrounded)
        {
            jumpSpeed = jumpVelocity;
            isGrounded = false;
        }

        if (!isGrounded)
        {
            jumpSpeed = jumpSpeed - gravity * dt;
            height = height + jumpSpeed * dt;
            if (height <= 0f)
            {
                height = 0f;
                jumpSpeed = 0f;
                isGrounded = true;
            }
        }

        var x = Mathf.SmoothDamp(playerPosition.x, LaneToX(lane), ref velocityX, laneSmoothTime);
        playerPosition = new Vector3(x, groundY + height, 0f);
        transform.position = playerPosition;

        health = Mathf.Min(health + regenPerSecond * dt, maxHealth);
    }

    public float LaneToX(int laneIndex)
    {
        return (laneIndex - 1) * laneWidth;
    }

    public float Height
    {
        get { return height; }
    }

    public void TakeDamage()
    {
        health = Mathf.Max(health - damagePerHit, 0f);
    }

    private bool LeftPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && (k.leftArrowKey.wasPressedThisFrame || k.aKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
#endif
    }

    private bool RightPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && (k.rightArrowKey.wasPressedThisFrame || k.dKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
#endif
    }

    private bool JumpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        return k != null && (k.spaceKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame || k.wKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
#endif
    }
}
