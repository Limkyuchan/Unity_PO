using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    Color color = Color.red;
    [SerializeField]
    bool m_drawLine;

    Waypoint[] m_wayPoints;


    public Waypoint[] Waypoints { get { return m_wayPoints; } }

    public Vector3[] Points
    {
        get
        {
            Vector3[] points = new Vector3[m_wayPoints.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = m_wayPoints[i].transform.position;
            }
            return points;
        }
    }

    void OnDrawGizmos()
    {
        m_wayPoints = GetComponentsInChildren<Waypoint>();

        if (m_drawLine)
        {
            for (int i = 0; i < m_wayPoints.Length - 1; i++)
            {
                m_wayPoints[i].color = color;
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(m_wayPoints[i].transform.position, m_wayPoints[i + 1].transform.position);
            }
            m_wayPoints[0].color = Color.white;
            m_wayPoints[m_wayPoints.Length - 1].color = Color.black;
        }
    }

    void Start()
    {
        m_wayPoints = GetComponentsInChildren<Waypoint>();
    }
}
