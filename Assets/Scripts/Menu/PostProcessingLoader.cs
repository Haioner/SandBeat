using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class PostProcessingLoader : MonoBehaviour
{
    [SerializeField] private List<Volume> m_Volume;
    private int data_PostProcessing = 1;

    private void Start()
    {
        if (PlayerPrefs.HasKey("postprocessing"))
            data_PostProcessing = PlayerPrefs.GetInt("postprocessing");
        else
            data_PostProcessing = 1;

        if (data_PostProcessing == 1)
            SetPostState(true);
        else
            SetPostState(false);
    }

    public void SetPostState(bool state)
    {
        for (int i = 0; i < m_Volume.Count; i++)
            m_Volume[i].enabled = state;
    }
}