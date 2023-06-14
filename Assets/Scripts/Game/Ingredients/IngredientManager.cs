using Cinemachine;
using UnityEngine;

public class IngredientManager : MonoBehaviour, Iinteractable
{
    [Header("Minigame Canvas")]
    [SerializeField] private BaseMinigame m_minigameCanvas;
    [SerializeField] private AudioSource m_ingredientSource;
    [SerializeField] private Transform m_minigameCanvasPos;

    [Header("Interact")]
    [SerializeField] private Transform m_interactPos;
    [SerializeField] private GameObject m_interactCanvas;
    [SerializeField] private CinemachineTargetGroup m_targetGroup;
    private bool m_canInteract = true;
    private GameObject m_currentInteractCanvas;

    #region Interact Methods
    public void Interact()
    {
        if (!m_canInteract) return;

        m_canInteract = false;
        m_ingredientSource.Play();
        GameManager.instance.InstantiateSoundVisual(transform);
        GameManager.instance.PlayerMovement.SetCanMove(false);
        BaseMinigame minigame = Instantiate(m_minigameCanvas, m_minigameCanvasPos);
        minigame.ingredientManager = this;

        m_targetGroup.m_Targets[2].target = transform;
        m_targetGroup.m_Targets[2].weight = 10;
        m_targetGroup.m_Targets[2].radius = 10;
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
        m_targetGroup.m_Targets[2].weight = 0;
        m_targetGroup.m_Targets[2].radius = 0;
        m_canInteract = true;
    }
    #endregion
}
