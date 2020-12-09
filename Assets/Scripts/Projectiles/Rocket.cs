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

        [SerializeField]
        private AudioClip _explosionSfx;

        public float SplashRadius { get; set; }

        public override void Launch(Enemy enemy)
        {
            // Get projected enemy position after time
            Vector2 targetPos = GetEnemyTargetPosition(enemy);

            transform.up = new Vector3(targetPos.x, targetPos.y, transform.position.z) - transform.position;

            // Move projectile towards enemy
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

                    // Explosion sound
                    AudioSource.PlayClipAtPoint(_explosionSfx, Camera.main.transform.position, 0.05f);

                    Destroy(gameObject);
                })
                .Play();
        }

        private Vector2 GetEnemyTargetPosition(Enemy enemy)
        {
            float distanceBetween = (transform.position - enemy.transform.position).magnitude;
            float projectileTravelDuration = distanceBetween / Speed;

            return enemy.GetPosition(projectileTravelDuration);
        }
    }
}