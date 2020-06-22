using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;

public class SceneExplorer : Control {

  private List<string> chaptersList;
  private Dictionary<string, string> chaptersDict;
  private Dictionary<string, List<string>> scenesList;
  private Dictionary<string, Dictionary<string, PackedScene>> scenesDict;

  private MarginContainer CurrentSceneContainer;
  private Button PrevChapterButton;
  private Button NextChapterButton;
  private OptionButton SelectChapterButton;
  private Button PrevExampleButton;
  private Button NextExampleButton;
  private OptionButton SelectExampleButton;
  private Button ReloadExampleButton;
  private RichTextLabel CodeLabel;
  private RichTextLabel SummaryLabel;
  private ColorRect CodeBackground;
  private Button ToggleCodeButton;

  private string currentChapter;
  private string currentScene;

  public SceneExplorer() {
    chaptersList = new List<string>();
    chaptersDict = new Dictionary<string, string>();
    scenesList = new Dictionary<string, List<string>>();
    scenesDict = new Dictionary<string, Dictionary<string, PackedScene>>();

    ScanChapters();
    ScanScenes();

    currentChapter = "";
    currentScene = "";
    if (chaptersList.Count > 0) {
      currentChapter = chaptersList[0];
    }

    if (currentChapter != "" && scenesList[currentChapter].Count > 0) {
      currentScene = scenesList[currentChapter][0];
    }
  }

  public void ScanChapters() {
    Regex rgx = new Regex(@"(?<idx>\d+)-(?<name>.+)");

    var dir = new Directory();
    dir.Open("res://chapters");
    dir.ListDirBegin(true);

    while (true) {
      var elem = dir.GetNext();
      if (elem == "") {
        break;
      }

      if (!elem.Contains(".")) {
        var groups = rgx.Match(elem).Groups;
        var chapterName = groups["idx"] + " - " + groups["name"].Value.Capitalize();

        chaptersList.Add(chapterName);
        chaptersDict.Add(chapterName, "res://chapters/" + elem);
      }
    }

    dir.ListDirEnd();
  }

  public void ScanScenes() {
    Regex rgx = new Regex(@"C(?<chapter>\d+)(?<category>(Example|Exercise))(?<idx>\d+)");
    Regex prettyRgx = new Regex(@"(?<category>(Example|Exercise)) (?<idx>\d+)");

    foreach (string chapterName in chaptersList) {
      string chapterPath = chaptersDict[chapterName];
      var list = new List<string>();
      var dict = new Dictionary<string, PackedScene>();

      Directory dir = new Directory();
      dir.Open(chapterPath);
      dir.ListDirBegin(true);

      while (true) {
        string elem = dir.GetNext();
        if (elem == "") {
          break;
        }

        if (elem.EndsWith(".tscn")) {
          string sceneFileName = elem.Substr(0, elem.Length - 5);

          var groups = rgx.Match(sceneFileName).Groups;
          string sceneName = groups["category"].Value + " " + groups["idx"].Value;

          list.Add(sceneName);
          dict.Add(sceneName, (PackedScene)GD.Load(chapterPath + "/" + elem));
        }
      }

      dir.ListDirEnd();

      // Sort scenes by name
      list.Sort(delegate (string x, string y) {
        GroupCollection xMatchGroups = prettyRgx.Match(x).Groups;
        GroupCollection yMatchGroups = prettyRgx.Match(y).Groups;

        string xCategory = xMatchGroups["category"].Value;
        string yCategory = yMatchGroups["category"].Value;
        int xIdx = xMatchGroups["idx"].Value.ToInt();
        int yIdx = yMatchGroups["idx"].Value.ToInt();

        if (xCategory == "Exercise" && yCategory != "Exercise") {
          return 1;
        }
        else if (xCategory != "Exercise" && yCategory == "Exercise") {
          return -1;
        }
        else {
          return xIdx.CompareTo(yIdx);
        }
      });

      scenesList[chapterName] = list;
      scenesDict[chapterName] = dict;
    }
  }

  public void SelectPrevChapter() {
    int chapPos = chaptersList.IndexOf(currentChapter);
    int prevPos;
    if (chapPos == 0) {
      prevPos = chaptersList.Count - 1;
    }
    else {
      prevPos = chapPos - 1;
    }

    SelectChapterFromId(prevPos);
  }

  public void SelectNextChapter() {
    int chapPos = chaptersList.IndexOf(currentChapter);
    int nextPos;
    if (chapPos == chaptersList.Count - 1) {
      nextPos = 0;
    }
    else {
      nextPos = chapPos + 1;
    }

    SelectChapterFromId(nextPos);
  }

