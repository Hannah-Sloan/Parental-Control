using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivAnimator : MonoBehaviour
{
    [Serializable]
    public struct JumpStateToSprite
    {
        public PlayerController.JumpState jumpState;
        public Sprite sprite;
    }
    public JumpStateToSprite[] sprites;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform squashStretchTool;
    [Range(0, 360)]
    [SerializeField] private float quickTurnTilt;
    [Range(0, 1)]
    [SerializeField] private float quickTurnStretch;

    private SpriteRenderer spriteRenderer;
    private Dictionary<PlayerController.JumpState, Sprite> spritesDic;

    private void Start()
    {
        spritesDic = new Dictionary<PlayerController.JumpState, Sprite>();

        foreach (var sprite in sprites)
        {
            spritesDic.Add(sprite.jumpState, sprite.sprite);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        spriteRenderer.sprite = spritesDic[playerController.jumpState];

        if (playerController.jumpState == PlayerController.JumpState.QuickTurn)
        {
            squashStretchTool.transform.localRotation = Quaternion.Euler(Vector3.forward * quickTurnTilt * playerController.jumpStateDir);
            squashStretchTool.transform.localScale = Vector3.right + Vector3.forward + Vector3.up + (Vector3.up * quickTurnStretch);

            transform.localRotation = Quaternion.Euler(Vector3.forward * -quickTurnTilt * playerController.jumpStateDir);
            transform.localScale = Vector3.right + Vector3.forward + Vector3.up - (Vector3.up * quickTurnStretch);
        }
        else
        {
            squashStretchTool.transform.localRotation = Quaternion.Euler(Vector3.zero);
            squashStretchTool.transform.localScale = Vector3.one;

            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

        if(playerController.jumpStateDir == 1)
            spriteRenderer.flipX = true;
        else if(playerController.jumpStateDir == -1)
            spriteRenderer.flipX = false;
    }
}
