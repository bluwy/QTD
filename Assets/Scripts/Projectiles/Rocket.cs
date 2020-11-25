using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace QTD.Projectiles
{
    public class Rocket : Projectile
    {

        [SerializeField]
        private GameObject _explosion;
        public float SplashRadius { get; set; }

        public override void Launch(Enemy enemy)
        {
            float distanceBetween = (transform.position - enemy.transform.position).magnitude;
            float projectileTravelDuration = distanceBetween / Speed;

            // Get projected enemy position after time
            // TODO: This position isn't accurate, because the distanceBetween would be different
            // after travelling this targetPos. The better method is to get the derirative.
            Vector2 targetPos = enemy.GetPosition(projectileTravelDuration);

            transform.up = new Vector3(targetPos.x, targetPos.y, transform.position.z) - transform.position;

            transform
                .DOMove(targetPos, Speed)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    Collider2D[] collideds = Physics2D.OverlapCircleAll(targetPos, SplashRadius, _enemyLayer.value);

                    foreach (Collider2D collided in collideds)
                    {
                        // TODO: Damage reduction per distance
                        collided.GetComponent<Enemy>().ReceiveDamage(Damage);
                    }

                    // Explosion boom
                    GameObject explosion = Instantiate(_explosion, targetPos, Quaternion.identity);
                    explosion.transform.localScale = new Vector3(SplashRadius, SplashRadius, SplashRadius);

                    Destroy(gameObject);
                })
                .Play();
        }
    }
}