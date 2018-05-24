using System.Xml.Serialization;

public class XmlDialogGoto : XmlDialogElement
{
    private string m_target;

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

}
