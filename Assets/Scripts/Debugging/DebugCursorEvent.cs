using Morbius.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Debugging
{
    public class DebugCursorEvent : MonoBehaviour
    {
        private int m_cursorSelection = 0;
        private int m_itemSelection = 0;
        private string[] m_cursorTypes;
        private string m_cursorText;
        private Texture2D[] m_textures;

        [SerializeField]
        private GameObject m_cursorTarget;
        [SerializeField]
        private GameObject m_itemTarget;
        [SerializeField]
        private Sprite[] m_items;

        private void Awake()
        {
            m_cursorTypes = Enum.GetNames(typeof(CursorState))
                .Where(x => x != CursorState.STATES.ToString())
                .ToArray();
            m_cursorText = "Hover Text";
            if (m_items != null)
            {
                List<Texture2D> textureList = m_items.Select(x => x.texture).ToList();
                textureList.Add(null);
                m_textures = textureList.ToArray();
            }
        }

        private void OnGUI()
        {
            m_cursorSelection = GUI.SelectionGrid(new Rect(25, 25, 300, 60), m_cursorSelection, m_cursorTypes, 3);
            if (m_textures != null)
            {
                m_itemSelection = GUI.SelectionGrid(new Rect(25, 95, 300, 200), m_itemSelection, m_textures, 6);
            }
            m_cursorText = GUI.TextField(new Rect(25, 300, 300, 30), m_cursorText);
            if (GUI.Button(new Rect(25, 350, 100, 30), "Update Cursor"))
            {
                if (m_items != null)
                {
                    if (m_itemSelection == m_items.Length)
                    {
                        ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetIcon(null));
                    }
                    else
                    {
                        ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetIcon(m_items[m_itemSelection]));
                    }
                }
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetText(m_cursorText));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetCursor((CursorState)m_cursorSelection));
            }
            if (GUI.Button(new Rect(130, 350, 100, 30), "Reset Cursor"))
            {
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetIcon(null));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetText(null));
                ExecuteEvents.Execute<IAnimatedCursorEventTarget>(m_cursorTarget, null, (x, y) => x.OnSetCursor(CursorState.DEFAULT));
            }
            if (GUI.Button(new Rect(235, 350, 100, 30), "Add Item"))
            {
                //temp
                if (m_items != null)
                {
                    if (m_itemSelection == m_items.Length)
                    {
                        ExecuteEvents.Execute<IInventoryEventTarget>(m_itemTarget, null, (x, y) => x.OnAdd(null));
                    }
                    else
                    {
                        ExecuteEvents.Execute<IInventoryEventTarget>(m_itemTarget, null, (x, y) => x.OnAdd(m_items[m_itemSelection]));
                    }
                }
            }
            if (GUI.Button(new Rect(340, 350, 100, 30), "Remove Item"))
            {
                if (m_items != null)
                {
                    if (m_itemSelection == m_items.Length)
                    {
                        ExecuteEvents.Execute<IInventoryEventTarget>(m_itemTarget, null, (x, y) => x.OnRemove(null));
                    }
                    else
                    {
                        ExecuteEvents.Execute<IInventoryEventTarget>(m_itemTarget, null, (x, y) => x.OnRemove(m_items[m_itemSelection]));
                    }
                }
            }
        }
    }
}
