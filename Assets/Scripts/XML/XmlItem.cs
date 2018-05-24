using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlItem
{
    [SerializeField]
    private string m_id;
    [SerializeField]
    private string m_name;
    [SerializeField]
    private string m_entfile;
    [SerializeField]
    private string m_imgfile;
    [SerializeField]
    private string m_collectable;
    [SerializeField]
    private string m_destroyable;
    [SerializeField]
    private List<XmlItemSequence> m_sequences;


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

    [XmlAttribute("entfile")]
    public string Entfile
    {
        get
        {
            return m_entfile;
        }

        set
        {
            m_entfile = value;
        }
    }

    [XmlAttribute("imgfile")]
    public string Imgfile
    {
        get
        {
            return m_imgfile;
        }

        set
        {
            m_imgfile = value;
        }
    }

    [XmlAttribute("collectable")]
    public string Collectable
    {
        get
        {
            return m_collectable;
        }

        set
        {
            m_collectable = value;
        }
    }

    [XmlAttribute("destroyable")]
    public string Destroyable
    {
        get
        {
            return m_destroyable;
        }

        set
        {
            m_destroyable = value;
        }
    }

    [XmlElement("sequence")]
    public List<XmlItemSequence> Sequences
    {
        get
        {
            return m_sequences;
        }

        set
        {
            m_sequences = value;
        }
    }
}
