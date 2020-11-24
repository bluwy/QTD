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
        /// Unit per second
        /// </summary>
        public float Speed { get; set; }

        public float Damage { get; set; }

        public abstract void Launch(Enemy enemy);
    }
}
