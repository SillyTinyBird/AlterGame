using UnityEngine;

public class CameraToWorldPosition : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera cam, Vector3 position)
    {
        position.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(position);
    }
}
