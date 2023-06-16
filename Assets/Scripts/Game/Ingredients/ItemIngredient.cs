using UnityEngine;

public class ItemIngredient : MonoBehaviour, Iinteractable
{
    [Header("Item")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    private IngredientSO m_ingredient;

    [Header("Interact")]
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private Transform m_interactPos;
    [SerializeField] private GameObject m_interactCanvas;
    private GameObject m_currentInteractCanvas;

    public IngredientSO GetIngredient()
    {
        return m_ingredient;
    }

    public void InitItem(IngredientSO ingredient)
    {
        m_ingredient = ingredient;
        m_spriteRenderer.sprite = ingredient.ingredientSprite;
    }

    public void SetColliderActive(bool state)
    {
        m_collider.enabled = state;
    }

    #region Interact Methods
    public void Interact()
    {
        GameManager.instance.playerHand.TakeItem(this);
    }

    public void InstantiateInteractCanvas()
    {
        if (m_currentInteractCanvas != null) return;
        m_currentInteractCanvas = Instantiate(m_interactCanvas, m_interactPos);
    }

    public void DestroyInteractCanvas()
    {
        if (m_currentInteractCanvas == null) return;
        Destroy(m_currentInteractCanvas);
        m_currentInteractCanvas = null;
    }

    public void SetCameraTarget(float value)
    {
        
    }
    #endregion
}
