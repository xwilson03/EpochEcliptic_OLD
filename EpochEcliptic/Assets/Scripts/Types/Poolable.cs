using System;
using UnityEngine;

public abstract class Poolable : MonoBehaviour {
    public event EventHandler<int> OnDeath = delegate {};
    public int id;

    public virtual void Init<T>(int id, T spawnData) {
        this.gameObject.SetActive(true);
    }

    protected void Die() {
        OnDeath.Invoke(this, id);
    }
}