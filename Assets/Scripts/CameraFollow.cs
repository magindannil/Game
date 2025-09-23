using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // игрок
    public Vector3 offset = new Vector3(0, 5, -7); // смещение камеры
    public float smoothSpeed = 0.125f; // плавность

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target); // камера всегда смотрит на игрока
    }
}