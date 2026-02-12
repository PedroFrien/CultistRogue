using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;




[RequireComponent(typeof(CharacterController))]
public class FPController : MonoBehaviour
{






    [Header("Movement Parameters")]
    public float MaxSpeed => SprintInput ? SprintSpeed : WalkSpeed;



    public float Acceleration = 15f;

    [SerializeField] float WalkSpeed = 3.5f;
    [SerializeField] float SprintSpeed = 8f;

    public bool MovementEnabled = true;
    public bool CameraEnabled = true;
    public bool LookEnabled = true;

    [Space(15)]
    [Tooltip("This is how high the character can jump.")]
    [SerializeField] float BaseJumpHeight = 3f;




    [SerializeField] float DashBase = 10f;
    bool CanDash = true;

    [SerializeField] LayerMask pickup;






    public bool Sprinting
    {
        get
        {
            return SprintInput && CurrentSpeed > 0.1f;
        }
    }

    [Header("Looking Parameters")]
    public Vector2 LookSensitivity = new Vector2(0.1f, 0.1f);

    public float PitchLimit = 85f;

    [SerializeField] float currentPitch = 0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -PitchLimit, PitchLimit);
        }
    }

    [Header("Camera Parameters")]
    [SerializeField] float CameraNormalFOV = 60f;
    [SerializeField] float CameraSprintFOV = 80f;
    [SerializeField] float CameraFOVSmoothing = 1f;

    float TargetCameraFOV
    {
        get
        {
            return Sprinting ? CameraSprintFOV : CameraNormalFOV;
        }
    }

    [Header("Physics Parameters")]
    [SerializeField] float GravityScale = 3f;

    public float VerticalVelocity = 0f;

    public Vector3 CurrentVelocity { get; private set; }
    public float CurrentSpeed { get; private set; }

    public bool IsGrounded => characterController.isGrounded;


    [Header("Input")]
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public bool SprintInput;

    public bool WheelInput;

    [Header("Interacting")]
    [SerializeField] float InteractDistance = 5f;
    private IInteractable selectedInteractable;

    [Header("Coyote Time")]
    [SerializeField] float CoyoteTimeDuration = 0.15f; // Adjust this value as needed
    private float coyoteTimeCounter = 0f;

    [Header("Ability Activation")]
    [SerializeField] private float slowTime = .3f;



    [Header("Components")]
    [SerializeField] CinemachineCamera fpCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] Image interactPopup;
    private GameManager gameManager;

    [SerializeField] private GameObject abilityWheel;
    private WeaponManager weaponManager;






    // Start is called once before the first execution of Update after the MonoBehaviour is created

    #region Unity Methods

    private void OnValidate()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (MovementEnabled)
        {
            MoveUpdate();       
        }
        if (CameraEnabled)
        {
            CameraUpdate();
        }

        if (LookEnabled) LookUpdate();

        CheckForInteract();

        // Update coyote time counter
        if (IsGrounded)
        {
            coyoteTimeCounter = CoyoteTimeDuration;
            CanDash = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        

       
    }

    


    #endregion

    #region Controller Methods



    public void Attack()
    {
        weaponManager.UseWeapon();
    }


    public void OpenWheel()
    {
        abilityWheel.SetActive(true);
        gameManager.SetMouseActive(true);
        gameManager.SetTimeScale(slowTime);
        CameraEnabled = false;
        LookEnabled = false;
        
    }

    public void CloseWheel()
    {
        abilityWheel.GetComponent<AbilityWheel>().ActivateHoveredAbility();


        abilityWheel.SetActive(false);
        gameManager.SetMouseActive(false);
        gameManager.SetTimeScale(1f);
        CameraEnabled = true;
        LookEnabled = true;



    }

    public void TryJump()
    {
        if (coyoteTimeCounter <= 0f || MovementEnabled == false)
        {
            return;
        }

        

        VerticalVelocity = Mathf.Sqrt(BaseJumpHeight * -2f * Physics.gravity.y * GravityScale);

        
        // Reset coyote time so player can't jump again mid-air
        coyoteTimeCounter = 0f;
    }

    public void TryDash()
    {
        Debug.Log("Trying dash");

        if (IsGrounded == false && MovementEnabled == true && CanDash)
        {
            float storedVelocity = DashBase;
            VerticalVelocity = 0f;

            Vector3 lookDir = transform.forward;

            CurrentVelocity = CurrentVelocity + (lookDir.normalized * storedVelocity);

            CanDash = false;
        }

    }

    public void Pause()
    {
        if (gameManager.paused)
        {
            gameManager.SetPauseMenu(false);
            MovementEnabled = true;
            CameraEnabled = true;
            LookEnabled = true;
        }

        else
        {
            gameManager.SetPauseMenu(true);
            MovementEnabled = false;
            CameraEnabled = false;
            LookEnabled = false;
        }

      
    }

    void MoveUpdate()
    {
        Vector3 motion = transform.forward * MoveInput.y + transform.right * MoveInput.x;
        motion.y = 0f;
        motion.Normalize();



        if (motion.sqrMagnitude >= 0.01f)
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, motion * MaxSpeed, Acceleration * Time.deltaTime);
        }
        else
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, Acceleration * Time.deltaTime);
        }

        if (IsGrounded && VerticalVelocity <= 0.01f)
        {
            VerticalVelocity = -3f;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
        }


        Vector3 fullVelocity = new Vector3(CurrentVelocity.x, VerticalVelocity, CurrentVelocity.z);

        characterController.Move(fullVelocity * Time.deltaTime);

        CurrentSpeed = CurrentVelocity.magnitude;
    }

    void LookUpdate()
    {
        Vector2 input = new Vector2(LookInput.x * LookSensitivity.x, LookInput.y * LookSensitivity.y);

        // looking up and down
        CurrentPitch -= input.y;

        fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);


        // looking left and right
        transform.Rotate(Vector3.up * input.x);
    }

    void CameraUpdate()
    {
        float targetFOV = CameraNormalFOV;

        if (Sprinting)
        {
            float speedRatio = CurrentSpeed / SprintSpeed;

            targetFOV = Mathf.Lerp(CameraNormalFOV, CameraSprintFOV, speedRatio);
        }


        fpCamera.Lens.FieldOfView = Mathf.Lerp(fpCamera.Lens.FieldOfView, targetFOV, CameraFOVSmoothing * Time.deltaTime);
    }

    public void Reload()
    {
        weaponManager.ReloadWeapon();
    }

    public void TryInteract()
    {

        if (selectedInteractable != null)
        {

            selectedInteractable.OnInteract();
        }
    }

    

    


    #endregion












    #region Interact

    private void CheckForInteract()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, InteractDistance, pickup))
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                interactPopup.gameObject.SetActive(true);
                selectedInteractable = hit.collider.GetComponent<IInteractable>();
            }
            else
            {
                interactPopup.gameObject.SetActive(false);
                selectedInteractable = null;
            }
        }
        else
        {
            interactPopup.gameObject.SetActive(false);
            selectedInteractable = null;
        }
    }

    #endregion

}
