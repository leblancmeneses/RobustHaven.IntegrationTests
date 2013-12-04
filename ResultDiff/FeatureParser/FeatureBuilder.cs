using System;
using System.Collections.Generic;
using System.Linq;
using NPEG;
using NPEG.Extensions;
using ResultDiff.FeatureParser.Models;

namespace ResultDiff.FeatureParser
{
	public class FeatureBuilder : IAstNodeReplacement
	{
		private readonly IInputIterator _inputIterator;

		private readonly Feature _feature = new Feature();

		public FeatureBuilder(IInputIterator inputIterator)
		{
			_inputIterator = inputIterator;
		}

		public Feature Result
		{
			get { return _feature; }
		}


		public override void VisitEnter(AstNode node)
		{
			switch (node.Token.Name)
			{
				case "Document":
					_feature.Tags.AddRange(LoadTags(node));
					break;
				case "FeatureLine":
					_feature.Title = node.Children["Title"].Token.ValueAsString(_inputIterator);
					break;
				case "Background":
					_feature.Background = new Scenario();
					_feature.Background.Gherkin.AddRange(LoadGherkin(node.Children["Gherkin"]));
					break;
				case "Scenario":
					var s = new Scenario() { Title = node.Children["Title"].Token.ValueAsString(_inputIterator) };
					s.Tags.AddRange(LoadTags(node));
					s.Gherkin.AddRange(LoadGherkin(node.Children["Gherkin"]));
					_feature.AddScenario(s);
					break;
			}
		}

		private IEnumerable<Statement> LoadGherkin(AstNode gherkinNode)
		{
			return gherkinNode.Children.Where(child => child.Token.Name.Equals("Line", StringComparison.InvariantCultureIgnoreCase)).Select(child => new Statement()
				{
					Key = child.Children["Key"].Token.ValueAsString(_inputIterator),
					Message = child.Children["Statement"].Token.ValueAsString(_inputIterator)
				}).ToList();
		}

		private IEnumerable<string> LoadTags(AstNode tagLinesParentNode)
		{
			return tagLinesParentNode.Children.Where(x => x.Token.Name.Equals("TagLine", StringComparison.InvariantCultureIgnoreCase)).SelectMany(tagLine => tagLine.Children.Where(x => x.Token.Name.Equals("Tag", StringComparison.InvariantCultureIgnoreCase))).Select(tag => tag.Children["Name"].Token.ValueAsString(_inputIterator)).ToList();
		}

		public override void VisitLeave(AstNode node)
		{
		}
	}
}