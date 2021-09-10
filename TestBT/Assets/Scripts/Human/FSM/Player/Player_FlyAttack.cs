﻿using System.Collections;
using UnityEngine;

namespace FSM.Player
{
    public class Player_FlyAttack : State<PlayerController>
    {
        private readonly WaitForSeconds m_FlyAttackTimer = new WaitForSeconds(6.0f);
        private readonly int m_FlyAttack;
        private CharacterController m_CharacterController;
        private PSMeshRendererUpdater m_PSUpdater;
        private Animator m_Anim;
        private const float FORCE = 100f;
        private const float MASS = 3f;
        private Vector3 m_Impact = Vector3.zero;
        private bool m_IsGround;

        public Player_FlyAttack() : base("Base Layer.Skill.FlyAttack") => m_FlyAttack = Animator.StringToHash("FlyAttack");

        protected override void ONInitialized()
        {
            m_Anim = m_Owner.GetComponent<Animator>();
            m_CharacterController = m_Owner.GetComponent<CharacterController>();
        }

        
        public override void OnStateEnter()
        {
            
            var currentInstance = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackEffect);
            ObjPool.ObjectPoolInstance.ReturnObject(currentInstance, EPrefabsName.FlyAttackEffect, 4.0f);
            currentInstance.transform.SetParent(m_Owner.gameObject .transform);
            m_PSUpdater = currentInstance.GetComponent<PSMeshRendererUpdater>();
            m_PSUpdater.UpdateMeshEffect(m_Owner.gameObject);
            
            m_Owner.Stamina -= 40f;
            m_Owner.m_ActiveFlyAttack = false;
            m_Anim.SetTrigger(m_FlyAttack);
            AddImpact((m_Owner.transform.forward), FORCE);
            
            m_Owner.m_AttackLeftTrail.Activate();
            m_Owner.m_AttackRightTrail.Activate();
            m_Owner.StartCoroutine(FlyAttackCoolDown());

            var startDust = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackStartDust);
            startDust.transform.position = m_Owner.transform.position;
            ObjPool.ObjectPoolInstance.ReturnObject(startDust, EPrefabsName.FlyAttackStartDust, 1f);
        }

        public override void OnStateUpdate(AnimatorStateInfo stateInfo)
        {
            if (!(stateInfo.normalizedTime > 0.9f))
            {
                if (m_IsGround)
                {
                    Debug.LogError($"Player_FlyAttack m_IsGround is {m_IsGround}");
                }
                return;
            }
            m_IsGround = true;
            m_PSUpdater.IsActive = false;
            var arrow = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackArrow);
            ObjPool.ObjectPoolInstance.ReturnObject(arrow, EPrefabsName.FlyAttackArrow, 3f);
            arrow.transform.position = m_Owner.transform.position;
            m_Owner.m_CurState = EPlayerState.Idle;
        }

        public override void OnFixedUpdate(float deltaTime, AnimatorStateInfo stateInfo)
        {
            if (m_Impact.magnitude > 0.2)
            {
                m_CharacterController.Move(m_Impact * Time.deltaTime);
            }

            m_Impact = Vector3.Lerp(m_Impact, Vector3.zero, 5 * Time.deltaTime);
        }

        public override void OnStateExit()
        {
            m_Owner.m_AttackRightTrail.Deactivate();
            m_Owner.m_AttackLeftTrail.Deactivate();
            m_IsGround = false;
        }

        private IEnumerator FlyAttackCoolDown()
        {
            yield return m_FlyAttackTimer;
            m_Owner.m_ActiveFlyAttack = true;
        }
        
        private void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0)
            {
                dir.y = -dir.y;
            }
            m_Impact += dir.normalized * force / MASS;
        }
    }
}