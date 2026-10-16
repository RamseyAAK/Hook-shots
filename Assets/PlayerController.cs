using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject leftClaw;
    public GameObject rightClaw;

    public RelativeJoint2D leftSpring;
    public RelativeJoint2D rightSpring;

    private Vector2 leftAim = default;
    private Vector2 rightAim = default;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool validatedInputValue<T>(InputAction.CallbackContext context, ref T value) where T : struct
    {
        try {
            value = context.ReadValue<T>();
            return true;
        }
        catch {
            Debug.Log($"Type mismatch, Looking for: {typeof(T).Name}, Actual type: {context.valueType}");
            return false;
        }
    }

    private RaycastHit2D HookTrace(Vector2 origin, Vector2 dir, float dist, bool debug)
    {
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, ~LayerMask.GetMask("Player"));
        if (debug) {
            if (hit.collider) {
                Debug.DrawLine(origin, hit.point, Color.green, 0.5f);
            } else {
                Debug.DrawLine(origin, (dir * dist) + origin, Color.red, 0.5f);
            }
        }

        return hit;
    }

    public void RightFire(InputAction.CallbackContext context)
    {
        if (context.started && rightAim.magnitude > 0.1) {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                RaycastHit2D hit = HookTrace(rightClaw.transform.position, rightAim, 6f, true);
                Debug.Log(hit.collider);
            } else {
                Debug.Log("Value is null");
            } 
        }
    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.started) {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                RaycastHit2D hit = HookTrace(leftClaw.transform.position, leftAim, 6f, true);
                Debug.Log(hit.collider);
            } else {
                Debug.Log("Value is null");
            } 
        }
    }
    
    public void RightAim(InputAction.CallbackContext context)
    {
        if (validatedInputValue<Vector2>(context, ref rightAim)) {
            rightSpring.linearOffset = rightAim;
        } else {
            Debug.Log("Value is null");
        } 
        
    }

    public void LeftAim(InputAction.CallbackContext context)
    {
        if (validatedInputValue<Vector2>(context, ref leftAim)) {
            leftSpring.linearOffset = leftAim;
        } else {
            Debug.Log("Value is null");
        } 
        
    }
}
