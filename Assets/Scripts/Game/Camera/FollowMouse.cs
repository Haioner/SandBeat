using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera m_camera;
    private Vector3 m_targetPosition;

    private void Start()
    {
        m_camera = Camera.main;
        InvokeRepeating(nameof(UpdatePosition), 0f, 0.1f);
    }

    private void Update() => transform.position = m_targetPosition;

    private void UpdatePosition()
    {
        Vector3 mousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        m_targetPosition = mousePosition;
    }
}