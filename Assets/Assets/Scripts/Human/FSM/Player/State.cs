using UnityEngine;

namespace FSM.Player
{
    public abstract class State<T>
    {
        internal readonly int m_StateToHash;
        protected StateMachine<T> Machine;
        private T m_Owner;


        protected State()
        {
        }

        protected State(string animStateName) : this(Animator.StringToHash(animStateName))
        {
        }

        private State(int animStateHash)
        {
            this.m_StateToHash = animStateHash;
        }


        internal void SetMachineAndContext(StateMachine<T> machine, T context)
        {
            Machine = machine;
            m_Owner = context;
            ONInitialized();
        }

        protected virtual void ONInitialized()
        {}


        public virtual void OnStateEnter()
        {}


        public virtual void ChangePoint()
        {}


        public abstract void OnStateUpdate(float deltaTime, AnimatorStateInfo stateInfo);
        
        public virtual void OnFixedUpdate(float deltaTime, AnimatorStateInfo stateInfo)
        {}

        public virtual void OnStateExit()
        {}
    }
}