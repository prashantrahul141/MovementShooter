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
    public float walkSpeed;
    public float groundDrag;
    public float airDrag;
    private float moveSpeed = 7f;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;

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
    private Vector3 groundNormal = new Vector3(0, 1, 0).normalized;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float pushDownForce;
    private bool pushedDownAlready;
    private RaycastHit midAirGroundCast;
    private bool pushDownPlayer;
    private float startYScale;
    private bool remainCrouched;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public float groundCheckPadding;
    private bool grounded;

    [Header("Slope")]
    public float maxSlopeAngle;
    public float slopeCheckPadding;
    private RaycastHit slopeRaycastHit;
    private bool exitingSlope;
    private bool onSlopeLocal;

    [Header("References")]
    public Transform playerOrientation;
    public MovementState movementState;
    public Rigidbody rb;

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
        StopPassingDownTerrain();
        rb.drag = grounded ? groundDrag : airDrag;
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f * (movementState == MovementState.CROUCHING ? 0.5f : 1f)
                + groundCheckPadding,
            groundLayer
        );
        if (grounded)
        {
            totalDash = 2;
            pushedDownAlready = false;
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

        if (Input.GetKeyDown(dashKey) && readyToDash && totalDash > 0)
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
            if (movementState == MovementState.AIR)
            {
                pushDownPlayer = true;
            }
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

        if (pushDownPlayer && !pushedDownAlready)
        {
            pushDownPlayer = false;
            PushDown();
        }

        onSlopeLocal = OnSlope();
        if (onSlopeLocal)
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

        rb.useGravity = !onSlopeLocal;
    }

    //  controls max speed
    private void SpeedControl()
    {
        if (!allowSpeedOverflow)
        {
            // limiting on slope
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }
            else
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
    }

    // detects if standing on a float
    private bool OnSlope()
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeRaycastHit,
                playerHeight * 0.5f + slopeCheckPadding
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
        if (moveDirection.magnitude != 0)
        {
            //  dashing in the direction of keys pressed
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.AddForce(moveDirection.normalized * dashForce, ForceMode.Impulse);
        }
        else if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            //  dashing in the direction of player velocity if no key is pressed
            rb.AddForce(
                Vector3.ProjectOnPlane(rb.velocity.normalized, groundNormal) * dashForce,
                ForceMode.Impulse
            );
        }
        else
        {
            // dashing forward if no key is pressed and player does not have any horizontal velocity.
            rb.AddForce(playerOrientation.forward * dashForce, ForceMode.Impulse);
        }
    }

    // resets player's dash
    private void ResetDash()
    {
        allowSpeedOverflow = false;
        readyToDash = true;
    }

    private void PushDown()
    {
        Physics.Raycast(rb.position, Vector3.down, out midAirGroundCast, 30, groundLayer);
        pushedDownAlready = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.down * midAirGroundCast.distance * pushDownForce, ForceMode.Impulse);
    }

    //  stops player from moving through the terrain;
    private void StopPassingDownTerrain()
    {
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }
}
