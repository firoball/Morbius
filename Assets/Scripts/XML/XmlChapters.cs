using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("chapters")]
public class XmlChapters
{
    private List<XmlChapter> m_chapters;

    [XmlElement("chapter", Type = typeof(XmlChapter))]
    public List<XmlChapter> Chapters
    {
        get
        {
            return m_chapters;
        }

        set
        {
            m_chapters = value;
        }
    }
}
