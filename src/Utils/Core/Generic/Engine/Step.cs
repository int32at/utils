using System;
using System.Collections.Generic;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Engine
{
    public class Step
    {
        private List<Step> _subs;

        public List<Step> Steps
        {
            get { return _subs; }
            protected set { _subs = value; }
        }
        public Step Parent { get; protected set; }
        public string Title { get; set; }
        public Action Action { get; set; }

        public Step(string title, Step sub) : this(title, null, new[] { sub }) { }

        public Step(string title, params Step[] subs) : this(title, null, subs) { }

        public Step(string title, Action action) : this(title, action, new Step[] { }) { }

        public Step(string title, Action action, Step sub) : this(title, action, new[] { sub }) { }

        public Step(string title, Action action, params Step[] subs)
        {
            Title = title;
            Action = action;
            _subs = new List<Step>();

            if (subs == null) return;

            subs.ForEach(i => i.Parent = this);
            _subs.AddRange(subs);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
