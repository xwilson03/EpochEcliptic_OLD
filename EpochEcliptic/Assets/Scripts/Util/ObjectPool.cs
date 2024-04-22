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

    public Poolable Spawn<U>(U spawnData) {
        Poolable obj = null;

        int i;
        for (i = 0; i < pool.Count; i++) {
            obj = pool[i];
            if (!obj.gameObject.activeInHierarchy) break;
        }

        if (obj == null) obj = ExpandPool();
        obj.Init(i, spawnData);
        
        return obj;
    }

    public void Kill(int id) {
        pool[id].gameObject.SetActive(false);
    }

    Poolable ExpandPool(int count = 1) {
        int size = pool.Count;
        for (int i = 0; i < count; i++)
            pool.Add(Object.Instantiate(prefab).GetComponent<Poolable>());
        return pool[size];
    }
}
