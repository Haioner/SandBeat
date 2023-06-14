using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LettuceMinigame : BaseMinigame
{
    [Header("Animations")]
    [SerializeField] private Animator m_knifeAnim;
    [SerializeField] private Sprite m_cutLettuce;

    [Header("Position")]
    [SerializeField] private RectTransform m_lettuce;
    [SerializeField] private RectTransform m_knife;
    [SerializeField] private float m_knifeSpeed;

    private void Start()
    {
        StartPositions();
        InvokeRepeating("KnifeCut", 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitMinigame();

        MoveKnife();
    }

    private void QuitMinigame()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.DestroyInteractCanvas();
        Destroy(gameObject);
    }

    private void StartPositions()
    {
        float randWidth = Random.Range(50f, 80f);
        Vector2 newSize = m_lettuce.sizeDelta;
        newSize.x = randWidth;
        m_lettuce.sizeDelta = newSize;

        float randPosX = Random.Range(-50f, 170f);
        float randPosY = Random.Range(-90f, 90f);
        Vector3 newPos = m_lettuce.anchoredPosition;
        newPos.x = randPosX;
        newPos.y = randPosY;
        m_lettuce.anchoredPosition = newPos;
    }

    private void KnifeCut()
    {
        m_knifeAnim.SetTrigger("Hit");
        CheckPinHit();
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

        newPosition.x = Mathf.Clamp(newPosition.x, -170, 170);
        newPosition.y = Mathf.Clamp(newPosition.y, -70, 70);

        m_knife.anchoredPosition = newPosition;
    }

    private void CheckPinHit()
    {
        Vector2 knifeScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_knife.position);
        Vector2 lettuceHitScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_lettuce.position);

        Rect pinRect = new Rect(knifeScreenPos - m_knife.sizeDelta * 0.5f, m_knife.sizeDelta);
        Rect greenHitRect = new Rect(lettuceHitScreenPos - m_lettuce.sizeDelta * 0.5f, m_lettuce.sizeDelta);

        if (pinRect.Overlaps(greenHitRect))
        {
            PlayRandomClip();
            m_lettuce.GetComponent<Image>().sprite = m_cutLettuce;
            Invoke("EndIngredient", .5f);
        }
        else
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_knife.transform);
            StartPositions();
        }
    }

    private void PlayRandomClip()
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        GameManager.instance.AddAudioSourcers(MinigameAudioSource.clip, m_lettuce.transform);
    }

    private void EndIngredient()
    {
        m_knife.gameObject.SetActive(false);
        GameManager.instance.PlayerMovement.SetCanMove(true);
        GameManager.instance.playerHand.SpawnIngredient(Ingredient);
        ingredientManager.DestroyInteractCanvas();
        Destroy(gameObject);
    }
}
