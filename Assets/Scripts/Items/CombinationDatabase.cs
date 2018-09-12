using UnityEngine;
using System.Collections.Generic;
using Morbius.Scripts.Items;

public class CombinationDatabase : MonoBehaviour
{
    private static CombinationDatabase s_singleton;

    [SerializeField]
    private List<Combination> m_combinations = new List<Combination>();
    [SerializeField]
    private List<Failure> m_failures = new List<Failure>();

    public List<Combination> Combinations
    {
        get
        {
            return m_combinations;
        }

        set
        {
            m_combinations = value;
        }
    }

    public List<Failure> Failures
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

    void Awake()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("CombinationDatabase: Multiple instances detected. Destroying...");
            Destroy(this);
        }

    }

    public static Combination GetCombination(int id1, int id2)
    {
        if (s_singleton)
        {
            return s_singleton.m_combinations.Find
                (x => ((x.Id1 == id1) && (x.Id2 == id2)) ||
                ((x.Id1 == id2) && (x.Id2 == id1))
                );
        }
        else
        {
            return null;
        }
    }

    public static Failure GetFailure()
    {
        if (s_singleton)
        {
            System.Random rand = new System.Random();
            int index = rand.Next(0, s_singleton.m_failures.Count);
            return s_singleton.m_failures[index];
        }
        else
        {
            return null;
        }
    }
}
