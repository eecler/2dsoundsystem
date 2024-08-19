using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SmoothVolumeChange : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;
    public float maxDistance = 10f;
    public float idealDistance = 5f;
    public float smoothTime = 0.5f;
    private float targetVolume;
    private float currentVelocity;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, idealDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= maxDistance)
        {
            targetVolume = Mathf.Clamp01(1 - (distance - idealDistance) / (maxDistance - idealDistance));
        }
        else
        {
            targetVolume = 0f;
        }
        audioSource.volume = Mathf.SmoothDamp(audioSource.volume, targetVolume, ref currentVelocity, smoothTime);
    }
}
