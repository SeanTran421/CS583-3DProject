using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float WalkSpeed = 5.5f;
    [SerializeField] private float RunningSpeed = 9.0f;

    [SerializeField] private float JumpForce = 8.0f; //probably might not need
    [SerializeField] private float Gravity = 20.0f; //probably might not need

    [SerializeField] private float LookSensitivity = 0.2f;
    [SerializeField] private float LookAngleLimit = 90f;

    private Camera mainCamera;
    private CharacterController characterController;
    private InputAction moveInput;
    private InputAction runInput;

    private InputAction jumpInput; //probably might not be needed
    private bool jumped = false;

    private float currentMoveSPeed = 0.0f;
    private Vector3 moveDIrection = Vector3.zero;

    private float lookAngle = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        runInput = InputSystem.actions.FindAction("Sprint");
        jumpInput = InputSystem.actions.FindAction("Jump");
        jumpInput.started += Jumped;

        Cursor.lockState = CursorLockMode.Locked; //for the mouse maybe
        Cursor.visible = false; //about the mouse icon

        currentMoveSPeed = WalkSpeed; //Tutorial said in a larger project, move each of them in separate classes
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), 
            Mouse.current.delta.y.ReadValue()); // handles mouse movement

        if(!characterController.isGrounded)
        {
            jumped = false;
        }

        currentMoveSPeed = runInput.IsPressed() ? RunningSpeed : WalkSpeed;
        HandleMovement(moveVector);
        HandleLooking(mouseDelta);
        
    }

    private void HandleMovement(Vector2 moveVector)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDIrection.y;

        Vector2 newSpeed = new Vector2(moveVector.y * currentMoveSPeed, moveVector.x * currentMoveSPeed);

        moveDIrection = (forward * newSpeed.x) + (right * newSpeed.y);

        if (jumped && characterController.isGrounded){
            moveDIrection.y = JumpForce;
        }
        else
        {
            moveDIrection.y = oldY; //keep the vertical movement if not jumping
        }

        if(!characterController.isGrounded)
        {
            moveDIrection.y -= Gravity * Time.deltaTime; // will make player fall down
        }

        characterController.Move(moveDIrection * Time.deltaTime);
    }

    private void Jumped(InputAction.CallbackContext _)
    {
        jumped = true;
    }

    private void HandleLooking(Vector2 mouseDelta) //will look back at the tutorial to see what these mean mb
    {
        lookAngle += -mouseDelta.y * LookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -LookAngleLimit, LookAngleLimit);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * LookSensitivity, 0);
    }


}
