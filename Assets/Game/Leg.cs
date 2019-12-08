using System.Linq;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public Joint2D hip;
    public HingeJoint2D calf;
    public SpringJoint2D spring;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hip.connectedBody == null)
        {
            hip.connectedBody = other.attachedRigidbody;
            var blob = FindObjectOfType<Blob>();
            if (blob != null)
            {
                blob.legs.Add(this);
            //     // var min  = blob.jointPoints.Min(x => (transform.position - x.transform.position).sqrMagnitude);
            //     var min = blob.jointPoints.Select(x => ((transform.position - x.transform.position).sqrMagnitude, x)).Min().Item2;
            //     transform.position = min.transform.position;
            //     transform.rotation = min.transform.rotation;
            //     hip.connectedBody = min.GetComponent<Rigidbody2D>();
            }
        }
    }
}
