using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private GameInputActions actionControl; // to adjust sensitivity

    public static Vector2 move_new;
    public static Vector2 rotate_new;

    public static float MOUSE_SENSITIVITY = 5;
    public static float GAMEPAD_SENSITIVITY = 13;

    public static bool game_paused;
    public static bool esc_pressed;
    public static bool jump_pressed;
    public static bool sprinting;
    public static bool shooting;
    public static bool slow_mo;

    void ChangeSensitivity(float new_sens, int target) // add targets here
    {
        actionControl.Disable();
        if (target == 0)
            GAMEPAD_SENSITIVITY = new_sens;
        if (target == 1)
            MOUSE_SENSITIVITY = new_sens;
        Awake(); // update actionControl properties
    }

    public static void LockCursor(bool to_lock)
    {
        if (to_lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Awake()
    {
        actionControl = new GameInputActions();
        actionControl.Gameplay.Move.performed       += context => move_new = context.ReadValue<Vector2>();
        actionControl.Gameplay.Move.canceled        += context => move_new = Vector2.zero;
        actionControl.Gameplay.Jump.performed       += context => jump_pressed = true;
        actionControl.Gameplay.Jump.canceled        += context => jump_pressed = false;
        actionControl.Gameplay.Sprint.performed     += context => sprinting = true;
        actionControl.Gameplay.Sprint.canceled      += context => sprinting = false;
        actionControl.Gameplay.LookAround.performed += context => rotate_new = context.ReadValue<Vector2>();
        actionControl.Gameplay.LookAround.canceled  += context => rotate_new = Vector2.zero;
        actionControl.Gameplay.Shoot.performed      += context => shooting = true;
        actionControl.Gameplay.Shoot.canceled       += context => shooting = false;
        actionControl.Gameplay.SlowMotion.performed += context => slow_mo = true;
        actionControl.Gameplay.SlowMotion.canceled  += context => slow_mo = false;
        actionControl.Gameplay.Pause.performed      += context => esc_pressed = true;
        actionControl.Gameplay.Pause.canceled       += context => esc_pressed = false;

        // action for testing
        actionControl.Gameplay.SensitivityG.performed += context =>
        {
            ChangeSensitivity(GAMEPAD_SENSITIVITY - 1, 0); // 0 - gamepad, 1 - mouse
        };
        actionControl.Gameplay.SensitivityM.performed += context =>
        {
            ChangeSensitivity(MOUSE_SENSITIVITY - 1, 1); // 0 - gamepad, 1 - mouse
        };

        string phr0 = "ScaleVector2(x=" + GAMEPAD_SENSITIVITY + ",y=" + GAMEPAD_SENSITIVITY + ")"; // add indexes here
        actionControl.Gameplay.LookAround.ChangeBinding(0).WithProcessor(phr0);
        string phr1 = "ScaleVector2(x=" + MOUSE_SENSITIVITY + ",y=" + MOUSE_SENSITIVITY + ")";
        actionControl.Gameplay.LookAround.ChangeBinding(1).WithProcessor(phr1);

        actionControl.Enable();
    }

    void OnEnable()
    {
        actionControl.Enable();
    }

    void OnDisable()
    {
        actionControl.Disable();
    }

    void Start()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        move_new = Vector2.zero;
        rotate_new = Vector2.zero;
        game_paused = false;
        esc_pressed = false;
        jump_pressed = false;
        sprinting = false;
        shooting = false;
        slow_mo = false;
    }
}
