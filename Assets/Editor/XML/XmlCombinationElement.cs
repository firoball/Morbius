using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlCombinationElement
{
    [SerializeField]
    protected string m_description;
    [SerializeField]
    protected string m_sound;

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
}
