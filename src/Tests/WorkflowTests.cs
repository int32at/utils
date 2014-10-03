using System;
using System.Diagnostics;
using int32.Utils.Core.Generic.Engine.Workflow;
using int32.Utils.Core.Generic.Engine.Workflow.Steps;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WorkflowTests
    {
        protected WorkflowEngine Engine;

        [SetUp]
        public void Setup()
        {
            Engine = new WorkflowEngine();
        }

        [TestCase]
        public void Workflow_Simple_Action()
        {
            var counter = 0;

            //create a workflow that does a non dependent action, like increasing a counter
            //or writing something to a log file etc.
            var workflow = new Workflow(
                new WorkflowStep(() => counter++)
                );

            var result = Engine.Start(workflow);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, counter);
            Assert.AreEqual(1, result.CompletedSteps.Count);
            Assert.AreEqual(0, result.FailedSteps.Count);
            Assert.IsFalse(result.HasErrors);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Target);
        }

        [TestCase]
        public void Workflow_Simple_Action_Generic()
        {
            var workflow = new Workflow<SampleModel>(new SampleModel(),
                new WorkflowStep<SampleModel>(model => model.Age = 17)
                );

            var result = Engine.Start(workflow);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CompletedSteps.Count);
            Assert.AreEqual(0, result.FailedSteps.Count);
            Assert.IsFalse(result.HasErrors);
            Assert.IsNull(result.Exception);

            //this time, target is not null, but it represents the target that was given
            //when creating the workflow -> new SampleModel() in this case.
            //this means that we give in an empty object, and use the workflow to fill it;
            //another possibillity would be to create the object before, and just pass in the reference
            //when the workflow is created
            Assert.IsNotNull(result.Target);
            Assert.AreEqual(typeof(SampleModel), result.Target.GetType());
            Assert.AreEqual(17, result.Target.Age);
        }

        [TestCase]
        public void Workflow_Multiple_Action()
        {
            var counterA = 0;
            var counterB = 10;
            var counterC = 0;

            var workflow = new Workflow(
                new WorkflowStep(() => counterA++),
                new WorkflowStep(() => counterB--),
                new WorkflowStep(() => { throw new Exception("should fail"); }),
                new WorkflowStep(() => counterC++) //should not be executed
                );

            var result = Engine.Start(workflow);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.CompletedSteps.Count);
            Assert.AreEqual(1, result.FailedSteps.Count);
            Assert.True(result.HasErrors);
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual("should fail", result.Exception.Message);

            Assert.AreEqual(1, counterA);
            Assert.AreEqual(9, counterB);
            Assert.AreEqual(0, counterC);
        }

        [TestCase]
        public void Workflow_Multiple_Action_Generic()
        {
            var workflow = new Workflow<SampleModel>(new SampleModel(),
                new WorkflowStep<SampleModel>(i => i.Age = 17),
                new WorkflowStep<SampleModel>(i => i.Type = ModelType.Test),
                new WorkflowStep<SampleModel>(i => { throw new Exception("should fail"); }),
                new WorkflowStep<SampleModel>(i => i.Title = "A") //should not be executed
                );

            var result = Engine.Start(workflow);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.CompletedSteps.Count);
            Assert.AreEqual(1, result.FailedSteps.Count);
            Assert.True(result.HasErrors);
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual("should fail", result.Exception.Message);

            Assert.AreEqual(17, result.Target.Age);
            Assert.AreEqual(ModelType.Test, result.Target.Type);

            //should be empty, because there is an exception before that step..
            Assert.IsNull(result.Target.Title);
        }
    }
}
