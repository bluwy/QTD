using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace QTD.Projectiles
{
    public class CannonBall : Projectile
    {
        public override void Launch(Enemy enemy)
        {
            float distanceBetween = (transform.position - enemy.transform.position).magnitude;
            float projectileTravelDuration = distanceBetween / Speed;

            // Get projected enemy position after time
            // TODO: This position isn't accurate, because the distanceBetween would be different
            // after travelling this targetPos. The better method is to get the derirative.
            Vector2 targetPos = enemy.GetPosition(projectileTravelDuration);

            transform
                .DOMove(targetPos, Speed)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    // Make sure enemy is still alive
                    if (enemy is object)
                        enemy.ReceiveDamage(Damage);

                    Destroy(gameObject);
                })
                .Play();
        }
    }
}
