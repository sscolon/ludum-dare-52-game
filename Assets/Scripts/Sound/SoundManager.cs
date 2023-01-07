using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Mechanizer
{
    public class SoundManager : MonoBehaviour
    {
        private List<AudioSource> _sources = new List<AudioSource>();
        private AudioSource _lastMusicBacking;
        private AudioSource _lastMusic
        {
            get
            {
                if (_lastMusicBacking == null)
                    _lastMusicBacking = gameObject.AddComponent<AudioSource>();
                return _lastMusicBacking;
            }
        }
        private AudioSource _activeMusicBacking;
        private AudioSource _activeMusic
        {
            get
            {
                if (_activeMusicBacking == null)
                {
                    _activeMusicBacking = gameObject.AddComponent<AudioSource>();
                    _activeMusic.outputAudioMixerGroup = musicGroup;
                    _activeMusicBacking.loop = true;
                }

                return _activeMusicBacking;
            }
        }

        [SerializeField] private float _volumeLossPerDistance = 0.025f;
        [SerializeField] private float _maxPitchVariation = 0.05f;
        [SerializeField] private float _musicTransitionSpeed = 10f;
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup musicGroup;
        private float _speedBacking = 1f;
        public float Speed
        {
            get
            {
                return _speedBacking;
            }
            set
            {
                _speedBacking = value;
                for (int i = 0; i < _sources.Count; i++)
                {
                    var source = _sources[i];
                    source.pitch = value;
                }
                _lastMusic.pitch = value;
                _activeMusic.pitch = value;
            }
        }

        private static SoundManager _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                if (transform.parent != null)
                {
                    transform.SetParent(null);
                }

                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

 
        private float CalculateVolume(int v)
        {
            float f = v;
            f = Mathf.Clamp(f, 0.001f, 10f);
            float normalizedValue = (f / 10);
            float volume = Mathf.Log10(normalizedValue) * 20;
            return volume;
        }

        private void UpdateConfig()
        {
            //Update Master Volume;
           // masterMixer.SetFloat("Volume", CalculateVolume(PlayConfig.Current.MasterVolume));
           // masterMixer.SetFloat("SoundVolume", CalculateVolume(PlayConfig.Current.SoundVolume));
           // masterMixer.SetFloat("MusicVolume", CalculateVolume(PlayConfig.Current.MusicVolume));
        }

        private AudioSource FindAudioSource(AudioClip clip)
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var source = _sources[i];
                if (source.clip == clip)
                    return source;
            }

            for (int i = 0; i < _sources.Count; i++)
            {
                var source = _sources[i];
                if (!source.isPlaying)
                    return source;
            }

            return null;
        }

        private AudioSource RequireAudioSource(AudioClip clip)
        {
            var source = FindAudioSource(clip);
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                _sources.Add(source);
            }

            return source;
        }

        public void Play(SoundSettings settings)
        {
            AudioSource source = null;
            switch (settings.type)
            {
                default:
                case AudioType.Sound:
                    source = RequireAudioSource(settings.clip);
                    source.outputAudioMixerGroup = sfxGroup;
                    source.clip = settings.clip;
                    break;
                case AudioType.Music:
                    source = _activeMusic;
                    source.pitch = Speed;
                    StartCoroutine(Transition());
                    IEnumerator Transition()
                    {
                        AudioClip lastClip = _lastMusic.clip;
                        float lastTime = _lastMusic.time;
                        _lastMusic.clip = source.clip;
                        _lastMusic.time = source.time;
                        float time = 0.0f;
                        if (source.isPlaying)
                        {
                            float startVolume = source.volume;
                            while (time < 1.0f)
                            {
                                time += Time.deltaTime * _musicTransitionSpeed;
                                source.volume = Mathf.Lerp(startVolume, 0, time);
                                yield return null;
                            }
                        }

                        time = 0.0f;
                        source.clip = settings.clip;
                        if (lastClip == settings.clip)
                        {
                            source.clip = lastClip;
                            source.time = lastTime;
                        }
                        else
                        {
                            source.time = 0f;
                        }

                        source.volume = 0f;
                        source.Play();
                        while (time < 1.0f)
                        {
                            time += Time.deltaTime * _musicTransitionSpeed;
                            source.volume = Mathf.Lerp(0, 1f, time);
                            yield return null;
                        }


                    }
                    return;
                case AudioType.Ambient:
                    source = RequireAudioSource(settings.clip);
                    source.clip = settings.clip;
                    break;
            }

            if (settings.useVolumeLoss)
            {
                if (Helpers.MainCamera != null)
                {
                    Vector3 position = Helpers.MainCamera.transform.position;
                    position.z = 0.0f;
                    Transform transform = settings.transform;
                    float distance = transform == null ? 0f : Vector2.Distance(transform.position, position);
                    float volume = 1f - distance * _volumeLossPerDistance;
                    source.volume = Mathf.Clamp(volume, 0, 1f);
                }
                else
                {
                    source.volume = 1f;
                }
            }
            else
            {
                source.volume = 1f;
            }

            source.pitch = settings.usePitchVariation ? UnityEngine.Random.Range(-_maxPitchVariation, _maxPitchVariation) + Speed : Speed;
            source.loop = settings.isLooping;
            source.time = 0f;
            source.Play();
        }

        public void Stop(SoundSettings settings)
        {
            AudioSource source;
            switch (settings.type)
            {
                case AudioType.Sound:
                case AudioType.Ambient:
                    source = FindAudioSource(settings.clip);
                    if (source != null)
                        source.Stop();
                    break;
                case AudioType.Music:
                    source = _activeMusic;
                    if (_activeMusic.isPlaying)
                    {
                        StartCoroutine(Transition());
                        IEnumerator Transition()
                        {
                            float time = 0.0f;
                            float startVolume = source.volume;
                            while (time < 1.0f)
                            {
                                time += Time.deltaTime;
                                source.volume = Mathf.Lerp(startVolume, 0, time);
                                yield return null;
                            }
                            source.Stop();
                        }
                    }
                    break;
            }
        }

        private void UpdateSpeed(float speed)
        {
            Speed = speed;
        }

        public static AudioClip GetActiveMusicClip()
        {
            if (_instance == null)
                return null;
            return _instance._activeMusic.clip;
        }

        public static void PlaySound(SoundSettings settings)
        {
            if (_instance == null)
                return;
            _instance.Play(settings);
        }

        public static void PlaySound(AudioClip clip)
        {
            var settings = SoundSettings.CreateDefaultSound(clip);
            PlaySound(settings);
        }

        public static void PlayTrueSound(AudioClip clip)
        {
            var settings = SoundSettings.CreateTrueSound(clip);
            PlaySound(settings);
        }

        public static void PlayMainMusic(AudioClip clip)
        {
            var settings = SoundSettings.CreateMainMusic(clip);
            PlaySound(settings);
        }

        public static void PlayLastMusic()
        {
            var settings = SoundSettings.CreateMainMusic(_instance._lastMusic.clip);
            PlaySound(settings);
        }

        public static void PlayAmbientSound(AudioClip clip)
        {
            var settings = SoundSettings.CreateAmbientSound(clip);
            PlaySound(settings);
        }

        public static void StopSound(SoundSettings settings)
        {
            if (_instance == null)
                return;
            _instance.Stop(settings);
        }
    }
}