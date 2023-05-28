using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public int MaxHP;
    private BotManager bm;
    private int hp;

    public float ExplosionScale = 3;

    void Start(){
        hp = MaxHP;
    }


    public void SetController(BotManager _bm){
        bm = _bm;
    }

    public void DealDamage(int damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            if (ExplosionManager.Instance != null)
                ExplosionManager.Instance.SpawnExplosion(transform.position, ExplosionScale);
            bm.KillMe(this.gameObject);
        }
    }
}
