using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Core.Generic.ViewEngine.Contracts;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Samples;

namespace Tests
{
    [TestClass]
    public class ViewEngineTests
    {
        [TestMethod]
        public void ViewEngine_Dynamic_SingleSubstitutions()
        {
            //strings and simple types
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model", "World")).Is("Hello World");
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model", 1)).Is("Hello 1");
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model", DateTime.Now.ToShortTimeString())).Is("Hello {0}".With(DateTime.Now.ToShortTimeString()));

            dynamic basic = new ExpandoObject();
            basic.Name = "World";

            dynamic extended = new ExpandoObject();
            extended.Data = new ExpandoObject();
            extended.Data = "World";

            var real = new SampleModel {Age = 38, Title = "Model", Type = ModelType.Example};

            //object
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model.Data", extended)).Is("Hello World");
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model.Age", real)).Is("Hello 38");
            MakeSure.That(ViewEngine.Instance.Render("Hello @Model.Type", real)).Is("Hello Example");

            //extensions
            var renderable = new RenderableModel {Title = "Cool"};
            MakeSure.That(renderable.Render("Hello @Model.Title")).Is("Hello Cool");
            MakeSure.That("Hello @Model.Title".RenderWith(renderable)).Is("Hello Cool");
        }

        [TestMethod]
        public void ViewEngine_Dynamic_ForEach()
        {
            MakeSure.That(ViewEngine.Instance.Render("@Each@Current@EndEach", Enumerable.Range(0, 5))).Is("01234");

            var renderables = new List<RenderableModel>
            {
                new RenderableModel {Title = "1"},
                new RenderableModel {Title = "2"},
                new RenderableModel {Title = "3"}
            };

            //extensions
            MakeSure.That(renderables.Render("@Each@Current.Title@EndEach")).Is("123");
        }

        [TestMethod]
        public void ViewEngine_Dynamic_Conditionals()
        {
            var m1 = new RenderableModel {HasUsers = true, HasAdmins = false, Title = "Cool"};
            MakeSure.That(m1.Render("@If.HasUsers @Model.Title @EndIf @IfNot.HasAdmins no admins! @EndIf")).Is("Cool no admins!");
        }
    }

    public class RenderableModel : IRenderable
    {
        public string Title { get; set; }
        public bool HasUsers { get; set; }
        public bool HasAdmins { get; set; }
    }
}
