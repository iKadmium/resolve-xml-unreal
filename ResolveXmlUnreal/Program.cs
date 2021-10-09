using System;
using CommandLine;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ResolveXmlUnreal
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var options = await Parser.Default
				.ParseArguments<Options>(args)
				.WithParsedAsync<Options>(async options => await Convert(options.InputFile, options.OutputFile));
		}

		static async Task Convert(string inputPath, string outputPath)
		{
			var inputStream = new FileStream(inputPath, FileMode.Open);
			var xml = await XDocument.LoadAsync(inputStream, LoadOptions.None, CancellationToken.None);

			FixXml(xml);

			var outputStream = new FileStream(outputPath, FileMode.Create);
			await xml.SaveAsync(outputStream, SaveOptions.None, CancellationToken.None);
		}

		public static void FixXml(XDocument xml)
		{
			//add a sequence id
			xml.Element("xmeml")
				.Element("sequence")
				.Add(new XAttribute("id", "sequence-id"));

			//remove the audio tracks
			xml.Element("xmeml")
				.Element("sequence")
				.Element("media")
				.Element("audio")
				.Remove();

			//remove spaces from video element names
			var clipNames = xml.Element("xmeml")
				.Element("sequence")
				.Element("media")
				.Element("video")
				.Elements("track")
				.Elements("clipitem")
				.Elements("name");

			foreach (var clipName in clipNames)
			{
				clipName.Value = clipName.Value.Replace(" ", "");
			}
		}
	}
}
