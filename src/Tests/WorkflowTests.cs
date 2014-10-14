using System;
using int32.Utils.Core.Generic.Tasks;
using int32.Utils.Core.Generic.Workflow;
using int32.Utils.Core.Generic.Workflow.Steps;
using int32.Utils.Tests;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WorkflowTests
    {
        protected Engine Engine = new Engine();

        [TestCase]
        public void Workflow_Simple_Action()
        {
            var counter = 0;

            //create a workflow that does a non dependent action, like increasing a counter
            //or writing something to a log file etc.
            var workflow = new Workflow(
                new Step(() => counter++)
                );

            var result = Engine.Start(workflow);

            MakeSure.That(result).IsNot(null);
            MakeSure.That(counter).Is(1);
            MakeSure.That(result.CompletedSteps.Count).Is(1);
            MakeSure.That(result.FailedSteps.Count).Is(0);
            MakeSure.That(result.HasErrors).Is(false);
            MakeSure.That(result.Exception).Is(null);
            MakeSure.That(result.Target).Is(null);
        }

        [TestCase]
        public void Workflow_Simple_Action_Generic()
        {
            var workflow = new Workflow<SampleModel>(new SampleModel(),
                new Step<SampleModel>(model => model.Age = 17)
                );

            var result = Engine.Start(workflow);

            MakeSure.That(result).IsNot(null);
            MakeSure.That(result.CompletedSteps.Count).Is(1);
            MakeSure.That(result.FailedSteps.Count).Is(0);
            MakeSure.That(result.HasErrors).Is(false);
            MakeSure.That(result.Exception).Is(null);

            //this time, target is not null, but it represents the target that was given
            //when creating the workflow -> new SampleModel() in this case.
            //this means that we give in an empty object, and use the workflow to fill it;
            //another possibillity would be to create the object before, and just pass in the reference
            //when the workflow is created
            MakeSure.That(result.Target).IsNot(null);
            MakeSure.That(result.Target.GetType()).Is(typeof(SampleModel));
            MakeSure.That(result.Target.Age).Is(17);
        }

        [TestCase]
        public void Workflow_Multiple_Action()
        {
            var counterA = 0;
            var counterB = 10;
            var counterC = 0;

            var workflow = new Workflow(
                new Step(() => counterA++),
                new Step(() => counterB--),
                new Step(() => { throw new Exception("should fail"); }),
                new Step(() => counterC++) //should not be executed
                );

            var result = Engine.Start(workflow);

            MakeSure.That(result).IsNot(null);
            MakeSure.That(result.CompletedSteps.Count).Is(2);
            MakeSure.That(result.FailedSteps.Count).Is(1);
            MakeSure.That(result.HasErrors).Is(true);
            MakeSure.That(result.Exception).IsNot(null);
            MakeSure.That(result.Exception.Message).Is("should fail");

            MakeSure.That(counterA).Is(1);
            MakeSure.That(counterB).Is(9);
            MakeSure.That(counterC).Is(0);
        }

        [TestCase]
        public void Workflow_Multiple_Action_Generic()
        {
            var workflow = new Workflow<SampleModel>(new SampleModel(),
                new Step<SampleModel>(i => i.Age = 17),
                new Step<SampleModel>(i => i.Type = ModelType.Test),
                new Step<SampleModel>(i => { throw new Exception("should fail"); }),
                new Step<SampleModel>(i => i.Title = "A") //should not be executed
                );

            var result = Engine.Start(workflow);

            MakeSure.That(result).IsNot(null);
            MakeSure.That(result.CompletedSteps.Count).Is(2);
            MakeSure.That(result.FailedSteps.Count).Is(1);
            MakeSure.That(result.HasErrors).Is(true);
            MakeSure.That(result.Exception).IsNot(null);
            MakeSure.That(result.Exception.Message).Is("should fail");

            //should be empty, because there is an exception before that step..
            MakeSure.That(result.Target.Age).Is(17);
            MakeSure.That(result.Target.Type).Is(ModelType.Test);
            MakeSure.That(result.Target.Title).Is(null);
        }
    }
}