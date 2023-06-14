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

    private void Update()
    {
        if (!GameManager.instance.IsPlaying) return;

        if (Input.GetKeyDown(m_dropKey) || Input.GetMouseButtonDown(0))
            DropItem();
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

        GameManager.instance.AddAudioSourcers(m_itemClip, currentIngredient.transform);
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

        int lastIndex = m_handItems.Count - 1;
        GameManager.instance.AddAudioSourcers(m_itemClip, m_handItems[lastIndex].transform);
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

            m_handItems.RemoveAt(lastIndex);
        }
    }
}
