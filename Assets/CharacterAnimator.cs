using UnityEngine;
using static PlayerController;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Transform offset;
    [SerializeField] float xRightOffset;
    [SerializeField] float xLeftOffset;
    [SerializeField] float wallSlideOffset = 0.174f; //-0.484   //0.174

    private bool left;
    public void UpdateAnimator(int moveX, JumpState jumpState, bool isGroundedThisFixedUpdate)
    {
        if (moveX < 0) left = true;
        if (moveX > 0) left = false;
        animator.SetInteger("MoveDir", moveX);
        animator.SetBool("TouchingGround", isGroundedThisFixedUpdate);
        animator.SetInteger("JumpState", (int)jumpState);
        renderer.flipX = left;

        float xOffset;
        if (left) 
            xOffset = jumpState == JumpState.WallSlide ? xLeftOffset - 0.174f : xLeftOffset;
        else 
            xOffset = jumpState == JumpState.WallSlide ? xRightOffset + 0.174f : xRightOffset;

        offset.localPosition = new Vector3(xOffset, offset.localPosition.y, offset.localPosition.z);
    }
}
