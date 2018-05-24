using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private List<XmlItem> m_items = new List<XmlItem>();

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
