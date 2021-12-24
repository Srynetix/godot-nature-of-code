using Godot;

namespace Examples
{
    /// <summary>
    /// Scene explorer screen.
    /// Uses `SceneLoader` to dynamically load examples.
    /// </summary>
    public class SceneExplorer : Control
    {
        private Control _currentSceneContainer;
        private Button _prevChapterButton;
        private Button _nextChapterButton;
        private OptionButton _selectChapterButton;
        private Button _prevExampleButton;
        private Button _nextExampleButton;
        private OptionButton _selectExampleButton;
        private Button _reloadExampleButton;
        private RichTextLabel _codeLabel;
        private RichTextLabel _summaryLabel;
        private VBoxContainer _selectionButtons;
        private ColorRect _codeBackground;
        private Button _toggleCodeButton;
        private Button _toggleUIButton;
        private Label _loadingLabel;
        private SceneLoader _sceneLoader;

        public override async void _Ready()
        {
            _currentSceneContainer = GetNode<Control>("CurrentScene");
            _codeLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Code");
            _summaryLabel = GetNode<RichTextLabel>("Container/VBox/TopControl/CodeHBox/Summary");
            _codeBackground = GetNode<ColorRect>("Container/VBox/TopControl/CodeBackground");
            _toggleCodeButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/LeftButtons/ToggleCodeButton");
            _toggleUIButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/LeftButtons/ToggleUIButton");
            _prevChapterButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/PrevChapter");
            _nextChapterButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/NextChapter");
            _selectChapterButton = GetNode<OptionButton>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ChapterSelection/SelectChapter");
            _prevExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/PrevExample");
            _nextExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/NextExample");
            _selectExampleButton = GetNode<OptionButton>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/SelectExample");
            _reloadExampleButton = GetNode<Button>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons/ExampleSelection/ReloadExample");
            _selectionButtons = GetNode<VBoxContainer>("Container/VBox/ButtonsMargin/Buttons/SelectionButtons");
            _loadingLabel = GetNode<Label>("Container/Loading");

            _prevChapterButton.Connect("pressed", this, nameof(SelectPrevChapter));
            _nextChapterButton.Connect("pressed", this, nameof(SelectNextChapter));
            _prevExampleButton.Connect("pressed", this, nameof(SelectPrevExample));
            _nextExampleButton.Connect("pressed", this, nameof(SelectNextExample));
            _toggleCodeButton.Connect("pressed", this, nameof(ToggleCodeLabel));
            _reloadExampleButton.Connect("pressed", this, nameof(LoadCurrentExample));
            _selectChapterButton.Connect("item_selected", this, nameof(SelectChapterFromId));
            _selectExampleButton.Connect("item_selected", this, nameof(SelectExampleFromId));
            _toggleUIButton.Connect("pressed", this, nameof(ToggleUI));

            _summaryLabel.Visible = false;
            _codeBackground.Visible = false;
            _codeLabel.Visible = false;

            _toggleCodeButton.Visible = false;
            _toggleUIButton.Visible = false;

            _sceneLoader = new SceneLoader();
            _sceneLoader.Connect(nameof(SceneLoader.ScenesLoaded), this, nameof(OnScenesLoaded));

            // Force min size for exercise selection
            var font = _selectChapterButton.GetFont("font");
            _selectExampleButton.RectMinSize = font.GetCharSize('w') * new Vector2(SceneLoader.SampleNameMaxLength, 1);

            await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            AddChild(_sceneLoader);
        }

        private void OnScenesLoaded()
        {
            _summaryLabel.Visible = true;
            _loadingLabel.Visible = false;

            _toggleCodeButton.Visible = true;
            _toggleUIButton.Visible = true;

            LoadChapterItems();
            SelectChapterFromId(0);
        }

        private void SelectPrevChapter()
        {
            var prevPos = _sceneLoader.GetPrevChapterId();
            if (prevPos != -1)
            {
                SelectChapterFromId(prevPos);
            }
        }

        private void SelectNextChapter()
        {
            var nextPos = _sceneLoader.GetNextChapterId();
            if (nextPos != -1)
            {
                SelectChapterFromId(nextPos);
            }
        }

        private void SelectChapter(string chapter)
        {
            _sceneLoader.SetCurrentChapter(chapter);
            LoadExampleItems();
            SelectExampleFromId(0);
        }

        private void SelectChapterFromId(int index)
        {
            _selectChapterButton.Selected = index;
            string itemName = _selectChapterButton.GetItemText(index);
            SelectChapter(itemName);
        }

        private void SelectExampleFromId(int index)
        {
            _selectExampleButton.Selected = index;
            string itemName = _selectExampleButton.GetItemText(index);
            SelectExample(itemName);
        }

        private void SelectPrevExample()
        {
            var prevPos = _sceneLoader.GetPrevSampleId();
            if (prevPos != -1)
            {
                SelectExampleFromId(prevPos);
            }
            else
            {
                SelectPrevChapter();
                SelectExampleFromId(_sceneLoader.GetCurrentChapterSamplesCount() - 1);
            }
        }

        private void SelectNextExample()
        {
            var nextPos = _sceneLoader.GetNextSampleId();
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
            _sceneLoader.SetCurrentSample(scene);
            LoadCurrentExample();
        }

        private void LoadCurrentExample()
        {
            var scene = _sceneLoader.GetCurrentSample();
            if (scene == null)
            {
                return;
            }

            foreach (Node child in _currentSceneContainer.GetChildren())
            {
                child.QueueFree();
            }

            var instance = scene.Instance();
            _currentSceneContainer.AddChild(instance);

            // Show code
            var script = (CSharpScript)instance.GetScript();
            string scriptPath = script.ResourcePath;
            _codeLabel.BbcodeEnabled = true;
            _codeLabel.BbcodeText = ReadSourceCodeAtPath(scriptPath);
            _codeLabel.ScrollToLine(0);

            // Set summary
            if (instance is Examples.IExample baseInstance)
            {
                _summaryLabel.Text = baseInstance.GetSummary();
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
            _selectChapterButton.Clear();

            foreach (string chapterName in _sceneLoader.GetChapterNames())
            {
                _selectChapterButton.AddItem(chapterName);
            }
        }

        private void LoadExampleItems()
        {
            _selectExampleButton.Clear();

            foreach (string sceneName in _sceneLoader.GetCurrentChapterSampleNames())
            {
                _selectExampleButton.AddItem(sceneName);
            }
        }

        private void ToggleCodeLabel()
        {
            _codeBackground.Visible = !_codeBackground.Visible;
            _codeLabel.Visible = !_codeLabel.Visible;
            _summaryLabel.Visible = !_summaryLabel.Visible;
        }

        private void ToggleUI()
        {
            _selectionButtons.Visible = !_selectionButtons.Visible;
            _toggleCodeButton.Visible = !_toggleCodeButton.Visible;
            _codeLabel.Visible = false;
            _codeBackground.Visible = false;
            _summaryLabel.Visible = _selectionButtons.Visible;
        }
    }
}
