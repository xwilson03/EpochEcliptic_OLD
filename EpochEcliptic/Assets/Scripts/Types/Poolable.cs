using UnityEngine;

public abstract class Poolable : MonoBehaviour {
    public int id;

    public abstract void Init<T>(int id, T spawnData);
    protected abstract void Die();
}