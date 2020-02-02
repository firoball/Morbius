using UnityEngine;
using System;
using Morbius.Scripts.Game;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(UIFader))]
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField]
        private SliderVisualizerUI m_masterVolumeSlider;
        [SerializeField]
        private SliderVisualizerUI m_musicVolumeSlider;
        [SerializeField]
        private SliderVisualizerUI m_qualitySlider;

        private UIFader m_fader;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
        }

        public void OnSliderChanged(SliderVisualizerUI ui)
        {
            if (!ui) return;

            if (ui == m_masterVolumeSlider)
            {
                GameStatus.Options.MasterVolume = ui.GetValue();
            }
            else if (ui == m_musicVolumeSlider)
            {
                GameStatus.Options.MusicVolume = ui.GetValue();
            }
            else if (ui == m_qualitySlider)
            {
                GameStatus.Options.QualityLevel = ui.GetValue();
            }
            else
            {
            }
            GameStatus.ApplySettings();
        }

        public void OnMenuOpen()
        {
            m_masterVolumeSlider?.SetValue(GameStatus.Options.MasterVolume);
            m_musicVolumeSlider?.SetValue(GameStatus.Options.MusicVolume);
            m_qualitySlider?.SetValue(GameStatus.Options.QualityLevel);
            m_fader.Show(false);
        }

        public void OnMenuClose()
        {
            m_fader.Hide(false);
        }

    }
}
