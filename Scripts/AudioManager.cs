
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    public void PlaySound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == soundName)
            {
                s.source.PlayOneShot(s.clip, s.volume);
                return;
            }
        }

        Debug.LogWarning("Sound олдсонгүй: " + soundName);
    }
}
