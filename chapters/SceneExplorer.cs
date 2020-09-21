using Godot;

namespace Examples
{
    /// <summary>
    /// Scene explorer screen.
    /// Uses `SceneLoader` to dynamically load examples.
    /// </summary>
    public class SceneExplorer : Control
    {
        private Control CurrentSceneContainer;
        private Button PrevChapterButton;
        private Button NextChapterButton;
        private OptionButton SelectChapterButton;
        private Button PrevExampleButton;
        private Button NextExampleButton;
        private OptionButton SelectExampleButton;
        private Button ReloadExampleButton;
        private RichTextLabel CodeLabel;
        private RichTextLabel SummaryLabel;
        private VBoxContainer SelectionButtons;
        private ColorRect CodeBackground;
        private Button ToggleCodeButton;
        private Button ToggleUIButton;
        private Label LoadingLabel;
        private SceneLoader sceneLoader;

        async public override void _Ready()
        {
            CurrentSceneContainer = GetNode<Control>("CurrentScene");
            CodeLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Code");
            SummaryLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Summary");
            CodeBackground = GetNode<ColorRect>("Container/VBox/TopControl/CodeBackground");
            ToggleCodeButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/LeftButtons/ToggleCodeButton");
            ToggleUIButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/LeftButtons/ToggleUIButton");
            PrevChapterButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/PrevChapter");
            NextChapterButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/NextChapter");
            SelectChapterButton = GetNode<OptionButton>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/SelectChapter");
            PrevExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/PrevExample");
            NextExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/NextExample");
            SelectExampleButton = GetNode<OptionButton>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/SelectExample");
            ReloadExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/ReloadExample");
            SelectionButtons = GetNode<VBoxContainer>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons");
            LoadingLabel = GetNode<Label>("Container/Loading");

            PrevChapterButton.Connect("pressed", this, nameof(SelectPrevChapter));
            NextChapterButton.Connect("pressed", this, nameof(SelectNextChapter));
            PrevExampleButton.Connect("pressed", this, nameof(SelectPrevExample));
            NextExampleButton.Connect("pressed", this, nameof(SelectNextExample));
            ToggleCodeButton.Connect("pressed", this, nameof(ToggleCodeLabel));
            ReloadExampleButton.Connect("pressed", this, nameof(LoadCurrentExample));
            SelectChapterButton.Connect("item_selected", this, nameof(SelectChapterFromId));
            SelectExampleButton.Connect("item_selected", this, nameof(SelectExampleFromId));
            ToggleUIButton.Connect("pressed", this, nameof(ToggleUI));

            SummaryLabel.Visible = false;
            CodeBackground.Visible = false;
            CodeLabel.Visible = false;

            ToggleCodeButton.Visible = false;
            ToggleUIButton.Visible = false;

            sceneLoader = new SceneLoader();
            sceneLoader.Connect(nameof(SceneLoader.ScenesLoaded), this, nameof(OnScenesLoaded));

            // Force min size for exercise selection
            var font = SelectChapterButton.GetFont("font");
            SelectExampleButton.RectMinSize = font.GetCharSize('w') * new Vector2(SceneLoader.SampleNameMaxLength, 1);

            await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            AddChild(sceneLoader);
        }

        private void OnScenesLoaded()
        {
            SummaryLabel.Visible = true;
            LoadingLabel.Visible = false;

            ToggleCodeButton.Visible = true;
            ToggleUIButton.Visible = true;

            LoadChapterItems();
            SelectChapterFromId(0);
        }

        private void SelectPrevChapter()
        {
            var prevPos = sceneLoader.GetPrevChapterId();
            if (prevPos != -1)
            {
                SelectChapterFromId(prevPos);
            }
        }

        private void SelectNextChapter()
        {
            var nextPos = sceneLoader.GetNextChapterId();
            if (nextPos != -1)
            {
                SelectChapterFromId(nextPos);
            }
        }

        private void SelectChapter(string chapter)
        {
            sceneLoader.SetCurrentChapter(chapter);
            LoadExampleItems();
            SelectExampleFromId(0);
        }

        private void SelectChapterFromId(int index)
        {
            SelectChapterButton.Selected = index;
            string itemName = SelectChapterButton.GetItemText(index);
            SelectChapter(itemName);
        }

        private void SelectExampleFromId(int index)
        {
            SelectExampleButton.Selected = index;
            string itemName = SelectExampleButton.GetItemText(index);
            SelectExample(itemName);
        }

        private void SelectPrevExample()
        {
            var prevPos = sceneLoader.GetPrevSampleId();
            if (prevPos != -1)
            {
                SelectExampleFromId(prevPos);
            }
            else
            {
                SelectPrevChapter();
                SelectExampleFromId(sceneLoader.GetCurrentChapterSamplesCount() - 1);
            }
        }

        private void SelectNextExample()
        {
            var nextPos = sceneLoader.GetNextSampleId();
            if (nextPos != -1)
            {
                SelectExampleFromId(nextPos);
            }
            else
            {
                SelectNextChapter();
            }
        }

        private void SelectExample(string scene)
        {
            sceneLoader.SetCurrentSample(scene);
            LoadCurrentExample();
        }

        private void LoadCurrentExample()
        {
            var scene = sceneLoader.GetCurrentSample();
            if (scene == null)
            {
                return;
            }

            foreach (Node child in CurrentSceneContainer.GetChildren())
            {
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
            if (instance is Examples.IExample baseInstance)
            {
                SummaryLabel.Text = baseInstance.GetSummary();
            }
        }

        private string ReadSourceCodeAtPath(string path)
        {
            var f = new File();
            f.Open(path, File.ModeFlags.Read);
            var code = f.GetAsText();
            f.Close();

            var highlighter = new SyntaxHighlighter();
            return highlighter.HighlightWithBBCode(code);
        }

        private void LoadChapterItems()
        {
            SelectChapterButton.Clear();

            foreach (string chapterName in sceneLoader.GetChapterNames())
            {
                SelectChapterButton.AddItem(chapterName);
            }
        }

        private void LoadExampleItems()
        {
            SelectExampleButton.Clear();

            foreach (string sceneName in sceneLoader.GetCurrentChapterSampleNames())
            {
                SelectExampleButton.AddItem(sceneName);
            }
        }

        private void ToggleCodeLabel()
        {
            CodeBackground.Visible = !CodeBackground.Visible;
            CodeLabel.Visible = !CodeLabel.Visible;
            SummaryLabel.Visible = !SummaryLabel.Visible;
        }

        private void ToggleUI()
        {
            SelectionButtons.Visible = !SelectionButtons.Visible;
            ToggleCodeButton.Visible = !ToggleCodeButton.Visible;
            CodeLabel.Visible = false;
            CodeBackground.Visible = false;
            SummaryLabel.Visible = SelectionButtons.Visible;
        }
    }
}
