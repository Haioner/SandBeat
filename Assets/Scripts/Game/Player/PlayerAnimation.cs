using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Vector2 m_inputMovement;

    void Update() => WalkIdleAnimation();

    private void WalkIdleAnimation()
    {
        if (!GameManager.instance.PlayerMovement.CanMove) return;

        m_inputMovement.x = Input.GetAxisRaw("Horizontal");
        m_inputMovement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Walk", m_inputMovement.magnitude);
    }
}
