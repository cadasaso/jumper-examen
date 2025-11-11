using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;            // Arrastra tu Player aquí

    [Header("Ajustes de seguimiento")]
    [Tooltip("Tiempo de suavizado (delay). 0.15–0.35 va bien.")]
    public float smoothTime = 0.25f;
    public Vector2 offset;              // Desplazamiento (x,y) si quieres centrar un poco arriba/derecha

    [Header("Límites (opcional)")]
    public bool useLimits = false;
    public Vector2 minLimits;
    public Vector2 maxLimits;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z // mantiene la Z de la cámara
        );

        // Movimiento con delay suave
        Vector3 newPos = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);

        // Limitar dentro de un rectángulo, si activado
        if (useLimits)
        {
            newPos.x = Mathf.Clamp(newPos.x, minLimits.x, maxLimits.x);
            newPos.y = Mathf.Clamp(newPos.y, minLimits.y, maxLimits.y);
        }

        transform.position = newPos;
    }
}