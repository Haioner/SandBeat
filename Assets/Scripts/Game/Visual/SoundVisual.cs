using System.Collections.Generic;
using UnityEngine;

public class SoundVisual : MonoBehaviour
{
    [SerializeField] private List<Sprite> m_sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer.sprite = m_sprites[Random.Range(0, m_sprites.Count)];
        m_spriteRenderer.color = GenerateRandomColor();
    }

    private Color GenerateRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        return new Color(r, g, b);
    }

    public void DestroyEffecct() => Destroy(gameObject);
}
