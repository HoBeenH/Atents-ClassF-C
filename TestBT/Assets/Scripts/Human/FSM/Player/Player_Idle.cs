using UnityEngine;
using XftWeapon;

namespace FSM.Player
{
    public class Player_Idle : State<PlayerController>
    {
        private CharacterController m_CharacterController;
        private const float GRAVITY = 9.81f;
        private Vector3 m_GravityVec;

        protected override void ONInitialized() => m_CharacterController = m_Owner.GetComponent<CharacterController>();

        public override void OnStateEnter() => Debug.Log($"StateEnter {ToString()}");

        public override void ChangePoint()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.S))
            {
                m_Machine.ChangeState<Player_Move>();
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_Machine.ChangeState<Player_AttackL>();
            }

            if (Input.GetKey(KeyCode.Q) && m_Owner.Stamina > 40f && m_Owner.m_ActiveFlyAttack)
            {
                m_Machine.ChangeState<Player_FlyAttack>();
            }

            if (Input.GetKey(KeyCode.E) && m_Owner.Stamina > 40f && m_Owner.m_ActiveFullSwing)
            {
                m_Machine.ChangeState<Player_FullSwing>();
            }

            if (Input.GetKey(KeyCode.Space) && m_Owner.m_ActiveArea)
            {
                m_Machine.ChangeState<Player_Area>();
            }
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            if (!m_CharacterController.isGrounded)
            {
                m_GravityVec += Vector3.down * GRAVITY * deltaTime;
                m_CharacterController.Move(m_GravityVec * deltaTime);
            }
        }
    }
}