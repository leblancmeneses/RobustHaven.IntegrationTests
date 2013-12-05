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
S1: ( NewLine / EndOfLineComment );
W: (s / S);
(?<Table>):(?<Row>
  s* ('|' (?<DataColumn>  (! ('|' / S)  .)+ ))+ '|' S1  
)+;
(?<Gherkin>): (((?<Line> s* (?<Key> 'Given'\i / 'When'\i / 'Then'\i / 'And'\i / 'But'\i )  (?<Statement> (!S .)+ ) ) W*) / Table)+;
(?<TagLine>): (?<Tag>'@'  ((?<Name> (!(s* '@' / s* S) .)+ )) s*)+ W;
(?<FeatureLine>): 'Feature'\i	 s* ':' s* (?<Title> (!S .)+ ) W+ 
  (?<InOrder> s* &'In order to'\i (?<Text> (!S .)+ ) S1)?
  (?<AsAn> s* &'As an'\i (?<Text> (!S .)+ ) S1)?
  (?<IWantTo> s* &'I want to'\i (?<Text> (!S .)+ ) S1)?;
(?<Background>): 'Background'\i	 s* ':' W* Gherkin ;
(?<Scenario>): TagLine* 'Scenario'\i s* ':'  (?<Title> (!S .)+ ) W* Gherkin?;
(?<ScenarioOutline>): TagLine* 'Scenario'\i s 'Outline'\i s* ':'  (?<Title> (!S .)+ ) W* Gherkin? (?<Example> s* 'Examples:'\i S1 Table);
(?<Document>):  W* TagLine* FeatureLine Background? W* ((Scenario/ScenarioOutline) W*)+ ;
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