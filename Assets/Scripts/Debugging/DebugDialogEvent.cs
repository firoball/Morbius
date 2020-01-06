using Morbius.Scripts.Dialog;
using Morbius.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morbius.Scripts.Debugging
{
    public class DebugDialogEvent : MonoBehaviour, IDialogResultTarget
    {
        [SerializeField]
        private GameObject m_dialogTarget;
        [SerializeField]
        private GameObject m_inventoryTarget;
        [SerializeField]
        private DialogPlayer m_dialogPlayer;

        private float m_dialogIndex;
        private float m_dialogStep;
        private int m_dialogIndexI;
        private int m_dialogStepI;

        private void OnGUI()
        {
            if (m_dialogPlayer)
            {
                /*float max;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;

                //pick dialog
                max = 0;// Convert.ToSingle(m_manager.Dialogs.Count - 1);
                m_dialogIndex = GUI.HorizontalSlider(new Rect(25, 450, 170, 30), m_dialogIndex, 0, max);
                m_dialogIndexI = Convert.ToInt32(m_dialogIndex);
                GUI.Label(new Rect(25, 430, 50, 50), "Dialog Id", style);
                GUI.Label(new Rect(200, 450, 30, 50), m_dialogIndexI.ToString(), style);

                //pick dialog element
                max = 0;// Convert.ToSingle(m_manager.Dialogs[m_dialogIndexI].Elements.Count - 1);
                m_dialogStep = Mathf.Min(max, m_dialogStep);
                m_dialogStep = GUI.HorizontalSlider(new Rect(25, 500, 170, 30), m_dialogStep, 0, max);
                m_dialogStepI = Convert.ToInt32(m_dialogStep);
                GUI.Label(new Rect(25, 480, 50, 50), "Dialog Element", style);
                GUI.Label(new Rect(200, 500, 30, 50), m_dialogStepI.ToString(), style);
                */
                if (GUI.Button(new Rect(25, 530, 100, 30),"Show Dialog"))
                {
                    m_dialogPlayer.Play();
                    //TODO: fix me later to use gameobject oriented approach
                    /*XmlDialogContent content = m_manager.Dialogs[m_dialogIndexI].Elements[m_dialogStepI];
                    //decision
                    if (content.ChoiceItems.Count > 0)
                    {
                        string[] decisions = content.ChoiceItems.Select(x => x.Text).ToArray();
                        ExecuteEvents.Execute<IDialogEventTarget>(m_dialogTarget, null, (x, y) => x.OnShowDecision(gameObject, decisions));
                    }
                    //just text
                    else if (!string.IsNullOrEmpty(content.Name) && !string.IsNullOrEmpty(content.Text))
                    {
                        ExecuteEvents.Execute<IDialogEventTarget>(m_dialogTarget, null, (x, y) => x.OnShowText(content.Name, content.Text));
                    }
                    //something else
                    else
                    {
                        EditorUtility.DisplayDialog("DebugDialogEvent", "Selected Element does not contain data", "Ok");
                    }*/
                    ExecuteEvents.Execute<IInventoryEventTarget>(m_inventoryTarget, null, (x, y) => x.OnHide());
                }

                if (GUI.Button(new Rect(130, 530, 100, 30), "Hide Dialog"))
                {
                    ExecuteEvents.Execute<IDialogEventTarget>(m_dialogTarget, null, (x, y) => x.OnHide());
                    ExecuteEvents.Execute<IInventoryEventTarget>(m_inventoryTarget, null, (x, y) => x.OnShow());
                }
            }

        }

        public void OnDecision(int index)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("DebugDialogEvent", "Decision index: " + index, "Ok");
#endif
        }
    }
}
