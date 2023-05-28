using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public abstract void Activate(Transform spawner);

    public abstract void Deactivate();
}