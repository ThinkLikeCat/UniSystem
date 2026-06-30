using System.Text.RegularExpressions;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.DocumentType;

public record TemplateToken(string Type, string Value);

public sealed record TemplateText
{
    private static readonly HashSet<string> AllowedPlaceholders = new(StringComparer.OrdinalIgnoreCase)
    {
        "{student_name}", "{group_name}", "{course}", "{faculty}", "{specialty}",
        "{single_date}", "{start_date}", "{end_date}", 
        "{subject}", "{reason}"
    };
    
    
    public string Value { get; private set; }

    public TemplateText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTemplateTextException();

        var cleaned = value.Trim();
        
        var matches = Regex.Matches(cleaned, @"\{[a-zA-Z0-9_]+\}");
        
        var templatePlaceholders = matches
            .Select(m => m.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        
        foreach (var placeholder in templatePlaceholders)
        {
            if (!AllowedPlaceholders.Contains(placeholder))
                throw new UnknownTemplatePlaceholderException(placeholder);
        }

        Value = cleaned;
    }

    public static implicit operator string(TemplateText templateText) => templateText.Value;
    
    public IReadOnlyList<TemplateToken> Tokenize(IReadOnlyDictionary<string, string> systemValues)
    {
        var tokens = new List<TemplateToken>();
        var parts = Regex.Split(Value, @"(\{[a-zA-Z0-9_]+\})");

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;
            
            if (part.StartsWith('{') && part.EndsWith('}') && systemValues.TryGetValue(part, out var realValue))
                tokens.Add(new TemplateToken("text", realValue));
            else if (part.StartsWith('{') && part.EndsWith('}'))
                tokens.Add(new TemplateToken("variable", part.Trim('{', '}')));
            else
                tokens.Add(new TemplateToken("text", part));
        }

        return tokens;
    }
}