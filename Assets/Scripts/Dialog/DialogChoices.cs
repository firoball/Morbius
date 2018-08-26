using System;
using System.Linq;
using UnityEngine;

namespace Morbius.Scripts.Dialog
{
    public class DialogChoices : DialogElement
    {
        [SerializeField]
        private DialogChoice[] m_choices;

        private int m_selection = 0;

        public DialogChoice[] Choices
        {
            get
            {
                return m_choices;
            }

            set
            {
                m_choices = value;
            }
        }

        public override bool IsChoice()
        {
            return true;
        }

        public override DialogElement Next
        {
            get
            {
                return m_choices[m_selection].Next;
            }

        }

        public void SelectChoice(int index)
        {
            m_selection = Math.Min(m_choices.Length - 1, Math.Max(0, index));
        }

        public string[] GetChoices()
        {
            return m_choices.Select(x => x.Text).ToArray();
        }
    }
}
