using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class XmlCombinationLink : XmlCombinationElement
{
    [SerializeField]
    private string m_id1;
    [SerializeField]
    private string m_id2;
    [SerializeField]
    private string m_morphTarget;
    [SerializeField]
    private string m_result;

    [XmlAttribute("id1")]
    public string Id1
    {
        get
        {
            return m_id1;
        }

        set
        {
            m_id1 = value;
        }
    }

    [XmlAttribute("id2")]
    public string Id2
    {
        get
        {
            return m_id2;
        }

        set
        {
            m_id2 = value;
        }
    }

    [XmlAttribute("morphtarget")]
    public string MorphTarget
    {
        get
        {
            return m_morphTarget;
        }

        set
        {
            m_morphTarget = value;
        }
    }

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
}
