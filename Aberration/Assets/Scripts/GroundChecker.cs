using UnityEngine;


    public class GroundChecker : MonoBehaviour
{
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

    public bool IsGrounded;
    public RaycastHit hit;
    public float Ypos;
    public RaycastHit hitY;
    void Update() {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out hit, groundDistance, groundLayers);
            Physics.SphereCast(transform.position, 1, Vector3.down, out hitY, 1, groundLayers);
        Ypos = hitY.point.y;
    }

        private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, groundDistance, transform.position.z));
    }
}
