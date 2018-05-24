using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ChapterFactory : GenericPrefabFactory<ChapterManager>
{
    public ChapterFactory(GameObject prefab) : base(prefab)
    {
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlChapters));
            XmlChapters chapters = (XmlChapters)serializer.Deserialize(reader);
            foreach (XmlChapter chapter in chapters.Chapters)
            {
                if (!m_manager.Chapters.Exists(i => i.Id == chapter.Id))
                {
                    m_manager.Chapters.Add(chapter);
                }
                else
                {
                    Debug.LogWarning("Chapter id " + chapter.Id + " already exists. Skipping...");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
