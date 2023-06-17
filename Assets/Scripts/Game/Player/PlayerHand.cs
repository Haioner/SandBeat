using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private KeyCode m_dropKey;
    [SerializeField] private Transform m_handPosition;
    [SerializeField] private AudioClip m_dropClip;
    [SerializeField] private AudioClip m_itemClip;
    [SerializeField] private List<ItemIngredient> m_handItems = new List<ItemIngredient>();
    [SerializeField] private GameObject m_topBreadSprite;
    private GameObject m_currentTopBread;

    private void Update()
    {
        if (!GameManager.instance.IsPlaying) return;

        if (Input.GetKeyDown(m_dropKey) || Input.GetMouseButtonDown(1))
            DropItem();
    }

    public List<IngredientSO> GetIngredients()
    {
        List<IngredientSO> ingredients = new List<IngredientSO>();
        foreach (var item in m_handItems)
        {
            ingredients.Add(item.GetIngredient());
        }
        return ingredients;
    }

    public void SpawnIngredient(IngredientSO ingredient)
    {
        if (m_handItems.Count >= 3) DropItem();

        ItemIngredient currentIngredient = Instantiate(ingredient.IngredientPrefab, m_handPosition);
        Vector3 newPos = currentIngredient.transform.position;
        newPos.y += 0.15f * m_handItems.Count;
        currentIngredient.transform.position = newPos;

        currentIngredient.SetColliderActive(false);
        currentIngredient.DestroyInteractCanvas();
        currentIngredient.InitItem(ingredient);
        m_handItems.Add(currentIngredient);
        UpdateOrderInLayer(currentIngredient.gameObject, m_handItems.Count + 1);

        GameManager.instance.AddAudioSourcers(m_itemClip, currentIngredient.transform);
        CheckFull();
    }

    public void TakeItem(ItemIngredient newItem)
    {
        if (m_handItems.Count >= 3) return;

        newItem.SetColliderActive(false);
        newItem.transform.SetParent(m_handPosition);
        newItem.transform.eulerAngles = Vector2.zero;
        newItem.transform.localScale = new Vector3(1, 1, 1);

        Vector3 newPos = m_handPosition.position;
        newPos.y += 0.15f * m_handItems.Count;
        newItem.transform.position = newPos;

        newItem.DestroyInteractCanvas();
        m_handItems.Add(newItem);
        UpdateOrderInLayer(newItem.gameObject, m_handItems.Count + 1);

        int lastIndex = m_handItems.Count - 1;
        GameManager.instance.AddAudioSourcers(m_itemClip, m_handItems[lastIndex].transform);
        CheckFull();
    }

    public void DropItem()
    {
        if(m_handItems.Count > 0 && GameManager.instance.PlayerMovement.CanMove)
        {
            int lastIndex = m_handItems.Count - 1;
            ItemIngredient itemToDrop = m_handItems[lastIndex];

            GameManager.instance.AddAudioSourcers(m_dropClip, m_handItems[lastIndex].transform);

            itemToDrop.transform.SetParent(null);
            itemToDrop.transform.position = transform.position;
            itemToDrop.transform.eulerAngles = Vector2.zero;
            itemToDrop.transform.localScale = new Vector3(1, 1, 1);
            itemToDrop.SetColliderActive(true);
            UpdateOrderInLayer(itemToDrop.gameObject, 0);

            m_handItems.RemoveAt(lastIndex);
            CheckFull();
        }
    }

    private void UpdateOrderInLayer(GameObject gameObject,int order)
    {
        SpriteRenderer currentRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        currentRenderer.sortingOrder = order;
    }

    private void CheckFull()
    {
        if (m_handItems.Count >= 3 && m_handItems[0].GetIngredient().ingredientType == IngredientType.Bread)
        {
            GameObject currentBread = Instantiate(m_topBreadSprite, m_handPosition);
            Vector3 newPos = currentBread.transform.position;
            newPos.y += 0.15f * m_handItems.Count;
            currentBread.transform.position = newPos;
            m_currentTopBread = currentBread;
        }
        else if (m_handItems.Count < 3)
        {
            Destroy(m_currentTopBread);
            m_currentTopBread = null;
        }
    }

    public void ClearHand()
    {
        for (int i = m_handItems.Count -1; i >= 0; i--)
        {
            Destroy(m_handItems[i].gameObject);
            m_handItems.RemoveAt(i);
        }
        CheckFull();
    }
}
