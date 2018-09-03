using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Morbius.Scripts.UI;

namespace Morbius.Scripts.Cursor
{
    public class CursorManager : MonoBehaviour
    {
        static CursorManager s_singleton;

        [SerializeField]
        private GameObject m_cursorUI;

        private Vector3 m_clickedPosition;
        private GameObject m_clickedObject;
        private Sprite m_icon;

        private void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
            }
            else
            {
                Debug.Log("CursorManager: Multiple instances detected. Destroying...");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            //TODO add event and make PlayerMovement receiver... or move to playermovement including last clicked object
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000))
                {
                    m_clickedPosition = hit.point;
                    m_clickedObject = hit.transform.gameObject;
                }
            }

        }

        public static void UpdateCursorItem(string text, bool collectable, bool inInventory)
        {
            if (s_singleton)
            {
                GameObject target = s_singleton.m_cursorUI;
                //already item in hold, indicate combination
                if (s_singleton.m_icon)
                {
                    ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetCursor(CursorState.COMBINE));
                }
                else
                {
                    //item may either be taken directly or just investigated.
                    //items from inventory are not "taken", show default
                    CursorState state = CursorState.INVESTIGATE;
                    if (collectable)
                    {
                        state = CursorState.GRAB;
                    }
                    if(inInventory)
                    {
                        state = CursorState.DEFAULT;
                    }

                    ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetCursor(state));
                }
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetText(text));
            }
        }

        public static void UpdateCursorText(string text)
        {
            if (s_singleton)
            {
                GameObject target = s_singleton.m_cursorUI;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetText(text));
            }
        }

        public static void UpdateCursorIcon(Sprite sprite)
        {
            if (s_singleton)
            {
                GameObject target = s_singleton.m_cursorUI;
                s_singleton.m_icon = sprite;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetIcon(sprite));
            }
        }

        public static void UpdateCursorState(CursorState state)
        {
            if (s_singleton)
            {
                GameObject target = s_singleton.m_cursorUI;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetCursor(state));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetText(null));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetIcon(null));
            }
        }

        public static void SetDefaultCursor()
        {
            if (s_singleton)
            {
                GameObject target = s_singleton.m_cursorUI;
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetCursor(CursorState.DEFAULT));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetText(null));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(target, null, (x, y) => x.OnSetIcon(s_singleton.m_icon));
            }
        }

        public static GameObject LastClickedObject()
        {
            if (s_singleton)
            {
                return s_singleton.m_clickedObject;
            }
            else
            {
                return null;
            }
        }

        public static Vector3 LastClickedPosition()
        {
            if (s_singleton)
            {
                return s_singleton.m_clickedPosition;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}
