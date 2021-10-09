using CommandLine;

namespace ResolveXmlUnreal
{
	public class Options
	{
		[Option('i', "input", Required = true, HelpText = "Select an input file from resolve")]
		public string InputFile { get; set; }

		[Option('o', "output", Required = true, HelpText = "Select an output file to save to")]
		public string OutputFile { get; set; }
	}
}