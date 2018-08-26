using System;
using UnityEngine;

namespace Morbius.Scripts.Ambient
{
    [Serializable]
    public class MusicItem
    {
        [SerializeField]
        private int m_sceneId;
        [SerializeField]
        private AudioClip m_music;

        public int SceneId
        {
            get
            {
                return m_sceneId;
            }

            set
            {
                m_sceneId = value;
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
