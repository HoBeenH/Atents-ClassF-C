﻿using System.Collections;
using PlayerScript;
using UnityEngine;

namespace FSM.Player
{
    public class Player_FlyAttack : State
    {
        private static readonly int FlyAttack = Animator.StringToHash("FlyAttack");
        private readonly WaitForSeconds m_FlyAttackTimer = new WaitForSeconds(6.0f);
        private const float FORCE = 100f;
        public override IEnumerator OnStateEnter()
        {
            var currentInstance = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackEffect);
            currentInstance.transform.SetParent(PlayerController.GetPlayerController.gameObject.transform);
            var psUpdater = currentInstance.GetComponent<PSMeshRendererUpdater>();
            psUpdater.UpdateMeshEffect(PlayerController.GetPlayerController.gameObject);
            PlayerController.GetPlayerController.m_CurState = EPlayerState.FLY_ATTACK;
            PlayerController.GetPlayerController.Stamina -= 40f;
            PlayerController.GetPlayerController.m_ActiveFlyAttack = false;
            PlayerController.GetPlayerController.m_Anim.SetTrigger(FlyAttack);

            while (!PlayerController.GetPlayerController.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("FlyAttack"))
                yield return null;
  
            PlayerController.GetPlayerController.m_AttackLeftTrail.Activate();
            PlayerController.GetPlayerController. m_AttackRightTrail.Activate();
            PlayerController.GetPlayerController.StartCoroutine(FlyAttackCoolDown());
            PlayerController.GetPlayerController.AddImpact((PlayerController.GetPlayerController.transform.forward), FORCE);
            
            var startDust = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackStartDust);
            startDust.transform.position = PlayerController.GetPlayerController.transform.position;
            ObjPool.ObjectPoolInstance.ReturnObject(startDust,EPrefabsName.FlyAttackStartDust,1f);
            
            while (PlayerController.GetPlayerController.m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime <0.9f)
                yield return null;

            psUpdater.IsActive = false;
            ObjPool.ObjectPoolInstance.ReturnObject(currentInstance,EPrefabsName.FlyAttackEffect,2.0f);
            
            
            if (PlayerController.GetPlayerController.m_CurState != EPlayerState.DIE)
            {
                PlayerController.GetPlayerController.ChangeState(EPlayerState.IDLE);
            }
        }

        public override void OnStateExit()
        {
            PlayerController.GetPlayerController.m_AttackLeftTrail.Deactivate();
            PlayerController.GetPlayerController. m_AttackRightTrail.Deactivate();
            
            var arrow = ObjPool.ObjectPoolInstance.GetObject(EPrefabsName.FlyAttackArrow);
            arrow.transform.position = PlayerController.GetPlayerController.transform.position;
            ObjPool.ObjectPoolInstance.ReturnObject(arrow, EPrefabsName.FlyAttackArrow, 3f);
        }

        private IEnumerator FlyAttackCoolDown()
        {
            yield return m_FlyAttackTimer;
            PlayerController.GetPlayerController.m_ActiveFlyAttack = true;
        }
    }
}