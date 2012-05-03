using System;
using System.Collections.Generic;

namespace RobustHaven.IntegrationTests.Framework
{
    public abstract class WebVisitor : AVisitor, IDisposable
    {
        #region Delegates

        public delegate void Run();

        #endregion

        protected readonly Stack<Run> stack = new Stack<Run>();

        public void Execute()
        {
            while (stack.Count > 0)
            {
                Run run = stack.Pop();
                run();
            }
        }


        public void VisitEnter(Sequence sequence)
        {
        }

        public void VisitExecute(Sequence sequence)
        {
        }

        public void VisitLeave(Sequence sequence)
        {
            Run localRight = stack.Pop();
            Run localLeft = stack.Pop();
            stack.Push(
                delegate
                {
                    localLeft();
                    localRight();
                }
                );
        }


        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
