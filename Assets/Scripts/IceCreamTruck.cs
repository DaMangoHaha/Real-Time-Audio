using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{
    public float speed = 2f;
    public AudioClip iceCreamJingle;
    public AudioClip hornSound;
    public AudioClip explosionSound;
    public GameObject explosionEffectPrefab;

    private AudioSource audioSource;
    private bool hasStopped = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Start the jingle
        audioSource.clip = iceCreamJingle;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (!hasStopped)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree") && !hasStopped)
        {
            hasStopped = true;

            // Stop the jingle
            audioSource.Stop();

            // Play the honk
            audioSource.clip = hornSound;
            audioSource.loop = false;
            audioSource.Play();

            // Start coroutine to explode after honk ends
            StartCoroutine(ExplodeAfterHorn());
        }
    }

    private System.Collections.IEnumerator ExplodeAfterHorn()
    {
        // Wait for the horn to finish
        yield return new WaitForSeconds(audioSource.clip.length);

        // Play explosion sound
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the truck
        Destroy(gameObject);
    }
}
