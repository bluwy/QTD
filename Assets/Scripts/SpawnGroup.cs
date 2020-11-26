using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD
{
    [Serializable]
    public class SpawnGroup
    {
        [SerializeField]
        private GameObject _enemy;

        [SerializeField]
        private int _count;

        [SerializeField]
        private float _interval;

        [SerializeField]
        private float _spawnDuration;

        public int Count => _count;

        public float Interval => _interval;

        public float SpawnDuration => _spawnDuration;

        public SpawnGroup(GameObject enemy, int count, float interval, float spawnDuration)
        {
            _enemy = enemy;
            _count = count;
            _interval = interval;
            _spawnDuration = spawnDuration;
        }

        public IEnumerator Spawn()
        {
            for (int i = 0; i < _count; i++)
            {
                List<Vector2> path = PathManager.instance.GetRandomPath();

                Enemy enemy = GameObject
                    .Instantiate(_enemy, path[0], Quaternion.identity)
                    .GetComponent<Enemy>();

                enemy.SetPath(path);
                enemy.StartWalking();

                yield return new WaitForSeconds(_interval);
            }
        }
    }
}
