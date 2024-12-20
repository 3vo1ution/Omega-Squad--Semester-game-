using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonControls : MonoBehaviour
{


    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object

    // Crouch settings
    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f; // Height of the player when crouching
    public float standingHeight = 2f; // Height of the player when standing
    public float crouchSpeed = 2.5f; // Speed at which the player moves when crouching
    private bool isCrouching = false; // Whether the player is currently crouching

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    //public Terrain switchTerrain; // Material to apply when switch is activated
    public GameObject currentTerrain; // Reference to the currently active terrain
    public GameObject newTerrain; // Reference to the new terrain to switch to

    //public GameObject[] objectsToChangeColor; // Array of objects to change color

    [Header("Inventory")]
    [Space(5)]
    private GameObject StowObject;
    private FirstPersonController firstpersonconroller;
    public int RequiredGumballs = 5; // Number of gumballs required to open the door
    public int RequiredGumballsGK = 4;
    public int[] inventory; //an array to store the different items (the backpack)
    public int itemCount = 0;
    public Text itemCountText; //for UI text
    public int FlyerCount = 0;
    public Text FlyerCountText; //for UI text

    [Header("Gumball Collection")] // Checking gumballs before opening a door
    public Text errorMessageText; // UI Text for displaying the error message
    public float errorMessageDuration = 3f; // How long before hiding the error message
    //health text 
    public GameObject healthText;
    public GameObject dialogue1Text;

    [Header("Flyer Interaction")]

    public float interactionDistance;
    //The distance from which the player can interact with an object

    public GameObject interactionText;
    //Text or crosshair that shows up to let the player know they can interact with an object they're looking at

    public LayerMask interactionLayers;
    //Layers the raycast can hit/interact with. Any layers unchecked will be ignored by the raycast.

    //sickly filter
    public GameObject sicklyFilter;

    public GameObject sugarRushFilter;

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public GameObject objectToMove; // The object that will be moved when interacted with
    public float OmoveDistance = 5f; // Distance the object will move
    public float OmoveSpeed = 2f;
    public ParticleSystem interactionEffect;

    public AudioSource soundEffects;
    public AudioClip raiseDoorSFX;

    [Header("Animations")]
    [Space(5)]
    public Animator animator;

    [Header("Sounds")]
    [Space(5)]
   
    public AudioClip PickUp;



    private void Awake()
    {
        inventory = new int[RequiredGumballs];
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        UpdateItemCountUI(); // Initialize the UI with the current item count
        UpdateFlyerCountUI();
       

    }


    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the ToggleCrouch method when crouch input is performed

        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        checkForPickup();
        
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Adjust speed if crouching
        float currentSpeed;

        if (moveInput.x == 0 && moveInput.y == 0)
        {
            currentSpeed = 0;  
        }
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }


        // Move the character controller based on the movement vector and speed
        characterController.Move(move * currentSpeed * Time.deltaTime);
        animator.SetFloat("Speed", currentSpeed);
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            // Jump amim
            animator.SetBool("Grounded", characterController.isGrounded);
        }
    }

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        {
            soundEffects.clip = PickUp;
            soundEffects.Play();
        }


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
            if (itemCount >= RequiredGumballs)
            {
                Debug.Log("You are carrying too much!!");
            }
            else if (hit.collider.CompareTag("Stow")) //changed the tag from PickUp to Stow 
            {

                StowObject = hit.collider.gameObject;
                Debug.Log("Object Hit");
                Destroy(StowObject);//We want it to destroy the object because we want is so
                                    // that once the item is picked up, it is added to the players 
                                    // backpack so that once a player has collected a certain amount of that item
                                    // they can then craft another item out of it 
                                    // so eventually we'll have a
                                    // if (itemCount==5)
                                    // { craft item2 (idk the code for this yet)}

                itemCount++;
                UpdateItemCountUI();
                Debug.Log(itemCount);
            }
            else if (hit.collider.CompareTag("Flyer")) //changed the tag from PickUp to Stow 
            {

                StowObject = hit.collider.gameObject;
                Debug.Log("Object Hit");
                Destroy(StowObject);//We want it to destroy the object because we want is so
                                    // that once the item is picked up, it is added to the players 
                                    // backpack so that once a player has collected a certain amount of that item
                                    // they can then craft another item out of it 
                                    // so eventually we'll have a
                                    // if (itemCount==5)
                                    // { craft item2 (idk the code for this yet)}

                FlyerCount++;
                UpdateFlyerCountUI();
                Debug.Log(FlyerCount);
            }
        }
    }

    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            // Stand up
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            // Crouch down
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                if (interactionEffect != null)
                {
                    interactionEffect.transform.position = hit.point; // Set the effect at the point of interaction
                    interactionEffect.Play(); // Play the particle effect
                }
                // Trigger the movement of the specified object
                if (objectToMove != null)
                {
                    StartCoroutine(MoveObject(objectToMove));
                }
                // Change the material color of the objects in the array
                //  foreach (GameObject obj in objectsToChangeColor)
                //  {
                //   Renderer renderer = obj.GetComponent<Renderer>();
                //  if (renderer != null)
                //{
                //  renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                //}
                // }
            }

            else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                if (itemCount >= RequiredGumballs) // Check if player has enough gumballs tp open the door
                {
                    // Open the door if enough gumballs are collected
                    StartCoroutine(RaiseDoor(hit.collider.gameObject));
                    errorMessageText.gameObject.SetActive(false); // Hide the error message if door is opened
                }
                else
                {
                    // Show error message if not enough gumballs are collected
                    errorMessageText.text = "You need " + RequiredGumballs + " gumballs to open this door!";
                    errorMessageText.gameObject.SetActive(true); // Show the error message
                    StartCoroutine(HideErrorMessageAfterDelay()); // Start coroutine to hide the message
                }
                // Start moving the door upwards
                //StartCoroutine(RaiseDoor(hit.collider.gameObject));


            }
            else if (hit.collider.CompareTag("G.door")) // Check if the object is a door
            {
                if (itemCount >= RequiredGumballsGK) // Check if player has enough gumballs tp open the door
                {
                    // Open the door if enough gumballs are collected
                    StartCoroutine(RaiseDoor(hit.collider.gameObject));
                    errorMessageText.gameObject.SetActive(false); // Hide the error message if door is opened
                }
                else
                {
                    // Show error message if not enough gumballs are collected
                    errorMessageText.text = "You need " + RequiredGumballsGK + " gumballs to open this door!";
                    errorMessageText.gameObject.SetActive(true); // Show the error message
                    StartCoroutine(HideErrorMessageAfterDelay()); // Start coroutine to hide the message
                }
            }
        }
    }

    void UpdateItemCountUI()  //updates the UI text with the current item count as a string
    {
        itemCountText.text = "- Gumballs Collected: " + itemCount.ToString() + "/5";
    }
    void UpdateFlyerCountUI()  //updates the UI text with the current item count as a string
    {
        FlyerCountText.text = "- Flyers Collected: " + FlyerCount.ToString() + "/5";
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount = 5f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising

        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }

        soundEffects.clip = raiseDoorSFX;
        soundEffects.Play();
    }
    private IEnumerator MoveObject(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position; 
        Vector3 endPosition = startPosition + Vector3.up * OmoveDistance; 

        
        while (Vector3.Distance(obj.transform.position, endPosition) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, endPosition, moveSpeed * Time.deltaTime);
            yield return null; 
        }

        // Ensure the object reaches the final position 
        obj.transform.position = endPosition;
    }

    private IEnumerator HideErrorMessageAfterDelay()
    {
        yield return new WaitForSeconds(errorMessageDuration); // Wait for the duration listed above
        errorMessageText.gameObject.SetActive(false);
    }

    public void checkForPickup()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("Health"))
            {
                healthText.SetActive(true);

            }
            else
            {
                healthText.SetActive(false);
            }

        }

        


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SicklyTrigger")
        {
            sicklyFilter.SetActive(true);
            Debug.Log("sickly on");
        }
        if (other.CompareTag("SugarRushPowerUp"))
        {
            // Trigger the speed boost and visual effect
            ActivateSpeedBoost();

            // Optionally, deactivate or destroy the power-up object
            other.gameObject.SetActive(false);
        }

    }

  


    public float normalSpeed = 5f;
    public float boostedSpeed = 10f;
    public float boostDuration = 5f;

    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoostRoutine());
    }

    private IEnumerator SpeedBoostRoutine()
    {
        // Store the original speed
        float originalSpeed = moveSpeed;

        // Set the player's speed to the boosted speed
        moveSpeed = boostedSpeed;

        // Activate the sugar rush visual filter
        sugarRushFilter.SetActive(true);

        // Wait for the boost duration
        yield return new WaitForSeconds(boostDuration);

        // Revert the player's speed to the original speed
        moveSpeed = originalSpeed;

        // Deactivate the sugar rush visual filter
        sugarRushFilter.SetActive(false);
    }
}