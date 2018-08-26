using UnityEngine;

public abstract class GenericPrefabFactory<T> where T : MonoBehaviour
{
    protected T m_component;
    private GameObject m_prefab;

    public GenericPrefabFactory(GameObject prefab)
    {
        m_prefab = prefab;
        m_component = m_prefab.GetComponent<T>();
        if (m_component == null && typeof(T) != typeof(MonoBehaviour))
        {
            m_component = m_prefab.AddComponent<T>();
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

    protected GameObject AddChild(string name)
    {
        GameObject element = new GameObject(name);
        element.transform.SetParent(m_prefab.transform);

        return element;
    }

}
