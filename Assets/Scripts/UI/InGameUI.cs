using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Morbius.Scripts.Game;
using Morbius.Scripts.Level;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(ScenePortal))]
    [RequireComponent(typeof(UIFader))]
    public class InGameUI : MonoBehaviour
    {
        private UIFader m_fader;
        private ScenePortal m_portal;

        [SerializeField]
        private Image m_reference;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_portal = GetComponent<ScenePortal>();
        }

        public void OnMenuOpen()
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
            m_fader.Show(false);
        }

        public void OnMenuClose()
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnUnblock());
            m_fader.Hide(false);
        }

        public void OnQuit()
        {
            MessageSystem.Execute<IInputBlockerMessage>((x, y) => x.OnBlock());
            m_fader.Hide(false);
            StartCoroutine(Delay());
        }

        private void SaveAndQuit()
        {
            if (m_reference)
            {
                Texture2D original = ScreenCapture.CaptureScreenshotAsTexture();
                float newHeight = m_reference.rectTransform.rect.height * 1.2f; //add some factor so UI is cut off
                float factor = newHeight / original.height;
                float newWidth = original.width * factor;
                Texture2D resized = Resize(original, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));

                int slot = GameStatus.Slot;
                GameStatus.Data.SetScreenshot(resized, slot);
                GameStatus.Data.Scene = SceneManager.GetActiveScene().name;
                MessageSystem.Execute<ISaveStatusMessage>((x, y) => x.OnSave());

                GameStatus.SaveGame(slot);
                m_portal.Load();
            }
        }

        private Texture2D Resize(Texture2D texture2D, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(width, height);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();
            return result;
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();
            SaveAndQuit();
            yield return null;
        }
    }
}
