using System.Collections.Generic;
using UnityEngine;
using utils;

namespace audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] Sound[] _soundsConfig;
        Dictionary<string, Queue<AudioSource>> _sources = new Dictionary<string, Queue<AudioSource>>();

        protected override void Awake()
        {
            base.Awake();

            foreach (var sound in _soundsConfig)
            {
                Queue<AudioSource> queueSources = new Queue<AudioSource>();

                for (int i = 0; i < sound.SourceAmounts; i++)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = sound.AudioClip;
                    audioSource.volume = sound.Volume;
                    audioSource.pitch = sound.Pitch;
                    audioSource.loop = sound.Loop;

                    queueSources.Enqueue(audioSource);
                }

                _sources.Add(sound.Id, queueSources);
            }
        }

        private void Start()
        {
            PlaySound("main music");
        }

        public void PlaySound(string id)
        {
            if (!_sources.ContainsKey(id))
            {
                Debug.LogWarning("Sound not found: " + id);
                return;
            }

            AudioSource audioSource = _sources[id].Dequeue();
            audioSource.Play();
            _sources[id].Enqueue(audioSource);
        }

        public void StopSound(string id)
        {
            if (!_sources.ContainsKey(id))
            {
                Debug.LogWarning("Sound not found: " + id);
                return;
            }

            foreach (var source in _sources[id])
            {
                source.Stop();
            }
        }
    }
}