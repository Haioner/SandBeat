using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CustomerSkinsSO m_skinsList;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private Skin m_currentSkin;
    private int m_currentFrameIndex;
    private float m_frameTimer;

    private void Start() => RandomSkin();

    private void Update()
    {
        UpdateFrame();
        SpeedAnim();
    }

    private void SpeedAnim() => anim.SetFloat("speed", rb.velocity.magnitude);

    private void RandomSkin()
    {
        int randSkinIndex = Random.Range(0, m_skinsList.Skins.Count);
        m_currentSkin = m_skinsList.Skins[randSkinIndex];

        if (m_currentSkin.IdleSprites.Count > 0)
            spriteRenderer.sprite = m_currentSkin.IdleSprites[0]; 
    }

    private void UpdateFrame()
    {
        if (m_currentSkin.IdleSprites.Count == 0)
            return;

        m_frameTimer += Time.deltaTime;
        if (m_frameTimer >= 0.5f)
        {
            m_frameTimer = 0f;
            m_currentFrameIndex++;

            //Change Skin Frame
            if (m_currentFrameIndex >= m_currentSkin.IdleSprites.Count)
                m_currentFrameIndex = 0;
            
            spriteRenderer.sprite = m_currentSkin.IdleSprites[m_currentFrameIndex];
        }
    }
}
