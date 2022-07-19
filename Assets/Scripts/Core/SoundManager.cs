using Essentials;
using UnityEngine;

namespace Core
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioSource effectSource;
        [SerializeField] private AudioSource musicSource;

        public void PlayEffect(AudioClip clip, float volume = -1)
        {
            if (volume > 0)
                effectSource.PlayOneShot(clip, volume);
            else 
                effectSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip, float volume = -1)
        {
            musicSource.volume = volume > 0 ? volume : musicSource.volume;
            musicSource.clip = clip;
            musicSource.Play();
        }
        
    }
}
