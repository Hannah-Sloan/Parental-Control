using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] new Rigidbody2D rigidbody;

    [SerializeField] float fallTime = .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            StartCoroutine(Fall());
    }

    private IEnumerator Fall() 
    {
        yield return new WaitForSeconds(fallTime);

        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
}
