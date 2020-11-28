using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask _enemyLayer;

        /// <summary>
        /// Move speed in units per second
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Damage on impact
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Launch this projectile towards enemy
        /// </summary>
        public abstract void Launch(Enemy enemy);
    }
}
