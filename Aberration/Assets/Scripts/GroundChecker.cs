using UnityEngine;


    public class GroundChecker : MonoBehaviour
{
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

    public bool IsGrounded;

        void Update() {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);
        }

        private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, groundDistance, transform.position.z));
    }
}
