using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour, Iinteractable
{
    [Header("Recipes")]
    [SerializeField] private RecipeSO m_recipes;
    private int currentRecipe;

    [Header("Order Canvas")]
    [SerializeField] private OrderItem m_orderItem;
    [SerializeField] private Transform m_orderCanvas;
    private OrderItem m_currentOrder;
    private float m_waitTimer;
    private bool m_canRemoveCustomer;

    [Header("Interact")]
    [SerializeField] private Transform m_canvasPos;
    [SerializeField] private GameObject m_interactCanvas;
    [SerializeField] private CinemachineTargetGroup m_targetGroup;
    [SerializeField] private AudioClip m_interactClip;
    private GameObject m_currentInteractCanvas;

    private CustomerSpawner m_spawner;

    private void Awake()
    {
        m_spawner = GetComponent<CustomerSpawner>();
    }

    private void Update()
    {
        if (!GameManager.instance.IsPlaying) return;

        UpdateOrderWaitTimer();
    }

    private void UpdateOrderWaitTimer()
    {
        if (m_waitTimer > 0)
        {
            m_waitTimer -= Time.deltaTime;
            m_canRemoveCustomer = true;
        }
        else
        {
            m_waitTimer = 0;
            CheckWaitTimer();
        }
    }

    private void CheckWaitTimer()
    {
        if(m_canRemoveCustomer)
        {
            m_canRemoveCustomer = false;
            m_spawner.RemoveCustomer();
            ClearOrder();
        }
    }

    public void StartOrder()
    {
        ClearOrder();
        RandomRecipe();

        OrderItem currentOrder = Instantiate(m_orderItem, m_orderCanvas);
        Recipes currentRecipeOrder = m_recipes.recipes[currentRecipe];
        currentOrder.InitOrder(currentRecipeOrder);
        m_currentOrder = currentOrder;
        m_waitTimer = currentRecipeOrder.WaitTime;
    }

    private void ClearOrder()
    {
        if (m_currentOrder == null) return;
        Destroy(m_currentOrder.gameObject);
        m_currentOrder = null;
    }

    private void RandomRecipe()
    {
        int randRecipe = Random.Range(0, m_recipes.recipes.Count);
        currentRecipe = randRecipe;
    }

    private void CheckIngredients()
    {
        List<IngredientSO> playerIngredients = GameManager.instance.playerHand.GetIngredients();
        Recipes customerRecipe = m_recipes.recipes[currentRecipe];

        //Check Ingredient in recipe
        int matchingIngredients = 0;
        foreach (IngredientSO playerIngredient in playerIngredients)
        {
            if (customerRecipe.RecipeList.Contains(playerIngredient))
                matchingIngredients++;
        }

        //Check Order
        bool isOrderCorrect = true;
        int recipeIndex = 0;
        foreach (IngredientSO playerIngredient in playerIngredients)
        {
            if (recipeIndex >= customerRecipe.RecipeList.Count || playerIngredient != customerRecipe.RecipeList[recipeIndex])
            {
                isOrderCorrect = false;
                break;
            }

            recipeIndex++;
        }

        //Check Wait Time
        float waitTimerPercentage = m_waitTimer / customerRecipe.WaitTime;
        int timerPoints = 0;
        if (matchingIngredients > 0 && isOrderCorrect)
            timerPoints = (waitTimerPercentage >= 0.5f) ? 1 : 0;

        //Calculate Score
        int matchScore = 0;
        if (matchingIngredients > 0 && matchingIngredients <= 2)
            matchScore = 1;
        else if(matchingIngredients >= 3)
            matchScore = 2;

        int score = matchScore + (isOrderCorrect ? 2 : 0) + timerPoints;

        GameManager.instance.SpawnStars(score, m_spawner.m_customers[0].transform);
    }

    #region Interact Methods
    public void Interact()
    {
        if (GameManager.instance.playerHand.GetIngredients().Count <= 0) return;
        if (m_currentOrder == null) return;

        GameManager.instance.AddAudioSourcers(m_interactClip, transform);
        CheckIngredients();
        GameManager.instance.playerHand.ClearHand();
        ClearOrder();
        m_spawner.RemoveCustomer();
    }

    public void InstantiateInteractCanvas()
    {
        if (GameManager.instance.playerHand.GetIngredients().Count <= 0) return;
        if (m_currentOrder == null) return;

        if (m_currentInteractCanvas != null) return;
        m_currentInteractCanvas = Instantiate(m_interactCanvas, m_canvasPos);

        m_targetGroup.m_Targets[2].target = transform;
        m_targetGroup.m_Targets[2].weight = 10;
        m_targetGroup.m_Targets[2].radius = 10;
    }

    public void DestroyInteractCanvas()
    {
        if (m_currentInteractCanvas == null) return;
        Destroy(m_currentInteractCanvas);
        m_currentInteractCanvas = null;

        m_targetGroup.m_Targets[2].weight = 0;
        m_targetGroup.m_Targets[2].radius = 0;
    }
    #endregion
}
