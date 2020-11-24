using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTD.Projectiles;

namespace QTD.Towers
{
    [Serializable]
    public struct RocketLauncherLevelInfo
    {
        public int cost;
        public float attackInterval;
        public float range;
        public int sellPrice;
        public float projectileSpeed;
        public float projectileDamage;
        public float projectileSplashRadius;
    }

    public class RocketLauncher : Tower
    {
        [Header("Levels")]
        [SerializeField]
        private List<RocketLauncherLevelInfo> _levelInfos = new List<RocketLauncherLevelInfo>();

        private RocketLauncherLevelInfo CurrentLevelInfo => _levelInfos[Mathf.Clamp(_level - 1, 0, _levelInfos.Count - 1)];

        private RocketLauncherLevelInfo NextLevelInfo => _levelInfos[Mathf.Clamp(_level, 0, _levelInfos.Count - 1)];

        public override bool IsMaxLevel => _level >= _levelInfos.Count;

        public override int InitialCost => _levelInfos[0].cost;

        public override int UpgradeCost => NextLevelInfo.cost;

        public override int SellPrice => CurrentLevelInfo.sellPrice;

        protected override float AttackInterval => CurrentLevelInfo.attackInterval;

        protected override float AttackRange => CurrentLevelInfo.range;

        protected override void LaunchProjectile(Enemy enemy)
        {
            Rocket projectile = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Rocket>();

            projectile.Speed = CurrentLevelInfo.projectileSpeed;
            projectile.Damage = CurrentLevelInfo.projectileDamage;
            projectile.SplashRadius = CurrentLevelInfo.projectileSplashRadius;

            projectile.Launch(enemy);
        }
    }
}
