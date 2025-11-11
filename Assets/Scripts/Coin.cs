using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [Header("Quién puede recoger")]
    [Tooltip("Tag del objeto que puede recoger la moneda")]
    public string playerTag = "Player";

    [Header("Feedback (opcionales)")]
    [Tooltip("Sonido al recoger")]
    public AudioClip pickupSfx;
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;
    [Tooltip("VFX/partículas al recoger")]
    public GameObject pickupVfx;
    [Tooltip("Retraso antes de destruir (para dejar sonar el SFX/VFX)")]
    public float destroyDelay = 0f;

    void Reset()
    {
        // Asegura que el collider sea Trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        name = "Coin";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // SFX
        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        // VFX
        if (pickupVfx != null)
            Instantiate(pickupVfx, transform.position, Quaternion.identity);

        // Destruir moneda
        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    }
}