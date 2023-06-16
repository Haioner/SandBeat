using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class CheeseMinigame : BaseMinigame
{
    [Header("Animations")]
    [SerializeField] private Animator m_knifeAnim;
    [SerializeField] private Sprite m_cutCheese;

    [Header("Position")]
    [SerializeField] private List<RectTransform> m_cheeseList = new List<RectTransform>();
    [SerializeField] private RectTransform m_knife;
    
    private int m_cheeseCount;
    private Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;
        StartPositions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            KnifeCut();

        if (Input.GetKeyDown(KeyCode.Escape))
            QuitMinigame();

        MoveKnife();
    }

    private void QuitMinigame()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }

    private void StartPositions()
    {
        foreach (var cheese in m_cheeseList)
        {
            float randPosX = Random.Range(-170f, 170f);
            float randPosY = Random.Range(-90f, 90f);
            Vector3 newPos = cheese.anchoredPosition;
            newPos.x = randPosX;
            newPos.y = randPosY;
            cheese.anchoredPosition = newPos;
        }
    }

    private void KnifeCut()
    {
        m_knifeAnim.SetTrigger("Hit");
        CheckPinHit();
    }

    private void MoveKnife()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = m_camera.ScreenToWorldPoint(mousePosition);

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(worldMousePosition.x, -170f, 170f),
            Mathf.Clamp(worldMousePosition.y, -70f, 70f),
            m_knife.position.z
        );
        m_knife.position = clampedPosition;
    }

    private void CheckPinHit()
    {
        Vector2 knifeScreenPos = RectTransformUtility.WorldToScreenPoint(m_camera, m_knife.position);
        bool cheeseHit = false;

        for (int i = 0; i < m_cheeseList.Count; i++)
        {
            if (m_cheeseList[i].GetComponent<Image>().sprite != m_cutCheese)
            {
                Vector2 cheeseHitScreenPos = RectTransformUtility.WorldToScreenPoint(m_camera, m_cheeseList[i].position);

                Rect pinRect = new Rect(knifeScreenPos - m_knife.sizeDelta * 0.5f, m_knife.sizeDelta);
                Rect cheeseRect = new Rect(cheeseHitScreenPos - m_cheeseList[i].sizeDelta * 0.5f, m_cheeseList[i].sizeDelta);

                if (pinRect.Overlaps(cheeseRect))
                {
                    PlayRandomClip();
                    m_cheeseList[i].GetComponent<Image>().sprite = m_cutCheese;
                    cheeseHit = true;
                    m_cheeseCount++;
                    Invoke("EndIngredient", .5f);
                    break;
                }
            }
        }

        if (!cheeseHit)
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_knife.transform);
        }
    }

    private void PlayRandomClip()
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        GameManager.instance.AddAudioSourcers(MinigameAudioSource.clip, m_knife.transform);
    }

    private void EndIngredient()
    {
        if(m_cheeseCount >= 3)
        {
            m_knife.gameObject.SetActive(false);
            GameManager.instance.PlayerMovement.SetCanMove(true);
            GameManager.instance.playerHand.SpawnIngredient(Ingredient);
            ingredientManager.SetCameraTarget(0);
            Destroy(gameObject);

        }
    }
}
