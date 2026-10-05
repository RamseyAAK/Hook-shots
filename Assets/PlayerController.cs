using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions controls;
    private Vector2 leftStick;
    private Vector2 rightStick;

    private Vector2 defaultPosL;
    private Vector2 defaultPosR;

    public GameObject leftClaw;
    public GameObject rightClaw;

    public RelativeJoint2D leftSpring;
    public RelativeJoint2D rightSpring;
 
    private void Awake()
    {
        // Instantiate the generated C# class layout
        controls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 tempL = controls.Gameplay.LeftClaw.ReadValue<Vector2>();
        // if (tempL != leftStick) 
        // {
            // leftClaw.transform.parent.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            // leftClaw.transform.localPosition = tempL;
            // leftStick = tempL;
        // }

        leftSpring.linearOffset = controls.Gameplay.LeftClaw.ReadValue<Vector2>();
        
        // Vector2 tempR = controls.Gameplay.RightClaw.ReadValue<Vector2>();
        // if (tempR != rightStick) 
        // {
            // rightClaw.transform.parent.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            // rightClaw.transform.localPosition = tempR;
            // rightStick = tempR;
        // }

        rightSpring.linearOffset = controls.Gameplay.RightClaw.ReadValue<Vector2>();
    }
}
