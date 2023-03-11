using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagicNote.Core.Models
{
	/// <summary>
	/// Represents the result of analzying the text with Azure Conversational Language Service (Cognitive Service).
	/// </summary>
	public class AITextAnalyzeResponse
	{
		[JsonPropertyName("query")]
		public string? Query { get; set; }

		[JsonPropertyName("predication")]
		public Prediction? Prediction { get; set; }
	}

	public class Prediction
	{
		[JsonPropertyName("topIntent")]
		public string? TopIntent { get; set; }
		[JsonPropertyName("projectKind")]
		public string? ProjectKind { get; set; }
		[JsonPropertyName("intents")]
		public Intent[]? Intents { get; set; }
		[JsonPropertyName("entities")]
		public Entity[]? Entities { get; set; }
	}

	public class Intent
	{
		[JsonPropertyName("category")]
		public string? Category { get; set; }
		[JsonPropertyName("confidenceScore")]
		public float? ConfidenceScore { get; set; }
	}

	public class Entity
	{
		[JsonPropertyName("category")]
		public string? Category { get; set; }
		[JsonPropertyName("text")]
		public string? Text { get; set; }
		[JsonPropertyName("offset")]
		public int? Offset { get; set; }
		[JsonPropertyName("length")]
		public int? Length { get; set; }
		[JsonPropertyName("confidenceScore")]
		public int? ConfidenceScore { get; set; }
		[JsonPropertyName("extraInformation")]
		public Extrainformation[]? ExtraInformation { get; set; }
		[JsonPropertyName("resolutions")]
		public Resolution[]? Resolutions { get; set; }
	}

	public class Extrainformation
	{
		[JsonPropertyName("extraInformationKind")]
		public string? ExtraInformationKind { get; set; }

		[JsonPropertyName("value")]
		public string? Value { get; set; }
	}

	public class Resolution
	{
		[JsonPropertyName("resolutionKind")]
		public string? ResolutionKind { get; set; }
		[JsonPropertyName("dateTimeSubKind")]
		public string? DateTimeSubKind { get; set; }
		[JsonPropertyName("timex")]
		public string? Timex { get; set; }
		[JsonPropertyName("value")]
		public string? Value { get; set; }
	}

}
