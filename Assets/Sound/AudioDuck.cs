using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioDuck : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string musicVolumeParameter = "MusicVolume";
    public float duckingAmount = -10f;
    public float duckingDuration = 0.5f;
    public float returnDuration = 0.5f;

    private float originalMusicVolume;
    private Coroutine duckingCoroutine;

    void Start()
    {
        audioMixer.GetFloat(musicVolumeParameter, out originalMusicVolume);
    }

    public void DuckAudio()
    {
        if (duckingCoroutine != null)
        {
            StopCoroutine(duckingCoroutine);
        }
        duckingCoroutine = StartCoroutine(DuckAndReturn());
    }

    private IEnumerator DuckAndReturn()
    {
        float currentVolume;
        audioMixer.GetFloat(musicVolumeParameter, out currentVolume);
        float targetVolume = originalMusicVolume + duckingAmount;

        // Плавное уменьшение громкости
        for (float t = 0; t < duckingDuration; t += Time.deltaTime)
        {
            float newVolume = Mathf.Lerp(currentVolume, targetVolume, t / duckingDuration);
            audioMixer.SetFloat(musicVolumeParameter, newVolume);
            yield return null;
        }
        audioMixer.SetFloat(musicVolumeParameter, targetVolume);

        // Ожидание перед возвратом громкости
        yield return new WaitForSeconds(duckingDuration);

        // Плавное возвращение громкости
        for (float t = 0; t < returnDuration; t += Time.deltaTime)
        {
            float newVolume = Mathf.Lerp(targetVolume, originalMusicVolume, t / returnDuration);
            audioMixer.SetFloat(musicVolumeParameter, newVolume);
            yield return null;
        }
        audioMixer.SetFloat(musicVolumeParameter, originalMusicVolume);
    }
}
