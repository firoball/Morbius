using System.Xml.Serialization;

public class XmlDialogElement
{
    protected string m_id;

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
}
