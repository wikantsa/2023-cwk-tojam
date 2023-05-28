using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public int MaxHP;
    private BotManager bm;
    private int hp;

    void Start(){
        hp = MaxHP;
    }


    public void SetController(BotManager _bm){
        bm = _bm;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") {
            if (hp <= 0) {
                bm.KillMe(this.gameObject);
            } else {
                hp--;
            }

        }
    }
}
