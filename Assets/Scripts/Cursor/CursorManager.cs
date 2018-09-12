using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Items;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Cursor
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_cursorUI;

        private GameObject m_hoveredObject;
        private bool m_hoveredObjectwasNull;
        private bool m_locked;
        private Sprite m_icon;
        private ICursorObject m_cursorObject;
        private CursorInfo m_cursorInfo;

        private void Awake()
        {
            m_hoveredObject = null;
            m_hoveredObjectwasNull = false;
            m_locked = false;
        }

        private void Update()
        {
            if (!m_locked)
            {
                Raycast();
                //handle hover
                UpdateCursorInfo();
                //handle hand icon 
                UpdateCursorIcon();
            }
        }

        private void Raycast()
        {
            GameObject lastHoveredObject = m_hoveredObject;
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                //pick topmost object
                m_hoveredObject = results[0].gameObject;
            }
            else
            {
                m_hoveredObject = null;
            }

            //initiate hover events, if any
            if (lastHoveredObject != m_hoveredObject || (lastHoveredObject == null && !m_hoveredObjectwasNull))
            {
                if (m_hoveredObject)
                {
                    GetCursorObject(m_hoveredObject);
                }
                else
                {
                    SetDefaultCursor();
                }
            }

            //store null pointer flag in case object is removed next frame.
            m_hoveredObjectwasNull = (m_hoveredObject == null);
        }

        private void UpdateCursorInfo()
        {
            //cyclically update cursor object
            //this is not perfect, but performance impact should be little and avoids trigger messages

            if (m_cursorObject != null)
            {
                CursorInfo info = m_cursorObject.GetCursorInfo();
                if (info != m_cursorInfo)
                {
                    m_cursorInfo = info;
                    UpdateCursor(info);
                }
            }
            else
            {

            }
        }

        private void UpdateCursorIcon()
        {
            Sprite icon;
            if (Inventory.ItemInHand)
            {
                icon = Inventory.ItemInHand.Icon;
            }
            else
            {
                icon = null;
            }
            if (icon != m_icon)
            {
                m_icon = icon;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetIcon(icon));
            }
        }

        private void GetCursorObject(GameObject target)
        {
            m_cursorObject = target.GetComponentInParent<ICursorObject>();
            if (m_cursorObject == null)
            {
                //object does not carry any cursor info
                SetDefaultCursor();
            }
        }

        private void UpdateCursor(CursorInfo info)
        {
            //Cursor
            CursorState state;
            if (m_icon)
            {
                //already item in hold, indicate combination
                state = CursorState.COMBINE;
            }
            else
            {
                if (info.IsGrabable)
                {
                    state = CursorState.GRAB;
                }
                else if (info.IsInteractable)
                {
                    state = CursorState.INVESTIGATE;
                }
                else if (info.IsPortal)
                {
                    state = CursorState.LEAVE;
                }
                else
                {
                    state = CursorState.DEFAULT;
                }
            }
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetCursor(state));

            //Icon
            if (info.Icon)
            {
                m_icon = info.Icon;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetIcon(info.Icon));
            }

            //Text
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetText(info.Label));
        }

        private void SetDefaultCursor()
        {
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetCursor(CursorState.DEFAULT));
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetText(null));
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetIcon(m_icon));

            m_cursorObject = null;
            m_cursorInfo = new CursorInfo();
        }

        public void OnDialogBegin()
        {
            m_locked = true;
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetCursor(CursorState.TALK));
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetText(null));
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetIcon(null));
        }

        public void OnDialogEnd()
        {
            m_locked = false;
            ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorUI, null, (x, y) => x.OnSetIcon(m_icon));
        }

    }
}
