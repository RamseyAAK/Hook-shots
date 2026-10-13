using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject leftClaw;
    public GameObject rightClaw;

    public RelativeJoint2D leftSpring;
    public RelativeJoint2D rightSpring;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool validatedInputValue<T>(InputAction.CallbackContext context, ref T value) where T : struct
    {
        try
        {
            value = context.ReadValue<T>();
            return true;
        }
        catch
        {
            Debug.Log($"Type mismatch, Looking for: {typeof(T).Name}, Actual type: {context.valueType}");
            return false;
        }
    }

    public void RightFire(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                Debug.Log($"RightFire Value: {value}");
            } else {
                Debug.Log("Value is null");
            } 
        }
    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                Debug.Log($"LeftFire Value: {value}");
            } else {
                Debug.Log("Value is null");
            } 
        }
    }
    
    public void RightAim(InputAction.CallbackContext context)
    {
        Vector2 value = default;
        if (validatedInputValue<Vector2>(context, ref value)) {
            rightSpring.linearOffset = value;
            Debug.Log($"RightAim Value: {value}");
        } else {
            Debug.Log("Value is null");
        } 
        
    }

    public void LeftAim(InputAction.CallbackContext context)
    {
        Vector2 value = default;
        if (validatedInputValue<Vector2>(context, ref value)) {
            leftSpring.linearOffset = value;
            Debug.Log($"LeftAim Value: {value}");
        } else {
            Debug.Log("Value is null");
        } 
        
    }
}
