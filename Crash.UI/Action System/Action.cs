using System;
using System.Collections.Generic;
using System.Reflection;

namespace Crash.UI
{
    public abstract class Action
    {
        internal static List<Action> actions;

        static Action()
        {
            actions = new List<Action>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!typeof(Action).IsAssignableFrom(type))
                    continue;
                if (type.IsAbstract)
                    continue;
                actions.Add((Action)Activator.CreateInstance(type));
            }
        }

        public static IEnumerable<Action> AllActions
        {
            get { return actions; }
        }

        public virtual bool CheckCompatibility(Controller c)
        {
            return true;
        }

        public virtual string GetText(Controller c)
        {
            return string.Format("{0} @ {1}",this,c);
        }

        public abstract Command Activate(Controller c);
    }

    public abstract class Action<T> : Action where T : Controller
    {
        public sealed override bool CheckCompatibility(Controller c)
        {
            if (!(c is T))
                return false;
            return CheckCompatibility((T)c);
        }

        public sealed override string GetText(Controller c)
        {
            if (!(c is T))
                return "ERROR";
            return GetText((T)c);
        }

        public sealed override Command Activate(Controller c)
        {
            if (!(c is T))
                return null;
            return Activate((T)c);
        }

        protected virtual bool CheckCompatibility(T c)
        {
            return true;
        }

        protected virtual string GetText(T c)
        {
            return string.Format("{0} @ {1}",this,c);
        }

        protected abstract Command Activate(T c);
    }
}
