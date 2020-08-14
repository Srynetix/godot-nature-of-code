using Godot;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SceneLoader : Node
{
  // Sent when all scenes are loaded
  [Signal] public delegate void scenes_loaded();

  private List<string> chaptersList;
  private Dictionary<string, string> chaptersDict;
  private Dictionary<string, List<string>> scenesList;
  private Dictionary<string, Dictionary<string, PackedScene>> scenesDict;
  private string currentChapter;
  private string currentScene;

  public SceneLoader()
  {
    chaptersList = new List<string>();
    chaptersDict = new Dictionary<string, string>();
    scenesList = new Dictionary<string, List<string>>();
    scenesDict = new Dictionary<string, Dictionary<string, PackedScene>>();
    currentChapter = "";
    currentScene = "";
  }

  public override void _Ready()
  {
    // Scan on ready
    ScanScenes();
  }

  public void ScanScenes()
  {
    ScanChapters();
    ScanSamples();

    if (chaptersList.Count > 0)
    {
      currentChapter = chaptersList[0];
    }

    if (currentChapter != "" && scenesList[currentChapter].Count > 0)
    {
      currentScene = scenesList[currentChapter][0];
    }

    // Send event
    EmitSignal(nameof(scenes_loaded));
  }

  public List<string> GetCurrentChapterSampleNames()
  {
    if (currentChapter != "")
    {
      return scenesList[currentChapter];
    }
    else
    {
      return new List<string>();
    }
  }

  public List<string> GetChapterNames()
  {
    return chaptersList;
  }

  public PackedScene GetCurrentSample()
  {
    if (currentChapter != "" && currentScene != "")
    {
      return scenesDict[currentChapter][currentScene];
    }

    return null;
  }

  public void SetCurrentSample(string name)
  {
    currentScene = name;
  }

  public void SetCurrentChapter(string name)
  {
    currentChapter = name;
  }

  public int GetNextSampleId()
  {
    if (currentChapter != "" && currentScene != "")
    {
      var scenePos = scenesList[currentChapter].IndexOf(currentScene);
      if (scenePos == scenesList[currentChapter].Count - 1)
      {
        return 0;
      }
      else
      {
        return scenePos + 1;
      }
    }

    return -1;
  }

  public int GetPrevSampleId()
  {
    if (currentChapter != "" && currentScene != "")
    {
      var scenePos = scenesList[currentChapter].IndexOf(currentScene);
      if (scenePos == 0)
      {
        return scenesList[currentChapter].Count - 1;
      }
      else
      {
        return scenePos - 1;
      }
    }

    return -1;
  }

  public int GetNextChapterId()
  {
    if (currentChapter != "")
    {
      int chapPos = chaptersList.IndexOf(currentChapter);
      if (chapPos == chaptersList.Count - 1)
      {
        return 0;
      }
      else
      {
        return chapPos + 1;
      }
    }

    return -1;
  }

  public int GetPrevChapterId()
  {
    if (currentChapter != "")
    {
      int chapPos = chaptersList.IndexOf(currentChapter);
      if (chapPos == 0)
      {
        return chaptersList.Count - 1;
      }
      else
      {
        return chapPos - 1;
      }
    }

    return -1;
  }

  private void ScanChapters()
  {
    Regex rgx = new Regex(@"(?<idx>\d+)-(?<name>.+)");

    var dir = new Directory();
    dir.Open("res://chapters");
    dir.ListDirBegin(true);

    while (true)
    {
      var elem = dir.GetNext();
      if (elem == "")
      {
        break;
      }

      if (!elem.Contains("."))
      {
        var groups = rgx.Match(elem).Groups;
        var chapterName = groups["idx"] + " - " + groups["name"].Value.Capitalize();

        chaptersList.Add(chapterName);
        chaptersDict.Add(chapterName, "res://chapters/" + elem);
      }
    }

    // Sort chapters by name
    chaptersList.Sort();

    dir.ListDirEnd();
  }

  private void ScanSamples()
  {
    Regex rgx = new Regex(@"C(?<chapter>\d+)(?<category>(Example|Exercise))(?<idx>\d+)");
    Regex prettyRgx = new Regex(@"(?<category>(Exam\.|Exer\.)) (?<idx>\d+)");

    foreach (string chapterName in chaptersList)
    {
      string chapterPath = chaptersDict[chapterName];
      var list = new List<string>();
      var dict = new Dictionary<string, PackedScene>();

      Directory dir = new Directory();
      dir.Open(chapterPath);
      dir.ListDirBegin(true);

      while (true)
      {
        string elem = dir.GetNext();
        if (elem == "")
        {
          break;
        }

        if (elem.EndsWith(".tscn"))
        {
          string sceneFileName = elem.Substr(0, elem.Length - 5);

          var groups = rgx.Match(sceneFileName).Groups;
          string sceneName = groups["category"].Value.Substring(0, 4) + ". " + groups["idx"].Value;

          var scene = (PackedScene)GD.Load(chapterPath + "/" + elem);
          var descr = ExtractSceneSummary(scene);
          sceneName += " - " + descr;

          list.Add(sceneName);
          dict.Add(sceneName, scene);
        }
      }

      dir.ListDirEnd();

      // Sort scenes by name
      list.Sort(delegate (string x, string y)
      {
        GroupCollection xMatchGroups = prettyRgx.Match(x).Groups;
        GroupCollection yMatchGroups = prettyRgx.Match(y).Groups;

        string xCategory = xMatchGroups["category"].Value;
        string yCategory = yMatchGroups["category"].Value;
        int xIdx = xMatchGroups["idx"].Value.ToInt();
        int yIdx = yMatchGroups["idx"].Value.ToInt();

        if (xCategory == "Exer." && yCategory != "Exer.")
        {
          return 1;
        }
        else if (xCategory != "Exer." && yCategory == "Exer.")
        {
          return -1;
        }
        else
        {
          return xIdx.CompareTo(yIdx);
        }
      });

      scenesList[chapterName] = list;
      scenesDict[chapterName] = dict;
    }
  }

  private string ExtractSceneSummary(PackedScene packedScene)
  {
    var inst = packedScene.Instance();
    var exampleInst = inst as IExample;
    if (exampleInst == null)
    {
      GD.PrintErr("Error while reading '" + packedScene.ResourcePath + "' example summary. Make sure you inherited the IExample interface.");
      return "";
    }

    var descr = exampleInst._Summary();
    inst.QueueFree();

    var splitString = descr.Split('\n');
    if (splitString.Length < 2)
    {
      GD.PrintErr("Error while reading '" + packedScene.ResourcePath + "' example summary. It should have at least 2 lines.");
      return "";
    }

    var secondLine = splitString[1];

    // Only get nth first characters
    var maxLength = 30;
    if (secondLine.Length > maxLength)
    {
      secondLine = secondLine.Substring(0, maxLength - 3) + "...";
    }
    return secondLine;
  }
}
