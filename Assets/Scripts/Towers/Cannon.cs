using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTD.Projectiles;

namespace QTD.Towers
{
    [Serializable]
    public struct CannonLevelInfo
    {
        public int cost;
        public float attackInterval;
        public float range;
        public int sellPrice;
        public float projectileSpeed;
        public float projectileDamage;
    }

    public class Cannon : Tower
    {
        [Header("Levels")]
        [SerializeField]
        private List<CannonLevelInfo> _levelInfos = new List<CannonLevelInfo>();

        private CannonLevelInfo CurrentLevelInfo => _levelInfos[Mathf.Clamp(_level - 1, 0, _levelInfos.Count - 1)];

        private CannonLevelInfo NextLevelInfo => _levelInfos[Mathf.Clamp(_level, 0, _levelInfos.Count - 1)];

        public override bool IsMaxLevel => _level >= _levelInfos.Count;

        public override int InitialCost => _levelInfos[0].cost;

        public override int UpgradeCost => NextLevelInfo.cost;

        public override int SellPrice => CurrentLevelInfo.sellPrice;

        protected override float AttackInterval => CurrentLevelInfo.attackInterval;

        protected override float AttackRange => CurrentLevelInfo.range;

        protected override void LaunchProjectile(Enemy enemy)
        {
            CannonBall projectile = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<CannonBall>();

            // Update stats
            projectile.Speed = CurrentLevelInfo.projectileSpeed;
            projectile.Damage = CurrentLevelInfo.projectileDamage;

            projectile.Launch(enemy);
        }
    }
}
