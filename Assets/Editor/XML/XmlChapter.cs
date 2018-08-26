using UnityEngine;
using System.Xml.Serialization;
using System;

[Serializable]
public class XmlChapter
{
    [SerializeField]
    private string m_id;
    [SerializeField]
    private string m_title;
    [SerializeField]
    private string[] m_text = new string[3];

    [XmlAttribute("id")]
    public string Id
    {
        get
        {
            return m_id;
        }

        set
        {
            m_id = value;
        }
    }

    [XmlAttribute("title")]
    public string Title
    {
        get
        {
            return m_title;
        }

        set
        {
            m_title = value;
        }
    }

    [XmlAttribute("line1")]
    public string Line1
    {
        get
        {
            return m_text[0];
        }

        set
        {
            m_text[0] = value;
        }
    }

    [XmlAttribute("line2")]
    public string Line2
    {
        get
        {
            return m_text[1];
        }

        set
        {
            m_text[1] = value;
        }
    }

    [XmlAttribute("line3")]
    public string Line3
    {
        get
        {
            return m_text[2];
        }

        set
        {
            m_text[2] = value;
        }
    }

    public string[] Text
    {
        get
        {
            return m_text;
        }
    }
}
