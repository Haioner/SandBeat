using UnityEngine;

public class MeatMinigame : BaseMinigame
{
    [Header("Spatula")]
    [SerializeField] private Animator m_anim;

    [Header("Notes")]
    [SerializeField] private RectTransform m_pin;
    [SerializeField] private RectTransform m_greenHit;

    private bool m_isRight = true;
    private float m_pinXPos;

    private void Start() => StartPositions();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            CheckPinHit();

        if (Input.GetKeyDown(KeyCode.Escape))
            QuitMinigame();

        UpdatePinPos();
    }

    private void QuitMinigame()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }

    private void StartPositions()
    {
        //Green Hit
        float randWidth = Random.Range(50f, 100f);
        Vector2 newSize = m_greenHit.sizeDelta;
        newSize.x = randWidth;
        m_greenHit.sizeDelta = newSize;

        float randPos = Random.Range(-153f, 153f);
        Vector3 newPos = m_greenHit.anchoredPosition;
        newPos.x = randPos;
        m_greenHit.anchoredPosition = newPos;


        //Pin
        Vector3 pinPos = Vector3.zero;
        m_pin.anchoredPosition = pinPos;
    }

    private void UpdatePinPos()
    {
        if (!m_pin.gameObject.activeInHierarchy) return;

        if (!m_isRight && m_pin.anchoredPosition.x > -170)
            m_pinXPos -= Time.deltaTime * GameManager.instance.MinigameSpeed * 50;
        else if (!m_isRight)
            m_isRight = true;

        if (m_isRight && m_pin.anchoredPosition.x < 170)
            m_pinXPos += Time.deltaTime * GameManager.instance.MinigameSpeed * 50;
        else if (m_isRight)
            m_isRight = false;

        Vector3 newPos = m_pin.anchoredPosition;
        newPos.x = m_pinXPos;
        m_pin.anchoredPosition = newPos;
    }

    private void CheckPinHit()
    {
        Vector2 pinScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_pin.position);
        Vector2 greenHitScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_greenHit.position);

        Rect pinRect = new Rect(pinScreenPos - m_pin.sizeDelta * 0.5f, m_pin.sizeDelta);
        Rect greenHitRect = new Rect(greenHitScreenPos - m_greenHit.sizeDelta * 0.5f, m_greenHit.sizeDelta);

        if (pinRect.Overlaps(greenHitRect))
        {
            PlayRandomClip();
            AnimateSpatula();
            Invoke("EndIngredient", .8f);
        }
        else
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_pin.transform);
            StartPositions();
        }
    }

    private void PlayRandomClip()
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        MinigameAudioSource.Play();
    }

    private void EndIngredient()
    {
        m_pin.gameObject.SetActive(false);
        GameManager.instance.PlayerMovement.SetCanMove(true);
        GameManager.instance.playerHand.SpawnIngredient(Ingredient);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }

    private void AnimateSpatula() => m_anim.SetTrigger("Hit");
}
