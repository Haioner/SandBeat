using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIngredient : MonoBehaviour, Iinteractable
{
    [Header("Item")]
    [SerializeField] private IngredientSO m_ingredient;

    [Header("Interact")]
    [SerializeField] private Transform m_interactPos;
    [SerializeField] private GameObject m_interactCanvas;
    private GameObject m_currentInteractCanvas;
    public bool CanInteract { get; set; }

    public void InitItem(IngredientSO ingredient)
    {
        m_ingredient = ingredient;
    }

    #region Interact Methods
    public void Interact()
    {
        if (CanInteract)
            GameManager.instance.playerHand.TakeItem(gameObject);
    }

    public void InstantiateInteractCanvas()
    {
        if (m_currentInteractCanvas != null || !CanInteract) return;
        m_currentInteractCanvas = Instantiate(m_interactCanvas, m_interactPos);
    }

    public void DestroyInteractCanvas()
    {
        if (m_currentInteractCanvas == null) return;
        Destroy(m_currentInteractCanvas);
        m_currentInteractCanvas = null;
    }
    #endregion
}
