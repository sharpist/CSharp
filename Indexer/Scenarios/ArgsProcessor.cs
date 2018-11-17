using System;
using Dictionary = System.Collections.Generic.Dictionary<System.String, System.Action>;

namespace IndexersSamples.SampleTwo
{
    struct ArgsActions
    {
        public Action this[string key] // индексатор
        {
            get
            {
                Action action;
                Action defaultAction = () => { };
                return argsActions.TryGetValue(key, out action) ?
                    action : defaultAction;
            }
            set { argsActions[key] = value; }
        }

        static Dictionary argsActions;
        static ArgsActions() => argsActions = new Dictionary();
    }

    class ArgsProcessor
    {
        public void Process(params string[] args)
        {
            foreach (var arg in args) actions[arg]?.Invoke();
        }

        ArgsActions actions;
        public ArgsProcessor(ArgsActions actions) => this.actions = actions;
    }
}