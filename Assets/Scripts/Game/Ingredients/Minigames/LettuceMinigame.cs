using UnityEngine.UI;
using UnityEngine;

public class LettuceMinigame : BaseMinigame
{
    [Header("Animations")]
    [SerializeField] private Animator m_knifeAnim;
    [SerializeField] private Sprite m_cutLettuce;

    [Header("Particles")]
    [SerializeField] private GameObject m_lettuceParticle;
    [SerializeField] private GameObject m_missParticle;

    [Header("Position")]
    [SerializeField] private RectTransform m_lettuce;
    [SerializeField] private RectTransform m_knife;
    [SerializeField] private float m_knifeSpeed;

    private void Start()
    {
        StartPositions();
        InvokeRepeating("KnifeCut", 1, 1);
    }

    public override void Update()
    {
        base.Update();
        MoveKnife();
    }

    private void StartPositions()
    {
        //Randomize Scale
        float randWidth = Random.Range(40f, 70f);
        Vector2 newSize = m_lettuce.sizeDelta;
        newSize.x = randWidth;
        newSize.y = randWidth;
        m_lettuce.sizeDelta = newSize;

        //Randomize Position
        float randPosX = Random.Range(-101f, 101f);
        float randPosY = Random.Range(-54f, 54f);
        Vector3 newPos = m_lettuce.anchoredPosition;
        newPos.x = randPosX;
        newPos.y = randPosY;
        m_lettuce.anchoredPosition = newPos;
    }

    private void KnifeCut()
    {
        m_knifeAnim.SetTrigger("Hit");
        CheckKnifeHit();
    }

    private void MoveKnife()
    {
        float moveInputHorizontal = Input.GetAxisRaw("Horizontal");
        float moveInputVertical = Input.GetAxisRaw("Vertical");

        float moveAmountHorizontal = moveInputHorizontal * m_knifeSpeed * 100 * Time.deltaTime;
        float moveAmountVertical = moveInputVertical * m_knifeSpeed * 100 * Time.deltaTime;

        Vector3 newPosition = m_knife.anchoredPosition;
        newPosition.x += moveAmountHorizontal;
        newPosition.y += moveAmountVertical;

        newPosition.x = Mathf.Clamp(newPosition.x, 28, 304);
        newPosition.y = Mathf.Clamp(newPosition.y, -116, 74);

        m_knife.anchoredPosition = newPosition;
    }

    private void CheckKnifeHit()
    {
        Vector2 knifeScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_knife.position);
        Vector2 lettuceHitScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_lettuce.position);

        Rect pinRect = new Rect(knifeScreenPos - m_knife.sizeDelta * 0.5f, m_knife.sizeDelta);
        Rect greenHitRect = new Rect(lettuceHitScreenPos - m_lettuce.sizeDelta * 0.5f, m_lettuce.sizeDelta);

        if (pinRect.Overlaps(greenHitRect))
        {
            Instantiate(m_lettuceParticle, m_knife);
            PlayRandomClip(m_lettuce.transform);
            m_lettuce.GetComponent<Image>().sprite = m_cutLettuce;
            Invoke("EndIngredient", 0.5f);
        }
        else
        {
            Instantiate(m_missParticle, m_knife);
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_knife.transform);
            StartPositions();
        }
    }
}
