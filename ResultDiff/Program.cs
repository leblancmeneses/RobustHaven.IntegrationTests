using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;
using Newtonsoft.Json;
using ResultDiff.Extensions;
using ResultDiff.Models;
using ResultDiff.Strategies;

namespace ResultDiff
{
	class Program
	{
		static int verbosity;

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			bool show_help = false;
			string testOutputFile = null;
			string bddFolder = null;
			string jsonOutputFile = Path.Combine(Environment.CurrentDirectory, "ResultDiff.json");

			var p = new OptionSet() {
				{ "t|testResult=", "the nunit TestResult.xml output.",
					v =>
						{
							testOutputFile = v;
							if (string.IsNullOrEmpty(testOutputFile))
							{
								throw new FileNotFoundException("t argument is empty");
							}

							testOutputFile = Path.IsPathRooted(v) ? v : Path.Combine(Environment.CurrentDirectory, v);
							if (!File.Exists(testOutputFile))
							{
								throw new FileNotFoundException(testOutputFile);
							}
						}
				},
				{ "j|json=", "location to write the json output file.",
					v =>
						{
							jsonOutputFile = v;
							if (string.IsNullOrEmpty(jsonOutputFile))
							{
								throw new FileNotFoundException("j argument is empty");
							}

							jsonOutputFile = Path.IsPathRooted(v) ? v : Path.Combine(Environment.CurrentDirectory, v);
						}
				},
				{ "bdd=", "the bdd folder containing *.feature files.",
					v =>
						{
							if (string.IsNullOrEmpty(v))
							{
								throw new DirectoryNotFoundException("bdd argument is empty");
							}

							bddFolder = Path.IsPathRooted(v) ? v : Path.Combine(Environment.CurrentDirectory, v);
							if (!Directory.Exists(bddFolder))
							{
								throw new DirectoryNotFoundException(bddFolder);
							}
						}
				},
				{ "v", "increase debug message verbosity",
				  v => { if (v != null) ++verbosity; } },
				{ "h|help",  "show help and exit", 
				  v => show_help = v != null },
			};

			List<string> extra;
			try
			{
				extra = p.Parse(args);
			}
			catch (OptionException e)
			{
				Console.Write("ResultDiff: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `ResultDiff --help' for more information.");
				return;
			}

			if (show_help)
			{
				ShowHelp(p);
				return;
			}

			string message;
			if (extra.Count > 0)
			{
				message = string.Join(" ", extra.ToArray());
				Debug("Extra arguments: {0}", message);
			}

			if (testOutputFile == null)
				throw new InvalidOperationException("Missing required option -t=TestResult.xml, see: ResultDiff -h");
			if (bddFolder == null)
				throw new InvalidOperationException("Missing required option -bdd=folderpath\bdd, see: ResultDiff -h");


			Diff(bddFolder, testOutputFile, jsonOutputFile);
		}


		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var f = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.Write(e.ExceptionObject.ToString());
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Error.WriteLine();
			Console.ForegroundColor = f;
			Environment.Exit(1);
		}

		static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: ResultDiff [OPTIONS]+");
			Console.WriteLine("ResultDiff provides a difference report on what the development team");
			Console.WriteLine("has developed versus what the product owners expect.  It should be used ");
			Console.WriteLine("as an iterative tool to spark refinement conversations on aligning ");
			Console.WriteLine("expectations when implementing features and scenarios.");
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}

		static void Debug(string format, params object[] args)
		{
			if (verbosity > 0)
			{
				Console.Write("# ");
				Console.WriteLine(format, args);
			}
		}

		static void Diff(string bddFolder, string testOutputFile, string jsonOutputFile)
		{
			var ctx = new DiffContext(bddFolder, testOutputFile);

			// MVP: currently only support nunit
			var strategy = new NUnitDiffStrategy();

			var result = strategy.Diff(ctx);

			foreach (var featureViewModel in result.Features.OrderBy(x => x.Status))
			{
				Console.WriteLine("{0,14}:  {1}", featureViewModel.Status.ToDescription(), featureViewModel.Name); 

				foreach (var scenarioViewModel in featureViewModel.Scenarios.OrderBy(x => x.Status))
				{
					Console.WriteLine("\t\t{0,9} - {1,6}:  {2}", scenarioViewModel.Status.ToDescription(), scenarioViewModel.TestStatus.ToDescription(), scenarioViewModel.Name); 
				}

				Console.WriteLine("");
				Console.WriteLine("");
			}

			var output = JsonConvert.SerializeObject(result, Formatting.Indented);
			File.WriteAllText(jsonOutputFile, output);

			if (result.Features.Any(x => x.Status == ItemStatus.ParsingFailed))
			{
				Environment.Exit(1);
			}
		}
	}
}
