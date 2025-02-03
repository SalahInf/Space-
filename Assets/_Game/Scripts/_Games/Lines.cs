using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    [SerializeField] Rigidbody[] m_rb;
    [SerializeField] LineRenderer m_line;

    private void Start()
    {
        for (int i = 0; i < m_rb.Length; i++)
        {
            m_line.SetPosition(i, m_rb[i].transform.position);
        }
    }

    public void FetchPosition(Vector3 pos)
    {
        m_rb[0].transform.position = pos;
        m_line.SetPosition(0, pos);

        for (int i = 1; i < m_rb.Length; i++)
        {
            m_line.SetPosition(i, m_rb[i].transform.position);
        }
    }
}
