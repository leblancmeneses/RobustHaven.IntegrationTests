using System;
using System.Collections.Generic;

namespace RobustHaven.IntegrationTests.Framework
{
	public abstract class BaseVisitor : AVisitor, IDisposable
	{
		#region Delegates

		public delegate void Run();

		#endregion

		protected readonly Stack<Run> Stack = new Stack<Run>();

		#region IDisposable Members

		public abstract void Dispose();

		#endregion

		public virtual void Execute()
		{
			while(Stack.Count > 1)
			{
				var localRight = Stack.Pop();
				var localLeft = Stack.Pop();
					Stack.Push(() =>
					{
						localLeft();
						localRight();
			}
				);
			}

			var run = Stack.Peek();
			run();
		}


		public virtual void VisitEnter(Sequence sequence)
		{
		}

		public virtual void VisitExecute(Sequence sequence)
		{
		}

		public virtual void VisitLeave(Sequence sequence)
		{
			var localRight = Stack.Pop();
			var localLeft = Stack.Pop();
			Stack.Push( () => {
						localLeft();
						localRight();
					}
				);
		}
	}
}