using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour, IAttackStrategy
{
    public void Attack(EnemyController enemy)
    {
        if (!enemy.IsEnemyAttack)
        {
            enemy.SetState(EnemyController.AiState.Attack);
            enemy.transform.LookAt(enemy.GetPlayer.transform);
            enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
        }
        else if (enemy.IsEnemyAttack)
        {
            if (enemy.CheckArea(enemy.GetPlayer.transform, enemy.GetAttackDist))
            {
                var dir = enemy.GetPlayer.transform.position - enemy.GetDummyFire.position;
                dir.y = 0f;

                var effectPrefab = enemy.GetRangeAttackEffect;
                var effect = GameObject.Instantiate(effectPrefab);
                effect.gameObject.transform.position = enemy.GetDummyFire.position;
                effect.transform.forward = dir.normalized;

                var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
                collisionHandler.Initialize(enemy);
            }
        }
    }
}
