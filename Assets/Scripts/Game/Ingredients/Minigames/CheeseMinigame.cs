using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CheeseMinigame : BaseMinigame
{
    [Header("Animations")]
    [SerializeField] private Animator m_knifeAnim;
    [SerializeField] private Sprite m_cutCheese;

    [Header("Particles")]
    [SerializeField] private GameObject m_cheeseParticle;
    [SerializeField] private GameObject m_missParticle;

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

    public override void Update()
    {
        base.Update();

        MoveKnife();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            KnifeCut();
    }

    private void StartPositions()
    {
        foreach (var cheese in m_cheeseList)
        {
            //Randomize chesse positions
            float randPosX = Random.Range(-132, 132f);
            float randPosY = Random.Range(-121f, 12f);
            Vector3 newPos = cheese.anchoredPosition;
            newPos.x = randPosX;
            newPos.y = randPosY;
            cheese.anchoredPosition = newPos;
        }
    }

    private void KnifeCut()
    {
        m_knifeAnim.SetTrigger("Hit");
        CheckKnifeHit();
    }

    private void MoveKnife()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = m_camera.ScreenToWorldPoint(mousePosition);

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(worldMousePosition.x, -132f, 132f),
            Mathf.Clamp(worldMousePosition.y, -121f, 12f),
            m_knife.position.z
        );
        m_knife.position = clampedPosition;
    }

    private void CheckKnifeHit()
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
                    Instantiate(m_cheeseParticle, m_knife);
                    PlayRandomClip(m_knife.transform);
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
            Instantiate(m_missParticle, m_knife);
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, m_knife.transform);
        }
    }

    public override void EndIngredient()
    {
        if (m_cheeseCount >= 3)
        {
            m_knife.gameObject.SetActive(false);
            base.EndIngredient();
        }
    }
}
