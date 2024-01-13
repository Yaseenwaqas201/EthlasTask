using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header(" === Reference of Player Controller ===")]
    public JoystickController joystickController;
    public float maxMoveSpeed = 5f;  // We can adjust from inspector as well
    public float jumpForce = 10f; // We can adjust from inspector as well
    private Rigidbody2D playerRigidBody;
    [SerializeField]
    private Collider2D playerCollider;
    public LayerMask platformLayer;
    public bool isPlayerOnGround=true;
    public GameObject playerSP;
    private float horizontalInput;
    private float currentMoveSpeed;
    private float normalizedSpeed;
    private float joystickDistance;

    public WeaponManager weaponManager;
    
    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
//        if(!joystickController)
//            joystickController = GameplayManager.gameplayManagerInstance.JoystickController;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        JumpPlayerWithKeyBoard();
    }

    

    // Function for Controlling the movement of Player
    private void MovePlayer()
    {
        if (joystickController == null)
        {
            return;
        }

        // Movement with Keyboard
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = Input.GetAxis("Horizontal");
            currentMoveSpeed = maxMoveSpeed ;
        }
        else
        {
            joystickDistance = joystickController.GetJoystickDistance();
//            normalizedSpeed = Mathf.Clamp01(joystickDistance / joystickController.GetMaxJoystickDistance()); // Speed Depend on joystick distance from initial point
//            currentMoveSpeed = maxMoveSpeed * normalizedSpeed;
            currentMoveSpeed = maxMoveSpeed ;
            horizontalInput = joystickController.GetHorizontalValue();
        }

        if (horizontalInput>0)
        {
            playerSP.transform.rotation=Quaternion.Euler(0,0,0);
        }
        if (horizontalInput<0)
        {
            playerSP.transform.rotation=Quaternion.Euler(0,-180,0);
        }
        Vector3 movement = currentMoveSpeed * Time.deltaTime * new Vector3(horizontalInput, 0, 0f) ;
        transform.Translate(movement);
    }


    private Collider2D ignoreCollider; // Platform Collider above the player
    public void JumpPlayerWithKeyBoard()
    {
        IgnorePlatformColliderIfPlayerUnderIt();
        if(ignoreCollider!=null)
            IgnoringTheColliderUptoFewSeconds();
        if (Input.GetKeyDown(KeyCode.UpArrow) )
        {
            JumpPlayerActions();
        }
        isPlayerOnGround = IsOnPlatform();
    }

    
    // Here We ignore the Collider for few seconds so Player Can easily  land on Platfrom that player pass from below
    private float delayTimeToIgnoreCollider=0.2f;
    private float delayTimeToIgnoreColliderTimer=0.2f;
    public void IgnoringTheColliderUptoFewSeconds()
    {
            if (delayTimeToIgnoreColliderTimer > 0)
            {
                delayTimeToIgnoreColliderTimer -= Time.deltaTime;
            }
            else
            {
                Physics2D.IgnoreCollision(playerCollider,ignoreCollider, false);
                ignoreCollider = null;
                delayTimeToIgnoreColliderTimer = delayTimeToIgnoreCollider;
            }
    }
    
    public void JumpPlayerActions()
    {
        if (isPlayerOnGround)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f); // Reset vertical velocity to prevent additive force
            playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        isPlayerOnGround = false;
    }
    
    [Header(" References of Box Raycast for ignore Platform that above of player")]
    [SerializeField]
    private Transform platformDetector;
    [SerializeField]
    private float distanceFromPLayerCollider_AP = .8f;
    [SerializeField]
    private float widthOfBox_AP =2f;
    [SerializeField]
    private float heightOfBox_Ap =0.3f;

    // Used for character pass from platform if player jump under platform
    void IgnorePlatformColliderIfPlayerUnderIt()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            platformDetector.position, // Center of the player's collider
            new Vector2(widthOfBox_AP,heightOfBox_Ap),   // Size of the player's collider
            0f,                      // Angle (0 for horizontal box cast)
            Vector2.up,            // Direction (down for checking the ground)
            distanceFromPLayerCollider_AP,     // Distance to check
            platformLayer              // Layer mask for the ground
        );
        if(hit.collider!=null)
            if (hit.collider.CompareTag("Platforms"))
            {
                ignoreCollider = hit.collider;
                Physics2D.IgnoreCollision(playerCollider,hit.collider, true);
            }
    }
    
    // Is Used For Checking is Character placed on Ground
    [Header(" References of Box Raycast for Ground Status")] [SerializeField]
    private Transform groundDetector;
    [SerializeField]
    private float widthOfGBox=2;
    [SerializeField]
    private float heightOGBox=0.1f;
    [SerializeField]
    private float distanceFromPlayerColliderForGB=0.8f;
    bool IsOnPlatform()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            groundDetector.position , // Center of the player's collider
            new Vector2(widthOfGBox,heightOGBox),       // Size of the player's collider
            0f,                         // Angle (0 for horizontal box cast)
            Vector2.down,            // Direction (down for checking the ground)
            distanceFromPlayerColliderForGB,     // Distance to check
            platformLayer              // Layer mask for the ground
        );
        return hit.collider != null ;
    }


    void OnDrawGizmos()
    {
        // Draw the box cast for Checking Detecting where player can ignore above platform collider
        DrawWireCube(platformDetector.position + new Vector3(0f, distanceFromPLayerCollider_AP / 2f, 0f), new Vector3(widthOfBox_AP, heightOfBox_Ap,0), Color.green);

        // Draw the Box cast for checking the player placing on ground
        DrawWireCube(groundDetector.position - new Vector3(0f, distanceFromPlayerColliderForGB / 2f, 0f), new Vector3(widthOfGBox,heightOGBox), Color.magenta);
        
        
        // Draw a wire frame cube in the Unity Editor
        void DrawWireCube(Vector3 center, Vector3 size, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
        }
    }



    [Header(" Reference For Health Of Player")]
    public GameObject playerHealthBar;
    public float totalHeathOfPlayer;
    public float healthRemaining;
    public string nameOfPlayer;

    [ContextMenu("Reduce Health")]
    public void ReducePlayerHealth(float damageHealth)
    {
        healthRemaining -= damageHealth;
        float normalizedHealthRemain = Mathf.Clamp01(healthRemaining / totalHeathOfPlayer); 
        playerHealthBar.transform.localScale =new Vector3(normalizedHealthRemain,playerHealthBar.transform.localScale.y);
    }


    public void Fire()
    {
        weaponManager.FireBullet();
    }

    
}
