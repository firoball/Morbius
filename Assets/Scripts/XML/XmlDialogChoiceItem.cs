using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlDialogChoiceItem
{
    [SerializeField]
    private string m_returnvalue;
    [SerializeField]
    private string m_target;
    [SerializeField]
    private string m_text;

    [XmlAttribute("returnvalue")]
    public string Returnvalue
    {
        get
        {
            return m_returnvalue;
        }

        set
        {
            m_returnvalue = value;
        }
    }

    [XmlAttribute("target")]
    public string Target
    {
        get
        {
            return m_target;
        }

        set
        {
            m_target = value;
        }
    }

    [XmlText]
    public string Text
    {
        get
        {
            return m_text;
        }

        set
        {
            m_text = value;
        }
    }


}
