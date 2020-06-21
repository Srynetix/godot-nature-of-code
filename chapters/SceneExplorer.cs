using Godot;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SceneExplorer : Control {

    private List<string> chaptersList;
    private Dictionary<string, string> chaptersDict;
    private Dictionary<string, List<string>> scenesList;
    private Dictionary<string, Dictionary<string, PackedScene>> scenesDict;

    private MarginContainer CurrentSceneContainer;
    private Button PrevChapterButton;
    private Button NextChapterButton;
    private Button SelectChapterButton;
    private Button PrevExampleButton;
    private Button NextExampleButton;
    private Button SelectExampleButton;
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
        var dir = new Directory();
        dir.Open("res://chapters");
        dir.ListDirBegin(true);

        while (true) {
            var elem = dir.GetNext();
            if (elem == "") {
                break;
            }

            if (!elem.Contains(".")) {
                chaptersList.Add(elem);
                chaptersDict.Add(elem, "res://chapters/" + elem);
            }
        }

        dir.ListDirEnd();
    }

    public void ScanScenes() {
        Regex rgx = new Regex(@"C(?<chapter>\d+)(?<category>(Example|Exercise))(?<idx>\d+)");

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
                    string sceneName = elem.Substr(0, elem.Length - 5);
                    list.Add(sceneName);
                    dict.Add(sceneName, (PackedScene)GD.Load(chapterPath + "/" + elem));
                }
            }

            dir.ListDirEnd();

            // Sort scenes by name
            list.Sort(delegate (string x, string y) {
                GroupCollection xMatchGroups = rgx.Match(x).Groups;
                GroupCollection yMatchGroups = rgx.Match(y).Groups;

                string xCategory = xMatchGroups["category"].Value;
                string yCategory = yMatchGroups["category"].Value;
                int xIdx = xMatchGroups["idx"].Value.ToInt();
                int yIdx = yMatchGroups["idx"].Value.ToInt();

                if (xCategory == "Exercise" && yCategory != "Exercise") {
                    return 1;
                } else if (xCategory != "Exercise" && yCategory == "Exercise") {
                    return -1;
                } else {
                    return xIdx.CompareTo(yIdx);
                }
            });

            scenesList[chapterName] = list;
            scenesDict[chapterName] = dict;
        }
    }

    public void SelectPrevChapter() {
        var chapPos = chaptersList.IndexOf(currentChapter);
        if (chapPos == 0) {
            SelectChapter(chaptersList[chaptersList.Count - 1]);
        } else {
            SelectChapter(chaptersList[chapPos - 1]);
        }
    }

    public void SelectNextChapter() {
        var chapPos = chaptersList.IndexOf(currentChapter);
        if (chapPos == chaptersList.Count - 1) {
            SelectChapter(chaptersList[0]);
        } else {
            SelectChapter(chaptersList[chapPos + 1]);
        }
    }

    public void SelectChapter(string chapter) {
        currentChapter = chapter;
        SelectChapterButton.Text = currentChapter;

        var firstExample = scenesList[currentChapter][0];
        SelectExample(firstExample);
    }

    public void SelectPrevExample() {
        var scenePos = scenesList[currentChapter].IndexOf(currentScene);
        if (scenePos == 0) {
            SelectExample(scenesList[currentChapter][scenesList[currentChapter].Count - 1]);
        } else {
            SelectExample(scenesList[currentChapter][scenePos - 1]);
        }
    }

    public void SelectNextExample() {
        var scenePos = scenesList[currentChapter].IndexOf(currentScene);
        if (scenePos == scenesList[currentChapter].Count - 1) {
            SelectExample(scenesList[currentChapter][0]);
        } else {
            SelectExample(scenesList[currentChapter][scenePos + 1]);
        }
    }

    public void SelectExample(string scene) {
        currentScene = scene;
        SelectExampleButton.Text = currentScene;
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
        CodeLabel.Text = ReadSourceCodeAtPath(scriptPath);
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

        return code;
    }

    public void ToggleCodeLabel() {
        CodeBackground.Visible = !CodeBackground.Visible;
        CodeLabel.Visible = !CodeLabel.Visible;
    }

    public override void _Ready() {
        CurrentSceneContainer = GetNode<MarginContainer>("Container/CurrentScene");
        CodeLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Code");
        SummaryLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Summary");
        CodeBackground = GetNode<ColorRect>("Container/VBox/TopControl/CodeBackground");
        ToggleCodeButton = GetNode<Button>("Container/VBox/Buttons/ToggleCodeButton");
        PrevChapterButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/PrevChapter");
        NextChapterButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/NextChapter");
        SelectChapterButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ChapterSelection/SelectChapter");
        PrevExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/PrevExample");
        NextExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/NextExample");
        SelectExampleButton = GetNode<Button>("Container/VBox/Buttons/SelectionButtons/ExampleSelection/SelectExample");

        PrevChapterButton.Connect("pressed", this, nameof(SelectPrevChapter));
        NextChapterButton.Connect("pressed", this, nameof(SelectNextChapter));
        PrevExampleButton.Connect("pressed", this, nameof(SelectPrevExample));
        NextExampleButton.Connect("pressed", this, nameof(SelectNextExample));
        SelectExampleButton.Connect("pressed", this, nameof(LoadCurrentExample));
        ToggleCodeButton.Connect("pressed", this, nameof(ToggleCodeLabel));

        ToggleCodeLabel();
        SelectChapter(currentChapter);
    }
}
