using UnityEngine;

// player states
public enum MovementState
{
    WALKING,
    CROUCHING,
    AIR
}

public class MovementScript : MonoBehaviour
{
    [Header("Basic Movement")]
    private float moveSpeed = 7f;
    public float walkSpeed;

    public float groundDrag;
    public float airDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Dashing")]
    public float dashCoolDown;
    public float dashForce;
    public int totalDash = 2;
    private bool readyToDash = true;
    private bool allowSpeedOverflow = false;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    private bool remainCrouched;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    private bool grounded;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeRaycastHit;
    private bool exitingSlope;

    public Transform playerOrientation;
    public MovementState movementState;
    public Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    void Update()
    {
        GetInput();
        SpeedControl();
        StateHandler();
        rb.drag = grounded ? groundDrag : airDrag;
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f * (movementState == MovementState.CROUCHING ? 0.5f : 1f) + 0.3f,
            groundLayer
        );

        if (grounded)
        {
            totalDash = 2;
        }

        MovePlayer();
    }

    // Gets all inputs
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKey(dashKey) && readyToDash && totalDash > 0)
        {
            readyToDash = false;
            totalDash--;
            allowSpeedOverflow = true;
            Dash();
            Invoke(nameof(ResetDash), dashCoolDown);
        }

        if (Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(
                transform.localScale.x,
                crouchYScale,
                transform.localScale.z
            );
            if (remainCrouched)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                remainCrouched = false;
            }
        }

        if (!Input.GetKey(crouchKey))
        {
            remainCrouched = true;
            transform.localScale = new Vector3(
                transform.localScale.x,
                startYScale,
                transform.localScale.z
            );
        }
    }

    // Manages player states
    private void StateHandler()
    {
        // set crouching
        if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.CROUCHING;
            moveSpeed = crouchSpeed;
        }
        // set walking
        else if (grounded)
        {
            movementState = MovementState.WALKING;
            moveSpeed = walkSpeed;
        }
        // set in air
        else
        {
            movementState = MovementState.AIR;
        }
    }

    // moves player
    private void MovePlayer()
    {
        moveDirection =
            playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

        if (OnSlope())
        {
            rb.AddForce(10f * moveSpeed * GetSlopeMoveDirection().normalized, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            }
        }
        else if (grounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(
                10f * airMultiplier * moveSpeed * moveDirection.normalized,
                ForceMode.Force
            );
        }

        rb.useGravity = !OnSlope();
    }

    //  controls max speed
    private void SpeedControl()
    {
        if (!allowSpeedOverflow)
        {
            Vector3 horizontalVel = new(rb.velocity.x, 0, rb.velocity.z);

            if (horizontalVel.magnitude > moveSpeed)
            {
                Vector3 horizontalVelDirection = horizontalVel.normalized * moveSpeed;
                rb.velocity = new Vector3(
                    horizontalVelDirection.x,
                    rb.velocity.y,
                    horizontalVelDirection.z
                );
            }
        }
    }

    // detects if standing on a float
    private bool OnSlope()
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeRaycastHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, slopeRaycastHit.normal);
            return angle < maxSlopeAngle && angle > 0;
        }

        return false;
    }

    // returns slope direction on which player is standing
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeRaycastHit.normal);
    }

    // adds force to player.
    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // resets jump after sometime.
    private void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    // adds force for player to dash
    private void Dash()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.AddForce(moveDirection.normalized * dashForce, ForceMode.Impulse);
    }

    // resets player's dash
    private void ResetDash()
    {
        allowSpeedOverflow = false;
        readyToDash = true;
    }
}
