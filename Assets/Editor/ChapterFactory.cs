using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using Morbius.Scripts.Level;

public class ChapterFactory
{
    private string m_parentFolder;
    private string m_path;

    public ChapterFactory(string parentFolder)
    {
        m_parentFolder = parentFolder;
    }

    public void Add(TextAsset asset)
    {
        Deserialize(asset);
    }

    private void CreateFolder(string folder)
    {
        string guid = AssetDatabase.CreateFolder(m_parentFolder, folder);
        m_path = AssetDatabase.GUIDToAssetPath(guid);
    }

    private void CreateAsset(Chapter chapter)
    {
        string path = AssetDatabase.GenerateUniqueAssetPath(m_path + "/" + chapter.name + ".asset");
        AssetDatabase.CreateAsset(chapter, path);
    }

    private Chapter BuildChapter(XmlChapter xmlChapter)
    {
        Chapter chapter = null;

        //get first non-empty string, if any
        string tester = xmlChapter.Text.Where(x => !String.IsNullOrEmpty(x)).FirstOrDefault();

        //don't create objects for empty chapter definitions
        if (!String.IsNullOrEmpty(xmlChapter.Title) || !String.IsNullOrEmpty(tester))
        {
            chapter = ScriptableObject.CreateInstance<Chapter>();

            chapter.Title = xmlChapter.Title;
            chapter.name = "Chapter " + chapter.Title;
            Array.Copy(xmlChapter.Text, chapter.Text, chapter.Text.Length);
        }

        return chapter;
    }

    private void Deserialize(TextAsset xmlAsset)
    {
        CreateFolder("Chapters");

        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlChapters));
            XmlChapters chapters = (XmlChapters)serializer.Deserialize(reader);

            int chapterCount = 0;
            foreach (XmlChapter xmlChapter in chapters.Chapters)
            {
                Chapter chapter = BuildChapter(xmlChapter);
                if (chapter)
                {
                    CreateAsset(chapter);
                }
                chapterCount++;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
