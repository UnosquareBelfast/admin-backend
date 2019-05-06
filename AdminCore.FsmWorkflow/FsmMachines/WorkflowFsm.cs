using AdminCore.Common;
using AdminCore.Constants.Enums;
using Newtonsoft.Json;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public abstract class WorkflowFsm<FsmData, State, Trigger> : ILeaveWorkflow
    {
        public FsmData FsmStateData { get; protected set; }
        
        protected StateMachine<State, Trigger> FsMachine;

        public abstract WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder);
        
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