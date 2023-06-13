using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_counterTMP;
    [SerializeField] private MovementController m_movementController;

    public void UpdateCounter(string textCounter) => m_counterTMP.text = textCounter;

    public void CharacterEnableCanMove() => m_movementController.CanMove = true;
    public void CharacterDisableCanMove() => m_movementController.CanMove = false;
}
