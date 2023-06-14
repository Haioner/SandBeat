using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private KeyCode m_dropKey;
    [SerializeField] private Transform m_handPosition;
    [SerializeField] private AudioSource m_dropSource;
    [SerializeField] private AudioSource m_itemSource;
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
        GameManager.instance.AddAudioSourcers(m_itemSource, m_currentItem.transform);
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
        GameManager.instance.AddAudioSourcers(m_itemSource, m_currentItem.transform);
    }

    public void DropItem()
    {
        if(m_currentItem != null)
        {
            GameManager.instance.AddAudioSourcers(m_dropSource, m_currentItem.transform);
            m_currentItem.transform.position = transform.position;
            m_currentItem.GetComponent<ItemIngredient>().CanInteract = true;
            m_currentItem.transform.SetParent(null);
            m_currentItem = null;
        }
    }
}
