using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public int sceneLoad = 0;

    [Range(-1,1)]
    public float saveZone;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!Mathf.Approximately(collision.attachedRigidbody.velocity.normalized.ScalarProjection(-transform.up), 0))
            {
                var angle = Vector2.Dot(collision.attachedRigidbody.velocity.normalized, -transform.up);
                if (angle < saveZone) 
                { 
                    SceneManager.LoadScene(sceneLoad);
                    PlayerDeath.Instance.PlayerDeathEvent?.Invoke();
                }
            }
        }
    }
}
