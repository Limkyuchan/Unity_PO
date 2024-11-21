using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackStrategy
{
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
                AudioManager.Instance.PlaySFX(AudioManager.Instance.m_rangeAttack);
            }
        }
    }

    public void SkillAttack_1(CharacterBase target) 
    {
        if (target is PlayerController player)
        {
            int attackCount = 2;
            float delay = 0.5f;

            // 스킬 애니메이션 시작
            player.GetAnimController.Play(PlayerAnimController.Motion.RangeAttack, false);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_rangeAttack);

            // 스킬 및 이펙트 데이터 불러오기
            var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
            var effectData = EffectTable.Instance.GetData(6);
            Transform m_dummyFire = Utility.FindChildObject(gameObject, effectData.Dummy).transform;

            StartCoroutine(CoSpawnAttack1Effect(player, skill, effectData, m_dummyFire, attackCount, delay));
            player.ResetSkillGauge();
        }
    }

    public void SkillAttack_2(CharacterBase target) 
    {
        if (target is PlayerController player)
        {
            // 스킬 애니메이션 시작
            player.GetAnimController.Play(PlayerAnimController.Motion.Skill3, false);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.m_rangeAttack);

            StartCoroutine(CoSpawnAttack2Effect(player, 0.46f));
        }
    }

    IEnumerator CoSpawnAttack1Effect(PlayerController player, SkillData skill, EffectData effectData, Transform dummyFire, int attackCount, float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        for (int i = 0; i < attackCount; i++)
        {
            var effect = EffectPool.Instance.Create(effectData.Prefabs[0]);

            // 위치, 방향 설정 (각도 편차)
            effect.transform.position = dummyFire.position;

            float angle = 0;
            if (i == 0)
            {
                angle = -5f;
            }
            else if (i == 1)
            {
                angle = 10f;
            }

            var rotation = Quaternion.Euler(0, angle, 0);
            var direction = rotation * transform.forward;
            effect.transform.forward = direction.normalized;

            var collisionHandler = effect.GetComponent<ParticleCollisionHandler>();
            collisionHandler.InitializePlayer(player, skill);
        }
    }

    IEnumerator CoSpawnAttack2Effect(PlayerController player, float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        player.GetVirtualEffectCam.SetActive(true);

        // 스킬 및 이펙트 데이터 불러오기
        var skill = SkillTable.Instance.GetSkillData(player.GetMotion);
        var effectData = EffectTable.Instance.GetData(7);

        // 주인공이 바라보는 방향으로 특정 거리 떨어진 위치 계산
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 5.0f;  // 5.0f 거리를 지정
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