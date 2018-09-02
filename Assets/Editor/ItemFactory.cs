using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;
using Morbius.Scripts.Items;

public class ItemFactory : GenericPrefabFactory<ItemList>
{
    private string m_parentFolder;
    private string m_path;
    private static ItemList s_itemList;

    public static ItemList ItemList
    {
        get
        {
            return s_itemList;
        }
    }

    public ItemFactory(GameObject prefab, string parentFolder) : base(prefab)
    {
        m_parentFolder = parentFolder;
        s_itemList = null;
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        CreateFolder("Items");
        s_itemList = m_component;

        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlItems));
            XmlItems items = (XmlItems)serializer.Deserialize(reader);
            foreach (XmlItem xmlItem in items.Items)
            {
                if(!m_component.ItemDatabase.Exists(i => Convert.ToString(i.Id) == xmlItem.Id))
                {
                    Item item = BuildItem(xmlItem);
                    CreateAsset(item);
                    m_component.ItemDatabase.Add(item);
                }
                else
                {
                    Debug.LogWarning("Item id " + xmlItem.Id + " already exists. Skipping...");
                }

            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        //try to link generated items with combinations
        CombinationFactory.LinkItems();
    }

    private void CreateFolder(string folder)
    {
        string guid = AssetDatabase.CreateFolder(m_parentFolder, folder);
        m_path = AssetDatabase.GUIDToAssetPath(guid);
    }

    private void CreateAsset(Item item)
    {
        string path = AssetDatabase.GenerateUniqueAssetPath(m_path + "/" + item.name + ".asset");
        AssetDatabase.CreateAsset(item, path);
    }

    private Item BuildItem(XmlItem xmlItem)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.Id = Convert.ToInt32(xmlItem.Id);
        item.Label = xmlItem.Name;
        item.Collectable = !string.IsNullOrEmpty(xmlItem.Collectable);
        item.Destroyable = !string.IsNullOrEmpty(xmlItem.Destroyable);
        item.Icon = AssetFinder.FindSprite(xmlItem.Imgfile);
        item.Prefab = AssetFinder.FindGameObject(xmlItem.Entfile);
        item.Sequences = xmlItem.Sequences.Select(x => new ItemSequence
        {
            TriggerId = Convert.ToInt32(x.Result),
            Description = x.Description.Replace("\\n", "\n"),
            Audio = AssetFinder.FindClip(x.Sound)
        }
        ).ToList();
        item.name = "Item " + item.Id.ToString() + " " + item.Label;

        return item;
    }

}
