using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossGiant : Enemy, IDamageable
{
    public int Health { get; set; }
    public override void Init()
    {
        base.Init();
        Health = base.health;
    }
    public override void Movement()
    {
        base.Movement();        
    }
    public void Damage()
    {
        if (isDead == true)
            return;

        Debug.Log("MossGiant::Damage()");
        Health--;
        anim.SetTrigger("Hit");
        isHit = true;
        anim.SetBool("InCombat", true);

        if (Health < 1)
        {
            isDead = true;
            anim.SetTrigger("Death");
            // Instantiate the diamond prefab at the enemy's position
            GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
            diamond.GetComponent<Diamond>().gems = base.gems;

            // Assume death animation is 1.5 seconds long
            float deathAnimationTime = 1f;
            Destroy(gameObject, deathAnimationTime);
        }
    }
}
