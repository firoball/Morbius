using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ItemFactory : GenericPrefabFactory<ItemManager>
{
    public ItemFactory(GameObject prefab) : base(prefab)
    {
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlItems));
            XmlItems items = (XmlItems)serializer.Deserialize(reader);
            foreach (XmlItem item in items.Items)
            {
                if (!m_manager.Items.Exists(i => i.Id == item.Id))
                {
                    m_manager.Items.Add(item);
                }
                else
                {
                    Debug.LogWarning("Item id " + item.Id + " already exists. Skipping...");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
