using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("combinations")]
public class XmlCombinations
{
    private List<XmlCombinationLink> m_links;
    private List<XmlCombinationElement> m_failures;

    [XmlElement("combination", Type = typeof(XmlCombinationLink))]
    public List<XmlCombinationLink> Links
    {
        get
        {
            return m_links;
        }

        set
        {
            m_links = value;
        }
    }

    [XmlElement("fail", Type = typeof(XmlCombinationElement))]
    public List<XmlCombinationElement> Failures
    {
        get
        {
            return m_failures;
        }

        set
        {
            m_failures = value;
        }
    }
}
