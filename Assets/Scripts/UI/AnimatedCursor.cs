using UnityEngine;
using UnityEngine.UI;

namespace Morbius.Scripts.UI
{
    public class AnimatedCursor : MonoBehaviour, IAnimatedCursorEventTarget
    {
        private Vector2 m_localPoint;

        [SerializeField]
        private Image m_iconTarget;
        [SerializeField]
        private Animator m_cursorTarget;
        [SerializeField]
        private Text m_textTarget;

        void Awake()
        {
            SetCursorState(CursorState.DEFAULT);
            SetIcon(null);
            SetText(null);
        }

        void Update()
        {
            //Cursor.visible = false;
            Position();
        }

        private void Position()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent.transform, Input.mousePosition, null, out m_localPoint);
            transform.localPosition = new Vector3(m_localPoint.x, m_localPoint.y, 0);
        }

        private void SetCursorState(CursorState state)
        {
            if (m_cursorTarget)
            {
                m_cursorTarget.SetInteger("state", (int)state);
            }
        }

        private void SetIcon(Sprite icon)
        {
            if (m_iconTarget)
            {
                if (icon)
                {
                    m_iconTarget.enabled = true;
                    m_iconTarget.sprite = icon;
                }
                else
                {
                    m_iconTarget.enabled = false;
                }
            }
        }

        private void SetText(string text)
        {
            if (m_textTarget)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    m_textTarget.enabled = true;
                    m_textTarget.text = text;
                }
                else
                {
                    m_textTarget.enabled = false;
                }
            }
        }

        public void OnSetText(string text)
        {
            SetText(text);
        }

        public void OnSetIcon(Sprite icon)
        {
            SetIcon(icon);
        }

        public void OnSetCursor(CursorState cursor)
        {
            SetCursorState(cursor);
        }

    }
}
