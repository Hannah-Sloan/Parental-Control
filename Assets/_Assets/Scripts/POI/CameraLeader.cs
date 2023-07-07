using UnityEngine;

public class CameraLeader : MonoBehaviour
{
    public Rigidbody2D body;

    [Range(0,1)]
    public float scaling = 0.08620689655f;

    private void Update()
    {
        transform.localPosition = body.velocity * body.velocity * scaling * ((Mathf.Sign(body.velocity.x) * Vector2.right) + (Mathf.Sign(body.velocity.y) * Vector2.up));
    }
}
