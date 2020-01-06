using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Morbius.Scripts.Events;

namespace Morbius.Scripts.UI
{
    public class DialogDecisionHandler : MonoBehaviour, IButtonEventTarget
    {
        [SerializeField]
        private GameObject m_decisionPrefab;
        private List<GameObject> m_decisions;
        private DialogUI m_parentUI;

        private void Start()
        {
            m_decisions = new List<GameObject>();
        }

        private void Awake()
        {
            m_parentUI = GetComponentInParent<DialogUI>();
        }

        public void AddDecision(string text)
        {
            GameObject objDecision = Instantiate(m_decisionPrefab, transform);
            Text txtDecision = objDecision.GetComponentInChildren<Text>();
            if (txtDecision)
            {
                txtDecision.text = text;
                m_decisions.Add(objDecision);
            }
            else
            {
                Debug.LogWarning("AddDecision: Invalid Decision Prefab");
            }
        }

        public void OnButtonNotification(GameObject sender)
        {
            int index = m_decisions.IndexOf(sender);
            if ((index >= 0) && m_parentUI)
            {
                m_parentUI.DecisionNotification(index);
            }
        }

        public void Clear()
        {
            foreach (GameObject obj in m_decisions)
            {
                Destroy(obj);
            }
            m_decisions.Clear();
        }
    }
}
