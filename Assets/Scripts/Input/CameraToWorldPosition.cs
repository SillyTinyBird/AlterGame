using UnityEngine;
public class CameraToWorldPosition : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Vector3 position)
    {
        position.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(position);
    }
}
