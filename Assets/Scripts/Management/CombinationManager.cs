using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombinationManager : MonoBehaviour
{
    [SerializeField]
    private List<XmlCombinationLink> m_links = new List<XmlCombinationLink>();
    [SerializeField]
    private List<XmlCombinationElement> m_failures = new List<XmlCombinationElement>();

    public List<XmlCombinationLink> Links
    {
        get
        {
            return m_links;
        }

        set
        {
            m_links = value;
        }
    }

    public List<XmlCombinationElement> Failures
    {
        get
        {
            return m_failures;
        }

        set
        {
            m_failures = value;
        }
    }
}
