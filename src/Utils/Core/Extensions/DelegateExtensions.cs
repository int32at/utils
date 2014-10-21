using System;

namespace int32.Utils.Core.Extensions
{
    public static class DelegateExtensions
    {
        public static object Execute(this Delegate del)
        {
            return del.Execute<object>();
        }

        public static T Execute<T>(this Delegate del)
        {
            return del.Execute<T>(null);
        }

        public static object Execute(this Delegate del, params object[] parameters)
        {
            return del.Execute<object>(parameters);
        }

        public static T Execute<T>(this Delegate del, params object[] parameters)
        {
            try
            {
                return del.IsNotNull() ? del.DynamicInvoke(parameters).As<T>() : ObjectExtensions.As<T>(null);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public static DelegateExecutioner Execute(this Delegate del, int count)
        {
            return new DelegateExecutioner(del, count);
        }
    }

    //sounds badass
    public class DelegateExecutioner
    {
        private readonly Delegate _action;
        private readonly int _count;

        public DelegateExecutioner(Delegate action, int count)
        {
            _action = action;
            _count = count;
        }

        public void Times()
        {
            for (var i = 0; i < _count; i++)
                _action.Execute();
        }
    }
}
