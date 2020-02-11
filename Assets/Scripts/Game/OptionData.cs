using UnityEngine;

namespace Morbius.Scripts.Game
{
    public class OptionData
    {
        private float m_masterVolume;
        private float m_musicVolume;
        private float m_qualityLevel;

        public float MasterVolume { get => m_masterVolume; set => m_masterVolume = value; }
        public float MusicVolume { get => m_musicVolume; set => m_musicVolume = value; }
        public float QualityLevel { get => m_qualityLevel; set => m_qualityLevel = value; }

        public OptionData()
        {
            m_masterVolume = 0.5f;
            m_musicVolume = 0.5f;
            m_qualityLevel = 2.0f;
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("Morbius.OD.EV", m_masterVolume);
            PlayerPrefs.SetFloat("Morbius.OD.MV", m_musicVolume);
            PlayerPrefs.SetFloat("Morbius.OD.QL", m_qualityLevel);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            m_masterVolume = PlayerPrefs.GetFloat("Morbius.OD.EV", m_masterVolume);
            m_musicVolume = PlayerPrefs.GetFloat("Morbius.OD.MV", m_musicVolume);
            m_qualityLevel = PlayerPrefs.GetFloat("Morbius.OD.QL", m_qualityLevel);
        }
    }

}
