using UnityEngine;
using System;
using System.Collections;
using Morbius.Scripts.Game;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(AudioSource))]
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
        private AudioSource m_audio;


        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_audio = GetComponent<AudioSource>();
        }

        public void OnSliderChanged(SliderVisualizerUI ui)
        {
            bool playSample = false;
            if (!ui || !m_fader || !m_fader.IsEnabled()) return;

            if (ui == m_masterVolumeSlider)
            {
                GameStatus.Options.MasterVolume = ui.GetValue();
                playSample = true;
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

            if (playSample)
            {
                StartCoroutine(PlayVolumeSample());
            }
        }

        private IEnumerator PlayVolumeSample()
        {
            //only play if slider hasn't changed position for some time
            float volume = GameStatus.Options.MasterVolume;
            yield return new WaitForSeconds(0.3f);
            if (volume == GameStatus.Options.MasterVolume)
            {
                m_audio.Play();
            }
            yield return null;
        }
        public void OnMenuOpen()
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
            UpdateSliders();
            m_fader.Show(false);
        }

        public void OnMenuClose()
        {
            m_fader.Hide(false);
            GameStatus.Options.Save();
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
        }

        private void UpdateSliders()
        {
            m_masterVolumeSlider?.SetValue(GameStatus.Options.MasterVolume);
            m_musicVolumeSlider?.SetValue(GameStatus.Options.MusicVolume);
            m_qualitySlider?.SetValue(GameStatus.Options.QualityLevel);
        }
    }
}
