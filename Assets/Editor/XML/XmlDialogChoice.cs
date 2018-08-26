using System.Collections.Generic;
using System.Xml.Serialization;

public class XmlDialogChoice : XmlDialogElement
{
    private List<XmlDialogChoiceItem> m_choiceItems;

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
