using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    public int Health { get; set; }

    public GameObject _acidEffectPrefab;
    public override void Init()
    {
        base.Init();
        Health = base.health;
    }
    public override void Movement()
    {
        //base.Movement();
    }
    public void Damage()
    {
        if (isDead == true)
            return;

        Health--;
        if (Health < 1)
        {
            isDead = true;
            anim.SetTrigger("Death");
            // Instantiate the diamond prefab at the enemy's position
            GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
            diamond.GetComponent<Diamond>().gems = base.gems;

            // Assume death animation is 1.5 seconds long
            float deathAnimationTime = 7f;
            Destroy(gameObject, deathAnimationTime);
        }
    }
    public void Attack()
    {
        Instantiate(_acidEffectPrefab, transform.position, Quaternion.identity);
    }
}