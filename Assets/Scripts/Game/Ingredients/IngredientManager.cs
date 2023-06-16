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
        Vector3 newPos = m_minigameCanvasPos.position;
        newPos.y += 4.5f;
        BaseMinigame minigame = Instantiate(m_minigameCanvas, newPos, Quaternion.identity);
        minigame.ingredientManager = this;

        SetCameraTarget(10);
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
        if (value > 0)
            m_targetGroup.m_Targets[2].target = m_minigameCanvasPos.transform;
        else
            m_canInteract = true;

        m_targetGroup.m_Targets[2].weight = value;
        m_targetGroup.m_Targets[2].radius = value;
    }
    #endregion
}
