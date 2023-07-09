using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float speed;

    private void Update()
    {
        var direction = Mathf.Sign(((target.position - transform.position).normalized).x);
        renderer.flipX = direction > 0;
        rigidbody.velocity = new Vector2(direction * speed, rigidbody.velocity.y);
    }
}
