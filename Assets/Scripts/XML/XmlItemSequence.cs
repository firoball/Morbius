using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlItemSequence
{
    [SerializeField]
    private string m_result;
    [SerializeField]
    private string m_sound;
    [SerializeField]
    private string m_description;

    [XmlAttribute("result")]
    public string Result
    {
        get
        {
            return m_result;
        }

        set
        {
            m_result = value;
        }
    }

    [XmlAttribute("sound")]
    public string Sound
    {
        get
        {
            return m_sound;
        }

        set
        {
            m_sound = value;
        }
    }

    [XmlAttribute("description")]
    public string Description
    {
        get
        {
            return m_description;
        }

        set
        {
            m_description = value;
        }
    }

}
