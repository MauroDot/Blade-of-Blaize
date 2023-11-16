using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiantHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable player = collision.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
