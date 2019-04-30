using FsmTestApp.EnumConstants;
using Newtonsoft.Json;
using Stateless;

namespace FsmTestApp.FsmMachines
{
    public abstract class WorkflowFsm<FsmData, State, Trigger> : ILeaveWorkflow<State, ApprovalState>
    {
        public State CurrentState => FsMachine.State;
        protected FsmData FsmStateData { get; set; }
        
        protected StateMachine<State, Trigger> FsMachine;

        public abstract void FireLeaveResponded(ApprovalState approvalState, string responder);
        
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(FsmStateData);
        }

        public virtual void FromJson(string jsonString)
        {
            FsmStateData = JsonConvert.DeserializeObject<FsmData>(jsonString);
            ConfigureFsm();
        }

        protected abstract void ConfigureFsm();
    }
}