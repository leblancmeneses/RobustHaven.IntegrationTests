using NPEG;
using NPEG.ApplicationExceptions;
using NPEG.GrammarInterpreter;

namespace ResultDiff.FeatureParser
{
	public class Parser
	{
		public static AExpression RuleInstance = null;
		public static object RuleInstanceLock = new object();

		public AstNode Parse(IInputIterator inputIterator)
		{

			string grammar = @"
NewLine: [\r][\n] / [\n][\r] / [\n] / [\r];
Space: ' ';
Tab: [\t];
(?<EndOfLineComment>): (('#' / '//') (?<msg>(!NewLine .)*) NewLine) ;
(?<MultilineComment>):  ('/*' (?<msg>(!'*/' .)*) '*/');
s: ( Space / Tab / MultilineComment )+;
S: ( NewLine / EndOfLineComment )+;
W: (s / S);
(?<Gherkin>): ((?<Line> s* (?<Key> 'Given'\i / 'When'\i / 'Then'\i / 'And'\i / 'But'\i )  (?<Statement> (!S .)+ ) ) W*)+;
(?<TagLine>): (?<Tag>'@'  ((?<Name> (!(s* '@' / s* S) .)+ )) s*)+ W;
(?<FeatureLine>): 'Feature'\i	 s* ':' s* (?<Title> (!S .)+ ) ;
(?<Background>): 'Background'\i	 s* ':' W* Gherkin ;
(?<Scenario>): TagLine* 'Scenario'\i s* ':'  (?<Title> (!S .)+ ) W* Gherkin?;
(?<Document>):  W* TagLine* FeatureLine W* Background? W* (Scenario W*)+ ;
".Trim();

			lock (RuleInstanceLock)
			{
				if (RuleInstance == null)
				{
					RuleInstance = PEGrammar.Load(grammar);
				}
			}
			var visitor = new NpegParserVisitor(inputIterator);
			RuleInstance.Accept(visitor);

			if (visitor.IsMatch)
			{
				return visitor.AST;
			}

			throw new InvalidInputException();
		}
	}
}