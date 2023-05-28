using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurvival : MonoBehaviour
{
    public enum Power
    {
        Bullets = 0,
        Missiles = 1,
        Laser = 2
    }

    public float BatteryPower = 100;
    public float InvulnerabilityWindow = 1f;
    public float BatteryDrainRate = 1f;
    public float ShootDrainRateMultiplier = 1f;
    public float BatteryPowerGainedOnEat = 50;

    public SkinnedMeshRenderer[] CharacterRenderers;
    Animator m_Animator;

    private float m_CurrentBatteryPower;
    public float GetCurrentBatteryPower => m_CurrentBatteryPower;

    private float m_InvulnerabilityCountdown;

    private ShootController m_ShootController;
    private Coroutine m_FlashRoutine;

    private Dictionary<Power, int> m_PowerLevels = new Dictionary<Power, int>()
    { 
        { Power.Bullets, 3 },
        { Power.Missiles, 3 },
        { Power.Laser, 3 }
    };

    public List<BaseShooter> m_Level0BulletsToRemove;
    public List<BaseShooter> m_Level1BulletsToRemove;
    public List<BaseShooter> m_Level2BulletsToRemove;

    public List<BaseShooter> m_Level0MissilesToRemove;
    public List<BaseShooter> m_Level1MissilesToRemove;
    public List<BaseShooter> m_Level2MissilesToRemove;

    public LaserShooter m_Laser;

    [HideInInspector]
    public Power PowerToEat;

    private int currentPowerLevel = 9;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentBatteryPower = BatteryPower;
        m_ShootController = GetComponent<ShootController>();
        m_Animator = GetComponentInChildren<Animator>();
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
            gameObject.SetActive(false);
            ExplosionManager.Instance.SpawnExplosion(transform.position, 3);
        }

        if (m_ShootController.IsShooting)
        {
            m_CurrentBatteryPower -= (BatteryDrainRate + currentPowerLevel * ShootDrainRateMultiplier) * Time.deltaTime;
        }
        else
        {
            m_CurrentBatteryPower -= BatteryDrainRate * Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (m_PowerLevels[PowerToEat] > 0)
            {
                ReducePowerLevel(PowerToEat);
                m_CurrentBatteryPower = Mathf.Clamp(m_CurrentBatteryPower + BatteryPowerGainedOnEat, 0, BatteryPower);

                if (m_PowerLevels[PowerToEat] == 0)
                {
                    if (isTherePowerToEat())
                        PowerToEat = GetNextValidPowerToEat(PowerToEat);
                }
            }
        }

        if (Input.GetButtonDown("Fire4"))
        {
            if (isTherePowerToEat())
                PowerToEat = GetNextValidPowerToEat(PowerToEat);
        }
    }

    bool isTherePowerToEat()
    {
        foreach (var power in m_PowerLevels.Keys)
        {
            if (m_PowerLevels[power] > 0)
                return true;
        }

        return false;
    }

    Power GetNextValidPowerToEat(Power power, bool alreadyLooped = false)
    {
        var nextPowerIndex = (int)power;
        nextPowerIndex++;

        if (nextPowerIndex > 2)
        {
            if (alreadyLooped)
            {
                return power;
            }
            else
            {
                nextPowerIndex = 0;
                alreadyLooped = true;
            }
        }

        var nextPower = (Power)nextPowerIndex;

        if (m_PowerLevels[nextPower] > 0)
            return nextPower;
        else
            return GetNextValidPowerToEat(nextPower, alreadyLooped);
    }

    void ReducePowerLevel(Power power)
    {
        currentPowerLevel--;
        m_PowerLevels[power]--;

        switch (power)
        {
            case Power.Bullets:
                if (m_PowerLevels[power] == 2)
                {
                    foreach (var shooter in m_Level2BulletsToRemove)
                    {
                        shooter.isDisabled = true;
                    }
                }
                else if (m_PowerLevels[power] == 1)
                {
                    foreach (var shooter in m_Level1BulletsToRemove)
                    {
                        shooter.isDisabled = true;
                    }
                }
                else if (m_PowerLevels[power] == 0)
                {
                    foreach (var shooter in m_Level0BulletsToRemove)
                    {
                        shooter.isDisabled = true;
                    }

                    m_Animator.SetBool("SingleArm", true);
                }
                break;
            case Power.Missiles:
                if (m_PowerLevels[power] == 2)
                {
                    foreach (var shooter in m_Level2MissilesToRemove)
                    {
                        shooter.isDisabled = true;
                    }
                }
                else if (m_PowerLevels[power] == 1)
                {
                    foreach (var shooter in m_Level1MissilesToRemove)
                    {
                        shooter.isDisabled = true;
                    }
                }
                else if (m_PowerLevels[power] == 0)
                {
                    foreach (var shooter in m_Level0MissilesToRemove)
                    {
                        shooter.isDisabled = true;
                    }
                }
                break;
            case Power.Laser:
                {
                    if (m_PowerLevels[power] > 0)
                    {
                        m_Laser.m_Laser.SetLaserLevel(m_PowerLevels[power]);
                    }
                    else if (m_PowerLevels[power] == 0)
                    {
                        m_Laser.isDisabled = true;
                    }
                }
                break;
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
            SetFlashStrength(0.5f);
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
