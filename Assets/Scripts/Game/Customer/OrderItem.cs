using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OrderItem : MonoBehaviour
{
    [SerializeField] private Transform m_iconContent;
    [SerializeField] private TextMeshProUGUI m_recipeName;
    [SerializeField] private Image m_timerFill;
    [SerializeField] private Transform m_ingredientContent;
    [SerializeField] private Image m_ingredientImagePrefab;
    [SerializeField] private Sprite m_topBreadSprite;
    private List<IngredientSO> m_ingredients = new List<IngredientSO>();
    private float m_waitTimer;
    private float m_currentWaitTimer;

    private void Update()
    {
        UpdateTimer();
    }

    public void InitOrder(Recipes recipe)
    {
        m_recipeName.text = recipe.RecipeName;
        m_ingredients = recipe.RecipeList;
        m_waitTimer = recipe.WaitTime;
        m_currentWaitTimer = recipe.WaitTime;
        RecipeIcon();
        SpawnIngredientsIcons();
    }

    private void RecipeIcon()
    {
        float yOffset = 0f;

        for (int i = 0; i < m_ingredients.Count; i++)
        {
            Image ingredientIcon = Instantiate(m_ingredientImagePrefab, m_iconContent);
            ingredientIcon.sprite = m_ingredients[i].ingredientSprite;

            ingredientIcon.transform.localPosition += new Vector3(0f, yOffset, 0f);
            yOffset += 10f;
        }

        if (m_ingredients[0].ingredientType == IngredientType.Bread)
        {
            Image ingredientIcon = Instantiate(m_ingredientImagePrefab, m_iconContent);
            ingredientIcon.sprite = m_topBreadSprite;

            ingredientIcon.transform.localPosition += new Vector3(0f, yOffset, 0f);
        }
    }

    private void SpawnIngredientsIcons()
    {
        for (int i = 0; i < m_ingredients.Count; i++)
        {
            Image ingredientIcon = Instantiate(m_ingredientImagePrefab, m_ingredientContent);
            ingredientIcon.sprite = m_ingredients[i].ingredientSprite;
        }
    }

    private void UpdateTimer()
    {
        if (m_currentWaitTimer > 0)
            m_currentWaitTimer -= Time.deltaTime;
        else
            m_currentWaitTimer = 0;

        m_timerFill.fillAmount = Mathf.Clamp01(m_currentWaitTimer / m_waitTimer);
    }
}
