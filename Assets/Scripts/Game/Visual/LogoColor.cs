using UnityEngine;

public class LogoColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        if (m_spriteRenderer == null)
            m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() => InvokeRepeating("ChangeLogoColor", 1, 1);

    private void ChangeLogoColor()
    {
        if (!GameManager.instance.IsPlaying) return;
        m_spriteRenderer.color = RandomColor.Generate();
    }
}
