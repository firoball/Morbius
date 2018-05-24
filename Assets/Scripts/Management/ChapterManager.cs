using UnityEngine;
using System.Collections.Generic;

public class ChapterManager : MonoBehaviour
{
    [SerializeField]
    private List<XmlChapter> m_chapters = new List<XmlChapter>();

    public List<XmlChapter> Chapters
    {
        get
        {
            return m_chapters;
        }

        set
        {
            m_chapters = value;
        }
    }

}
