using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("items")]
public class XmlItems
{
    private List<XmlItem> m_items;

    [XmlElement("item", Type = typeof(XmlItem))]
    public List<XmlItem> Items
    {
        get
        {
            return m_items;
        }

        set
        {
            m_items = value;
        }
    }
}
