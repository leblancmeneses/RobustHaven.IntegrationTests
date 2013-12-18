using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NPEG.ApplicationExceptions;
using ResultDiff.Extensions;
using ResultDiff.Models;

namespace ResultDiff.Strategies
{
	public class NUnitDiffStrategy : IDiffStrategy
	{
		public override DiffResult Diff(DiffContext ctx)
		{
			var diffResult = new DiffResult(); 

			var files = Directory.GetFiles(ctx.BddFolder, "*.feature", SearchOption.TopDirectoryOnly);
			foreach (var file in files)
			{
				FeatureParser.Models.Feature f = null;

				var feature = new FeatureViewModel();

				try
				{
					f = GetFeatureFromFile(file);
					feature.Name = f.Title.CodeLabel();
					feature.Status = ItemStatus.XNotFound;

					foreach (var svm in f.Scenarios.Select(scenario => new ScenarioViewModel
						{
							Name = scenario.Title.CodeLabel(),
							Status = ItemStatus.XNotFound,
							Diff = { Left = scenario.ToString() }
						}))
					{
						feature.Scenarios.Add(svm);
					}
				}
				catch (InvalidInputException)
				{
					feature.Name = Path.GetFileNameWithoutExtension(file);
					feature.Status = ItemStatus.ParsingFailed;
				}
				
				diffResult.Features.Add(feature);
			}

			using (var filestream = File.OpenRead(ctx.TestOutputFile))
			{
				var xml = XElement.Load(filestream);
				foreach (var suite in xml.Descendants("test-suite").Where(x=> x.Attribute("type").Value == "TestFixture"))
				{
					var featureCodeLabel = suite.Attribute("name").Value.Replace("FeatureTests", "");
					var feature = diffResult.Features.FirstOrDefault(x => x.Name.Equals(featureCodeLabel));
					if (feature == null)
					{
						feature = new FeatureViewModel()
						{
							Name = featureCodeLabel,
							Status = ItemStatus.XDeleted
						};

						diffResult.Features.Add(feature);
					}
					else
					{
						feature.Status = ItemStatus.XOkay;
					}

					foreach (var testCase in xml.Descendants("test-case"))
					{
						var sections = testCase.Attribute("name")
											   .Value.Split(new[] { suite.Attribute("name").Value + "." }, StringSplitOptions.RemoveEmptyEntries);
						var scenarioCodeLabel = sections[1];
						var scenario = feature.Scenarios.FirstOrDefault(x => x.Name.Equals(scenarioCodeLabel));
						if (scenario == null)
						{
							scenario = new ScenarioViewModel()
							{
								Name = scenarioCodeLabel,
								Status = ItemStatus.XDeleted
							};

							feature.Scenarios.Add(scenario);
						}
						else
						{
							feature.Status = ItemStatus.XOkay;
						}

						scenario.DidPass = bool.Parse(testCase.Attribute("success").Value);

						var message = testCase.Descendants("message").Single();
						var final = Regex.Replace(message.Value, @"TearDown\s:\s", "");
						final = Regex.Replace(final, @"^\s", "", RegexOptions.Multiline);

						if (scenario.DidPass)
						{
							scenario.Diff.Right = GetFeatureFromString(final).Scenarios.Single().ToString();
						}

						if (scenario.Status == ItemStatus.XNotFound)
						{
							scenario.Status = ItemStatus.XOkay;
						}

						if (!scenario.Diff.IsEqual() && scenario.Status != ItemStatus.XDeleted)
						{
							scenario.Status = ItemStatus.XModified;
						} 

						if (scenario.DidPass)
						{
							scenario.TestStatus = TestStatus.XPassed;
						}
						else
						{
							scenario.TestStatus = TestStatus.XFailed;
							scenario.ErrorText = final;
						}
					}
				}
			}

			return diffResult;
		}
	}
}
