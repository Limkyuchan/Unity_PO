using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //            // ����Ʈ ���� �� ���� ����
    //            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);
    //            effect.gameObject.transform.position = m_dummyFire.position;
    //            effect.transform.forward = dir.normalized;

    //            var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
    //            collisionHandler.Initialize(enemy);
    //        }
    //    }
    //}

    float m_attackRange = 15f;

    public void AnimEvent_Attack(CharacterBase target)
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
                    // ����Ʈ ������ �ҷ�����
                    var effectData = EffectTable.Instance.GetData(6);
                    var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

                    // ����Ʈ ���� �� ���� ����
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
            // ��ų �� ����Ʈ ������ �ҷ�����
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var effectData = EffectTable.Instance.GetData(6);
            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

            // ����Ʈ ���� �� ���� ����
            Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;
            effect.gameObject.transform.position = m_dummyFire.position;
            var targetPosition = m_dummyFire.position + transform.forward * m_attackRange;
            var dir = targetPosition - m_dummyFire.position;
            dir.y = 0f;
            effect.transform.forward = dir.normalized;

            var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
            collisionHandler.InitializePlayer(player, skill);
        }
    }

    public void BasicAttack(CharacterBase target)
    {
        if (target is PlayerController player)
        {
            if (!player.IsPlayerAttack)
            {
                player.GetAnimController.Play(PlayerAnimController.Motion.RangeAttack);
            }
        }
    }

    public void SkillAttack_1(CharacterBase target) 
    {
        if (target is PlayerController player)
        {
            int attackCount = 3;
            float angleOffset = 10f;

            // ��ų �ִϸ��̼� ����
            player.GetAnimController.Play(PlayerAnimController.Motion.RangeAttack, false);

            // ��ų �� ����Ʈ ������ �ҷ�����
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var effectData = EffectTable.Instance.GetData(6);
            Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;

            for (int i = 0; i < attackCount; i++)
            {
                var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

                // ��ġ, ���� ���� (���� ����)
                effect.transform.position = m_dummyFire.position;

                var angle = (i - 1) * angleOffset;  // -10, 0, +10�� ������ �߻�
                var rotation = Quaternion.Euler(0, angle, 0);
                var direction = rotation * transform.forward;
                effect.transform.forward = direction.normalized;

                var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
                collisionHandler.InitializePlayer(player, skill);
            }
        }
    }

    public void SkillAttack_2(CharacterBase target) 
    {
        if (target is PlayerController player)
        {
            // ��ų �ִϸ��̼� ����
            player.GetAnimController.Play(PlayerAnimController.Motion.Skill3, false);

            // ��ų �� ����Ʈ ������ �ҷ�����
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var effectData = EffectTable.Instance.GetData(7);

            // ���ΰ��� �ٶ󺸴� �������� Ư�� �Ÿ� ������ ��ġ ���
            Vector3 spawnPosition = player.transform.position + player.transform.forward * 5.0f;  // 5.0f �Ÿ��� ����
            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);
            effect.transform.position = spawnPosition;
            effect.transform.forward = player.transform.forward;

            BoxCollider boxCollider = effect.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }

            var collisionHandler = effect.GetComponent<EffectCollisionHandler>();
            if (collisionHandler == null)
            {
                collisionHandler = effect.AddComponent<EffectCollisionHandler>();
            }
            collisionHandler.InitializePlayer(player, skill);
        }
    }
}