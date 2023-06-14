using System.Collections.Generic;
using UnityEngine;

public class SoundVisual : MonoBehaviour
{
    [SerializeField] private List<Sprite> m_sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer.sprite = m_sprites[Random.Range(0, m_sprites.Count)];
        m_spriteRenderer.color = RandomColor.Generate();
    }

    public void DestroyEffecct() => Destroy(gameObject);
}
