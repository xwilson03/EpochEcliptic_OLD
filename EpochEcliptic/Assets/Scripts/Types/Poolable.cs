using System;
using UnityEngine;

public abstract class Poolable : MonoBehaviour {
    public event EventHandler<int> OnDeath = delegate {};
    public int id;

    public abstract void Init<T>(int id, T spawnData);
    protected void Die() {
        OnDeath.Invoke(this, id);
    }
}