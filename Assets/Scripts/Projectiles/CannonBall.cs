using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace QTD.Projectiles
{
    public class CannonBall : Projectile
    {
        [SerializeField]
        private AudioClip _shootSfx;

        public override void Launch(Enemy enemy)
        {
            // Get projected enemy position after time
            Vector2 targetPos = GetEnemyTargetPosition(enemy);

            transform
                .DOMove(targetPos, Speed)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnPlay(() =>
                {
                    AudioSource.PlayClipAtPoint(_shootSfx, Camera.main.transform.position, 0.3f);
                })
                .OnComplete(() =>
                {
                    // Make sure enemy is still alive
                    if (enemy is object)
                        enemy.ReceiveDamage(Damage);

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
