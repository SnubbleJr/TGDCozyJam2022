using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController Controller;
    private float CamYRotation = 0f;
    [SerializeField]
    private GameObject PlayerCamera;
    [SerializeField]
    private GameObject PlayerBody;

    public Rigidbody PlayerHand;
    
    [HideInInspector]
    public enum MovementState
    {
        Walking,
        Running
    }

    private MovementState CurrentMovementState;

    [Header("Player speeds")]
    public float MouseSensitivity = 5.0f;
    public float RunSpeed = 10f;
    public float WalkSpeed = 5f;
    public Dictionary<MovementState, float> MovementSpeedMap = new Dictionary<MovementState, float>();


    // Start is called before the first frame update
    void Start()
    {
        Controller = gameObject.GetComponent<CharacterController>();
        CurrentMovementState = MovementState.Walking;
        MovementSpeedMap.Add(MovementState.Running, RunSpeed);
        MovementSpeedMap.Add(MovementState.Walking, WalkSpeed);

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();

        Debug.DrawLine(PlayerCamera.transform.position, PlayerCamera.transform.position + (PlayerCamera.transform.TransformDirection(Vector3.forward) * 3));

        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            int layermask = 1 << 3;
            if(Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, 100f, layermask))
            {
                Interactable DesiredInteraction = hit.transform.gameObject.GetComponent<Interactable>();
                if (DesiredInteraction != null)
                {
                    DesiredInteraction.DoInteraction(this);
                }
            }
        }
    }

    void HandlePlayerMovement()
    {
        Vector3 MoveTo = Vector3.zero;

        Vector2 MovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 LookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * MouseSensitivity;

        CamYRotation -= LookInput.y;
        CamYRotation = Mathf.Clamp(CamYRotation, -75.0f, 75.0f);

        transform.Rotate(Vector3.up * LookInput.x);
        PlayerCamera.transform.localRotation = Quaternion.Euler(CamYRotation, 0.0f, 0.0f);

        MoveTo = (transform.right * MovementInput.x) + (transform.forward * MovementInput.y);
        Controller.Move(MoveTo * Time.deltaTime * MovementSpeedMap[CurrentMovementState]);
    }
}
