using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     if (Keyboard.current.shiftKey.isPressed)
         Debug.Log("shift");
     if (Mouse.current.delta.IsActuated())
         Debug.Log("delta");
     public void OnControlsChanged()
     
     */
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Camera playerCamera; // to rotate
    [SerializeField]
    public Transform playerBody; // to get data and rotate

    private CharacterController playerControl; // to move around

    private Vector3 move_directions;
    private Vector2 move_lerp;
    private Vector2 rotate_directions;

    private const float MOVEMENT_SPEED = 12f;
    private const float GRAVITY = -9.81f * 3f;
    private const float JUMP_VELOCITY = 15f; // 15f
    private float SLOW_MO = 1f;

    private bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerBody.position, playerBody.TransformDirection(Vector3.down), out hit, playerControl.height / 2 * 2.2f))
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        return false;
    }

    void MakeMove()
    {
        if (playerControl.isGrounded)
        {
            if (move_directions.y < 0f) move_directions.y = 0f;

            move_lerp.x = Mathf.Lerp(move_lerp.x, InputController.move_new.x, 0.15f); // making smooth movements
            if (InputController.sprinting)
            {
                move_lerp.y = Mathf.Lerp(move_lerp.y, InputController.move_new.y * 2f, 0.1f);
            }
            else
            {
                move_lerp.y = Mathf.Lerp(move_lerp.y, InputController.move_new.y, 0.15f);
            }

            move_directions = playerBody.right * move_lerp.x * MOVEMENT_SPEED + playerBody.forward * move_lerp.y * MOVEMENT_SPEED; // making move vector
            if (InputController.jump_pressed) move_directions.y += JUMP_VELOCITY;
            else if (OnSlope())
                move_directions.y += GRAVITY / 3; // add more gravity
        }

        else if(!InputController.slow_mo) // can be removed // allow to move in the air, not in slow mo
        {
            float downforce = move_directions.y;
            move_directions = playerBody.right * move_lerp.x * MOVEMENT_SPEED + playerBody.forward * move_lerp.y * MOVEMENT_SPEED; // making move vector
            move_directions.y = downforce;
        }
        if (InputController.jump_pressed) InputController.jump_pressed = false;
        move_directions.y += GRAVITY * Time.deltaTime * SLOW_MO;
        playerControl.Move(move_directions * Time.deltaTime * SLOW_MO);
    }

    void MakeRotation()
    {
        rotate_directions.x = InputController.rotate_new.x * Time.deltaTime; // sensitivity is a variable value already applied to context. 
        rotate_directions.y -= InputController.rotate_new.y * Time.deltaTime; // storing the current angle, not delta
        rotate_directions.y = Mathf.Clamp(rotate_directions.y, -89f, 89f); // if angle is out of bounds, normalise it

        playerCamera.transform.localEulerAngles = new Vector3(rotate_directions.y, 0f, 0f);
        playerBody.Rotate(new Vector3(0f, rotate_directions.x, 0f), Space.Self);
    }

    private float pushPower = 5.0f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        //body.velocity = pushDir * pushPower;
        //body.AddForceAtPosition(pushDir * pushPower, hit.point);
    }

    void ApplySlowMotion()
    {
        if (InputController.slow_mo)
        {
            SLOW_MO = 0.25f;
        }
        else
        {
            SLOW_MO = 1f;
        }
        ChangeRBSpeed(SLOW_MO);
    }

    void ChangeRBSpeed(float modifier)
    {
        var foundRB = FindObjectsOfType<Rigidbody>();
        for (int i = 0; i < foundRB.Length; i++){
            Debug.Log(i);
        }
    }

    void Start()
    {
        InputController.LockCursor(true);
        Cursor.visible = false;
        playerControl = GetComponent<CharacterController>();
        move_lerp = Vector2.zero;
        rotate_directions = Vector2.zero;
    }

    void Update()
    {
        if (!InputController.game_paused)
        {
            ApplySlowMotion();
            MakeRotation();
            MakeMove();
        }
    }
}









/*
void AddGravity(float amount)
    {
        playerRB.AddForce(playerBody.up * amount);
    }
 
     playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
     playerRB.AddForce(playerBody.right * InputController.move_new.x * MOVEMENT_SPEED * Time.deltaTime * SLOW_MO);
     playerRB.AddForce(playerBody.forward * InputController.move_new.y * MOVEMENT_SPEED * (InputController.sprinting ? 2f : 1f) * Time.deltaTime * SLOW_MO);
     playerRB = GetComponent<Rigidbody>();

     SURFACE_TYPE GetSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerBody.position, playerBody.TransformDirection(Vector3.down), out hit, playerCollider.height / 2 + 0.1f))
        {
            if (hit.normal != Vector3.up)
            {
                Debug.Log("OnSlope");
                return SURFACE_TYPE.SLOPE;
            }
            Debug.Log("On the ground");
            return SURFACE_TYPE.GROUND;
        }
        Debug.Log("In the air");
        return SURFACE_TYPE.NONE;
    }

    playerRB.AddForce(playerBody.up * JUMP_VELOCITY * Time.deltaTime * SLOW_MO, ForceMode.Impulse);
*/
