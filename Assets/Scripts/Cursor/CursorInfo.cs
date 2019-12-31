using UnityEngine;

namespace Morbius.Scripts.Cursor
{
    public struct CursorInfo
    {
        bool m_isPortal;
        bool m_isInteractable;
        bool m_isGrabable;
        bool m_isDialog;
        Sprite m_icon;
        string m_label;

        public static bool operator == (CursorInfo info1, CursorInfo info2)
        {
            return (
                info1.IsPortal == info2.IsPortal &&
                info1.IsInteractable == info2.IsInteractable &&
                info1.IsGrabable == info2.IsGrabable &&
                info1.IsDialog == info2.IsDialog &&
                info1.Icon == info2.Icon &&
                info1.Label == info2.Label
                );
        }

        public static bool operator != (CursorInfo info1, CursorInfo info2)
        {
            return !(info1 == info2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(CursorInfo))
                return false;

            return (this == (CursorInfo)obj);
        }

        public override int GetHashCode()
        {
            return m_isPortal.GetHashCode() ^ 
                m_isInteractable.GetHashCode() ^ 
                m_isGrabable.GetHashCode() ^
                m_isDialog.GetHashCode() ^
                m_icon.GetHashCode() ^ 
                m_label.GetHashCode();
        }

        public bool IsPortal
        {
            get
            {
                return m_isPortal;
            }

            set
            {
                m_isPortal = value;
            }
        }

        public bool IsInteractable
        {
            get
            {
                return m_isInteractable;
            }

            set
            {
                m_isInteractable = value;
            }
        }

        public bool IsGrabable
        {
            get
            {
                return m_isGrabable;
            }

            set
            {
                m_isGrabable = value;
            }
        }

        public bool IsDialog
        {
            get
            {
                return m_isDialog;
            }

            set
            {
                m_isDialog = value;
            }
        }

        public Sprite Icon
        {
            get
            {
                return m_icon;
            }

            set
            {
                m_icon = value;
            }
        }

        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                m_label = value;
            }
        }
    }
}
