using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurvival : MonoBehaviour
{
    public float BatteryPower = 100;
    public float InvulnerabilityWindow = 1f;
    public float BatteryDrainRate = 1f;

    public SkinnedMeshRenderer[] CharacterRenderers;

    private float m_CurrentBatteryPower;
    private float m_InvulnerabilityCountdown;

    private ShootController m_ShootController;
    private Coroutine m_FlashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentBatteryPower = BatteryPower;
        m_ShootController = GetComponent<ShootController>();
    }

    void Update()
    {
        if(m_InvulnerabilityCountdown >= 0)
        {
            m_InvulnerabilityCountdown -= Time.deltaTime;

            if (m_InvulnerabilityCountdown <= 0)
            {
                if (m_FlashRoutine != null)
                    StopCoroutine(m_FlashRoutine);
                SetFlashStrength(0);
            }
        }
        if(m_CurrentBatteryPower <= 0)
        {
            Destroy(gameObject);
        }

        if (m_ShootController.IsShooting)
        {
            m_CurrentBatteryPower -= BatteryDrainRate * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        DamageSource source = collision.gameObject.GetComponent<DamageSource>();
        if(source != null && m_InvulnerabilityCountdown <= 0)
        {
            int damage = source.Damage;
            m_CurrentBatteryPower -= damage;
            m_InvulnerabilityCountdown = InvulnerabilityWindow;
            m_FlashRoutine = StartCoroutine(FlashCharacter());
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        DamageSource source = collider.gameObject.GetComponent<DamageSource>();
        if(source != null && m_InvulnerabilityCountdown <= 0)
        {
            int damage = source.Damage;
            m_CurrentBatteryPower -= damage;
            m_InvulnerabilityCountdown = InvulnerabilityWindow;
            m_FlashRoutine = StartCoroutine(FlashCharacter());
        }
    }

    IEnumerator FlashCharacter()
    {
        while (true)
        {
            SetFlashStrength(1);
            yield return new WaitForSeconds(0.1f);
            SetFlashStrength(0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SetFlashStrength(float strength)
    {
        foreach (var renderer in CharacterRenderers)
        {
            renderer.materials[0].SetFloat("_FlashStrength", strength);
        }
    }
}
