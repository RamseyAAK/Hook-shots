using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject leftClaw;
    public GameObject rightClaw;

    public RelativeJoint2D leftSpring;
    public RelativeJoint2D rightSpring;

    public DistanceJoint2D leftConnection;
    public DistanceJoint2D rightConnection;

    private Vector2 leftAim = default;
    private Vector2 rightAim = default;

    private float REACH = 6f;

    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (leftConnection.connectedBody != null) {
            Debug.DrawLine(leftClaw.transform.position, leftConnection.connectedBody.transform.TransformPoint(leftConnection.connectedAnchor), Color.green);
        }

        if (rightConnection.connectedBody != null) {
            Debug.DrawLine(rightClaw.transform.position, rightConnection.connectedBody.transform.TransformPoint(rightConnection.connectedAnchor), Color.green);
        }

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
        // dir.Normalize();
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
                RaycastHit2D hit = HookTrace(rightClaw.transform.position, rightAim, REACH, true);
                if (hit.rigidbody) {
                    rightConnection.connectedBody = hit.rigidbody;
                    rightConnection.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    rightConnection.distance = Vector2.Distance(hit.point, rightClaw.transform.position);
                    rightConnection.enabled = true;
                    Debug.Log(hit.rigidbody);
                }
            } else {
                Debug.Log("Value is null");
            } 
        } else if (context.canceled && rightConnection.connectedBody != null) {
            rightConnection.enabled = false;
            rightConnection.connectedBody = null;
        }
    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.started && leftAim.magnitude > 0.1) {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                RaycastHit2D hit = HookTrace(leftClaw.transform.position, leftAim, REACH, true);
                if (hit.rigidbody) {
                    leftConnection.connectedBody = hit.rigidbody;
                    leftConnection.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    leftConnection.distance = Vector2.Distance(hit.point, leftClaw.transform.position);
                    leftConnection.enabled = true;
                    Debug.Log(hit.rigidbody);
                }
            } else {
                Debug.Log("Value is null");
            } 
        } else if (context.canceled && leftConnection.connectedBody != null) {
            leftConnection.enabled = false;
            leftConnection.connectedBody = null;
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
