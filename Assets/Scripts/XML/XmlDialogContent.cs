using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlDialogContent
{
    [SerializeField]
    protected string m_id;
    [SerializeField]
    private string m_name;
    [SerializeField]
    private string m_text;
    [SerializeField]
    private string m_file;
    [SerializeField]
    private string m_target;
    [SerializeField]
    private List<XmlDialogChoiceItem> m_choiceItems;

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

    [XmlAttribute("name")]
    public string Name
    {
        get
        {
            return m_name;
        }

        set
        {
            m_name = value;
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

    [XmlAttribute("file")]
    public string File
    {
        get
        {
            return m_file;
        }

        set
        {
            m_file = value;
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

    [XmlElement("ChoiceItem")]
    public List<XmlDialogChoiceItem> ChoiceItems
    {
        get
        {
            return m_choiceItems;
        }

        set
        {
            m_choiceItems = value;
        }
    }

}
