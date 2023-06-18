using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StarOrder : MonoBehaviour
{
    [SerializeField] private List<Image> m_starsList = new List<Image>();

    public void SetStars(int score)
    {
        for (int i = 0; i < score; i++)
        {
            m_starsList[i].gameObject.SetActive(true);
        }
    }

    public void DestroyStars() => Destroy(gameObject);
}
