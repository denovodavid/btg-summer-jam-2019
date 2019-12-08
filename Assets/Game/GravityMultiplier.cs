using UnityEngine;

public class GravityMultiplier : MonoBehaviour
{
    public float Multiplier = 3f;
    public float FallMultiplier = 2f;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        var velocity = rb.velocity;

        // don't add force so the object can sleep
        if (0.5f * rb.mass * velocity.sqrMagnitude < Physics.sleepThreshold)
        {
            return;
        }

        var gravity = Physics.gravity;

        // add extra gravity
        var force = gravity * (Multiplier - 1);

        // is the object falling with respect to gravity
        var gravitationalVelocity = Vector3.Project(velocity, gravity);
        if (gravitationalVelocity.y < 0)
        {
            force += gravity * (FallMultiplier - 1);
        }

        rb.AddForce(force * rb.mass, ForceMode2D.Force);
    }
}
