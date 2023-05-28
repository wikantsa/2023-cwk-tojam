using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public int MaxHP;
    private BotManager bm;
    private int hp;

    public float ExplosionScale = 3;

    private float timer;

    public MeshRenderer[] CharacterRenderers;
    public SkinnedMeshRenderer[] SkinnedRenderers;

    void Start(){
        hp = MaxHP;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                SetFlashStrength(0);
            }
        }
    }


    public void SetController(BotManager _bm){
        bm = _bm;
    }

    public void DealDamage(int damageAmount)
    {
        hp -= damageAmount;

        timer = 0.1f;
        SetFlashStrength(0.5f);

        if (hp <= 0)
        {
            if (ExplosionManager.Instance != null)
                ExplosionManager.Instance.SpawnExplosion(transform.position, ExplosionScale);
            bm.KillMe(this.gameObject);
        }
    }

    void SetFlashStrength(float strength)
    {
        foreach (var renderer in CharacterRenderers)
        {
            renderer.materials[0].SetFloat("_FlashStrength", strength);
        }

        foreach (var renderer in SkinnedRenderers)
        {
            renderer.materials[0].SetFloat("_FlashStrength", strength);
        }
    }
}
