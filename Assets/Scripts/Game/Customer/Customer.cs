using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private Rigidbody2D rb;
    private Vector3 m_positionToMove;
    private bool m_isLeaving;

    private void FixedUpdate()
    {
        MoveToPosition();
    }

    public void SetPositionToMove(Vector3 posToMove)
    {
        m_positionToMove = posToMove;
    }

    private void MoveToPosition()
    {
        Vector3 direction = m_positionToMove - transform.position;
        float distance = direction.magnitude;

        if (distance > 0.1f)
        {
            direction.Normalize();
            Vector3 velocity = direction * m_speed;
            rb.velocity = velocity;
        }
        else
        {
            if (m_isLeaving) Destroy(gameObject);
            rb.velocity = Vector3.zero;
        }
    }

    public void LeaveCustomer(Vector3 posToMove)
    {
        m_positionToMove = posToMove;
        m_isLeaving = true;
    }
}
