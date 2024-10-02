using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackStrategy
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
                var dummy_fire = EffectTable.Instance.GetData(6).Dummy;
                Transform m_dummyFire = Utility.FindChildObject(gameObject, "dummy_fire").transform;

                //var dir = enemy.GetPlayer.transform.position - enemy.GetDummyFire.position;
                var dir = enemy.GetPlayer.transform.position - m_dummyFire.position;
                dir.y = 0f;

                //var effectPrefab = enemy.GetRangeAttackEffect;
                var effectPrefab = EffectPool.Instance.GetPrefabList["FX_Fireball_Shooting_Straight"];

                var effect = GameObject.Instantiate(effectPrefab);
                //effect.gameObject.transform.position = enemy.GetDummyFire.position;
                effect.gameObject.transform.position = m_dummyFire.position;
                effect.transform.forward = dir.normalized;

                var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
                collisionHandler.Initialize(enemy);
            }
        }
    }
}