  public void SelectChapter(string chapter) {
    currentChapter = chapter;
    LoadExampleItems();
    SelectExampleFromId(0);
  }

  private void SelectChapterFromId(int index) {
    SelectChapterButton.Selected = index;
    string itemName = SelectChapterButton.GetItemText(index);
    SelectChapter(itemName);
  }

  private void SelectExampleFromId(int index) {
    SelectExampleButton.Selected = index;
    string itemName = SelectExampleButton.GetItemText(index);
    SelectExample(itemName);
  }

  public void SelectPrevExample() {
    var scenePos = scenesList[currentChapter].IndexOf(currentScene);
    int prevPos;
    if (scenePos == 0) {
      prevPos = scenesList[currentChapter].Count - 1;
    }
    else {
      prevPos = scenePos - 1;
    }

    SelectExampleFromId(prevPos);
  }

  public void SelectNextExample() {
    var scenePos = scenesList[currentChapter].IndexOf(currentScene);
    int nextPos;
    if (scenePos == scenesList[currentChapter].Count - 1) {
      nextPos = 0;
    }
    else {
      nextPos = scenePos + 1;
    }

    SelectExampleFromId(nextPos);
  }

  public void SelectExample(string scene) {
    currentScene = scene;
    LoadCurrentExample();
  }

  public void LoadCurrentExample() {
    var scene = scenesDict[currentChapter][currentScene];
    foreach (Node child in CurrentSceneContainer.GetChildren()) {
      child.QueueFree();
    }

    var instance = scene.Instance();
    CurrentSceneContainer.AddChild(instance);

    // Show code
    CSharpScript script = (CSharpScript)instance.GetScript();
    string scriptPath = script.ResourcePath;
    CodeLabel.BbcodeEnabled = true;
    CodeLabel.BbcodeText = ReadSourceCodeAtPath(scriptPath);
    CodeLabel.ScrollToLine(0);

    // Set summary
    if (instance is IExample baseInstance) {
      SummaryLabel.Text = baseInstance._Summary();
    }
  }

  private string ReadSourceCodeAtPath(string path) {
    var f = new File();
    f.Open(path, File.ModeFlags.Read);
    var code = f.GetAsText();
    f.Close();

    var highlighter = new SyntaxHighlighter();
    return highlighter.HighlightWithBBCode(code);
  }

  private void LoadChapterItems() {
    SelectChapterButton.Clear();

    foreach (string chapterName in chaptersList) {
      SelectChapterButton.AddItem(chapterName);
    }
  }

  private void LoadExampleItems() {
    SelectExampleButton.Clear();

    foreach (string sceneName in scenesList[currentChapter]) {
      SelectExampleButton.AddItem(sceneName);
    }
  }

  public void ToggleCodeLabel() {
    CodeBackground.Visible = !CodeBackground.Visible;
    CodeLabel.Visible = !CodeLabel.Visible;
    SummaryLabel.Visible = !SummaryLabel.Visible;
  }

  public override void _Ready() {
    CurrentSceneContainer = GetNode<MarginContainer>("Container/CurrentScene");
    CodeLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Code");
    SummaryLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Summary");
    CodeBackground = GetNode<ColorRect>("Container/VBox/TopControl/CodeBackground");
    ToggleCodeButton = GetNode<Button>("Container/VBox/Buttons/ToggleCodeButton");
    PrevChapterButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/PrevChapter");
    NextChapterButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/NextChapter");
    SelectChapterButton = GetNode<OptionButton>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/SelectChapter");
    PrevExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/PrevExample");
    NextExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/NextExample");
    SelectExampleButton = GetNode<OptionButton>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/SelectExample");
    ReloadExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/ReloadExample");

    PrevChapterButton.Connect("pressed", this, nameof(SelectPrevChapter));
    NextChapterButton.Connect("pressed", this, nameof(SelectNextChapter));
    PrevExampleButton.Connect("pressed", this, nameof(SelectPrevExample));
    NextExampleButton.Connect("pressed", this, nameof(SelectNextExample));
    ToggleCodeButton.Connect("pressed", this, nameof(ToggleCodeLabel));
    ReloadExampleButton.Connect("pressed", this, nameof(LoadCurrentExample));
    SelectChapterButton.Connect("item_selected", this, nameof(SelectChapterFromId));
    SelectExampleButton.Connect("item_selected", this, nameof(SelectExampleFromId));

    SummaryLabel.Visible = true;
    CodeBackground.Visible = false;
    CodeLabel.Visible = false;

    LoadChapterItems();
    SelectChapterFromId(0);
  }
}
