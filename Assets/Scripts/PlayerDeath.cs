using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [Header("Muerte por caída")]
    public float minYToKill = -20f;   // Cambia según tu nivel

    [Header("General")]
    public float reloadDelay = 0f;    // Retraso antes de recargar (0 = inmediato)
    public string hazardTag = "Hazard";

    private bool isDying = false;

    void Update()
    {
        if (!isDying && transform.position.y < minYToKill)
            Die();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isDying && col.collider.CompareTag(hazardTag))
            Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDying && other.CompareTag(hazardTag))
            Die();
    }

    public void Die()
    {
        if (isDying) return;
        isDying = true;

        if (reloadDelay <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Invoke(nameof(Reload), reloadDelay);
        }
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
