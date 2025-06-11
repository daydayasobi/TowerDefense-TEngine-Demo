#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;
using System.Diagnostics;
using Type = System.Type;
using Delegate = System.Delegate;
using Action = System.Action;
using static VTabs.VTabsCache;
using static VTabs.VTabs;
using static VTabs.Libs.VUtils;
using static VTabs.Libs.VGUI;




namespace VTabs
{
    public class VTabsAddTabWindow : EditorWindow
    {

        void OnGUI()
        {

            void background()
            {
                windowRect.Draw(windowBackground);

            }
            void closeOnEscape()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.Escape) return;

                Close();

                dockArea.GetMemberValue<EditorWindow>("actualView").Repaint(); // for + button to fade

                GUIUtility.ExitGUI();

            }
            void addTabOnEnter()
            {
                // if (!curEvent.isKeyDown) return; // searchfield steals fpcus
                if (curEvent.keyCode != KeyCode.Return) return;

                if (keyboardFocusedRowIndex == -1) return;
                if (keyboardFocusedEntry == null) return;

                AddTab(keyboardFocusedEntry);

            }
            void arrowNavigation()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.UpArrow && curEvent.keyCode != KeyCode.DownArrow) return;

                curEvent.Use();


                if (curEvent.keyCode == KeyCode.UpArrow)
                    if (keyboardFocusedRowIndex == 0)
                        keyboardFocusedRowIndex = rowCount - 1;
                    else
                        keyboardFocusedRowIndex--;

                if (curEvent.keyCode == KeyCode.DownArrow)
                    if (keyboardFocusedRowIndex == rowCount - 1)
                        keyboardFocusedRowIndex = 0;
                    else
                        keyboardFocusedRowIndex++;


                keyboardFocusedRowIndex = keyboardFocusedRowIndex.Clamp(0, rowCount - 1);

            }
            void updateSearch()
            {
                if (searchString == prevSearchString) return;

                prevSearchString = searchString;


                if (searchString == "") { keyboardFocusedRowIndex = -1; return; }

                UpdateSearch();

                keyboardFocusedRowIndex = 0;

            }


            void searchField_()
            {
                var searchRect = windowRect.SetHeight(18).MoveY(1).AddWidthFromMid(-2);


                if (searchField == null)
                {
                    searchField = new SearchField();
                    searchField.SetFocus();

                }


                searchString = searchField.OnGUI(searchRect, searchString);

            }
            void rows()
            {
                void bookmarked()
                {
                    if (searchString != "") return;
                    if (!bookmarkedEntries.Any()) return;

                    bookmarksRect = windowRect.SetHeight(bookmarkedEntries.Count * rowHeight + gaps.Sum());

                    BookmarksGUI();

                }
                void divider()
                {
                    if (searchString != "") return;
                    if (!bookmarkedEntries.Any()) return;

                    var splitterColor = Greyscale(.36f);
                    var splitterRect = bookmarksRect.SetHeightFromBottom(0).SetHeight(dividerHeight).SetHeightFromMid(1).AddWidthFromMid(-10);

                    splitterRect.Draw(splitterColor);

                }
                void notBookmarked()
                {
                    if (searchString != "") return;

                    if (bookmarkedEntries.Any())
                        nextRowY = bookmarksRect.yMax + dividerHeight;

                    foreach (var entry in allEntries)
                    {
                        if (bookmarkedEntries.Contains(entry)) continue;
                        if (entry == draggedBookmark) continue;

                        RowGUI(windowRect.SetHeight(rowHeight).SetY(nextRowY), entry);

                        nextRowY += rowHeight;
                        nextRowIndex++;

                    }

                }
                void searched()
                {
                    if (searchString == "") return;

                    foreach (var entry in searchedEntries)
                    {
                        RowGUI(windowRect.SetHeight(rowHeight).SetY(nextRowY), entry);

                        nextRowY += rowHeight;
                        nextRowIndex++;

                    }

                }


                scrollPos = GUI.BeginScrollView(windowRect.AddHeightFromBottom(-firstRowOffsetTop), Vector2.up * scrollPos, windowRect.SetHeight(scrollAreaHeight), GUIStyle.none, GUIStyle.none).y;

                nextRowY = 0;
                nextRowIndex = 0;

                bookmarked();
                divider();
                notBookmarked();
                searched();

                scrollAreaHeight = nextRowY;
                rowCount = nextRowIndex;

                GUI.EndScrollView();

            }
            void noResults()
            {
                if (searchString == "") return;
                if (searchedEntries.Any()) return;


                GUI.enabled = false;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                GUI.Label(windowRect.AddHeightFromBottom(-14), "No results");

                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                GUI.enabled = true;

            }

            void outline()
            {
                if (Application.platform == RuntimePlatform.OSXEditor) return;

                position.SetPos(0, 0).DrawOutline(Greyscale(.1f));

            }
            // void resizing()
            // {

            // }


            background();
            closeOnEscape();
            addTabOnEnter();
            arrowNavigation();
            updateSearch();

            searchField_();
            rows();
            noResults();

            outline();


            if (draggingBookmark || animatingDroppedBookmark || animatingGaps)
                this.Repaint();

        }

        Rect windowRect => position.SetPos(0, 0);
        Rect bookmarksRect;

        SearchField searchField;

        Color windowBackground => Greyscale(isDarkTheme ? .23f : .8f);

        string searchString = "";
        string prevSearchString = "";

        float scrollPos;

        float rowHeight => 22;
        float dividerHeight => 11;
        float firstRowOffsetTop => bookmarkedEntries.Any() && searchString == "" ? 21 : 20;

        int nextRowIndex;
        float nextRowY;

        float scrollAreaHeight = 1232;
        int rowCount = 123;

        int keyboardFocusedRowIndex = -1;









        void RowGUI(Rect rowRect, TabEntry entry)
        {

            var isHovered = rowRect.IsHovered();
            var isPressed = entry == pressedEntry;
            var isDragged = draggingBookmark && draggedBookmark == entry;
            var isDropped = animatingDroppedBookmark && droppedBookmark == entry;
            var isFocused = entry == keyboardFocusedEntry;
            var isBookmarked = bookmarkedEntries.Contains(entry) || entry == draggedBookmark;

            var showBlueBackground = isFocused || isPressed || isDragged;

            if (isDropped)
                isHovered = rowRect.SetY(droppedBookmarkYTarget).IsHovered();


            void draggedShadow()
            {
                if (!isDragged) return;

                var shadowRect = rowRect.AddHeightFromMid(-4);

                var shadowOpacity = .3f;
                var shadowRadius = 13;

                shadowRect.DrawBlurred(Greyscale(0, shadowOpacity), shadowRadius);

            }
            void blueBackground()
            {
                if (!curEvent.isRepaint) return;
                if (!showBlueBackground) return;


                var backgroundRect = rowRect.AddHeightFromMid(-3);

                backgroundRect.Draw(GUIColors.selectedBackground);


            }
            void icon()
            {
                if (!curEvent.isRepaint) return;


                Texture iconTexture = EditorIcons.GetIcon(entry.iconName, returnNullIfNotFound: true);

                if (!iconTexture) return;


                var iconRect = rowRect.SetWidth(16).SetHeightFromMid(16).MoveX(4 + 1);

                iconRect = iconRect.SetWidthFromMid(iconRect.height * iconTexture.width / iconTexture.height);


                GUI.DrawTexture(iconRect, iconTexture);

            }
            void name()
            {
                if (!curEvent.isRepaint) return;

                var nameRect = rowRect.MoveX(21 + 1);

                var nameText = searchString != "" ? namesFormattedForFuzzySearch_byEntry[entry] : entry.name;


                var color = showBlueBackground ? Greyscale(123, 123)
                                               : isHovered && !isPressed ? Greyscale(1.1f)
                                                                         : Greyscale(1);
                SetGUIColor(color);

                GUI.skin.label.richText = true;

                GUI.Label(nameRect, nameText);

                GUI.skin.label.richText = false;

                ResetGUIColor();

            }
            void starButton()
            {
                if (!isHovered && !isBookmarked) return;
                if (isFocused && !isHovered) return;


                var buttonRect = rowRect.SetWidthFromRight(16).MoveX(-6 + 1).SetSizeFromMid(rowHeight);


                var iconName = isBookmarked ^ buttonRect.IsHovered() ? "Star" : "Star Hollow";
                var iconSize = 16;
                var colorNormal = Greyscale(isDarkTheme ? (isBookmarked ? .5f : .7f) : .3f);
                var colorHovered = Greyscale(isDarkTheme ? (isBookmarked ? .9f : 1) : 0f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .5f);
                var colorDisabled = Greyscale(isDarkTheme ? .53f : .55f);


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                if (isBookmarked)
                    bookmarkedEntries.Remove(entry);
                else
                    bookmarkedEntries.Add(entry);

            }
            void enterHint()
            {
                if (!curEvent.isRepaint) return;
                if (!isFocused) return;
                if (isHovered) return;
                if (!isDarkTheme) return;


                var hintRect = rowRect.SetWidthFromRight(33);


                SetLabelFontSize(10);
                SetGUIColor(Greyscale(.9f));

                GUI.Label(hintRect, "Enter");

                ResetGUIColor();
                ResetLabelStyle();


            }
            void hoverHighlight()
            {
                if (!isHovered) return;
                if (isPressed || isDragged) return;


                var backgroundRect = rowRect.AddHeightFromMid(-2);

                var backgroundColor = Greyscale(isDarkTheme ? 1 : 0, isPressed ? .085f : .12f);


                backgroundRect.Draw(backgroundColor);

            }

            void mouse()
            {
                void down()
                {
                    if (!curEvent.isMouseDown) return;
                    if (!rowRect.IsHovered()) return;

                    isMousePressedOnEntry = true;
                    pressedEntry = entry;

                    mouseDownPosition = curEvent.mousePosition;

                    this.Repaint();

                }
                void up()
                {
                    if (!curEvent.isMouseUp) return;

                    isMousePressedOnEntry = false;
                    pressedEntry = null;

                    this.Repaint();


                    if (!isHovered) return;
                    if (draggingBookmark) return;
                    if ((curEvent.mousePosition - mouseDownPosition).magnitude > 2) return;

                    curEvent.Use();

                    AddTab(entry);

                }

                down();
                up();

            }
            void setFocusedEntry()
            {
                var rowIndex = (rowRect.y / rowHeight).FloorToInt();

                if (rowIndex == keyboardFocusedRowIndex)
                    keyboardFocusedEntry = entry;

            }


            rowRect.MarkInteractive();

            draggedShadow();
            blueBackground();
            icon();
            name();
            starButton();
            enterHint();
            hoverHighlight();

            mouse();
            setFocusedEntry();

        }

        TabEntry pressedEntry;

        bool isMousePressedOnEntry;

        Vector2 mouseDownPosition;

        TabEntry keyboardFocusedEntry;




        void AddTab(TabEntry entry)
        {
            var windowType = Type.GetType(entry.typeString);

            var window = ScriptableObject.CreateInstance(windowType) as EditorWindow;


            var windowName = entry.name;
            var windowIcon = EditorIcons.GetIcon(entry.iconName, returnNullIfNotFound: true);

            window.titleContent = new GUIContent(windowName, windowIcon);


            dockArea.InvokeMethod("AddTab", window, true);


            window.Focus();

            this.Close();

        }












        public void BookmarksGUI()
        {
            void normalBookmark(int i)
            {
                if (bookmarkedEntries[i] == droppedBookmark && animatingDroppedBookmark) return;

                var bookmarkRect = bookmarksRect.SetHeight(rowHeight)
                                                .SetY(GetBookmarY(i));

                RowGUI(bookmarkRect, bookmarkedEntries[i]);

            }
            void normalBookmarks()
            {
                for (int i = 0; i < bookmarkedEntries.Count; i++)
                    normalBookmark(i);

            }
            void draggedBookmark_()
            {
                if (!draggingBookmark) return;


                var bookmarkRect = bookmarksRect.SetHeight(rowHeight)
                                                .SetY(draggedBookmarkY);

                RowGUI(bookmarkRect, draggedBookmark);

            }
            void droppedBookmark_()
            {
                if (!animatingDroppedBookmark) return;

                var bookmarkRect = bookmarksRect.SetHeight(rowHeight)
                                                .SetY(droppedBookmarkY);

                RowGUI(bookmarkRect, droppedBookmark);

            }


            BookmarksDragging();
            BookmarksAnimations();

            normalBookmarks();
            draggedBookmark_();
            droppedBookmark_();

        }

        int GetBookmarkIndex(float mouseY)
        {
            return ((mouseY - bookmarksRect.y) / rowHeight).FloorToInt();
        }

        float GetBookmarY(int i, bool includeGaps = true)
        {
            var centerY = bookmarksRect.y
                        + i * rowHeight
                        + (includeGaps ? gaps.Take(i + 1).Sum() : 0);


            return centerY;

        }









        void BookmarksDragging()
        {
            void init()
            {
                if (draggingBookmark) return;
                if ((curEvent.mousePosition - mouseDownPosition).magnitude <= 2) return;

                if (!isMousePressedOnEntry) return;
                if (!bookmarkedEntries.Contains(pressedEntry)) return;


                var i = GetBookmarkIndex(mouseDownPosition.y);

                if (i >= bookmarkedEntries.Count) return;
                if (i < 0) return;


                animatingDroppedBookmark = false;

                draggingBookmark = true;

                draggedBookmark = bookmarkedEntries[i];
                draggedBookmarkHoldOffsetY = GetBookmarY(i) - mouseDownPosition.y;

                gaps[i] = rowHeight;


                this.RecordUndo();

                bookmarkedEntries.Remove(draggedBookmark);

            }
            void update()
            {
                if (!draggingBookmark) return;


                EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Passive);

                draggedBookmarkY = (curEvent.mousePosition.y + draggedBookmarkHoldOffsetY).Clamp(0, bookmarksRect.yMax - rowHeight);

                insertDraggedBookmarkAtIndex = GetBookmarkIndex(curEvent.mousePosition.y + draggedBookmarkHoldOffsetY + rowHeight / 2).Clamp(0, bookmarkedEntries.Count);

            }
            void accept()
            {
                if (!draggingBookmark) return;
                if (!curEvent.isMouseUp && !curEvent.isIgnore) return;

                curEvent.Use();
                EditorGUIUtility.hotControl = 0;

                // DragAndDrop.PrepareStartDrag(); // fixes phantom dragged component indicator after reordering bookmarks

                this.RecordUndo();

                draggingBookmark = false;
                isMousePressedOnEntry = false;

                bookmarkedEntries.AddAt(draggedBookmark, insertDraggedBookmarkAtIndex);

                gaps[insertDraggedBookmarkAtIndex] -= rowHeight;
                gaps.AddAt(0, insertDraggedBookmarkAtIndex);

                droppedBookmark = draggedBookmark;

                droppedBookmarkY = draggedBookmarkY;
                droppedBookmarkYDerivative = 0;
                animatingDroppedBookmark = true;

                draggedBookmark = null;
                pressedEntry = null;

                EditorGUIUtility.hotControl = 0;

            }

            init();
            accept();
            update();

        }

        bool draggingBookmark;

        float draggedBookmarkHoldOffsetY;

        float draggedBookmarkY;
        int insertDraggedBookmarkAtIndex;

        TabEntry draggedBookmark;
        TabEntry droppedBookmark;






        void BookmarksAnimations()
        {
            if (!curEvent.isLayout) return;

            void gaps_()
            {
                var makeSpaceForDraggedBookmark = draggingBookmark;

                // var lerpSpeed = 1;
                var lerpSpeed = 11;

                for (int i = 0; i < gaps.Count; i++)
                    if (makeSpaceForDraggedBookmark && i == insertDraggedBookmarkAtIndex)
                        gaps[i] = MathUtil.Lerp(gaps[i], rowHeight, lerpSpeed, editorDeltaTime);
                    else
                        gaps[i] = MathUtil.Lerp(gaps[i], 0, lerpSpeed, editorDeltaTime);



                for (int i = 0; i < gaps.Count; i++)
                    if (gaps[i].Approx(0))
                        gaps[i] = 0;



                animatingGaps = gaps.Any(r => r > .1f);


            }
            void droppedBookmark_()
            {
                if (!animatingDroppedBookmark) return;

                // var lerpSpeed = 1;
                var lerpSpeed = 8;

                droppedBookmarkYTarget = GetBookmarY(bookmarkedEntries.IndexOf(droppedBookmark), includeGaps: false);

                MathUtil.SmoothDamp(ref droppedBookmarkY, droppedBookmarkYTarget, lerpSpeed, ref droppedBookmarkYDerivative, editorDeltaTime);

                if ((droppedBookmarkY - droppedBookmarkYTarget).Abs() < .5f)
                    animatingDroppedBookmark = false;

            }

            gaps_();
            droppedBookmark_();

        }

        float droppedBookmarkY;
        float droppedBookmarkYTarget;
        float droppedBookmarkYDerivative;

        bool animatingDroppedBookmark;
        bool animatingGaps;

        List<float> gaps
        {
            get
            {
                while (_gaps.Count < bookmarkedEntries.Count + 1) _gaps.Add(0);
                while (_gaps.Count > bookmarkedEntries.Count + 1) _gaps.RemoveLast();

                return _gaps;

            }
        }
        List<float> _gaps = new();




















        public static void UpdateAllEntries()
        {
            void fillWithDefaults()
            {

                allEntries.Clear();


                foreach (var type in TypeCache.GetTypesWithAttribute<EditorWindowTitleAttribute>())
                {
                    var titleAttribute = type.GetCustomAttribute<EditorWindowTitleAttribute>();

                    var entry = new TabEntry();

                    entry.typeString = type.AssemblyQualifiedName;
                    entry.name = titleAttribute.title ?? "";
                    entry.iconName = titleAttribute.useTypeNameAsIconName ? type.FullName : titleAttribute.icon ?? "";


                    if (entry.iconName.IsNullOrEmpty()) continue; // filters out internal windows and such

                    allEntries.Add(entry);

                }


                allEntries.Add(new TabEntry() { name = "Preferences", iconName = "d_Settings@2x", typeString = "UnityEditor.PreferenceSettingsWindow, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });
                allEntries.Add(new TabEntry() { name = "Project Settings", iconName = "d_Settings@2x", typeString = "UnityEditor.ProjectSettingsWindow, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });

                allEntries.Add(new TabEntry() { name = "Background Tasks", iconName = "", typeString = "UnityEditor.ProgressWindow, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });

                allEntries.Add(new TabEntry() { name = "Frame Debugger", iconName = "", typeString = "UnityEditor.FrameDebuggerWindow, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });
                allEntries.Add(new TabEntry() { name = "Physics Debug", iconName = "", typeString = "UnityEditor.PhysicsDebugWindow, UnityEditor.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });
                allEntries.Add(new TabEntry() { name = "UI Toolkit Debugger", iconName = "", typeString = "UnityEditor.UIElements.Debugger.UIElementsDebugger, UnityEditor.UIElementsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });
                allEntries.Add(new TabEntry() { name = "UI Builder", iconName = "d_UIBuilder@2x", typeString = "Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });

                allEntries.Add(new TabEntry() { name = "Test Runnder", iconName = "", typeString = "UnityEditor.TestTools.TestRunner.TestRunnerWindow, UnityEditor.TestRunner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" });
                allEntries.Add(new TabEntry() { name = "Search", iconName = "d_SearchWindow@2x", typeString = "UnityEditor.Search.SearchWindow, UnityEditor.QuickSearchModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" });

                allEntries.Add(new TabEntry() { name = "Build Settings", iconName = "", typeString = "UnityEditor.BuildPlayerWindow, UnityEditor.CoreModule, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null" });
                allEntries.Add(new TabEntry() { name = "Build Profiles", iconName = "", typeString = "UnityEditor.Build.Profile.BuildProfileWindow, UnityEditor.BuildProfileModule, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null" });

                allEntries.Add(new TabEntry() { name = "Shortcuts", iconName = "", typeString = "UnityEditor.ShortcutManagement.ShortcutManagerWindow, UnityEditor.CoreModule, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null" });



                allEntries.RemoveAll(r => allEntries.Count(rr => rr.name == r.name) > 1);



                var order = new string[]
                {
                    "Scene",
                    "Game",
                    "Project",
                    "Console",
                    "Inspector",
                    "Hierarchy",
                    "Package Manager",
                    "Project Settings",

                    "Animation",
                    "Animator",

                    "Profiler",

                    "Lighting",
                    "Light Explorer",
                    // "Viewer",
                    "Occlusion",

                    "UI Toolkit Debugger",
                    "UI Builder",

                    "Frame Debugger",
                    "Physics Debug",

                    "Preferences",
                    "Simulator",

                    "Build Settings",
                    "Build Profiles",



                }.ToList();


                allEntries.SortBy(r => order.IndexOf(r.name) is int i && i != -1 ? i : 1232);

            }
            void rememberAllOpenTabs()
            {
                foreach (var window in VTabs.allEditorWindows)
                    RememberWindow(window);

            }
            void removeBlacklisted()
            {
                allEntries.RemoveAll(r => r.name == "Asset Store");
                allEntries.RemoveAll(r => r.name == "UI Toolkit Samples");
            }
            void removeUnresolvableTypes()
            {
                allEntries.RemoveAll(r => Type.GetType(r.typeString) == null);
            }


            if (allEntries.Count < 15 || allEntries.Any(r => r == null || r.typeString.IsNullOrEmpty()))
                fillWithDefaults();

            rememberAllOpenTabs();
            removeBlacklisted();
            removeUnresolvableTypes();

        }

        public static void RememberWindow(EditorWindow window)
        {
            if (!window.docked) return;
            if (window.GetType() == t_PropertyEditor) return;
            if (window.GetType() == t_InspectorWindow) return;
            if (window.GetType() == t_ProjectBrowser) return;


            var typeString = window.GetType().AssemblyQualifiedName;

            if (allEntries.Any(r => r.typeString == typeString)) return;


            var name = window.titleContent.text;

            var iconName = window.titleContent.image ? window.titleContent.image.name : "";


            allEntries.Add(new TabEntry { typeString = typeString, name = name, iconName = iconName });

        }

        static List<TabEntry> allEntries => VTabsCache.instance.allTabEntries;



        void GetBookmakedEntries()
        {
            var bookmarkedTabTypeStrings = EditorPrefs.GetString("vTabs-bookmarked-tab-types").Split("---");

            bookmarkedEntries = bookmarkedTabTypeStrings.Where(r => Type.GetType(r) != null)
                                                        .Select(bookmarkedTypeString => allEntries.FirstOrDefault(r => r.typeString == bookmarkedTypeString))
                                                        .Where(r => r != null)
                                                        .ToList();

        }
        void SaveBookmarkedEntries()
        {
            var bookmarkedTabTypeStrings = bookmarkedEntries.Select(r => r.typeString);

            EditorPrefs.SetString("vTabs-bookmarked-tab-types", string.Join("---", bookmarkedTabTypeStrings));

        }

        List<TabEntry> bookmarkedEntries = new();



        void OnEnable() { UpdateAllEntries(); GetBookmakedEntries(); }

        void OnDisable() { SaveBookmarkedEntries(); VTabsCache.Save(); }










        void UpdateSearch()
        {

            bool tryMatch(string name, string query, int[] matchIndexes, ref float cost)
            {

                var wordInitialsIndexes = new List<int> { 0 };

                for (int i = 1; i < name.Length; i++)
                {
                    var separators = new[] { ' ', '-', '_', '.', '(', ')', '[', ']', };

                    var prevChar = name[i - 1];
                    var curChar = name[i];
                    var nextChar = i + 1 < name.Length ? name[i + 1] : default(char);

                    var isSeparatedWordStart = separators.Contains(prevChar) && !separators.Contains(curChar);
                    var isCamelcaseHump = (curChar.IsUpper() && prevChar.IsLower()) || (curChar.IsUpper() && nextChar.IsLower());
                    var isNumberStart = curChar.IsDigit() && (!prevChar.IsDigit() || prevChar == '0');
                    var isAfterNumber = prevChar.IsDigit() && !curChar.IsDigit();

                    if (isSeparatedWordStart || isCamelcaseHump || isNumberStart || isAfterNumber)
                        wordInitialsIndexes.Add(i);

                }



                var nextWordInitialsIndexMap = new int[name.Length];

                var nextWordIndex = 0;

                for (int i = 0; i < name.Length; i++)
                {
                    if (i == wordInitialsIndexes[nextWordIndex])
                        if (nextWordIndex + 1 < wordInitialsIndexes.Count)
                            nextWordIndex++;
                        else break;

                    nextWordInitialsIndexMap[i] = wordInitialsIndexes[nextWordIndex];

                }





                var iName = 0;
                var iQuery = 0;

                var prevMatchIndex = -1;

                void registerMatch(int matchIndex)
                {
                    matchIndexes[iQuery] = matchIndex;
                    iQuery++;

                    iName = matchIndex + 1;

                    prevMatchIndex = matchIndex;


                }


                cost = 0;

                while (iName < name.Length && iQuery < query.Length)
                {
                    var curQuerySymbol = query[iQuery].ToLower();
                    var curNameSymbol = name[iName].ToLower();

                    if (curNameSymbol == curQuerySymbol)
                    {
                        var gapLength = iName - prevMatchIndex - 1;

                        cost += gapLength;


                        registerMatch(iName);

                        continue;

                        // consecutive matches cost 0
                        // distance between index 0 and first match also counts as a gap

                    }



                    var nextWordInitialIndex = nextWordInitialsIndexMap[iName]; // wordInitialsIndexes.FirstOrDefault(i => i > iName);
                    var nextWordInitialSymbol = nextWordInitialIndex == default ? default : name[nextWordInitialIndex].ToLower();

                    if (nextWordInitialSymbol == curQuerySymbol)
                    {
                        var gapLength = nextWordInitialIndex - prevMatchIndex - 1;

                        cost += (gapLength * .01f).ClampMax(.9f);


                        registerMatch(nextWordInitialIndex);

                        continue;

                        // word-initial match costs less than a gap (1+) 
                        // but more than a consecutive match (0)

                    }



                    iName++;

                }






                var allCharsMatched = iQuery >= query.Length;

                return allCharsMatched;



                // this search works great in practice
                // but fails in more theoretical scenarios, mostly when user skips first letters of words
                // eg searching "arn" won't find "barn_a" because search will jump to last a (word-initial) and fail afterwards
                // so unity search is used as a fallback

            }
            bool tryMatch_unitySearch(string name, string query, int[] matchIndexes, ref float cost)
            {
                long score = 0;

                List<int> matchIndexesList = new();


                var matched = UnityEditor.Search.FuzzySearch.FuzzyMatch(searchString, name, ref score, matchIndexesList);


                for (int i = 0; i < matchIndexesList.Count; i++)
                    matchIndexes[i] = matchIndexesList[i];

                cost = 123212 - score;


                return matched;


                // this search is fast but isn't tuned for real use cases
                // quering "vis" ranks "Invisible" higher than "VInspectorState"
                // quering "lst" ranks "SmallShadowTemp" higher than "List"
                // also sometimes it favors matches that are further away from zeroth index 

            }

            string formatName(string name, IEnumerable<int> matchIndexes)
            {
                var formattedName = "";

                for (int i = 0; i < name.Length; i++)
                    if (matchIndexes.Contains(i))
                        formattedName += "<b>" + name[i] + "</b>";
                    else
                        formattedName += name[i];


                return formattedName;

            }



            var costs_byEntry = new Dictionary<TabEntry, float>();

            var matchIndexes = new int[searchString.Length];
            var matchCost = 0f;


            foreach (var entry in allEntries)
                if (tryMatch(entry.name, searchString, matchIndexes, ref matchCost) || tryMatch_unitySearch(entry.name, searchString, matchIndexes, ref matchCost))
                {
                    costs_byEntry[entry] = matchCost;
                    namesFormattedForFuzzySearch_byEntry[entry] = formatName(entry.name, matchIndexes);
                }


            searchedEntries = costs_byEntry.Keys.OrderBy(r => costs_byEntry[r])
                                                .ThenBy(r => r.name)
                                                .ToList();
        }

        List<TabEntry> searchedEntries = new();

        Dictionary<TabEntry, string> namesFormattedForFuzzySearch_byEntry = new();







        void OnLostFocus()
        {
            EditorApplication.delayCall += () =>
            {
                if (EditorWindow.focusedWindow != this)
                {
                    dockArea.GetMemberValue<EditorWindow>("actualView").Repaint(); // for + button to fade

                    Close();
                }
            };

            // delay is needed to prevent reopening after clicking + button for the second time
        }



        public static void Open(Object dockArea)
        {
            instance = ScriptableObject.CreateInstance<VTabsAddTabWindow>();

            instance.ShowPopup();
            instance.Focus();



            var gui = VTabs.guis_byDockArea[dockArea];

            var windowRect = dockArea.GetMemberValue("actualView").GetMemberValue<Rect>("position");

            var lastTabEndPosition = windowRect.position + Vector2.right * gui.tabEndPositions.Last().ClampMax(windowRect.width - 30);


            var width = 190;
            // var height = 288;
            var height = 296;
            // var height = 276;

            var offsetX = -26;
            var offsetY = 24;
            // var offsetY = 80;


            instance.position = instance.position.SetPos(lastTabEndPosition + new Vector2(offsetX, offsetY)).SetSize(width, height);



            instance.dockArea = dockArea;

            UpdateAllEntries();

        }

        public Object dockArea;

        public static VTabsAddTabWindow instance;

    }
}
#endif