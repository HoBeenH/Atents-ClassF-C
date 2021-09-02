using System.Collections;
using UnityEngine;

namespace FSM.Player
{
    public class Player_Attack : IState
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private readonly PlayerController m_Player;
        private Animator m_Anim;

        public Player_Attack(PlayerController player)
        {
            m_Player = player;
        }

        public void OnStateEnter()
        {
            m_Player.m_PlayerDamage = 20f;
            m_Anim = m_Player.GetComponent<Animator>();
            m_Anim.SetTrigger(Attack);
        }

        public void OnStateFixedUpdate()
        {
        }

        public void OnStateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_Anim.SetTrigger(Attack);
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("LastAttack"))
            {
                m_Anim.ResetTrigger(Attack);
            }
        }

        public void OnStateExit()
        {
        }
    }
}