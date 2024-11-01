using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public PoolType poolType;
        public GameObject prefab;
        public List<GameObject> objs = new List<GameObject>();
        private Dictionary<GameObject, Dictionary<System.Type, Component>> componentCache = new Dictionary<GameObject, Dictionary<System.Type, Component>>();

        public void InstantiateSomeObjs()
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.SetParent(PoolManager.Ins.transform);
                objs.Add(obj);
            }
        }

        public GameObject GetObject()
        {
            GameObject obj = null;
            foreach (GameObject go in objs)
            {
                if (go != null && !go.activeInHierarchy && !go.activeSelf)
                {
                    obj = go;
                    break;
                }
            }
            if (obj == null)
            {
                obj = Instantiate(prefab);
                objs.Add(obj);
            }
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            if (objs.Contains(obj))
            {
                obj.transform.SetParent(PoolManager.Ins.transform);
            }
        }

        public void ReturnAll()
        {
            foreach (var obj in objs)
            {
                ReturnToPool(obj);
            }
        }

        public T GetCachedComponent<T>(GameObject obj) where T : Component
        {
            if (!componentCache.ContainsKey(obj))
            {
                componentCache[obj] = new Dictionary<System.Type, Component>();
            }
            if (!componentCache[obj].ContainsKey(typeof(T)))
            {
                componentCache[obj][typeof(T)] = obj.GetComponent<T>();
            }
            return componentCache[obj][typeof(T)] as T;
        }

        public List<T> GetListObjects<T>() where T : Component
        {
            List<T> components = new List<T>();
            foreach (var obj in objs)
            {
                T component = GetCachedComponent<T>(obj);
                if (component != null)
                {
                    components.Add(component);
                }
            }
            return components;
        }

    }

    public List<Pool> pools = new List<Pool>();
    private Dictionary<PoolType, Pool> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<PoolType, Pool>();

        foreach (Pool pool in pools)
        {
            pool.InstantiateSomeObjs();
            poolDictionary.Add(pool.poolType, pool);
        }
    }

    public T Spawn<T>(PoolType poolType) where T : Component
    {
        if (!poolDictionary.ContainsKey(poolType))
        {
            Debug.LogWarning("Pool with type " + poolType.ToString() + " doesn't exist.");
            return null;
        }

        GameObject objToSpawn = poolDictionary[poolType].GetObject();
        objToSpawn.transform.position = Vector3.zero;
        objToSpawn.SetActive(true);
        return poolDictionary[poolType].GetCachedComponent<T>(objToSpawn);
    }

    public void Despawn(PoolType poolType, GameObject obj)
    {
        poolDictionary[poolType].ReturnToPool(obj);
    }

    public void DespawnAll()
    {
        foreach (var keyValue in poolDictionary)
        {
            Pool pool = keyValue.Value;
            pool.ReturnAll();
        }
    }

    public Pool GetPool(PoolType poolType)
    {
        foreach (var pool in pools)
        {
            if (pool.poolType == poolType)
            {
                return pool;
            }
        }
        return null;
    }
}

public enum PoolType
{
    ArrowTile, Dot, MinimapSquare, LevelUI
}
