using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [Header("Feedback (opcionales)")]
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField, Range(0f,1f)] private float sfxVolume = 0.8f;
    [SerializeField] private GameObject pickupVfx; // Partículas opcionales

    void Reset()
    {
        // Asegura Trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Puntaje (si tienes GameManager)
        GameManager.I?.AddScore(1);

        // 2) Feedback visual/sonoro (opcionales)
        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);
        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);

        // 3) Flash en el personaje (si usas el script PickupFlash que te pasé)
        other.GetComponentInChildren<PickupFlash>()?.Flash();

        // 4) Destruir la moneda
        Destroy(gameObject);
    }
}
