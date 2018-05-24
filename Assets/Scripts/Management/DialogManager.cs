using UnityEngine;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private List<XmlDialog> m_dialogs = new List<XmlDialog>();

    public List<XmlDialog> Dialogs
    {
        get
        {
            return m_dialogs;
        }

        set
        {
            m_dialogs = value;
        }
    }

}
