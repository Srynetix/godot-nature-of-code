using System.Text.RegularExpressions;

using Godot;

/**
Adapted from https://codingvision.net/interface/c-simple-syntax-highlighting
*/

public class SyntaxHighlighter
{

  private string ColorizeMatches(string source, MatchCollection collection, Color color)
  {
    int currentOffset = 0;
    var outputCode = source;

    foreach (Match m in collection)
    {
      int startIndex = m.Index + currentOffset;
      int endIndex = m.Index + m.Length + currentOffset;

      string stringTag = "[color=#" + color.ToHtml(false) + "]";
      string endTag = "[/color]";

      var newOutputCode = outputCode.Insert(startIndex, stringTag);
      newOutputCode = newOutputCode.Insert(endIndex + stringTag.Length, endTag);

      outputCode = newOutputCode;
      currentOffset += stringTag.Length + endTag.Length;
    }

    return outputCode;
  }

  public string HighlightWithBBCode(string code)
  {
    string outputCode = code;

    // getting keywords/functions
    string keywords = @"\b(public|private|protected|partial|static|namespace|class|using|void|float|Vector2|int|bool|string|foreach|in|var|override|if|for|else|new|true|false)\b";
    MatchCollection keywordMatches = Regex.Matches(outputCode, keywords);
    outputCode = ColorizeMatches(outputCode, keywordMatches, Colors.Cyan);

    // getting types/classes from the text 
    string types = @"\b(GD|OS|Engine)\b";
    MatchCollection typeMatches = Regex.Matches(outputCode, types);
    outputCode = ColorizeMatches(outputCode, typeMatches, Colors.DarkCyan);

    // getting strings
    string strings = "\".+?\"";
    MatchCollection stringMatches = Regex.Matches(outputCode, strings);
    outputCode = ColorizeMatches(outputCode, stringMatches, Colors.Brown);

    // inline comments
    string comments = @"(\/\/.*)";
    MatchCollection commentMatches = Regex.Matches(outputCode, comments);
    outputCode = ColorizeMatches(outputCode, commentMatches, Colors.Green);

    // multiline comments
    comments = @"(\/\*.+?\*\/)";
    commentMatches = Regex.Matches(outputCode, comments, RegexOptions.Multiline | RegexOptions.Singleline);
    outputCode = ColorizeMatches(outputCode, commentMatches, Colors.Green);

    return outputCode;
  }
}
