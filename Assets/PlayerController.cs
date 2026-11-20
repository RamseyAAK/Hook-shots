using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
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

    public float hookshotReach = 6f;
    public float armStrength = 100f;

    public PhysicsMaterial2D frictionMaterial;
    public PhysicsMaterial2D frictionlessMaterial;

    public Material leftRope;
    public Material rightRope;

    private static readonly int _PointOneTwo = Shader.PropertyToID("_PointOneTwo");
    private static readonly int _Length = Shader.PropertyToID("_Length");

    [Header("Motility Toggles")]
    public bool gravity = true;
    public bool arm_mass = true;
    public bool arm_collision = true;
    public bool arm_friction = true;
    public bool body_friction = true;


    

    void Start()
    {
        leftSpring.maxForce = armStrength;
        leftSpring.maxTorque = armStrength;
        rightSpring.maxForce = armStrength;
        rightSpring.maxTorque = armStrength;

        Rigidbody2D main_rb = GetComponent<Rigidbody2D>();
        Rigidbody2D left_rb = leftClaw.GetComponent<Rigidbody2D>();
        Rigidbody2D right_rb = rightClaw.GetComponent<Rigidbody2D>();

        if (gravity) {
            main_rb.gravityScale = 1;
            left_rb.gravityScale = 1;
            right_rb.gravityScale = 1;
        } else {
            main_rb.gravityScale = 0;
            left_rb.gravityScale = 0;
            right_rb.gravityScale = 0;
        }

        if (arm_mass) {
            left_rb.mass = 1;
            right_rb.mass = 1;
        } else {
            left_rb.mass = 0;
            right_rb.mass = 0;
        }

        if (!arm_collision) {
            left_rb.excludeLayers = LayerMask.GetMask("Default");
            right_rb.excludeLayers = LayerMask.GetMask("Default");
        }

        if (arm_friction) {
            left_rb.sharedMaterial = frictionMaterial;
            right_rb.sharedMaterial = frictionMaterial;
        } else {
            left_rb.sharedMaterial = frictionlessMaterial;
            right_rb.sharedMaterial = frictionlessMaterial;
        }

        if (body_friction) {
            main_rb.sharedMaterial = frictionMaterial;
        } else {
            main_rb.sharedMaterial = frictionlessMaterial;
        }



    }

    // Update is called once per frame
    void Update()
    {
        Vector4 oneTwo = default;
        if (leftConnection.connectedBody != null) {
            oneTwo.Set(leftClaw.transform.position.x, leftClaw.transform.position.y, leftConnection.connectedBody.transform.TransformPoint(leftConnection.connectedAnchor).x, leftConnection.connectedBody.transform.TransformPoint(leftConnection.connectedAnchor).y);
            leftRope.SetVector(_PointOneTwo, oneTwo);
            Debug.DrawLine(leftClaw.transform.position, leftConnection.connectedBody.transform.TransformPoint(leftConnection.connectedAnchor), Color.green);
        }

        if (rightConnection.connectedBody != null) {
            oneTwo.Set(rightClaw.transform.position.x, rightClaw.transform.position.y, rightConnection.connectedBody.transform.TransformPoint(rightConnection.connectedAnchor).x, rightConnection.connectedBody.transform.TransformPoint(rightConnection.connectedAnchor).y);
            rightRope.SetVector(_PointOneTwo, oneTwo);
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
                RaycastHit2D hit = HookTrace(rightClaw.transform.position, rightAim, hookshotReach, true);
                if (hit.rigidbody) {
                    rightConnection.connectedBody = hit.rigidbody;
                    rightConnection.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    rightConnection.distance = Vector2.Distance(hit.point, rightClaw.transform.position);
                    rightRope.SetFloat(_Length, rightConnection.distance * 1.001f);
                    rightConnection.enabled = true;
                    Debug.Log(hit.rigidbody);
                }
            } else {
                Debug.Log("Value is null");
            } 
        } else if (context.canceled && rightConnection.connectedBody != null) {
            rightConnection.enabled = false;
            rightConnection.connectedBody = null;
            rightRope.SetFloat(_Length, 0.0f);
        }
    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.started && leftAim.magnitude > 0.1) {
            System.Single value = default;
            if (validatedInputValue<System.Single>(context, ref value)) {
                RaycastHit2D hit = HookTrace(leftClaw.transform.position, leftAim, hookshotReach, true);
                if (hit.rigidbody) {
                    leftConnection.connectedBody = hit.rigidbody;
                    leftConnection.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    leftConnection.distance = Vector2.Distance(hit.point, leftClaw.transform.position);
                    leftRope.SetFloat(_Length, leftConnection.distance * 1.001f);
                    leftConnection.enabled = true;
                    Debug.Log(hit.rigidbody);
                }
            } else {
                Debug.Log("Value is null");
            } 
        } else if (context.canceled && leftConnection.connectedBody != null) {
            leftConnection.enabled = false;
            leftConnection.connectedBody = null;
            leftRope.SetFloat(_Length, 0.0f);
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
