using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 m_targetPosition;
    private Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;
        InvokeRepeating(nameof(UpdatePosition), 0f, 0.1f);
    }

    private void Update()
    {
        if (!GameManager.instance.IsPlaying) return;
        transform.position = m_targetPosition;
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        m_targetPosition = mousePosition;
    }
}