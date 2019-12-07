using UnityEngine;

public class Blob : MonoBehaviour
{
    public Rigidbody2D rb;
    public float rotateSpeed;
    public float maxRotateSpeed;
    public PolygonCollider2D polygonCollider;
    public float stretchSpeed;
    public float maxStretch;
    public HingeJoint2D[] legs;
    public GameObject blobby;

    Vector2[] startPoints;
    Vector3 blobbyScale;

    private void Start()
    {
        startPoints = polygonCollider.GetPath(0);
        blobbyScale = blobby.transform.localScale;
    }

    private void Update()
    {
        Stretch(Input.GetAxis("Vertical"));

        var torque = -Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        rb.AddTorque(torque, ForceMode2D.Impulse);
        if (Mathf.Abs(rb.angularVelocity) > maxRotateSpeed)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxRotateSpeed;
        }

        var jump = Input.GetKey(KeyCode.Space);
        foreach (var leg in legs)
        {
            leg.useMotor = jump;
        }

        var scale = polygonCollider.bounds.size;
        var ratioX = scale.x / scale.y;
        var ratioY = scale.y / scale.x;
        scale.x = blobbyScale.x * ratioX;
        scale.y = blobbyScale.y * ratioY;
        blobby.transform.localScale = scale;
        blobby.transform.position = transform.position;
    }

    private void Stretch(float direction)
    {
        var center = polygonCollider.bounds.center;
        var path = polygonCollider.GetPath(0);
        for (int i = 0; i < path.Length; ++i)
        {
            var point = transform.TransformPoint(path[i]) - center;

            point.x = point.x + stretchSpeed * Mathf.Sign(point.x) * -direction;
            point.y = point.y + stretchSpeed * Mathf.Sign(point.y) * direction;

            point = transform.InverseTransformPoint(point + center);

            var startPoint = startPoints[i];
            var translation = (Vector2)point - startPoint;
            translation = Vector2.ClampMagnitude(translation, maxStretch);
            path[i] = startPoint + translation;
        }
        polygonCollider.SetPath(0, path);
    }

    private void Squash()
    {

    }

}
