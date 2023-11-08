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
        Health--;
        if (Health < 1)
        {
            isDead = true;
            anim.SetTrigger("Death");
        }
    }
    public void Attack()
    {
        Instantiate(_acidEffectPrefab, transform.position, Quaternion.identity);
    }
}