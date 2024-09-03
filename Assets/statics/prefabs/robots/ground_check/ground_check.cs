using UnityEngine;
namespace Robots {

public class ground_check : MonoBehaviour {
    private bool isGrounded = false;
    public LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    void Update() {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
    }

    public bool IsGrounded() {
        return isGrounded;
    }
}
}