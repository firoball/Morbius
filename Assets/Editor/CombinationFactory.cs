﻿using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CombinationFactory : GenericPrefabFactory<CombinationManager>
{
    public CombinationFactory(GameObject prefab) : base(prefab)
    {
    }


    protected override void Deserialize(TextAsset xmlAsset)
    {
        try
        {
            StringReader reader = new StringReader(xmlAsset.ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(XmlCombinations));
            XmlCombinations combinations = (XmlCombinations)serializer.Deserialize(reader);

            foreach (XmlCombinationLink link in combinations.Links)
            {
                if (!m_manager.Links.Exists(i => (i.Id1 == link.Id1) && (i.Id2 == link.Id2)))
                {
                    m_manager.Links.Add(link);
                }
                else
                {
                    Debug.LogWarning("Combination for ids " + link.Id1 + "," + link.Id2 + " already exists. Skipping...");
                }
            }

            foreach (XmlCombinationElement failure in combinations.Failures)
            {
                if (!m_manager.Failures.Exists(i => i.Description == failure.Description))
                {
                    m_manager.Failures.Add(failure);
                }
                else
                {
                    Debug.LogWarning("Failure '" + failure.Description + "' already exists. Skipping...");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
