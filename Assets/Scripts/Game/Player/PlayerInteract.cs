using System.Collections;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private KeyCode m_keyCode;
    [SerializeField] private float m_interactRange = 3f;
    [SerializeField] private LayerMask m_interactableLayer;
    private MovementController m_playerMovement;
    private Transform m_nearestInteractable;

    private void Awake()
    {
        m_playerMovement = GetComponent<MovementController>();
    }

    private void Update()
    {
        GetNearest();
        InteractNearest();
    }

    private void GetNearest()
    {
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, m_interactRange, m_interactableLayer);
        if (interactables.Length <= 0)
        {
            if(m_nearestInteractable != null)
            {
                m_nearestInteractable.GetComponent<Iinteractable>().DestroyInteractCanvas();
                m_nearestInteractable = null;
            }
            return;
        }

        foreach (Collider2D item in interactables)
        {
            if(item.GetComponent<Iinteractable>() != null)
            {
                float distance = Vector2.Distance(transform.position, item.transform.position);
                if (m_nearestInteractable == null || distance < Vector2.Distance(transform.position, m_nearestInteractable.transform.position))
                {
                    if (m_nearestInteractable != null)
                        m_nearestInteractable.GetComponent<Iinteractable>().DestroyInteractCanvas();

                    m_nearestInteractable = item.transform;
                    m_nearestInteractable.GetComponent<Iinteractable>().InstantiateInteractCanvas();
                }
            }
        }
    }

    private void InteractNearest()
    {
        if (m_nearestInteractable == null) return;

        if (Input.GetKeyDown(m_keyCode))
        {
            m_nearestInteractable.GetComponent<Iinteractable>().Interact();
            m_nearestInteractable = null;
        }
    }
}
