using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerNavigator : MonoBehaviour
{
    [SerializeField]
    private float m_walkSpeed = 1.0f;
    [SerializeField]
    private float m_runSpeed = 2.0f;

    private NavMeshAgent agent;
    private Animator animator;
    private float m_pressedTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        m_pressedTime = 0.0f;
    }

    /*private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), m_pressedTime.ToString());
        GUI.Label(new Rect(10, 50, 300, 30), agent.speed.ToString() + " " + agent.velocity.sqrMagnitude.ToString());
    }*/

    void Update()
    {
        //TODO this code is doubled with CursorManager....
        //TODO: add touch support
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000))
            {
                agent.destination = hit.point;
            }
        }

        agent.speed = m_walkSpeed;
        bool isRunning = false;

        if (Input.GetMouseButton(0))
        {
            if (m_pressedTime >= 0.3f)
            {
                agent.speed = m_runSpeed;
                isRunning = true;
            }
            else
            {
                m_pressedTime += Time.deltaTime;
            }
        }
        else
        {
            m_pressedTime = 0.0f;
        }

        if (animator)
        {
            bool isMoving = (agent.velocity.sqrMagnitude > 0.05f);
            animator.SetBool("walk", (isMoving && !isRunning));
            animator.SetBool("run", (isMoving && isRunning));
        }
    }
}