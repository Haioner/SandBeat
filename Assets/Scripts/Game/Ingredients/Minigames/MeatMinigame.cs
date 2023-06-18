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

    public override void Update()
    {
        base.Update();

        UpdatePinPos();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            CheckPinHit();
    }

    private void StartPositions()
    {
        //Green Hit Scale/Position
        float randWidth = Random.Range(50f, 100f);
        Vector2 newSize = m_greenHit.sizeDelta;
        newSize.x = randWidth;
        m_greenHit.sizeDelta = newSize;

        float randPos = Random.Range(-153f, 153f);
        Vector3 newPos = m_greenHit.anchoredPosition;
        newPos.x = randPos;
        m_greenHit.anchoredPosition = newPos;

        //Pin Position
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
        if (!m_pin.gameObject.activeInHierarchy) return;

        Vector2 pinScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_pin.position);
        Vector2 greenHitScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_greenHit.position);

        Rect pinRect = new Rect(pinScreenPos - m_pin.sizeDelta * 0.5f, m_pin.sizeDelta);
        Rect greenHitRect = new Rect(greenHitScreenPos - m_greenHit.sizeDelta * 0.5f, m_greenHit.sizeDelta);

        if (pinRect.Overlaps(greenHitRect))
        {
            PlayRandomClip(m_pin.transform);
            m_pin.gameObject.SetActive(false);
            AnimateSpatula();
            Invoke("EndIngredient", .8f);
        }
        else
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_pin.transform);
            StartPositions();
        }
    }

    private void AnimateSpatula() => m_anim.SetTrigger("Hit");
}
