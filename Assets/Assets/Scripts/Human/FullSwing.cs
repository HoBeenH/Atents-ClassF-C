﻿using System.Collections;
using UnityEngine;

namespace Human
{
    public sealed class FullSwing : State<PlayerController>
    {
        private readonly WaitForSeconds m_FullSwingTimer = new WaitForSeconds(7.0f);
        private GameObject m_RightCharge;
        private readonly int m_FullSwing;
        private GameObject m_LeftCharge;

        public FullSwing() : base("Base Layer.Skill.FullSwing")
        {
            m_FullSwing = Animator.StringToHash("FullSwing");
        }
 
        public override void Begin()
        {
            PlayerController.GetPlayerController.m_CurState = EPlayerState.FULL_SWING;
            PlayerController.GetPlayerController.m_Anim.SetTrigger(m_FullSwing);
            PlayerController.GetPlayerController.Stamina -= 40f;
            PlayerController.GetPlayerController.m_AttackLeftTrail.Activate();
            PlayerController.GetPlayerController. m_AttackRightTrail.Activate();
            PlayerController.GetPlayerController.m_ActiveFullSwing = false;
            PlayerController.GetPlayerController.StartCoroutine(FullSwingCoolDown());
            
            m_LeftCharge = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.ChargingFullAttack);
            m_RightCharge = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.ChargingFullAttack);
        }

        public override void Update(float deltaTime, AnimatorStateInfo stateInfo)
        {
            m_LeftCharge.transform.position =
                PlayerController.GetPlayerController.m_AttackLeftTrail.transform.position;
            m_RightCharge.transform.position =
                PlayerController.GetPlayerController.m_AttackRightTrail.transform.position;
            if (stateInfo.normalizedTime >= 0.9f)
            {
                PlayerController.GetPlayerController.m_StateMachine.ChangeState<Idle>();
            }
        }

        public override void End()
        {
            ObjPool.ObjectPoolInstance.ReturnObject(m_LeftCharge, EPrefabsName.ChargingFullAttack);
            ObjPool.ObjectPoolInstance.ReturnObject(m_RightCharge, EPrefabsName.ChargingFullAttack);
            PlayerController.GetPlayerController. m_AttackRightTrail.Deactivate();
            PlayerController.GetPlayerController.m_AttackLeftTrail.Deactivate();
        }

        private IEnumerator FullSwingCoolDown()
        {
            yield return m_FullSwingTimer;
            PlayerController.GetPlayerController.m_ActiveFullSwing = true;
        }
    }
}