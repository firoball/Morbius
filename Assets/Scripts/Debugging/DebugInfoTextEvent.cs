using Morbius.Scripts.UI;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Morbius.Scripts.Debugging
{
    public class DebugInfoTextEvent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_infoTarget;

        private string m_infoText;

        private void Awake()
        {
            m_infoText = "Was haben wir hier? Honorar für Personenschutz - das muss wohl von der Person sein, die Emdayelle beschützen sollte - \n" +
                            "und...nein, das kann nicht sein... was hast DU mit IHM zu tun?\n" +
                            "Warum bist du in dieser Sache involviert ? !";

        }
        private void OnGUI()
        {
            m_infoText = GUI.TextArea(new Rect(235, 400, 205, 120), m_infoText);

            if (GUI.Button(new Rect(235, 530, 100, 30),"Show Info"))
            {
                if (!string.IsNullOrEmpty(m_infoText))
                {
                    ExecuteEvents.Execute<IInfoTextEventTarget>(m_infoTarget, null, (x, y) => x.OnShow(m_infoText));
                }
            }

            if (GUI.Button(new Rect(340, 530, 100, 30), "Hide Info"))
            {
                ExecuteEvents.Execute<IInfoTextEventTarget>(m_infoTarget, null, (x, y) => x.OnHide());
            }

        }
    }
}
