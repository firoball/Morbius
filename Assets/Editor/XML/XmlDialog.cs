using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Dialog")]
[Serializable]
public class XmlDialog
{
    /*[SerializeField]
    private List<XmlDialogContent> m_elements;

    [XmlElement("Choice")]
    [XmlElement("End")]
    [XmlElement("Goto")]
    [XmlElement("Player")]
    [XmlElement("NPC")]
    public List<XmlDialogContent> Elements
    {
        get
        {
            return m_elements;
        }

        set
        {
            m_elements = value;
        }
    }
    */
    //Unity Prefab Serialization cannot handle Lists with mixed inherited member classes
    //for now use one class to catch all attributes
    [SerializeField]
    private List<XmlDialogElement> m_elements;

    [XmlElement("Choice", Type = typeof(XmlDialogChoice))]
    [XmlElement("End", Type = typeof(XmlDialogElement))]
    [XmlElement("Goto", Type = typeof(XmlDialogGoto))]
    [XmlElement("Player", Type = typeof(XmlDialogText))]
    [XmlElement("NPC", Type = typeof(XmlDialogText))]
    public List<XmlDialogElement> Elements
    {
        get
        {
            return m_elements;
        }

        set
        {
            m_elements = value;
        }
    }
    
}
