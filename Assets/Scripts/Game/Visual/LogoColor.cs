using UnityEngine;

public class LogoColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private void Start() => InvokeRepeating("ChangeLogoColor", 1, 1);

    private void ChangeLogoColor() => m_spriteRenderer.color = RandomColor.Generate();
}
