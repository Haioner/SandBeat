using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip m_ArrivedFirstOrderClip;

    [Header("Customer")]
    [SerializeField] private Customer m_customerPrefab;
    [SerializeField] private Vector2 m_cooldownRange;
    [SerializeField] private AudioClip m_spawnClip;
    [HideInInspector] public List<Customer> m_customers = new List<Customer>();

    [Header("Positions")]
    [SerializeField] private Transform m_spawnPosition;
    [SerializeField] private Transform m_exitPosition;
    [SerializeField] private List<Transform> m_rowPositions;

    private bool isFirstCustomerInRow0Checked = false;
    private int m_maxCustomers = 4;
    private float m_spawnCooldown;

    private CustomerController m_controller;

    private void Start()
    {
        m_controller = GetComponent<CustomerController>();
        ResetSpawnCooldown();
    }

    private void Update()
    {
        if (!GameManager.instance.IsPlaying) return;

        if (m_spawnCooldown <= 0f && m_customers.Count < m_maxCustomers)
        {
            SpawnCustomer();
            ResetSpawnCooldown();
        }

        m_spawnCooldown -= Time.deltaTime;

        CheckFirstCustomer();
    }

    private void SpawnCustomer()
    {
        Customer newCustomer = Instantiate(m_customerPrefab, m_spawnPosition.position, Quaternion.identity);
        m_customers.Add(newCustomer);
        GameManager.instance.AddAudioSourcers(m_spawnClip, newCustomer.transform);
        UpdatePositions();
    }

    private void CheckFirstCustomer()
    {
        if (!isFirstCustomerInRow0Checked && m_customers.Count > 0)
        {
            float distance = Vector3.Distance(m_customers[0].transform.position, m_rowPositions[0].position);
            if (distance <= 0.1f)
            {
                isFirstCustomerInRow0Checked = true;
                GameManager.instance.AddAudioSourcers(m_ArrivedFirstOrderClip, m_customers[0].transform);
                m_controller.StartOrder();
            }
        }
    }

    private void UpdatePositions()
    {
        for (int i = 0; i < m_customers.Count; i++)
        {
            m_customers[i].SetPositionToMove(m_rowPositions[i].position);
        }
    }

    private void ResetSpawnCooldown() => m_spawnCooldown = Random.Range(m_cooldownRange.x, m_cooldownRange.y);

    public void RemoveCustomer()
    {
        if (m_customers.Count <= 0) return;

        m_customers[0].LeaveCustomer(m_exitPosition.position);
        isFirstCustomerInRow0Checked = false;
        m_customers.RemoveAt(0);
        UpdatePositions();
    }
}
