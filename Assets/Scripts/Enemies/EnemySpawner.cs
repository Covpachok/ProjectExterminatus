using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Serializable]
        struct ConveyorItem
        {
            [SerializeField] private int _id;
            [SerializeField] private float _delayBeforeNext;
            [SerializeField] private bool _randomSpawnPosition;
            [SerializeField] private Vector3 _spawnPosition;
            [SerializeField] private Vector3 _maxSpawnPosition;

            public int ID => _id;
            public float DelayBeforeNext => _delayBeforeNext;
            public Vector3 SpawnPosition => _spawnPosition;
            public Vector3 MaxSpawnPosition => _maxSpawnPosition;
            public bool RandomSpawnPosition => _randomSpawnPosition;
        }

        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private ConveyorItem[] _conveyor;

        private void Start()
        {
            var json = JsonConvert.SerializeObject(_conveyor, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            });
            print(json);

            StartCoroutine(SpawnFromConveyor());
        }

        IEnumerator SpawnFromConveyor()
        {
            yield return new WaitForSeconds(2);
            foreach (var item in _conveyor)
            {
                var enemy = Instantiate(_enemies[item.ID]);

                if (!item.RandomSpawnPosition)
                    enemy.transform.position = item.SpawnPosition;
                else
                    enemy.transform.position = Utils.Vector3Lerp(item.SpawnPosition, item.MaxSpawnPosition, Random.value);

                yield return new WaitForSeconds(item.DelayBeforeNext);
            }
        }
    }
}