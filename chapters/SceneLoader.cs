using Godot;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Examples
{
    /// <summary>
    /// Dynamic scene loader.
    /// </summary>
    public class SceneLoader : Node
    {
        /// <summary>
        /// Signal sent when all scenes are loaded
        /// </summary>
        [Signal] public delegate void ScenesLoaded();

        /// <summary>Sample name max length</summary>
        public const int SampleNameMaxLength = 30;

        private readonly List<string> _chaptersList;
        private readonly Dictionary<string, string> _chaptersDict;
        private readonly Dictionary<string, List<string>> _scenesList;
        private readonly Dictionary<string, Dictionary<string, PackedScene>> _scenesDict;
        private string _currentChapter;
        private string _currentScene;

        public SceneLoader()
        {
            _chaptersList = new List<string>();
            _chaptersDict = new Dictionary<string, string>();
            _scenesList = new Dictionary<string, List<string>>();
            _scenesDict = new Dictionary<string, Dictionary<string, PackedScene>>();
            _currentChapter = "";
            _currentScene = "";
        }

        /// <summary>
        /// Get current chapter sample names.
        /// </summary>
        /// <returns>Sample names</returns>
        public List<string> GetCurrentChapterSampleNames()
        {
            if (_currentChapter != "")
            {
                return _scenesList[_currentChapter];
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Get current chapter sample count.
        /// </summary>
        /// <returns>Sample count</returns>
        public int GetCurrentChapterSamplesCount()
        {
            if (_currentChapter != "")
            {
                return _scenesDict[_currentChapter].Count;
            }

            return -1;
        }

        /// <summary>
        /// Get chapter names.
        /// </summary>
        /// <returns>Chapter names</returns>
        public List<string> GetChapterNames()
        {
            return _chaptersList;
        }

        /// <summary>
        /// Get current sample scene.
        /// </summary>
        /// <returns>Sample scene</returns>
        public PackedScene GetCurrentSample()
        {
            if (_currentChapter != "" && _currentScene != "")
            {
                return _scenesDict[_currentChapter][_currentScene];
            }

            return null;
        }

        /// <summary>
        /// Set current sample from name.
        /// </summary>
        /// <param name="name">Sample name</param>
        public void SetCurrentSample(string name)
        {
            _currentScene = name;
        }

        /// <summary>
        /// Set current chapter from name.
        /// </summary>
        /// <param name="name">Chapter name</param>
        public void SetCurrentChapter(string name)
        {
            _currentChapter = name;
        }

        /// <summary>
        /// Get next sample index.
        /// Returns '-1' if next sample does not exist.
        /// </summary>
        /// <returns>Sample index</returns>
        public int GetNextSampleId()
        {
            if (_currentChapter != "" && _currentScene != "")
            {
                var scenePos = _scenesList[_currentChapter].IndexOf(_currentScene);
                if (scenePos == _scenesList[_currentChapter].Count - 1)
                {
                    return -1;
                }
                else
                {
                    return scenePos + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get previous sample index.
        /// Returns '-1' if previous sample does not exist.
        /// </summary>
        /// <returns>Sample index</returns>
        public int GetPrevSampleId()
        {
            if (_currentChapter != "" && _currentScene != "")
            {
                var scenePos = _scenesList[_currentChapter].IndexOf(_currentScene);
                if (scenePos == 0)
                {
                    return -1;
                }
                else
                {
                    return scenePos - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get next chapter index.
        /// Returns '-1' if next chapter does not exist.
        /// </summary>
        /// <returns>Chapter index</returns>
        public int GetNextChapterId()
        {
            if (_currentChapter != "")
            {
                int chapPos = _chaptersList.IndexOf(_currentChapter);
                if (chapPos == _chaptersList.Count - 1)
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

        /// <summary>
        /// Get previous chapter index.
        /// Returns '-1' if previous chapter does not exist.
        /// </summary>
        /// <returns>Chapter index</returns>
        public int GetPrevChapterId()
        {
            if (_currentChapter != "")
            {
                int chapPos = _chaptersList.IndexOf(_currentChapter);
                if (chapPos == 0)
                {
                    return _chaptersList.Count - 1;
                }
                else
                {
                    return chapPos - 1;
                }
            }

            return -1;
        }

        public override void _Ready()
        {
            ScanScenes();
        }

        private void ScanScenes()
        {
            ScanChapters();
            ScanSamples();

            if (_chaptersList.Count > 0)
            {
                _currentChapter = _chaptersList[0];
            }

            if (_currentChapter != "" && _scenesList[_currentChapter].Count > 0)
            {
                _currentScene = _scenesList[_currentChapter][0];
            }

            // Send event
            EmitSignal(nameof(ScenesLoaded));
        }

        private void ScanChapters()
        {
            var rgx = new Regex(@"(?<idx>\d+)-(?<name>.+)");

            var dir = new Directory();
            dir.Open("res://chapters");
            dir.ListDirBegin(true);

            while (true)
            {
                var elem = dir.GetNext();
                if (elem.Length == 0)
                {
                    break;
                }

                if (!elem.Contains("."))
                {
                    var groups = rgx.Match(elem).Groups;
                    var chapterName = groups["idx"] + " - " + groups["name"].Value.Replace("-", " ").Capitalize();

                    _chaptersList.Add(chapterName);
                    _chaptersDict.Add(chapterName, "res://chapters/" + elem);
                }
            }

            // Sort chapters by name
            _chaptersList.Sort();

            dir.ListDirEnd();
        }

        private void ScanSamples()
        {
            var rgx = new Regex(@"C(?<chapter>\d+)(?<category>(Example|Exercise))(?<idx>\d+)");
            var prettyRgx = new Regex(@"(?<idx>\d+)");

            foreach (string chapterName in _chaptersList)
            {
                string chapterPath = _chaptersDict[chapterName];
                var list = new List<string>();
                var dict = new Dictionary<string, PackedScene>();

                var dir = new Directory();
                dir.Open(chapterPath);
                dir.ListDirBegin(true);

                while (true)
                {
                    string elem = dir.GetNext();
                    if (elem.Length == 0)
                    {
                        break;
                    }

                    if (elem.EndsWith(".tscn"))
                    {
                        string sceneFileName = elem.Substr(0, elem.Length - 5);

                        var groups = rgx.Match(sceneFileName).Groups;
                        var category = groups["category"].Value;
                        var exampleId = groups["idx"].Value;
                        string sceneName = (category == "Exercise") ? exampleId + "x" : exampleId;

                        var scene = (PackedScene)GD.Load(chapterPath + "/" + elem);
                        var descr = ExtractSceneSummary(scene);
                        sceneName += " - " + descr;

                        list.Add(sceneName);
                        dict.Add(sceneName, scene);
                    }
                }

                dir.ListDirEnd();

                // Sort scenes by name
                list.Sort((x, y) =>
                {
                    GroupCollection xMatchGroups = prettyRgx.Match(x).Groups;
                    GroupCollection yMatchGroups = prettyRgx.Match(y).Groups;
                    int xIdx = xMatchGroups["idx"].Value.ToInt();
                    int yIdx = yMatchGroups["idx"].Value.ToInt();

                    return xIdx.CompareTo(yIdx);
                });

                _scenesList[chapterName] = list;
                _scenesDict[chapterName] = dict;
            }
        }

        private string ExtractSceneSummary(PackedScene packedScene)
        {
            var inst = packedScene.Instance();
            if (inst is IExample exampleInst)
            {
                var descr = exampleInst.GetSummary();
                inst.QueueFree();

                var splitString = descr.Split('\n');
                if (splitString.Length < 2)
                {
                    GD.PrintErr("Error while reading '" + packedScene.ResourcePath + "' example summary. It should have at least 2 lines.");
                    return "";
                }

                var secondLine = splitString[1];

                // Only get nth first characters
                if (secondLine.Length > SampleNameMaxLength)
                {
                    secondLine = secondLine[0..(SampleNameMaxLength - 3)] + "...";
                }
                return secondLine;
            }
            else
            {
                GD.PrintErr("Error while reading '" + packedScene.ResourcePath + "' example summary. Make sure you inherited the IExample interface.");
                return "";
            }
        }
    }
}
