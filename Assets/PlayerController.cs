using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera FirstPersonCam;
    [SerializeField] private Camera ThirdPersonCam;
    [SerializeField] private float Sensitivity;
    [SerializeField] public Rigidbody rb;
    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale, moveDirection;

    public bool isMoving, isCrouching, isRunning;

    public float JumpForce;
    private float X, Y;
    private float gravity = 20.0f;

    public Camera ChosenCam;

    private void Start()
    {
        ChosenCam = FirstPersonCam;
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(ChosenCam == FirstPersonCam)
            {
                ChosenCam = ThirdPersonCam;
            }
            else if (ChosenCam == ThirdPersonCam)
            {
                ChosenCam = FirstPersonCam;
            }
        }

        var mousePos = Input.mousePosition;
        var wantedPos = ChosenCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mousePos.z));

        player.transform.LookAt(wantedPos);

        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion
        transform.localRotation = Quaternion.Euler(Y, X, 0.0f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;

        cc.SimpleMove((forward + right) * speed);
        // Determines if the speed = run or walk
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("runs");
            for (int i = 0; i < JumpForce; i++)
            {
                Debug.Log("runs many times");
                cc.Move(new Vector3(0, JumpForce / JumpForce + i, 0));
            }
        }
        if (!cc.isGrounded)
        {
            
        }
           
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }
        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }
}
