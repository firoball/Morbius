using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Morbius.Scripts.Ambient
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager s_singleton;

        [SerializeField]
        private List<MusicItem> m_musicList;

        private AudioSource m_musicSource;
        private AudioClip m_currentMusic;
        private float m_musicDefaultVolume;
        private float m_musicTargetVolume;
        private bool m_delayedStop;
        private float m_fadeSpeed;
        private float m_musicFadeDelay;

        private AudioSource m_audioSource;
        private AudioClip m_nextClip;
        private bool m_audioFadeAndStop;
        private float m_audioDefaultVolume;

        private const float c_musicfadeSpeedDefault = 1.0f;
        private const float c_musicfadeDelayTime = 1.0f;
        private const float c_audiofadeSpeedDefault = 2.0f;
        private const float c_musicBackgroundVolumeFac = 0.3f;

        private const float c_pitchLimit = 5.0f;

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                AudioSource[] audioSources = GetComponents<AudioSource>();
                m_audioSource = audioSources[0];
                if (audioSources.Length > 1)
                {
                    m_musicSource = audioSources[1];
                }
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("AudioManager: Multiple instances detected. Destroying...");
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
            if (m_musicSource)
            {
                m_musicDefaultVolume = m_musicSource.volume;
                m_musicTargetVolume = m_musicSource.volume;
                //SetMusicVolume(m_musicDefaultVolume);
                SetMusicVolume(0.0f);
                m_musicSource.loop = true;
                m_delayedStop = false;
                m_fadeSpeed = c_musicfadeSpeedDefault;
                AudioClip music = FindMusicClip(SceneManager.GetActiveScene().name);
                PlayMusic(music);
            }

            m_audioFadeAndStop = false;
            m_nextClip = null;
            m_audioDefaultVolume = m_audioSource.volume;
        }

        private void Update()
        {
            MusicFader();
            AudioFader();
        }

        private void MusicFader()
        {
            float targetVolume = m_musicTargetVolume;
            if (m_audioSource.isPlaying)
            {
                m_musicFadeDelay = c_musicfadeDelayTime;
                targetVolume *= c_musicBackgroundVolumeFac;
            }
            else if (m_musicFadeDelay > 0.0f)
            {
                m_musicFadeDelay = Mathf.Max(m_musicFadeDelay - Time.deltaTime, 0.0f);
                targetVolume *= c_musicBackgroundVolumeFac;
            }

            if (m_musicSource)
            {
                if (targetVolume < m_musicSource.volume)
                {
                    m_musicSource.volume = Mathf.Max(m_musicSource.volume - m_fadeSpeed * Time.deltaTime, targetVolume);
                    if (m_delayedStop && targetVolume >= m_musicSource.volume)
                    {
                        StopMusic();
                    }

                }
                if (targetVolume > m_musicSource.volume)
                {
                    m_musicSource.volume = Mathf.Min(m_musicSource.volume + m_fadeSpeed * Time.deltaTime, targetVolume);
                }
            }
        }

        private void AudioFader()
        {
            if (m_audioFadeAndStop && m_audioSource.volume > 0.0f)
            {
                m_audioSource.volume = Mathf.Max(m_audioSource.volume - c_audiofadeSpeedDefault * Time.deltaTime, 0.0f);
                if (m_audioSource.volume <= 0.0f)
                {
                    m_audioSource.Stop();
                    m_audioSource.volume = m_audioDefaultVolume;
                    m_audioFadeAndStop = false;
                    if (m_nextClip && !m_delayedStop)
                    {
                        PlayAudio(m_nextClip);
                    }
                }
            }
        }

        private void SetMusicVolume(float volume)
        {
            m_musicTargetVolume = volume;
            m_musicSource.volume = volume;
        }

        private AudioClip FindMusicClip(string sceneName)
        {
            MusicItem item = m_musicList.Find(x => x.SceneName == sceneName);
            if (item != null)
            {
                return item.Music;
            }
            else
            {
                return null;
            }
        }

        private void PlayMusic(AudioClip music)
        {
            m_delayedStop = false;
            m_fadeSpeed = c_musicfadeSpeedDefault;
            FadeMusicInternal(m_musicDefaultVolume);
            if (music != m_currentMusic && m_musicSource)
            {
                m_musicSource.clip = music;
                m_musicSource.Play();
                m_currentMusic = music;
            }

        }

        private void StopMusic()
        {
            if (m_musicSource)
            {
                m_musicSource.Stop();
                m_musicSource.clip = null;
                m_currentMusic = null;
                m_delayedStop = false;
            }
        }

        private void PlayAudio(AudioClip clip)
        {
            m_audioSource.PlayOneShot(clip);
            m_nextClip = null;
            m_audioFadeAndStop = false;
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            AudioClip music = FindMusicClip(scene.name);
            if (music != null)
                PlayMusic(music);
        }

        private void FadeMusicInternal(float volume)
        {
            m_musicTargetVolume = Mathf.Clamp01(volume);
        }

        public static void ScheduleVoice(AudioClip clip)
        {
            if (s_singleton != null && clip != null)
            {
                if (s_singleton.m_audioSource.isPlaying)
                {
                    s_singleton.m_nextClip = clip;
                    s_singleton.m_audioFadeAndStop = true;
                }
                else
                {
                    s_singleton.PlayAudio(clip);
                }
            }
        }

        public static void StopAudio()
        {
            if (s_singleton != null)
            {
                s_singleton.m_audioFadeAndStop = true;
            }
        }

        public static void FadeAndStop(float fadeSpeed)
        {
            if (s_singleton != null)
            {
                s_singleton.m_fadeSpeed = Mathf.Max(0.0f, fadeSpeed);
                s_singleton.m_delayedStop = true;
                s_singleton.FadeMusicInternal(0.0f);
                s_singleton.m_audioFadeAndStop = true;
            }
        }

        public static void PitchMusic(float factor)
        {
            if (s_singleton != null && s_singleton.m_musicSource)
            {
                factor = Mathf.Max(-c_pitchLimit, Mathf.Min(factor, c_pitchLimit));
                s_singleton.m_musicSource.pitch = factor;
            }
        }

        public static void Destroy()
        {
            if (s_singleton != null)
            {
                Destroy(s_singleton.gameObject);
            }
        }

    }
}
