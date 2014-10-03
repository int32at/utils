using System.Diagnostics;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Workflow;
using int32.Utils.Core.Generic.Workflow.Steps;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WorkflowTests
    {
        [TestCase]
        public void Workflow_Simple_Action()
        {
            var r1 = new Engine().Start(
                new Workflow(
                    new Step(() => Debug.WriteLine("TEST"))
                    )
                );

            var r2 = new Engine().Start(
                new Workflow<SampleModel>(new SampleModel(),
                    new Step<SampleModel>(i => i.Age = 23)
                    )
                );

            var steps =
                new IfEqualStep<SampleModel>("Age", 13,
                    new[] { new Step<SampleModel>(model => model.Age = 17) },
                    new[] { new Step<SampleModel>(model => model.Age = 25) }
                );

            var r3 = new Engine().Start(new Workflow<SampleModel>(new SampleModel { Age = 13 }, steps));

            Assert.AreEqual(17, r3.Target.Age);

            Assert.IsNotNull(r1);
            Assert.IsNull(r1.Target);
            Assert.IsFalse(r1.HasErrors);

            Assert.IsNotNull(r2);
            Assert.AreEqual(23, r2.Target.Age);
            Assert.IsFalse(r2.HasErrors);
        }

        [TestCase]
        public void Workflow_Simple_Action1()
        {
            var wf = new Workflow<SampleModel>(new SampleModel(),
                new Step<SampleModel>(i => i.Age = 13),
                new Step<SampleModel>(i => i.Type = ModelType.Test),
                new IfNullStep<SampleModel>("Title",
                    new[] { new Step<SampleModel>(i => i.Title = "TEST") },
                    null
                    ),
                new Step<SampleModel>(i => Debug.WriteLine("YEA"))
                );

            var r = new Engine().Start(wf);

            Assert.AreEqual(13, r.Target.Age);
            Assert.AreEqual(ModelType.Test, r.Target.Type);
            Assert.AreEqual("TEST", r.Target.Title);
        }

        //[TestCase]
        //public void Workflow_Simple_Action()
        //{
        //    var counter = 0;

        //    //create a workflow that does a non dependent action, like increasing a counter
        //    //or writing something to a log file etc.
        //    var workflow = new Workflow(
        //        new Step(() => counter++)
        //        );

        //    var result = Engine.Start(workflow);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, counter);
        //    Assert.AreEqual(1, result.CompletedSteps.Count);
        //    Assert.AreEqual(0, result.FailedSteps.Count);
        //    Assert.IsFalse(result.HasErrors);
        //    Assert.IsNull(result.Exception);
        //    Assert.IsNull(result.Target);
        //}

        //[TestCase]
        //public void Workflow_Simple_Action_Generic()
        //{
        //    var workflow = new Workflow<SampleModel>(new SampleModel(),
        //        new Step<SampleModel>(model => model.Age = 17)
        //        );

        //    var result = Engine.Start(workflow);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.CompletedSteps.Count);
        //    Assert.AreEqual(0, result.FailedSteps.Count);
        //    Assert.IsFalse(result.HasErrors);
        //    Assert.IsNull(result.Exception);

        //    //this time, target is not null, but it represents the target that was given
        //    //when creating the workflow -> new SampleModel() in this case.
        //    //this means that we give in an empty object, and use the workflow to fill it;
        //    //another possibillity would be to create the object before, and just pass in the reference
        //    //when the workflow is created
        //    Assert.IsNotNull(result.Target);
        //    Assert.AreEqual(typeof(SampleModel), result.Target.GetType());
        //    Assert.AreEqual(17, result.Target.Age);
        //}

        //[TestCase]
        //public void Workflow_Multiple_Action()
        //{
        //    var counterA = 0;
        //    var counterB = 10;
        //    var counterC = 0;

        //    var workflow = new Workflow(
        //        new Step(() => counterA++),
        //        new Step(() => counterB--),
        //        new Step(() => { throw new Exception("should fail"); }),
        //        new Step(() => counterC++) //should not be executed
        //        );

        //    var result = Engine.Start(workflow);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.CompletedSteps.Count);
        //    Assert.AreEqual(1, result.FailedSteps.Count);
        //    Assert.True(result.HasErrors);
        //    Assert.IsNotNull(result.Exception);
        //    Assert.AreEqual("should fail", result.Exception.Message);

        //    Assert.AreEqual(1, counterA);
        //    Assert.AreEqual(9, counterB);
        //    Assert.AreEqual(0, counterC);
        //}

        //[TestCase]
        //public void Workflow_Multiple_Action_Generic()
        //{
        //    var workflow = new Workflow<SampleModel>(new SampleModel(),
        //        new Step<SampleModel>(i => i.Age = 17),
        //        new Step<SampleModel>(i => i.Type = ModelType.Test),
        //        new Step<SampleModel>(i => { throw new Exception("should fail"); }),
        //        new Step<SampleModel>(i => i.Title = "A") //should not be executed
        //        );

        //    var result = Engine.Start(workflow);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.CompletedSteps.Count);
        //    Assert.AreEqual(1, result.FailedSteps.Count);
        //    Assert.True(result.HasErrors);
        //    Assert.IsNotNull(result.Exception);
        //    Assert.AreEqual("should fail", result.Exception.Message);

        //    Assert.AreEqual(17, result.Target.Age);
        //    Assert.AreEqual(ModelType.Test, result.Target.Type);

        //    //should be empty, because there is an exception before that step..
        //    Assert.IsNull(result.Target.Title);
        //}

        //[TestCase]
        //public void Workflow_SetValueStep()
        //{
        //    var result = new WorkflowEngine().Start(
        //        new Workflow<SampleModel>(new SampleModel(),
        //            new SetValueStep<SampleModel>("Age", 13)
        //            )
        //        );

        //    Assert.AreEqual(13, result.Target.Age);
        //}

        //[TestCase]
        //public void Workflow_IfStep_And_SetValueStep()
        //{

        //}
    }
}
