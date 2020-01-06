using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Morbius.Scripts.Items
{
    [ExecuteInEditMode]
    public class ItemEditorVisualizer : MonoBehaviour
    {
#if UNITY_EDITOR
        private Collider m_collider;
        private ItemInstance m_instance;
        private Item m_item;

        void Start()
        {
            Setup();
        }

        void OnDrawGizmos()
        {
            if (Application.isEditor)
            {
                Setup();
            }
            DrawProperties();
            DrawTriggerRange();
        }

        private void Setup()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            m_collider = colliders.Where(x => x.isTrigger).FirstOrDefault();
            ItemInstance m_instance = GetComponent<ItemInstance>();
            if (m_instance)
                m_item = m_instance.Item;
            else
                m_item = null;
        }

        private void DrawProperties()
        {
            if (!m_item)
                return;

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;
            Handles.Label(transform.position, m_item.ToString(), style);
        }

        private void DrawTriggerRange()
        {
            if (!m_collider)
                return;

            if (m_collider is CapsuleCollider)
            {
                CapsuleCollider capsule = m_collider as CapsuleCollider;
                Handles.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.1f);
                Handles.zTest = CompareFunction.LessEqual;
                Vector3 pos = transform.position + capsule.center * transform.localScale.y;
                Handles.DrawSolidDisc(pos, Vector3.up, capsule.radius * transform.localScale.y);
                Handles.color = Color.magenta;
                Handles.DrawWireDisc(pos, Vector3.up, capsule.radius * transform.localScale.y);
            }
            else if (m_collider is BoxCollider)
            {
                BoxCollider box = m_collider as BoxCollider;
                Vector3 distance = Vector3.Scale(box.size, transform.localScale) / 2.0f;
                Vector3[] p = new Vector3[8];
                p[0] = new Vector3(distance.x, distance.y, distance.z);
                p[1] = new Vector3(-distance.x, distance.y, distance.z);
                p[2] = new Vector3(-distance.x, distance.y, -distance.z);
                p[3] = new Vector3(distance.x, distance.y, -distance.z);
                p[4] = new Vector3(distance.x, -distance.y, distance.z);
                p[5] = new Vector3(-distance.x, -distance.y, distance.z);
                p[6] = new Vector3(-distance.x, -distance.y, -distance.z);
                p[7] = new Vector3(distance.x, -distance.y, -distance.z);

                for (int i = 0; i < 8; i++)
                {
                    p[i] = transform.TransformDirection(p[i] + Vector3.Scale(box.center, transform.localScale)) + transform.position;
                }

                //sort by y pos
                p = p.OrderBy(x => x.y).ToArray();

                //take the 4 points with lowest y pos
                Vector3[] vertLower = p.Take(4).ToArray();
                //sort by x position
                vertLower = vertLower.OrderBy(x => x.x).ToArray();

                Vector3[] vertUpper = p.Skip(4).ToArray();
                //sort by x position
                vertUpper = vertUpper.OrderBy(x => x.x).ToArray();

                //get middle plane
                vertLower = vertLower.Select((x, i) => (x + vertUpper[i]) * 0.5f).ToArray();

                Handles.zTest = CompareFunction.LessEqual;
                Color transCol = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.1f);

                Vector3[] rect = new Vector3[] { vertLower[0], vertLower[2], vertLower[3], vertLower[1] };
                Handles.DrawSolidRectangleWithOutline(rect, transCol, Color.magenta);
                //Vector3[] rect2 = new Vector3[] { vertUpper[0], vertUpper[2], vertUpper[3], vertUpper[1] };
                //Handles.DrawSolidRectangleWithOutline(rect2, transCol, Color.magenta);
            }
            else
            {

            }
        }
#endif
    }
}
