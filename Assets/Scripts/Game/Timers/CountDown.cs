using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_counterTMP;

    public void UpdateCounter(string textCounter) => m_counterTMP.text = textCounter;

    public void CharacterEnableCanMove() => GameManager.instance.PlayerMovement.SetCanMove(true);
    public void CharacterDisableCanMove() => GameManager.instance.PlayerMovement.SetCanMove(false);
}
