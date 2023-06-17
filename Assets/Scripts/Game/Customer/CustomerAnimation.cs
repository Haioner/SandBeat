using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CustomerSkinsSO m_skinsList;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private Skin currentSkin;
    private int currentFrameIndex;
    private float frameTimer;

    private void Start()
    {
        RandomSkin();
    }

    private void Update()
    {
        UpdateFrame();
        SpeedAnim();
    }

    private void SpeedAnim() => anim.SetFloat("speed", rb.velocity.magnitude);

    private void RandomSkin()
    {
        int randSkinIndex = Random.Range(0, m_skinsList.Skins.Count);
        currentSkin = m_skinsList.Skins[randSkinIndex];

        if (currentSkin.IdleSprites.Count > 0)
            spriteRenderer.sprite = currentSkin.IdleSprites[0]; 
    }

    private void UpdateFrame()
    {
        if (currentSkin.IdleSprites.Count == 0)
            return;

        frameTimer += Time.deltaTime;
        if (frameTimer >= 0.5f)
        {
            frameTimer = 0f;
            currentFrameIndex++;

            //Change Skin Frame
            if (currentFrameIndex >= currentSkin.IdleSprites.Count)
                currentFrameIndex = 0;
            
            spriteRenderer.sprite = currentSkin.IdleSprites[currentFrameIndex];
        }
    }
}
