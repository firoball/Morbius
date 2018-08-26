using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Morbius.Scripts.Level;

public class ChapterFactory : GenericPrefabFactory<MonoBehaviour>
{
    public ChapterFactory(GameObject prefab) : base(prefab)
    {
    }

    private void BuildChapter(XmlChapter xmlChapter, string name)
    {
        //get first non-empty string, if any
        string tester = xmlChapter.Text.Where(x => !String.IsNullOrEmpty(x)).FirstOrDefault();

        //don't create child GOs for empty chapter definitions
        if (!String.IsNullOrEmpty(xmlChapter.Title) || !String.IsNullOrEmpty(tester))
        {
            GameObject element = AddChild(name);
            Chapter chapter = element.AddComponent<Chapter>();

            chapter.Title = xmlChapter.Title;
            Array.Copy(xmlChapter.Text, chapter.Text, chapter.Text.Length);
        }
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlChapters));
            XmlChapters chapters = (XmlChapters)serializer.Deserialize(reader);

            int chapterCount = 0;
            foreach (XmlChapter chapter in chapters.Chapters)
            {
                BuildChapter(chapter, "Chapter" + chapterCount);
                chapterCount++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
