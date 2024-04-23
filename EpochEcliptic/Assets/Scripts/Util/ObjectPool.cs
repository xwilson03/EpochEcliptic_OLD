using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    readonly List<Poolable> pool;
    readonly GameObject prefab;

    public ObjectPool(GameObject prefab, int startCapacity) {
        this.prefab = prefab;

        pool = new();
        ExpandPool(startCapacity);
    }

    public void Clear() {
        foreach(var obj in pool)
            obj.gameObject.SetActive(false);
    }

    public void Spawn<U>(U spawnData) {
        Poolable obj = null;

        int i;
        for (i = 0; i < pool.Count; i++) {
            if (!pool[i].gameObject.activeInHierarchy) {
                obj = pool[i];
                break;
            }
        }
        if (obj == null) obj = ExpandPool();
        
        obj.Init(i, spawnData);
    }

    public void Kill(object sender, int id) {
        pool[id].gameObject.SetActive(false);
    }

    Poolable ExpandPool(int count = 1) {
        int size = pool.Count;
        for (int i = 0; i < count; i++) {
            Poolable obj = Object.Instantiate(prefab).GetComponent<Poolable>();

            obj.OnDeath += Kill;
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
        return pool[size];
    }
}
