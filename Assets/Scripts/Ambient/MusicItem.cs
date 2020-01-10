using System;
using UnityEngine;

namespace Morbius.Scripts.Ambient
{
    [Serializable]
    public class MusicItem
    {
        [SerializeField]
        private string m_sceneName;
        [SerializeField]
        private AudioClip m_music;

        public string SceneName
        {
            get
            {
                return m_sceneName;
            }

            set
            {
                m_sceneName = value;
            }
        }

        public AudioClip Music
        {
            get
            {
                return m_music;
            }

            set
            {
                m_music = value;
            }
        }
    }
}
