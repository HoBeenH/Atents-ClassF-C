﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace FSM.Player
{
    public class Player_Area : State<PlayerController>
    {
        private readonly WaitForSeconds m_SkillTimer = new WaitForSeconds(8.0f);
        private readonly int m_Skill;

        public Player_Area() : base("Base Layer.Skill.Skill") => m_Skill = Animator.StringToHash("Skill");
        
        public override void OnStateEnter()
        {
            m_Machine.m_Animator.SetTrigger(m_Skill);
            m_Owner.StartCoroutine(AreaCoolDown());

            var player = m_Owner.gameObject;
            PlayerManager.Instance.TrailSwitch();
            PlayerManager.Instance.GetEffect(player, EPrefabsName.Area,7f);
            PlayerManager.Instance.GetEffect(player, EPrefabsName.HealWeapon, 10f, 8f, player.transform);
            PlayerManager.Instance.GetEffect(player, EPrefabsName.AreaEffect, 7f).transform.DOMoveY(player.transform.position.y + 5.0f,2.5f);
        }

        public override void OnStateUpdate()
        {
            if (m_Machine.IsEnd())
            {
                m_Machine.ChangeState<Player_Idle>();
            }
        }

        private IEnumerator AreaCoolDown()
        {
            m_Owner.m_ActiveArea = false;
            yield return m_SkillTimer;
            m_Owner.m_ActiveArea = true;
        }
    }
}