using System.Xml.Serialization;

public class XmlDialogText : XmlDialogElement
{
    private string m_name;
    private string m_text;
    private string m_file;

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
}
