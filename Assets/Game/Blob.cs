using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public Rigidbody2D rb;
    public float rotateSpeed;
    public float maxRotateSpeed;
    public PolygonCollider2D polygonCollider;
    public float stretchSpeed;
    public float maxStretch;
    public float legForce = 100;
    public List<Leg> legs;
    public GameObject blobby;
    public GameObject jointPointPrefab;

    [HideInInspector]
    public List<GameObject> jointPoints;

    Vector2[] startPoints;
    Vector3 blobbyScale;

    private void Awake()
    {
        // legs = new List<Leg>();
    }

    private void Start()
    {
        startPoints = polygonCollider.GetPath(0);
        blobbyScale = blobby.transform.localScale;

        // for (int i = 0; i < startPoints.Length; i++)
        // {
        //     jointPoints.Add(Instantiate(jointPointPrefab, transform.TransformPoint(startPoints[i]), Quaternion.identity));
        // }
    }

    // private void FixedUpdate()
    // {
    //     var path = polygonCollider.GetPath(0);
    //     for (int i = 0; i < path.Length; i++)
    //     {
    //         var pos = transform.TransformPoint(path[i]);
    //         var rot = transform.TransformVector(path[i]);
    //         // jointPoints[i].transform.rotation = Quaternion.LookRotation(rot, jointPoints[i].transform.up);
    //         // jointPoints[i].transform.up = rot;
    //         // jointPoints[i].transform.Rotate()
    //         // jointPoints[i].GetComponent<Rigidbody2D>().MoveRotation(rot);
    //         jointPoints[i].GetComponent<Rigidbody2D>().MovePosition(pos);
    //         // jointPoints[i].transform.position = jointPoints[i].transform.position;
    //     }
    // }

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
            // if (Input.GetAxis("Horizontal") != 0)
            // {
            //     var motor = leg.motor;
            //     motor.motorSpeed = Mathf.Abs(motor.motorSpeed) * Mathf.Sign(Input.GetAxis("Horizontal"));
            //     leg.motor = motor;
            // }
            // leg.useMotor = jump;
            leg.spring.distance = jump ? 0.2f : 1.8f;
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     rb.AddForce(Vector2.up * 100, ForceMode2D.Impulse);
        // }

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
