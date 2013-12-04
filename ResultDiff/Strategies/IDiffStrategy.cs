using System.IO;
using NPEG;
using ResultDiff.FeatureParser;
using ResultDiff.FeatureParser.Models;

namespace ResultDiff.Strategies
{
	public abstract class IDiffStrategy
	{
		public abstract DiffResult Diff(DiffContext ctx);

		public Feature GetFeatureFromFile(string path)
		{
			using (var stream = new FileStream(path, FileMode.Open))
			{
				var inputIterator = new StreamInputIterator(stream);

				var parser = new Parser();
				var node = parser.Parse(inputIterator);

				var builder = new FeatureBuilder(inputIterator);
				node.Accept(builder);

				return builder.Result;
			}
		}

		public Feature GetFeatureFromString(string featureText)
		{ 
				var inputIterator = new ByteInputIterator(System.Text.Encoding.UTF8.GetBytes(featureText));

				var parser = new Parser();
				var node = parser.Parse(inputIterator);

				var builder = new FeatureBuilder(inputIterator);
				node.Accept(builder);

				return builder.Result; 
		}
	}
}
