using System;
using UnityEngine;

public class PlayerController : LevelSingleton<PlayerController>
{
    [Serializable]
    public enum JumpState
    {
        None,
        WallSlide,
        QuickTurn,
        Upward,
        Top,
        Fall
    }

    [HideInInspector]
    public JumpState jumpState;
    [HideInInspector]
    public int jumpStateDir;

    #region Variables
    //Cached components
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private GameControls gameControls;
    [SerializeField] private CharacterAnimator animator;

    ContactFilter2D cf2d = new ContactFilter2D();
    Collider2D[] collider2dArray = new Collider2D[1];

    [Header("Refs")]
    [SerializeField] private Collider2D vertAirColliderL;
    [SerializeField] private Collider2D vertHitColliderL;
    [Space(5)]
    [SerializeField] private Collider2D vertAirColliderR;
    [SerializeField] private Collider2D vertHitColliderR;
    [Space(15)]
    [SerializeField] private Collider2D horzAirColliderL;
    [SerializeField] private Collider2D horzHitColliderL;
    [Space(5)]
    [SerializeField] private Collider2D horzAirColliderR;
    [SerializeField] private Collider2D horzHitColliderR;

    [Space(15)]

    [SerializeField] LayerMask wallIgnoreLayer;

    [Space(15)]

    #region Constants
    [Header("Horizontal")]

    [Range(0,1)]
    [SerializeField] private float deadZone = 0.2f;
    [Range(0, 0.001f)]
    [SerializeField] private float velocityZeroThreshold;

    [Space(10)]

    [Range(10f, 50f)]
    [SerializeField] private float accelerateSpeed; //In tiles/second^2
    [Range(10f, 50f)]
    [SerializeField] private float airAccelerateSpeed; //In tiles/second^2
    [Range(10f, 100f)]
    [SerializeField] private float decelerateSpeed; //In tiles/second^2
    [Range(1f, 100f)]
    [SerializeField] private float airDecelerateSpeed; //In tiles/second^2
    [Range(0, 1)]
    [SerializeField] private float accelerationDamping;
    [Range(0, 1)]
    [SerializeField] private float decelerationDamping;
    [Range(0, 1)]
    [SerializeField] private float airAccelerationDamping;
    [Range(0, 1)]
    [SerializeField] private float airDecelerationDamping;
    [Range(1, 30)]
    [SerializeField] private float maxSpeed; //In tiles/second
    [Range(1, 10)]
    [SerializeField] private float airMaxSpeed; //In tiles/second
    [HideInInspector] public float maxSpeedValue;
    [Range(0, 100)]
    [SerializeField] private float groundDeceleratePastMax; //In tiles/second^2
    [Range(0, 100)]
    [SerializeField] private float airDeceleratePastMax; //In tiles/second^2
    [Range(0, 100)]
    [SerializeField] private float dirChangeEffeciency; //In percent
    [Range(0, 100)]
    [SerializeField] private float airDirChangeEffeciency; //In percent

    [Space(15)]

    [Header("Jump")]

    [Range(5,20)]
    [SerializeField] private float jumpVelocity; //In tiles/second
    [Range(10, 20)]
    [SerializeField] public float maxJumpVelocity; //In tiles/second
    [Range(1, 40)]
    [SerializeField] private float maxAirFallSpeed; //In Tiles/second

    [Space(10)]

    [Header("Gravity Multipliers")]

    [Range(0, 10)]
    [SerializeField] private float floatVelocityRange; //In tiles/second
    [Range(0, 1)]
    [SerializeField] private float topHeightGravityMultiplier;
    [Range(1,10)]
    [SerializeField] private float gravityFallMultiplier;
    [Range(1, 10)]
    [SerializeField] private float lowJumpGravityMultiplier;
    [Range(0, 1)]
    [SerializeField] private float wallSlideGravityMultiplier;
    [Range(0, 10)]
    [SerializeField] private float wallJumpZeroGravThreshold; //In tiles/second

    [Space(15)]

    [Header("Wall Jump")]

    [Range(5, 20)]
    [SerializeField] private float wallJumpVelocity; //In tiles/second
    [Range(0, 30)]
    [SerializeField] private float wallJumpVerticalVelocity; //In tiles/second
    [Range(1, 20)]
    [SerializeField] private float maxWallFallSpeed; //In Tiles/second

    [Space(15)]

