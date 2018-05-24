using UnityEngine;

public abstract class GenericPrefabFactory<T> where T : MonoBehaviour
{
    protected T m_manager;
    private GameObject m_prefab;

    public GenericPrefabFactory(GameObject prefab)
    {
        m_prefab = prefab;
        m_manager = m_prefab.GetComponent<T>();
        if (m_manager == null)
        {
            m_manager = m_prefab.AddComponent<T>();
        }
    }

    public void Add(TextAsset asset)
    {
        Deserialize(asset);
    }

    protected virtual void Deserialize(TextAsset xmlAsset)
    {
        Debug.LogWarning("Deserialize: routine not implemented.");
    }
}
