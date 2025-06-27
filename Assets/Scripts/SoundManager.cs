using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    [SerializeField] private AudioSource sfxObj;
    [Header("Reeling")]
    [SerializeField] AudioClip[] reelingSounds;
    [SerializeField] AudioSource reelingSfxObj;
    [SerializeField] float FADE_TIME_SECONDS;
    [Header("Casting")]
    [SerializeField] AudioClip[] castingSounds;
    [SerializeField] AudioSource castingSfxObj;
    [SerializeField, Range(0, 1)] float castingVolume;



    public void Awake()
    {
        if(instance == null)
            instance = this;
    }


    public void PlaySound(ref AudioSource sfxObj, AudioClip clip, Transform location, float volume, float? length = null)
    {
        if (sfxObj != null)
            return;

        sfxObj = Instantiate(this.sfxObj, location.position, Quaternion.identity);

        sfxObj.clip = clip;

        sfxObj.volume = volume;

        sfxObj.Play();

        float clipLen = length ?? sfxObj.clip.length;

        Destroy(sfxObj.gameObject, clipLen);
    }

    public void PlayReelingSound(Transform location)
    {
        int index = Random.Range(0, reelingSounds.Length - 1);
        PlaySound(ref reelingSfxObj, reelingSounds[index], location, 1, .2f);
        StartCoroutine(FadeOut(reelingSfxObj, 0f));
    }

    public void PlayCastingSound(Transform location)
    {
        int index = Random.Range(0, castingSounds.Length - 1);
        PlaySound(ref castingSfxObj, castingSounds[index], location, castingVolume);
    }

    IEnumerator FadeOut(AudioSource audioSource, float delay)
    {
        float timeElapsed = 0;

        while (audioSource != null && audioSource.volume > 0)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timeElapsed / FADE_TIME_SECONDS);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
