using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;
using Morbius.Scripts.Items;

public class CombinationFactory : GenericPrefabFactory<CombinationDatabase>
{
    private string m_parentFolder;
    private string m_path;
    private static bool s_readyForLink;

    public CombinationFactory(GameObject prefab, string parentFolder) : base(prefab)
    {
        m_parentFolder = parentFolder;
        s_readyForLink = false;
    }

    protected override void Deserialize(TextAsset xmlAsset)
    {
        CreateFolder("Combinations");
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlCombinations));
            XmlCombinations combinations = (XmlCombinations)serializer.Deserialize(reader);

            foreach (XmlCombinationLink xmlLink in combinations.Links)
            {
                int id1 = Convert.ToInt32(xmlLink.Id1);
                int id2 = Convert.ToInt32(xmlLink.Id2);

                if (!m_component.Combinations.Exists(i => (i.Id1 == id1) && (i.Id2 == id2)))
                {
                    Combination combination = BuildCombination(xmlLink);
                    m_component.Combinations.Add(combination);
                    CreateAsset(combination);
                }
                else
                {
                    Debug.LogWarning("Combination for ids " + xmlLink.Id1 + "," + xmlLink.Id2 + " already exists. Skipping...");
                }
            }

            int count = 0;
            foreach (XmlCombinationElement xmlFailure in combinations.Failures)
            {
                if (!m_component.Failures.Exists(i => i.Description == xmlFailure.Description))
                {
                    Failure failure = BuildFailure(xmlFailure, count);
                    m_component.Failures.Add(failure);
                    CreateAsset(failure);
                    count++;
                }
                else
                {
                    Debug.LogWarning("Failure '" + xmlFailure.Description + "' already exists. Skipping...");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        s_readyForLink = true;
    }

    private void CreateFolder(string folder)
    {
        string guid = AssetDatabase.CreateFolder(m_parentFolder, folder);
        m_path = AssetDatabase.GUIDToAssetPath(guid);
    }

    private void CreateAsset(ScriptableObject scriptObject)
    {
        string path = AssetDatabase.GenerateUniqueAssetPath(m_path + "/" + scriptObject.name + ".asset");
        AssetDatabase.CreateAsset(scriptObject, path);
    }

    private Combination BuildCombination(XmlCombinationLink xmlCombination)
    {
        Combination combination = ScriptableObject.CreateInstance<Combination>();

        combination.Id1 = Convert.ToInt32(xmlCombination.Id1);
        combination.Id2 = Convert.ToInt32(xmlCombination.Id2);
        combination.Audio = AssetFinder.FindClip(xmlCombination.Sound);
        combination.MorphId = Convert.ToInt32(xmlCombination.MorphTarget);
        combination.TriggerId = Convert.ToInt32(xmlCombination.Result);
        combination.Description = xmlCombination.Description != null ? xmlCombination.Description.Replace("\\n", "\n") : null;

        combination.name = "Combination " + combination.Id1.ToString() + " " + combination.Id2.ToString();

        return combination;
    }

    private Failure BuildFailure(XmlCombinationElement xmlCombination, int count)
    {
        Failure failure = ScriptableObject.CreateInstance<Failure>();

        failure.Audio = AssetFinder.FindClip(xmlCombination.Sound);
        failure.Description = xmlCombination.Description != null ? xmlCombination.Description.Replace("\\n", "\n") : null;

        failure.name = "Failure " + count.ToString();

        return failure;
    }

    public static void LinkItems()
    {
        if (s_readyForLink)
        {
            //ItemFactory.ItemManager
            s_readyForLink = false;
        }
    }
}