    [Header("Collision Detection")]

    [Range(0, 1)]
    [SerializeField] private float doubleJumpFixTime; //In Seconds
    [Range(0, 1)]
    [SerializeField] private float doubleWallJumpFixTime; //In Seconds
    [Range(0, .65f)]
    [SerializeField] private float groundCheckDist; //In Tiles
    [Range(0, .35f)]
    [SerializeField] private float wallCheckDist; //In Tiles

    [Space(10)]

    [Header("Timers")]

    [Range(0,1)]
    [SerializeField] private float jumpGraceTime; //In Seconds
    [Range(0, 1)]
    [SerializeField] private float jumpBufferTime; //In Seconds
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] private float wallJumpGraceTime; //In Seconds
    [Range(0, 1)]
    [SerializeField] private float currentlyWallJumpingTime; //In Seconds
    [Range(0, 1)]
    [SerializeField] private float wallJumpChangeDirLockTime; //In Seconds
    [Range(0, 5)]
    public float currentlyQuickTurnJumpingTime; //In Seconds

    [Space(15)]
    #endregion

    //User Input
    private int moveX; //-1, 0, or 1
    private float rawMoveX; //-1 to 1
    private bool jumpHeld;
    private bool jumpStatusChanged;


    //Timers
    private Cooldown jumpGraceCooldown;
    private Cooldown wallJumpGraceCooldown;
    private Cooldown jumpBufferCooldown;
    private Cooldown doubleJumpFixCooldown;
    private Cooldown doubleJumpWallFixCooldown;
    private Cooldown currentlyWallJumpingCooldown;
    private Cooldown wallJumpChangeDirLockCooldown;

    //State Vars
    private bool isGroundedThisFixedUpdate;

    private int lastWallTouched; //-1 for left, +1 for right
    [HideInInspector]
    public int quickTurnFullSpeedDir; //-1 for left, +1 for right
    #endregion

    [HideInInspector]
    public bool throwing;

    private void Awake()
    {
        gameControls = new GameControls();
        maxSpeedValue = maxSpeed;
    }

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        rigidbody.velocity = Vector2.zero;

        moveX = 0;
        velocitySign = 0;

        //Initialize Timers
        jumpGraceCooldown = new Cooldown(jumpGraceTime);
        wallJumpGraceCooldown = new Cooldown(wallJumpGraceTime);
        jumpBufferCooldown = new Cooldown(jumpBufferTime);
        currentlyWallJumpingCooldown = new Cooldown(currentlyWallJumpingTime);
        wallJumpChangeDirLockCooldown = new Cooldown(wallJumpChangeDirLockTime);
        doubleJumpFixCooldown = new Cooldown(doubleJumpFixTime);
        doubleJumpWallFixCooldown = new Cooldown(doubleWallJumpFixTime);
        quickTurnFullSpeedDir = 0;

        jumpState = JumpState.None;
    }

    private void Update()
    {
        //Horizontal Movement
        rawMoveX = gameControls.Game.Move.ReadValue<float>();
            
        int inputMoveX;

        if (rawMoveX >= deadZone)
            inputMoveX = 1;
        else if (rawMoveX <= -deadZone)
            inputMoveX = -1;
        else
            inputMoveX = 0;

        //Vertical Movement
        bool inputJumpHeld = gameControls.Game.Jump.ReadValue<float>() == 1;
        bool inputJumpStatusChanged = gameControls.Game.Jump.triggered;

        if (PauseManager.Instance.IsPausedAny() || IngredientManager.Instance.HarvestStatus())
        {
            inputMoveX = 0;
            inputJumpHeld = false;
            inputJumpStatusChanged = false;
        }
        else if (gameControls.Game.Interact.IsPressed() && inputMoveX != 0 && Inventory.Instance.potion != null)
        {
            inputMoveX = 0;
            inputJumpHeld = false;
            inputJumpStatusChanged = false;

            throwing = true;
            Debug.Log("Throwing");
            Thrower.Instance.Throwing();
        }
        else if (throwing && !gameControls.Game.Interact.IsPressed() && inputMoveX != 0 && Inventory.Instance.potion != null)
        {
            throwing = false;
            Thrower.Instance.Throw();
        }
        else 
        {
            Thrower.Instance.NotThrowing();
            throwing = false;
        }

        PivStepLogic(inputJumpHeld, inputMoveX, inputJumpStatusChanged);
    }

    public void PivStepLogic(bool inputJumpHeld, int inputMoveX, bool inputJumpStatusChanged) 
    {
        moveX = inputMoveX;
        jumpHeld = inputJumpHeld;
        jumpStatusChanged = inputJumpStatusChanged;

        isGroundedThisFixedUpdate = IsTouchingGround();

        CollisionNudging();

        Jumping();
        HorizontalMovement();

        animator.UpdateAnimator(moveX, jumpState, isGroundedThisFixedUpdate);
    }

    private void CollisionNudging() 
    {
        //Vert
        if (rigidbody.velocity.y > 0 && !IsGrounded())
        {
            if (vertHitColliderL.OverlapCollider(cf2d, collider2dArray) > 0 && vertAirColliderL.OverlapCollider(cf2d, collider2dArray) == 0)
            {
                rigidbody.MovePosition(transform.position - (vertHitColliderL.transform.position - vertAirColliderL.transform.position));
                print("vNudge - R");
            }
            if (vertHitColliderR.OverlapCollider(cf2d, collider2dArray) > 0 && vertAirColliderR.OverlapCollider(cf2d, collider2dArray) == 0)
            {
                rigidbody.MovePosition(transform.position - (vertHitColliderR.transform.position - vertAirColliderR.transform.position));
                print("vNudge - L");
            }
        }

        //Horz
        if (horzHitColliderL.OverlapCollider(cf2d, collider2dArray) > 0 && horzAirColliderL.OverlapCollider(cf2d, collider2dArray) == 0 && rigidbody.velocity.x < 0)
        {
            rigidbody.MovePosition(transform.position - ((horzHitColliderL.transform.position - horzAirColliderL.transform.position) - (.01f * Vector3.up)));
            print("hNudge - L");
        }
        if (horzHitColliderR.OverlapCollider(cf2d, collider2dArray) > 0 && horzAirColliderR.OverlapCollider(cf2d, collider2dArray) == 0 && rigidbody.velocity.x > 0)
        {
            rigidbody.MovePosition(transform.position - ((horzHitColliderR.transform.position - horzAirColliderR.transform.position) - (.01f * Vector3.up)));
            print("hNudge - R");
        }
    }

    private int velocitySign;
    private float velocityMagnitude;
    private void HorizontalMovement() 
    {
        float accelerateSpeedValue;
        if (IsGrounded()) 
            accelerateSpeedValue = accelerateSpeed;
        else
            accelerateSpeedValue = airAccelerateSpeed;

        float decelerateSpeedValue;
        if (IsGrounded())
            decelerateSpeedValue = decelerateSpeed;
        else
            decelerateSpeedValue = airDecelerateSpeed;

        float dirChangeEffeciencyValue = IsGrounded() ? dirChangeEffeciency : airDirChangeEffeciency;

        if (IsGrounded())
            maxSpeedValue = maxSpeed;
        else
            maxSpeedValue = airMaxSpeed;

        float decelerationPastMaxValue = IsGrounded() ? groundDeceleratePastMax : airDeceleratePastMax;

        float accelerationDampingValue = IsGrounded() ? accelerationDamping : airAccelerationDamping;

        float decelerationDampingValue = IsGrounded() ? decelerationDamping : airDecelerationDamping;

        velocityMagnitude = Mathf.Abs(rigidbody.velocity.x);
        if (velocityMagnitude < velocityZeroThreshold)
            velocitySign = 0;
        else
            velocitySign = (int) Mathf.Sign(rigidbody.velocity.x);

        if (moveX != 0 && !(!wallJumpChangeDirLockCooldown.IsCool() && velocitySign != moveX))
        {
            //Accelerate
            if ((velocitySign == moveX && (velocityMagnitude <= maxSpeedValue || (velocityMagnitude > maxSpeed && !IsGrounded()))) || velocitySign == 0)
            {
                rigidbody.velocity += Vector2.right * moveX * Mathf.Min(accelerateSpeedValue * Time.deltaTime, Mathf.Clamp(maxSpeedValue - velocityMagnitude, 0, Mathf.Infinity));
                rigidbody.velocity *= (Vector2.right * Mathf.Abs(rawMoveX) * Mathf.Pow(1 - accelerationDampingValue, Time.deltaTime)) + Vector2.up;
            }
            else if (velocitySign == moveX && velocityMagnitude > maxSpeedValue/* && IsGrounded()*/) //Decelerate to maxspeed (when grounded)
            {
                if (Mathf.Abs(decelerationPastMaxValue * Time.deltaTime) > velocityMagnitude - maxSpeedValue)
                    rigidbody.velocity = new Vector2(maxSpeedValue * velocitySign, rigidbody.velocity.y);
                else
                    rigidbody.velocity += Vector2.right * -velocitySign * decelerationPastMaxValue * Time.deltaTime;
            }
            else if(currentlyWallJumpingCooldown.IsCool())//Change Dir
                rigidbody.velocity = new Vector2(moveX * Mathf.Abs(rawMoveX) * (velocityMagnitude * (dirChangeEffeciencyValue / 100f)), rigidbody.velocity.y);
        }
        else //Decelerate. AKA moveX 0
        {
            if (decelerateSpeedValue * Time.deltaTime > velocityMagnitude)
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            else 
            {
                rigidbody.velocity += Vector2.right * -velocitySign * decelerateSpeedValue * Time.deltaTime;
                rigidbody.velocity *= (Vector2.right * Mathf.Pow(1 - decelerationDampingValue, Time.deltaTime)) + Vector2.up;
            }
        }
    }

    private void Jumping() 
    {
        //Wall Jump Grace
        if (CanWallJump() != 0 && doubleJumpWallFixCooldown.IsCool())
        {
            lastWallTouched = CanWallJump();
            wallJumpGraceCooldown.Start();
        }

        if (IsTouchingGround() && doubleJumpFixCooldown.IsCool()) jumpGraceCooldown.Start(); //Grace Timer

        if (jumpStatusChanged && jumpHeld) jumpBufferCooldown.Start(); //Jump Buffering

        //Set Jump State
        if (!wallJumpGraceCooldown.IsCool())
        {
            jumpState = JumpState.WallSlide;
            jumpStateDir = CanWallJump();
        }
        else
        {
            if (!jumpGraceCooldown.IsCool()) 
            {
                jumpState = JumpState.QuickTurn;
                jumpStateDir = quickTurnFullSpeedDir;
            }
            else
                jumpState = JumpState.None;
        }


        //Jump
        if (!jumpBufferCooldown.IsCool() && (!jumpGraceCooldown.IsCool() || !wallJumpGraceCooldown.IsCool()))
        {
            Trauma.Instance.AddTrauma(0.3f);

            jumpBufferCooldown.SetCool(); //Consume Buffer
            doubleJumpFixCooldown.Start();

            if (rigidbody.velocity.y < 0)
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            
            if (!jumpGraceCooldown.IsCool()) //Normal Jump
            {
                jumpGraceCooldown.SetCool();

                rigidbody.velocity += Vector2.up * Mathf.Min(Mathf.Clamp(maxJumpVelocity - rigidbody.velocity.y, 0, Mathf.Infinity), jumpVelocity);

                print("Jump");
            }
            else if (!wallJumpGraceCooldown.IsCool()) //Wall Jump
            {
                doubleJumpWallFixCooldown.Start();

                wallJumpGraceCooldown.SetCool();
                rigidbody.velocity += -lastWallTouched * Vector2.right * wallJumpVelocity;

                rigidbody.velocity += Vector2.up * Mathf.Min(Mathf.Clamp(maxJumpVelocity - rigidbody.velocity.y, 0, Mathf.Infinity), wallJumpVerticalVelocity);

                currentlyWallJumpingCooldown.Start();
                wallJumpChangeDirLockCooldown.Start();

                var dir = "";
                if (lastWallTouched == 1) dir = "R";
                else if (lastWallTouched == -1) dir = "L";
                else if (lastWallTouched == 0) dir = "?";

                print("wall jump " + dir);
            }

            //print("jump vel: " + rigidbody.velocity.y);
        }

        float maxFallSpeed = maxAirFallSpeed;

        //Gravity Manipulation
        if (rigidbody.velocity.y < 0 && !IsGrounded() && ((IsTouchingWallRight() && moveX == 1) || (IsTouchingWallLeft() && moveX == -1))) 
        { 
            rigidbody.gravityScale = wallSlideGravityMultiplier;
            maxFallSpeed = maxWallFallSpeed;
        }
        else if (rigidbody.velocity.y < -floatVelocityRange && !IsGrounded()) //Fall hard
        {
            rigidbody.gravityScale = gravityFallMultiplier;
            jumpState = JumpState.Fall;
        }
        else if (rigidbody.velocity.y >= -floatVelocityRange && rigidbody.velocity.y <= floatVelocityRange && jumpHeld && !IsGrounded()) //Float at top of jump
        {
            rigidbody.gravityScale = topHeightGravityMultiplier;
            jumpState = JumpState.Upward;
        }
        else if (!IsGrounded() && !jumpHeld) //Hold jump to jump higher
        {
            rigidbody.gravityScale = lowJumpGravityMultiplier;
            jumpState = JumpState.Upward;
        }
        else
            rigidbody.gravityScale = 1;

        if(!currentlyWallJumpingCooldown.IsCool() && rigidbody.velocity.y <= wallJumpZeroGravThreshold)
            rigidbody.gravityScale = 0;

        if (rigidbody.velocity.y < -maxFallSpeed)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -maxFallSpeed, Mathf.Infinity));
    }

    public bool IsGrounded() 
    {
        return isGroundedThisFixedUpdate && doubleJumpFixCooldown.IsCool();
    }

    private int CanWallJump() 
    {
        if (!IsGrounded() && IsTouchingWallRight())
            return 1;
        if (!IsGrounded() && IsTouchingWallLeft())
            return -1;
        
        return 0;
    }

    private bool IsTouchingGround()
    {
        Color col;
        var check1 = Physics2D.Raycast(transform.position, -Vector2.up, collider.bounds.extents.y + groundCheckDist);
        col = check1 ? Color.green : Color.red;
        Debug.DrawRay(transform.position, -Vector3.up * (collider.bounds.extents.y + groundCheckDist), col);

        var check2 = Physics2D.Raycast(transform.position + (transform.right * collider.bounds.extents.x), -Vector2.up, collider.bounds.extents.y + groundCheckDist);
        col = check2 ? Color.green : Color.red;
        Debug.DrawRay(transform.position + (transform.right * collider.bounds.extents.x), -Vector3.up * (collider.bounds.extents.y + groundCheckDist), col);

        var check3 = Physics2D.Raycast(transform.position + (transform.right * -collider.bounds.extents.x), -Vector2.up, collider.bounds.extents.y + groundCheckDist);
        col = check3 ? Color.green : Color.red;
        Debug.DrawRay(transform.position + (transform.right * -collider.bounds.extents.x), -Vector3.up * (collider.bounds.extents.y + groundCheckDist), col);

        return check1 || check2 || check3;
    }

    private bool checkRight;
    private bool IsTouchingWallRight() 
    {
        checkRight = Physics2D.Raycast(collider.bounds.center + (transform.up * collider.bounds.extents.y), Vector2.right, collider.bounds.extents.x + wallCheckDist, ~wallIgnoreLayer);
        var checkRight2 = Physics2D.Raycast(collider.bounds.center, Vector2.right, collider.bounds.extents.x + wallCheckDist, ~wallIgnoreLayer);
        checkRight = checkRight || checkRight2;
        Debug.DrawRay(collider.bounds.center + (transform.up * collider.bounds.extents.y), Vector2.right * (collider.bounds.extents.x + wallCheckDist), checkRight ? Color.green : Color.red);
        Debug.DrawRay(collider.bounds.center, Vector2.right * (collider.bounds.extents.x + wallCheckDist), checkRight2 ? Color.green : Color.red);

        return checkRight;
    }

    private bool checkLeft;
    private bool IsTouchingWallLeft()
    {
        checkLeft = Physics2D.Raycast(collider.bounds.center + (transform.up * collider.bounds.extents.y), Vector2.left, collider.bounds.extents.x + wallCheckDist, ~wallIgnoreLayer);
        var checkLeft2 = Physics2D.Raycast(collider.bounds.center, Vector2.left, collider.bounds.extents.x + wallCheckDist, ~wallIgnoreLayer);
        checkLeft = checkLeft || checkLeft2;
        Debug.DrawRay(collider.bounds.center + (transform.up * collider.bounds.extents.y), Vector2.left * (collider.bounds.extents.x + wallCheckDist), checkLeft ? Color.green : Color.red);
        Debug.DrawRay(collider.bounds.center, Vector2.left * (collider.bounds.extents.x + wallCheckDist), checkLeft2 ? Color.green : Color.red);

        return checkLeft;
    }
}
