using System;
using System.ComponentModel;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Engine.Events;
using int32.Utils.Core.Generic.Tasks;

namespace int32.Utils.Core.Generic.Engine
{
    public class Engine
    {
        public bool HasError { get; protected set; }

        public event EventHandler<StepCompletedEventArgs> OnStepCompleted;
        public event EventHandler<StepErrorEventArgs> OnStepError;
        public event EventHandler<WorkflowCompletedEventArgs> OnCompleted;

        public void Start(Step step)
        {
            Reset();

            var executionTime = Timing.Measure(() =>
            {
                var failure = StartRecursive(step);

                if (!failure.IsNotNull()) return;

                HasError = true;
                OnStepError.Raise(this, new StepErrorEventArgs(failure));
            });

            OnCompleted.Raise(this, new WorkflowCompletedEventArgs { ExecutionTime = executionTime });
        }

        public void StartAsync(Step step)
        {
            BackgroundWorker worker = null;
            try
            {
                worker = new BackgroundWorker();
                worker.DoWork += (o, e) => Start(step);

                //report progress back to the subscriber
                worker.ProgressChanged +=
                    (o, e) => OnStepCompleted.Raise(this, new StepCompletedEventArgs(e.UserState.As<Step>()));
                worker.RunWorkerAsync();
            }
            finally
            {
                if (worker != null)
                    worker.Dispose();
            }
        }

        private void Reset()
        {
            HasError = false;
        }

        private Step StartRecursive(Step step)
        {
            Step t = null;
            try
            {
                if (step.IsNull())
                    return null;

                if (step.Action.IsNotNull())
                    step.Action();

                OnStepCompleted.Raise(this, new StepCompletedEventArgs(step));

                foreach (var sub in step.Steps)
                    if (t == null)
                        t = StartRecursive(sub);
            }
            catch (Exception ex)
            {
                t = step;
            }

            return t;
        }
    }
}