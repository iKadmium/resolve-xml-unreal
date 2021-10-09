using System;
using Xunit;
using System.Xml.Linq;
using ResolveXmlUnreal;
using System.Linq;

namespace ResolveXmlUnreal.Test
{
	public class ProgramTests
	{
		private static XDocument GetDocument()
		{
			var doc = new XDocument(
				new XElement("xmeml",
					new XElement("sequence",
						new XElement("media",
							new XElement("video",
								new XElement("track",
									new XElement("clipitem",
										new XElement("name", "Video Clip")
									)
								)
							),
							new XElement("audio",
								new XElement("anything")
							)
						)
					)
				)
			);
			return doc;
		}

		[Fact]
		public void When_FixXmlIsCalled_Then_ItShouldAddASequenceId()
		{
			var doc = GetDocument();

			var sequenceElement = doc
				.Element("xmeml")
				.Element("sequence");

			Assert.Null(sequenceElement.Attribute("id")?.Value);

			Program.FixXml(doc);

			Assert.NotNull(sequenceElement.Attribute("id")?.Value);
		}

		[Fact]
		public void When_FixXmlIsCalled_Then_ItShouldRemoveTheAudioTracks()
		{
			var doc = GetDocument();

			var audioElements = doc
				.Element("xmeml")
				.Element("sequence")
				.Element("media")
				.Elements("audio");

			Assert.True(audioElements.Count() > 0);

			Program.FixXml(doc);

			Assert.False(audioElements.Count() > 0);
		}

		[Fact]
		public void When_FixXmlIsCalled_Then_ItShouldRemoveSpacesFromVideoClipNames()
		{
			var doc = GetDocument();

			var clipNames = doc
				.Element("xmeml")
				.Element("sequence")
				.Element("media")
				.Element("video")
				.Elements("track")
				.Elements("clipitem")
				.Elements("name");

			Assert.Contains(clipNames, x => x.Value.Contains(" "));

			Program.FixXml(doc);

			Assert.DoesNotContain(clipNames, x => x.Value.Contains(" "));
		}
	}
}
