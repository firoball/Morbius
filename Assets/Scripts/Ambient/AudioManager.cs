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

        private AudioSource m_audioSource;
        private AudioSource m_musicSource;
        private AudioClip m_currentMusic;
        private float m_musicDefaultVolume;
        private float m_musicTargetVolume;
        private bool m_delayedStop;

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
                Destroy(this);
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
                AudioClip music = FindMusicClip(SceneManager.GetActiveScene().buildIndex);
                PlayMusic(music);
            }
        }

        private void Update()
        {
            if (m_musicSource)
            {
                if (m_musicTargetVolume < m_musicSource.volume)
                {
                    m_musicSource.volume = Mathf.Max(m_musicSource.volume - 3 * Time.deltaTime, m_musicTargetVolume);
                    if (m_delayedStop && m_musicTargetVolume >= m_musicSource.volume)
                    {
                        StopMusic();
                    }

                }
                if (m_musicTargetVolume > m_musicSource.volume)
                {
                    m_musicSource.volume = Mathf.Min(m_musicSource.volume + 3 * Time.deltaTime, m_musicTargetVolume);
                }
            }
        }

        private void SetMusicVolume(float volume)
        {
            m_musicTargetVolume = volume;
            m_musicSource.volume = volume;
        }

        private AudioClip FindMusicClip(int sceneId)
        {
            MusicItem item = m_musicList.Find(x => x.SceneId == sceneId);
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
            if (music != m_currentMusic && m_musicSource)
            {
                FadeMusicInternal(m_musicDefaultVolume);
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

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            AudioClip music = FindMusicClip(scene.buildIndex);
            PlayMusic(music);
        }

        private void FadeMusicInternal(float volume)
        {
            m_musicTargetVolume = Mathf.Clamp01(volume);
        }

        //todo: remove?
        public static void Play(AudioClip clip)
        {
            if (s_singleton != null)
            {
                s_singleton.m_audioSource.PlayOneShot(clip);
            }
        }

        public static float ScheduleVoice(AudioClip clip)
        {
            if (s_singleton != null && clip != null)
            {
                //todo fade music volume, then play
                s_singleton.m_audioSource.PlayOneShot(clip);
                return clip.length;
            }
            else {
                return 0;
            }
        }

        public static void FadeAndStopMusic()
        {
            if (s_singleton != null)
            {
                s_singleton.FadeMusicInternal(0.0f);
                s_singleton.m_delayedStop = true;
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
