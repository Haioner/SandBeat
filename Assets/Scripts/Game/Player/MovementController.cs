using UnityEngine.UI;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_currentSpeed = 10f;
    [SerializeField] private float m_acceleration = 45f;
    [SerializeField] private float m_deceleration = 35f;
    public bool CanMove { get; private set; } = true;

    [Header("Flip")]
    [SerializeField] private Transform m_flipTransform;

    [Header("Dash")]
    [SerializeField] private Slider m_dashCooldownSlider;
    [SerializeField] private float m_dashForce = 5f;
    [SerializeField] private float m_dashCooldown = 1f;
    public bool m_canDash = true;
    private float m_dashTime = 0.2f;
    private bool m_isDashing;
    private float m_initialDashTime;
    private float m_initialDashCooldown;
    private Vector3 m_mousePosition;
    private Vector2 m_dashDirection;

    private Camera m_camera;
    private Rigidbody2D rb;
    private Vector2 m_inputMovement;

    private void Awake()
    {
        m_camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        m_initialDashTime = m_dashTime;
        m_initialDashCooldown = m_dashCooldown;
        m_dashCooldown = 0;
    }

    private void Update() => Dash();

    private void FixedUpdate()
    {
        Movement();
        ApplyDash();
    }

    #region Move
    public void SetCanMove(bool canMove)
    {
        CanMove = canMove;
        if (!canMove) rb.velocity = Vector2.zero;
    }

    private void Movement()
    {
        if (!CanMove) return;

        m_inputMovement.x = Input.GetAxisRaw("Horizontal");
        m_inputMovement.y = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(m_inputMovement.x, m_inputMovement.y).normalized;
        Acceleration_Deceleration(rb, move);
        Flip();
    }

    private Rigidbody2D Acceleration_Deceleration(Rigidbody2D _rb, Vector2 move)
    {
        if (move.magnitude > 0)
            _rb.velocity += move * m_acceleration * Time.deltaTime;
        else
            _rb.velocity = Vector2.MoveTowards(_rb.velocity, Vector2.zero, m_deceleration * Time.deltaTime);

        if (_rb.velocity.magnitude > m_currentSpeed)
            _rb.velocity = _rb.velocity.normalized * m_currentSpeed;
        return _rb;
    }

    private void Flip()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 characterPos = m_camera.WorldToScreenPoint(transform.position);
        if(mousePos.x > characterPos.x)
            m_flipTransform.localScale = new Vector3(Mathf.Abs(m_flipTransform.localScale.x), m_flipTransform.localScale.y, m_flipTransform.localScale.z);
        else
            m_flipTransform.localScale = new Vector3(-Mathf.Abs(m_flipTransform.localScale.x), m_flipTransform.localScale.y, m_flipTransform.localScale.z);
    }
    #endregion

    #region Dash
    private void UpdateDashSlider() => m_dashCooldownSlider.value = m_dashCooldown / m_initialDashCooldown;

    private void Dash()
    {
        if (!CanMove || !m_canDash) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_canDash = false;
            CanMove = false;
            m_isDashing = true;
            rb.velocity = Vector3.zero;

            m_mousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);
            m_dashDirection = (m_mousePosition - transform.position).normalized;

            m_dashCooldown = m_initialDashCooldown;
            m_dashTime = m_initialDashTime;
        }
    }

    private void ApplyDash()
    {
        if (m_isDashing)
        {
            float dashMagnitude = m_dashForce / m_dashDirection.magnitude;
            rb.AddForce(m_dashDirection * dashMagnitude, ForceMode2D.Impulse);
            m_dashTime -= Time.deltaTime;

            if (m_dashTime <= 0)
            {
                CanMove = true;
                m_isDashing = false;
            }
        }

        m_dashCooldown -= Time.deltaTime;
        if (m_dashCooldown <= 0)
        {
            m_canDash = true;
            m_dashCooldown = 0;
        }
        UpdateDashSlider();
    }
    #endregion
}
