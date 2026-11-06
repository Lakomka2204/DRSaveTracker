using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DRSTCore;


public partial class IniParser
{
    public List<IniSection> Sections { get; set; }

    [GeneratedRegex(@"^\[(.+)\]$")]
    private static partial Regex SectionRegex();

    [GeneratedRegex(@"^(.+)=""(.+)""$")]
    private static partial Regex ParameterRegex();

    public IniParser(IEnumerable<string> _lines)
    {
        var lines = _lines.ToArray();
        Sections = [];
        if (lines.Length == 0)
            return;
        IniSection? currentSection = null;
        var secRegex = SectionRegex();
        var parRegex = ParameterRegex();
        for (int i = 0; i < lines.Length; i++)
        {
            
            var line = lines[i].Trim();
            if (line.StartsWith('#'))
                continue;
            var secMatch = secRegex.Match(line);
            if (secMatch.Success)
            {
                if (currentSection is not null)
                    Sections.Add(currentSection);
                currentSection = new()
                {
                    Name=secMatch.Groups[1].Value  
                };
                continue;
            }
            var parMatch = parRegex.Match(line);
            if (parMatch.Success)
            {
                if (currentSection is null)
                    continue;
                IniParameter param = new()
                {
                    Name = parMatch.Groups[1].Value,
                    Value = parMatch.Groups[2].Value
                };
                currentSection.Parameters.Add(param);
            }
        }
        if (currentSection is not null)
            Sections.Add(currentSection);
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        foreach(var sec in Sections)
            sb.AppendLine(sec.ToString());
        return sb.ToString();
    }
    public IniSection? GetSection(string name)
        => Sections.Find(s => s.Name == name);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="_">Create new section if doesn't exists regardless of this argument</param>
    /// <returns></returns>
    public IniSection GetSection(string name, bool _)
    {
        var s = GetSection(name);
        if (s is null)
        {
            s = new IniSection(){Name=name};
            Sections.Add(s);
        }
        return s;
    }
}
public class IniSection
{
    public string Name { get; set; } = string.Empty;
    public List<IniParameter> Parameters {get;init;}
    public IniSection()
    {
        Parameters = [];
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(string.Format("[{0}]",Name));
        foreach(var param in Parameters)
            sb.AppendLine(param.ToString());
        return sb.ToString().Trim();
    }
    public IniParameter? GetParameter(string name)
        => Parameters.Find(p => p.Name == name);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="_">Create new parameter if doesn't exists regardless of this argument</param>
    /// <returns></returns>
    public IniParameter GetParameter(string name, bool _)
    {
        var p = GetParameter(name);
        if (p is null)
        {
            p = new IniParameter() {Name = name};
            Parameters.Add(p);
        }
        return p;
    }
}
public class IniParameter
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public override string ToString()
    {
        return string.Format("{0}=\"{1}\"",Name,Value);
    }
    private static readonly string DoubleFormat = "F6";
    public static string ConvertNumbersToString(object value)
    {
        return value switch
        {
            double d => d.ToString(DoubleFormat, CultureInfo.InvariantCulture),
            int i => ((double)i).ToString(DoubleFormat, CultureInfo.InvariantCulture),
            float f => f.ToString(DoubleFormat, CultureInfo.InvariantCulture),
            long l => ((double)l).ToString(DoubleFormat, CultureInfo.InvariantCulture),
            string s => s,
            _ => throw new NotSupportedException($"Type {value?.GetType().Name} is not yet supported!!"),
        };
    }
}