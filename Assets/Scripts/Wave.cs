using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD
{
    [Serializable]
    public class Wave
    {
        [SerializeField]
        [Tooltip("The wave name to display")]
        private string _name;

        [SerializeField]
        [Tooltip("The list of enemy groups to spawn")]
        private List<SpawnGroup> _spawnGroups;

        public string Name
        {
            get { return _name; }
        }

        public Wave(string name, List<SpawnGroup> spawnGroups)
        {
            _name = name;
            _spawnGroups = spawnGroups;
        }

        /// <summary>
        /// Spawn the wave.
        /// NOTE: caller is used for StartCoroutine only, ugly but it works lol
        /// </summary>
        public IEnumerator Spawn(MonoBehaviour caller)
        {
            for (int i = 0; i < _spawnGroups.Count; i++)
            {
                // Don't wait for spawn, but wait for its defined duration.
                // This allows us to stack spawn groups.
                caller.StartCoroutine(_spawnGroups[i].Spawn());
                yield return new WaitForSeconds(_spawnGroups[i].SpawnDuration);
            }
        }
    }
}
