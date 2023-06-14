using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private KeyCode m_dropKey;
    [SerializeField] private Transform m_handPosition;
    private GameObject m_currentItem;

    private void Update()
    {
        if (Input.GetKey(m_dropKey))
            DropItem();
    }

    public void SpawnIngredient(IngredientSO ingredient)
    {
        ItemIngredient currentIngredient = Instantiate(ingredient.IngredientPrefab, m_handPosition);
        currentIngredient.InitItem(ingredient);
        currentIngredient.CanInteract = false;
        currentIngredient.DestroyInteractCanvas();
        m_currentItem = currentIngredient.gameObject;
    }

    public void TakeItem(GameObject newItem)
    {
        DropItem();
        newItem.transform.SetParent(m_handPosition);
        newItem.transform.position = m_handPosition.position;
        ItemIngredient newIngredient = newItem.GetComponent<ItemIngredient>();
        newIngredient.CanInteract = false;
        newIngredient.DestroyInteractCanvas();
        m_currentItem = newItem;
    }

    public void DropItem()
    {
        if(m_currentItem != null)
        {
            m_currentItem.transform.position = transform.position;
            m_currentItem.GetComponent<ItemIngredient>().CanInteract = true;
            m_currentItem.transform.SetParent(null);
            m_currentItem = null;
        }
    }
}
