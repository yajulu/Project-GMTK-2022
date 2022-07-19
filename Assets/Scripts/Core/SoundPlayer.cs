using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private bool playOnEnable;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool isMusic;
        [SerializeField] private AudioClip clip;
        [SerializeField, PropertyRange(0, 1)] private float volume;

        private void Awake()
        {
            if (playOnAwake)
                Play();
        }

        private void OnEnable()
        {
            if (playOnEnable)
                Play();
        }

        [Button]
        public void Play()
        {
            if (isMusic)
                SoundManager.Instance.PlayMusic(clip, volume);
            else
                SoundManager.Instance.PlayEffect(clip, volume);
        }
    }
}
