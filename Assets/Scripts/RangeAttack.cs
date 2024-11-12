using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackStrategy
{
    //public void Attack(EnemyController enemy)
    //{
    //    if (enemy.GetPlayer.PlayerCurHp <= 0)
    //    {
    //        enemy.IsEnemyAttack = false;
    //        enemy.SetState(EnemyController.AiState.Idle);
    //        return;
    //    }

    //    if (!enemy.IsEnemyAttack)
    //    {
    //        enemy.SetState(EnemyController.AiState.Attack);
    //        enemy.transform.LookAt(enemy.GetPlayer.transform);
    //        enemy.GetAnimator.Play(EnemyAnimController.Motion.Attack1);
    //    }
    //    else if (enemy.IsEnemyAttack)
    //    {
    //        if (enemy.CheckArea(enemy.GetPlayer.transform, enemy.GetAttackDist))
    //        {
    //            var effectData = EffectTable.Instance.GetData(6);
    //            Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;

    //            var dir = enemy.GetPlayer.transform.position - m_dummyFire.position;
    //            dir.y = 0f;

    //            // 이펙트 생성 및 방향 설정
    //            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);
    //            effect.gameObject.transform.position = m_dummyFire.position;
    //            effect.transform.forward = dir.normalized;

    //            var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
    //            collisionHandler.Initialize(enemy);
    //        }
    //    }
    //}

    float m_attackRange = 15f;
    //float m_effectDuration = 3f;

    public void Attack(CharacterBase target)
    {
        if (target is EnemyController enemy)
        {
            if (enemy.GetPlayer.PlayerCurHp <= 0)
            {
                enemy.IsEnemyAttack = false;
                enemy.SetState(EnemyController.AiState.Idle);
                return;
            }

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
                    // 이팩트 데이터 불러오기
                    var effectData = EffectTable.Instance.GetData(6);
                    var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

                    // 이펙트 생성 및 방향 설정
                    Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;
                    var dir = enemy.GetPlayer.transform.position - m_dummyFire.position;
                    dir.y = 0f;
                    effect.gameObject.transform.position = m_dummyFire.position;
                    effect.transform.forward = dir.normalized;

                    var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
                    collisionHandler.InitializeEnemy(enemy);
                }
            }
        }
        else if (target is PlayerController player)
        {
            // 스킬 및 이팩트 데이터 불러오기
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var effectData = EffectTable.Instance.GetData(6);
            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

            // 이팩트 생성 및 방향 설정
            Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;
            effect.gameObject.transform.position = m_dummyFire.position;
            var targetPosition = m_dummyFire.position + transform.forward * m_attackRange;
            var dir = targetPosition - m_dummyFire.position;
            dir.y = 0f;
            effect.transform.forward = dir.normalized;

            // 3초뒤 이팩트 자동 삭제
            //Destroy(effect.gameObject, m_effectDuration);

            var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
            collisionHandler.InitializePlayer(player, skill);
        }
    }
}