using UnityEngine;

public class POI_Camera : MonoBehaviour
{
    [Range(0,100)]
    public float speedXMin;
    [Range(0, 100)]
    public float speedXMax;
    [Range(0, 100)]
    public float speedYMin;
    [Range(0, 100)]
    public float speedYMaxUp;
    [Range(0, 100)]
    public float speedYMaxDown;
    [Space(20)]
    [Range(0, 1)]
    public float leadAheadX;
    [Range(0, 1)]
    public float leadAheadY;

    private new Rigidbody2D rigidbody;

    public bool ovverideYlock = false;

    private void Update()
    {
        if(!rigidbody)
            rigidbody = PlayerController.Instance.GetComponent<Rigidbody2D>();
        if (!rigidbody) return;

        var speedYMax = rigidbody.velocity.y > 0 ? speedYMaxUp : speedYMaxDown;

        var speedX = Mathf.Lerp(speedXMin, speedXMax, Mathf.Abs(rigidbody.velocity.x) / PlayerController.Instance.maxSpeedValue);
        var speedY = Mathf.Lerp(speedYMin, speedYMax, Mathf.Abs(rigidbody.velocity.y) / PlayerController.Instance.maxJumpVelocity);
        
        Vector2 target = POI_Manager.Instance.FindPoint();
        var xLookAhead = leadAheadX * rigidbody.velocity.x;
        var yLookAhead = leadAheadY * rigidbody.velocity.y;
        if (rigidbody.velocity.y > 0 && !PlayerController.Instance.IsGrounded() && !ovverideYlock) yLookAhead = 0;
        target = target + new Vector2(xLookAhead, yLookAhead);
            
        var x = Mathf.Lerp(transform.position.x, target.x, speedX * Time.deltaTime);
        var y = Mathf.Lerp(transform.position.y, target.y, speedY * Time.deltaTime);

        transform.localPosition = new Vector2(x, y);
    }
}