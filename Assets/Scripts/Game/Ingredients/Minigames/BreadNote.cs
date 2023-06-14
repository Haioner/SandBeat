using UnityEngine;

public class BreadNote : MonoBehaviour
{
    private float m_movementSpeed;

    private void Start() => m_movementSpeed = GameManager.instance.MinigameSpeed;

    private void Update() => MoveNote();

    private void MoveNote()
    {
        Vector3 newPosition = transform.position + Vector3.left * m_movementSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}
