using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEditor;
using QTD;
using QTD.Towers;

namespace Tests
{
    [TestFixture]
    public class TowerTest
    {
        private const string TOWER_PREFAB_PATH = "Assets/Prefabs/Towers/Cannon.prefab";
        private readonly GameObject _towerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TOWER_PREFAB_PATH);

        private const string ENEMY_PREFAB_PATH = "Assets/Prefabs/Enemies/Minion 1.prefab";
        private readonly GameObject _enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ENEMY_PREFAB_PATH);

        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        [UnityTest]
        public IEnumerator ShouldDetectNearbyEnemies()
        {
            Tower tower = Object.Instantiate(_towerPrefab, Vector2.zero, Quaternion.identity).GetComponent<Tower>();
            Enemy enemy = Object.Instantiate(_enemyPrefab, Vector2.zero, Quaternion.identity).GetComponent<Enemy>();
            yield return null;
            Assert.AreEqual(enemy, tower.GetClosestEnemy());
        }

        [UnityTest]
        public IEnumerator ShouldDeleteSelfWhenSell()
        {
            Tower tower = Object.Instantiate(_towerPrefab, Vector2.zero, Quaternion.identity).GetComponent<Tower>();
            tower.Sell();
            yield return null;
            Assert.True(tower == null);
        }
    }
}
