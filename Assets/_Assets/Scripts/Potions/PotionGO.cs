using UnityEngine;

public class PotionGO : MonoBehaviour
{
    public Potion potion;

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        Debug.Log(collision2D.transform.name);
        Destroy(gameObject);
    }
}
