
namespace Morbius.Scripts.Game
{
    public class OptionData
    {
        private float m_masterVolume;
        private float m_musicVolume;
        private float m_qualityLevel;
        private bool m_initialized = false;

        public float MasterVolume { get => m_masterVolume; set => m_masterVolume = value; }
        public float MusicVolume { get => m_musicVolume; set => m_musicVolume = value; }
        public float QualityLevel { get => m_qualityLevel; set => m_qualityLevel = value; }

        public void Initialize()
        {
            if (!m_initialized)
            {
                m_initialized = true;
                m_masterVolume = 0.5f;
                m_musicVolume = 0.5f;
                m_qualityLevel = 2.0f;
            }
        }
    }

}
