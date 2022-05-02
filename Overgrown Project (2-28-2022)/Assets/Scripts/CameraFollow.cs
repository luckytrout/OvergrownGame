using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 pos, fw, up;

    // Update is called once per frame
    private void Start()
    {
        /*
        pos = target.transform.InverseTransformPoint(transform.position);
        fw = target.transform.InverseTransformDirection(transform.forward);
        up = target.transform.InverseTransformDirection(transform.up);*/
    }

    void FixedUpdate()
    {
        /*Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);*/

        /*var newpos = target.transform.TransformPoint(pos);
        var newfw = target.transform.TransformDirection(fw);
        var newup = target.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;*/

    }
}
