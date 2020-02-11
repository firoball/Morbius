using UnityEngine;
using UnityEngine.UI;
using System;
using Morbius.Scripts.Game;
using Morbius.Scripts.Level;

namespace Morbius.Scripts.UI
{
    [RequireComponent(typeof(ScenePortal))]
    [RequireComponent(typeof(UIFader))]
    public class LoadGameUI : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_defaultSprite;
        [SerializeField]
        private Image[] m_slotImage;

        private UIFader m_fader;
        private ScenePortal m_portal;

        private void Awake()
        {
            m_fader = GetComponent<UIFader>();
            m_portal = GetComponent<ScenePortal>();
        }

        public void OnLoad(int slot)
        {
            if ((m_slotImage != null) && (slot >= 0) && (slot < m_slotImage.Length))
            {
                PortalInfo.Identifier = "";
                GameStatus.LoadGame(slot);
                m_fader.Hide(false);
                if (!string.IsNullOrWhiteSpace(GameStatus.Data.Scene))
                {
                    m_portal.Load(GameStatus.Data.Scene);
                }
                else
                {
                    m_portal.Load();
                }
            }
        }

        public void OnDelete(int slot)
        {
            if ((m_slotImage != null) && (slot >= 0) && (slot < m_slotImage.Length))
            {
                Destroy(m_slotImage[slot].sprite); //may not be required here
                m_slotImage[slot].sprite = m_defaultSprite;
                GameStatus.DeleteGame(slot);
            }
        }

        public void OnMenuOpen()
        {
            UpdateSprites();
            m_fader.Show(false);
        }

        public void OnMenuClose()
        {
            m_fader.Hide(false);
            //todo: open mainmenu
        }

        private void UpdateSprites()
        {
            for (int slot = 0; slot < m_slotImage.Length; slot++)
            {
                Texture2D tex = GameStatus.Data.GetScreenshot(slot);
                Sprite sprite = BuildSprite(tex, m_slotImage[slot]);
                m_slotImage[slot].sprite = (sprite != null)? sprite: m_defaultSprite;
            }
        }

        private Sprite BuildSprite(Texture2D texture, Image target)
        {
            if (!texture || !target) return null;

            Rect baseRect = target.rectTransform.rect;
            float x = (texture.width - baseRect.width) * 0.5f;
            float y = (texture.height - baseRect.height) * 0.5f;
            float w = Mathf.Min(baseRect.width, texture.width);
            float h = Mathf.Min(baseRect.height, texture.height);
            Rect newRect = new Rect(x, y, w, h);

            return Sprite.Create(texture, newRect, target.sprite.pivot);
        }

    }
}
