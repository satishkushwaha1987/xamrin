using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CHSBackOffice.Support
{
    class ProcessQueue
    {
        private Queue<(Func<bool> Condition, Func<Task<bool>> Method)> _conditions;

        public void EnqueueCondition(Func<bool> condition, Func<Task<bool>> methodForInvoke)
        {
            if (_conditions == null)
                _conditions = new Queue<(Func<bool> Condition, Func<Task<bool>> Method)>();
            if (condition == null)
                condition = () => { return false; };
            _conditions.Enqueue((Condition: condition, Method: methodForInvoke));
        }

        public async Task<bool> TryProcess()
        {
            if (_conditions == null || _conditions.Count == 0) return false;
            while (_conditions.Count > 0)
            {
                var condition = _conditions.Dequeue();
                if ((bool)condition.Condition.DynamicInvoke())
                {
                    if (await condition.Method.Invoke())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
