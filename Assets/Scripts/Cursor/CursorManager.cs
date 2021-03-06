﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.Items;
using Morbius.Scripts.Messages;

namespace Morbius.Scripts.Cursor
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_hoveredObject;
        private bool m_hoveredObjectwasNull;
        private bool m_hoveredObjectWasUI;
        private Sprite m_icon;
        private bool m_lastIconWasNull;
        private bool m_iconHasChanged;
        private ICursorObject m_cursorObject;
        private CursorInfo m_cursorInfo;

        private void Awake()
        {
            m_hoveredObject = null;
            m_hoveredObjectwasNull = false;
            m_hoveredObjectWasUI = false;
            m_lastIconWasNull = false;
            m_iconHasChanged = true;
        }

        private void Update()
        {
            Raycast();
            //handle hand icon 
            UpdateCursorIcon();
            //handle hover
            UpdateCursorInfo();
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
                //Cursor over UI element?
                DetectUI();
            }

            //store null pointer flag in case object is removed next frame.
            m_hoveredObjectwasNull = (m_hoveredObject == null);
        }

        private void DetectUI()
        {
            if (m_hoveredObject)
            {
                //hovering UI object
                if (m_hoveredObject.GetComponent<RectTransform>())
                {
                    //only trigger if not hovering UI object before
                    if (!m_hoveredObjectWasUI)
                    {
                        m_hoveredObjectWasUI = true;
                        MessageSystem.Execute<ICursorUIMessage>((x, y) => x.OnUIEnter());
                    }
                }
                //not hovering UIobject
                else
                {
                    //only trigger if hovering UI object before
                    if (m_hoveredObjectWasUI)
                    {
                        m_hoveredObjectWasUI = false;
                        MessageSystem.Execute<ICursorUIMessage>((x, y) => x.OnUIExit());
                    }
                }
            }
            //hovering no object at all (= pointer offscreen)
            else
            {
                m_hoveredObjectWasUI = true; //hovering no object equals to hovering UI object
                MessageSystem.Execute<ICursorUIMessage>((x, y) => x.OnUIEnter());
            }
        }

        private void UpdateCursorInfo()
        {
            //cyclically update cursor object
            //this is not perfect, but performance impact should be little and avoids trigger messages

            if (m_cursorObject != null)
            {
                CursorInfo info = m_cursorObject.GetCursorInfo();
                if (info != m_cursorInfo || m_iconHasChanged)
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
            if (icon != m_icon || (icon == null && !m_lastIconWasNull))
            {
                m_iconHasChanged = true;
                m_icon = icon;
                MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetIcon(icon));
            }
            else
            {
                m_iconHasChanged = false;
            }
            //store null pointer flag in case icon is removed next frame.
            m_lastIconWasNull = (icon == null);
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
                else if (info.IsDialog)
                {
                    state = CursorState.TALK;
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
            MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetCursor(state));

            //Icon
            if (info.Icon)
            {
                m_icon = info.Icon;
                MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetIcon(info.Icon));
            }

            //Text
            MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetText(info.Label));
        }

        private void SetDefaultCursor()
        {
            MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetCursor(CursorState.DEFAULT));
            MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetText(null));
            MessageSystem.Execute<IAnimatedCursorMessage>((x, y) => x.OnSetIcon(m_icon));

            m_cursorObject = null;
            m_cursorInfo = new CursorInfo();
        }

    }
}
