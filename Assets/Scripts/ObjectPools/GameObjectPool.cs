using System;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPools
{
    public class GameObjectPool: MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _maxPoolSize;
        [SerializeField] private bool _spawn;
        
        public ObjectPool<GameObject> Pool { get; private set; }

        public virtual void Start()
        {
            CreatePool();
        }

        public void Update()
        {
            if (_spawn)
            {
                Pool.Get();
                _spawn = false;
            }
        }

        protected void CreatePool()
        {
            Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                OnDestroyPooledObject, true, _maxPoolSize);
        }

        protected virtual GameObject CreatePooledItem()
        {
            var go = Instantiate(_prefab, gameObject.transform, true);
            go.GetComponent<Enemy>().TEMPPOOL = this;
            return go;
        }

        protected virtual void OnTakeFromPool(GameObject go)
        {
            go.SetActive(true);
        }

        protected virtual void OnReturnedToPool(GameObject go)
        {
            go.SetActive(false);
        }

        protected virtual void OnDestroyPooledObject(GameObject go)
        {
            Destroy(go);
        }

    }
}