using System;
using System.IO;
using System.Text;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	//http://stackoverflow.com/questions/2547428/net-console-textwriter-that-understands-indent-unindent-indentlevel
	public class ConsoleLogger : TextWriter
	{
		private readonly TextWriter _mOldConsole;
		private bool _mDoIndent;

		public ConsoleLogger()
		{
			_mOldConsole = Console.Out;
			Console.SetOut(this);
		}

		public int Indent { get; set; }

		public override Encoding Encoding
		{
			get { return _mOldConsole.Encoding; }
		}

		public void Given(string input, params object[] values)
		{
			WriteLine("Given " + input, values);
		}

		public void When(string input, params object[] values)
		{
			WriteLine("When " + input, values);
		}

		public void And(string input, params object[] values)
		{
			Indent++;
			WriteLine("And " + input, values);
			Indent--;
		}

		public void Then(string input, params object[] values)
		{
			WriteLine("Then " + input, values);
		}


		public override void Write(char ch)
		{
			if (_mDoIndent)
			{
				_mDoIndent = false;
				for (int ix = 0; ix < Indent; ++ix) _mOldConsole.Write("  ");
			}
			_mOldConsole.Write(ch);
			if (ch == '\n') _mDoIndent = true;
		}
	}
}