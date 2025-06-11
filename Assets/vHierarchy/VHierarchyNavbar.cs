#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;
using Type = System.Type;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;
// using static VTools.VDebug;
using static VHierarchy.VHierarchyData;
using static VHierarchy.VHierarchy;



namespace VHierarchy
{
    public class VHierarchyNavbar
    {

        public void OnGUI(Rect navbarRect)
        {
            void updateState()
            {
                if (!curEvent.isLayout) return;



                var isTreeFocused = window.GetFieldValue("m_SceneHierarchy").GetMemberValue<int>("m_TreeViewKeyboardControlID") == GUIUtility.keyboardControl;

                var isWindowFocused = window == EditorWindow.focusedWindow;



                if (!isTreeFocused && isSearchActive)
                    EditorGUI.FocusTextInControl("SearchFilter");


                if (isTreeFocused || !isWindowFocused)
                    if (window.GetMemberValue("m_SearchFilter").ToString().IsNullOrEmpty())
                        isSearchActive = false;


                // in vFolders the following is used to check if search is active:
                // GUI.GetNameOfFocusedControl() == "SearchFilter";
                // but in hierarchy focused control changes erratically when multiple scene headers are visible
                // so a bool state is used instead




                this.defaultParent = typeof(SceneView).InvokeMethod<Transform>("GetDefaultParentObjectIfSet")?.gameObject;

            }

            void background()
            {
                var backgroundColor = Greyscale(isDarkTheme ? .235f : .8f);
                var lineColor = Greyscale(isDarkTheme ? .13f : .58f);

                navbarRect.Draw(backgroundColor);

                navbarRect.SetHeightFromBottom(1).MoveY(1).Draw(lineColor);

            }
            void hiddenMenu()
            {
                if (!curEvent.holdingAlt) return;
                if (!curEvent.isMouseUp) return;
                if (curEvent.mouseButton != 1) return;
                if (!navbarRect.IsHovered()) return;


                void selectData()
                {
                    Selection.activeObject = data;
                }
                void selectPalette()
                {
                    Selection.activeObject = palette;
                }
                void clearCache()
                {
                    VHierarchyCache.Clear();
                }



                GenericMenu menu = new();

                menu.AddDisabledItem(new GUIContent("vHierarchy hidden menu"));

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Select data"), false, selectData);
                menu.AddItem(new GUIContent("Select palette"), false, selectPalette);

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Clear cache"), false, clearCache);

                menu.ShowAsContext();

            }


            void plusButton()
            {

                var buttonRect = navbarRect.SetWidth(28).MoveX(4.5f);

                if (Application.unityVersion.StartsWith("6000"))
                    buttonRect = buttonRect.MoveY(-.49f);


                var iconName = "Plus Thicker";
                var iconSize = 16;
                var colorNormal = Greyscale(isDarkTheme ? .7f : .44f);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .42f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .6f);

                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;


                GUIUtility.hotControl = 0;

                var sceneHierarchy = window.GetMemberValue("m_SceneHierarchy");
                var m_CustomParentForNewGameObjects = window.GetMemberValue("m_SceneHierarchy").GetMemberValue<Transform>("m_CustomParentForNewGameObjects");
                var targetSceneHandle = m_CustomParentForNewGameObjects != null ? m_CustomParentForNewGameObjects.gameObject.scene.handle : 0;


                var menu = new GenericMenu();

                sceneHierarchy.GetType().GetMethod("AddCreateGameObjectItemsToMenu", maxBindingFlags).Invoke(sceneHierarchy, new object[] { menu, null, true, true, false, targetSceneHandle, 3 });

                typeof(UnityEditor.SceneManagement.SceneHierarchyHooks).InvokeMethod("AddCustomItemsToCreateMenu", menu);

                menu.DropDown(buttonRect);


            }

            void searchButton()
            {
                if (searchAnimationT == 1) return;


                var buttonRect = navbarRect.SetWidthFromRight(28).MoveX(-5);

                var iconName = "Search_";
                var iconSize = 16;
                var colorNormal = Greyscale(isDarkTheme ? .75f : .2f);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .2f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .5f);


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                EditorGUI.FocusTextInControl("SearchFilter");

                EditorApplication.delayCall += () => EditorGUI.FocusTextInControl("SearchFilter");

                isSearchActive = true;

            }
            void searchOnCtrlF()
            {
                if (searchAnimationT == 1) return;

                if (!curEvent.isKeyDown) return;
                if (!curEvent.holdingCmd && !curEvent.holdingCtrl) return;
                if (curEvent.keyCode != KeyCode.F) return;


                EditorGUI.FocusTextInControl("SearchFilter");

                EditorApplication.delayCall += () => EditorGUI.FocusTextInControl("SearchFilter");

                isSearchActive = true;


                curEvent.Use();

            }
            void collapseAllButton()
            {
                if (searchAnimationT == 1) return;


                var buttonRect = navbarRect.SetWidthFromRight(28).MoveX(-33);

                var iconName = "Collapse";
                var iconSize = 16;
                var colorNormal = Greyscale(isDarkTheme ? .71f : .44f);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .42f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .6f);


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                controller.CollapseAll();

            }
            void bookmarks()
            {
                if (searchAnimationT == 1) return;
                if (isSearchActive && !curEvent.isRepaint) return;

                void createData()
                {
                    if (data) return;
                    if (!navbarRect.IsHovered()) return;
                    if (!DragAndDrop.objectReferences.Any()) return;

                    data = ScriptableObject.CreateInstance<VHierarchyData>();

                    AssetDatabase.CreateAsset(data, GetScriptPath("VHierarchy").GetParentPath().CombinePath("vHierarchy Data.asset"));

                }
                void divider()
                {
                    if (!data) return;
                    if (!data.bookmarks.Any(r => r.go)) return;


                    var dividerRect = navbarRect.SetWidthFromRight(1).SetHeightFromMid(16).MoveX(-65).MoveX(1.5f);

                    var dividerColor = Greyscale(isDarkTheme ? .33f : .64f);


                    dividerRect.Draw(dividerColor);

                }
                void gui()
                {
                    if (!data) return;

                    this.navbarRect = navbarRect;
                    this.bookmarksRect = navbarRect.AddWidth(-69).AddWidthFromRight(-60).MoveX(2).MoveX(-3);

                    BookmarksGUI();

                }

                createData();
                divider();
                gui();

            }

            void searchField()
            {
                if (searchAnimationT == 0) return;

                var searchFieldRect = navbarRect.SetHeightFromMid(20).AddWidth(-33).SetWidthFromRight(200f.Min(window.position.width - 120)).Move(-1, 2);


                GUILayout.BeginArea(searchFieldRect);
                GUILayout.BeginHorizontal();

                Space(2);
                window.InvokeMethod("SearchFieldGUI");

                GUILayout.EndHorizontal();
                GUILayout.EndArea();

            }
            void closeSearchButton()
            {
                if (searchAnimationT == 0) return;


                var buttonRect = navbarRect.SetWidthFromRight(30).MoveX(-4);

                var iconName = "CrossIcon";
                var iconSize = 15;
                var colorNormal = Greyscale(isDarkTheme ? .9f : .2f);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .2f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .5f);


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                window.InvokeMethod("ClearSearchFilter");

                GUIUtility.keyboardControl = 0;

                isSearchActive = false;

            }
            void closeSearchOnEsc()
            {
                if (!isSearchActive) return;
                if (curEvent.keyCode != KeyCode.Escape) return;

                window.InvokeMethod("ClearSearchFilter");

                GUIUtility.keyboardControl = 0;

                isSearchActive = false;

            }

            void searchAnimation()
            {
                if (!curEvent.isLayout) return;


                var lerpSpeed = 8f;

                if (isSearchActive)
                    MathUtil.SmoothDamp(ref searchAnimationT, 1, lerpSpeed, ref searchAnimationDerivative, editorDeltaTime);
                else
                    MathUtil.SmoothDamp(ref searchAnimationT, 0, lerpSpeed, ref searchAnimationDerivative, editorDeltaTime);


                if (isSearchActive && searchAnimationT > .99f)
                    searchAnimationT = 1;

                if (!isSearchActive && searchAnimationT < .01f)
                    searchAnimationT = 0;


                animatingSearch = searchAnimationT != 0 && searchAnimationT != 1;

            }

            void buttonsAndBookmarks()
            {
                SetGUIColor(Greyscale(1, (1 - searchAnimationT).Pow(2)));
                GUI.BeginGroup(window.position.SetPos(0, 0).MoveX(-searchAnimationDistance * searchAnimationT));

                searchButton();
                searchOnCtrlF();
                collapseAllButton();
                bookmarks();

                GUI.EndGroup();
                ResetGUIColor();

            }
            void search()
            {
                SetGUIColor(Greyscale(1, searchAnimationT.Pow(2)));
                GUI.BeginGroup(window.position.SetPos(0, 0).MoveX(searchAnimationDistance * (1 - searchAnimationT)));

                searchField();
                closeSearchButton();
                closeSearchOnEsc();

                GUI.EndGroup();
                ResetGUIColor();

            }



            updateState();

            background();
            hiddenMenu();

            plusButton();

            searchAnimation();
            buttonsAndBookmarks();
            search();



            if (draggingBookmark || animatingDroppedBookmark || animatingGaps || animatingTooltip || animatingSearch)
                window.Repaint();

        }

        bool animatingSearch;

        float searchAnimationDistance = 90;
        float searchAnimationT;
        float searchAnimationDerivative;

        string openedFolderPath;

        public bool isSearchActive;

        bool isDefaultParentTextPressed;

        GameObject defaultParent;

        GUIStyle defaultParentTextGUIStyle;

        Rect navbarRect;
        Rect bookmarksRect;












        void BookmarksGUI()
        {
            void bookmark(Vector2 centerPosition, Bookmark bookmark)
            {
                if (bookmark == null) return;
                if (curEvent.isLayout) return;


                var bookmarkRect = Rect.zero.SetSize(bookmarkWidth, bookmarksRect.height).SetMidPos(centerPosition);


                void shadow()
                {
                    if (!draggingBookmark) return;
                    if (draggedBookmark != bookmark) return;

                    bookmarkRect.SetSizeFromMid(bookmarkWidth - 4, bookmarkWidth - 4).DrawBlurred(Greyscale(0, .3f), 15);

                }
                void background()
                {
                    if (!bookmarkRect.IsHovered()) return;
                    if (draggingBookmark && draggedBookmark != bookmark) return;

                    var backgroundColor = Greyscale(isDarkTheme ? .35f : .7f);

                    var backgroundRect = bookmarkRect.SetSizeFromMid(bookmarkRect.width - 2, bookmarkWidth - 2);

                    backgroundRect.DrawRounded(backgroundColor, 4);


                }
                void icon()
                {
                    var opacity = 1f;
                    var iconTexture = default(Texture);

                    void set_opacity()
                    {
                        var opacityNormal = .9f;
                        var opacityHovered = 1f;
                        var opacityPressed = .75f;
                        var opacityDragged = .75f;
                        var opacityDisabled = .4f;

                        var isDisabled = !bookmark.isLoadable;


                        opacity = opacityNormal;

                        if (draggingBookmark)
                            opacity = bookmark == draggedBookmark ? opacityDragged : opacityNormal;

                        else if (bookmark == pressedBookmark)
                            opacity = opacityPressed;

                        else if (bookmarkRect.IsHovered())
                            opacity = opacityHovered;

                        if (isDisabled)
                            opacity = opacityDisabled;

                    }
                    void getTexture()
                    {
                        var iconName = "";

                        if (bookmark.go)
                            if (VHierarchy.GetGameObjectData(bookmark.go, createDataIfDoesntExist: false) is GameObjectData goData && !goData.iconNameOrGuid.IsNullOrEmpty())
                                iconName = goData.iconNameOrGuid.Length == 32 ? goData.iconNameOrGuid.ToPath() : goData.iconNameOrGuid;
                            else
                                iconName = AssetPreview.GetMiniThumbnail(bookmark.go).name;

                        if (iconName.IsNullOrEmpty())
                            iconName = "GameObject icon";


                        iconTexture = EditorIcons.GetIcon(iconName);

                    }
                    void drawTexture()
                    {
                        if (!iconTexture) return;


                        SetGUIColor(Greyscale(1, opacity));

                        GUI.DrawTexture(bookmarkRect.SetSizeFromMid(iconSize), iconTexture);

                        ResetGUIColor();

                    }


                    set_opacity();
                    getTexture();
                    drawTexture();

                }
                void tooltip()
                {
                    if (bookmark != (draggingBookmark ? (draggedBookmark) : (lastHoveredBookmark))) return;
                    if (tooltipOpacity == 0) return;

                    var fontSize = 11;
                    var tooltipText = bookmark.name;

                    Rect tooltipRect;

                    void set_tooltipRect()
                    {
                        var width = tooltipText.GetLabelWidth(fontSize) + 6;
                        var height = 16 + (fontSize - 12) * 2;

                        var yOffset = 28;
                        var rightMargin = -1;


                        tooltipRect = Rect.zero.SetMidPos(centerPosition.x, centerPosition.y + yOffset).SetSizeFromMid(width, height);


                        var maxXMax = navbarRect.xMax - rightMargin;

                        if (tooltipRect.xMax > maxXMax)
                            tooltipRect = tooltipRect.MoveX(maxXMax - tooltipRect.xMax);

                    }
                    void shadow()
                    {
                        var shadowAmount = .33f;
                        var shadowRadius = 10;

                        tooltipRect.DrawBlurred(Greyscale(0, shadowAmount).MultiplyAlpha(tooltipOpacity), shadowRadius);

                    }
                    void background()
                    {
                        var cornerRadius = 5;

                        var backgroundColor = Greyscale(isDarkTheme ? .13f : .9f);
                        var outerEdgeColor = Greyscale(isDarkTheme ? .25f : .6f);
                        var innerEdgeColor = Greyscale(isDarkTheme ? .0f : .95f);

                        tooltipRect.Resize(-1).DrawRounded(outerEdgeColor.SetAlpha(tooltipOpacity.Pow(2)), cornerRadius + 1);
                        tooltipRect.Resize(0).DrawRounded(innerEdgeColor.SetAlpha(tooltipOpacity.Pow(2)), cornerRadius + 0);
                        tooltipRect.Resize(1).DrawRounded(backgroundColor.SetAlpha(tooltipOpacity), cornerRadius - 1);

                    }
                    void text()
                    {
                        var textRect = tooltipRect.MoveY(-.5f);

                        var textColor = Greyscale(1f);

                        SetLabelAlignmentCenter();
                        SetLabelFontSize(fontSize);
                        SetGUIColor(textColor.SetAlpha(tooltipOpacity));

                        GUI.Label(textRect, tooltipText);

                        ResetLabelStyle();
                        ResetGUIColor();

                    }

                    set_tooltipRect();
                    shadow();
                    background();
                    text();

                }
                void click()
                {
                    if (!bookmarkRect.IsHovered()) return;
                    if (!curEvent.isMouseUp) return;

                    curEvent.Use();


                    if (draggingBookmark) return;
                    if ((curEvent.mousePosition - mouseDownPosiion).magnitude > 2) return;
                    if (!bookmark.isLoadable) return;

                    controller.RevealObject(bookmark.go, expand: true, highlight: true, snapToTopMargin: true);

                    lastClickedBookmark = bookmark;

                    hideTooltip = true;



                    if (curEvent.mouseButton == 2 && VHierarchyMenu.setDefaultParentEnabled)
                        EditorUtility.SetDefaultParentObject(bookmark.go);

                }


                bookmarkRect.MarkInteractive();

                shadow();
                background();
                icon();
                tooltip();
                click();

            }

            void normalBookmark(int i, float centerX)
            {
                if (data.bookmarks[i] == droppedBookmark && animatingDroppedBookmark) return;

                var centerY = bookmarksRect.height / 2;


                var minX = centerX - bookmarkWidth / 2;

                if (minX < bookmarksRect.x) return;

                lastBookmarkX = minX;


                bookmark(new Vector2(centerX, centerY), data.bookmarks[i]);

            }
            void normalBookmarks()
            {
                var curCenterX = bookmarksRect.xMax - bookmarkWidth / 2;

                for (int i = 0; i < data.bookmarks.Count; i++)
                {
                    curCenterX -= gaps[i];

                    if (!data.bookmarks[i].go) continue;


                    normalBookmark(i, curCenterX);


                    curCenterX -= bookmarkWidth;

                }

            }
            void draggedBookmark_()
            {
                if (!draggingBookmark) return;

                var centerX = curEvent.mousePosition.x + draggedBookmarkHoldOffset.x;
                var centerY = bookmarksRect.IsHovered() ? bookmarksRect.height / 2 : curEvent.mousePosition.y;

                bookmark(new Vector2(centerX, centerY), draggedBookmark);

            }
            void droppedBookmark_()
            {
                if (!animatingDroppedBookmark) return;

                var centerX = droppedBookmarkX;
                var centerY = bookmarksRect.height / 2;

                bookmark(new Vector2(centerX, centerY), droppedBookmark);

            }


            BookmarksMouseState();
            BookmarksDragging();
            BookmarksAnimations();

            normalBookmarks();
            draggedBookmark_();
            droppedBookmark_();

        }

        float bookmarkWidth => 24;
        float iconSize => 16;

        float lastBookmarkX;



        int GetBookmarkIndex(float mouseX)
        {
            var curBookmarkWidthSum = 0f;

            for (int i = 0; i < data.bookmarks.Count; i++)
            {
                if (!data.bookmarks[i].go) continue;

                curBookmarkWidthSum += bookmarkWidth;

                if (bookmarksRect.xMax - curBookmarkWidthSum < mouseX + .5f)
                    return i;
            }

            return data.bookmarks.IndexOfLast(r => r.go) + 1;

        }

        float GetBookmarkCenterX(int i, bool includeGaps = true)
        {
            return bookmarksRect.xMax
                 - bookmarkWidth / 2
                 - data.bookmarks.Take(i).Sum(r => r.go ? bookmarkWidth : 0)
                 - (includeGaps ? gaps.Take(i + 1).Sum() : 0);
        }







        void BookmarksMouseState()
        {
            void down()
            {
                if (!curEvent.isMouseDown) return;
                if (!bookmarksRect.IsHovered()) return;

                mousePressed = true;

                mouseDownPosiion = curEvent.mousePosition;

                var pressedBookmarkIndex = GetBookmarkIndex(mouseDownPosiion.x);

                if (pressedBookmarkIndex.IsInRangeOf(data.bookmarks))
                    pressedBookmark = data.bookmarks[pressedBookmarkIndex];

                doubleclickUnhandled = curEvent.clickCount == 2;

                curEvent.Use();

            }
            void up()
            {
                if (!curEvent.isMouseUp) return;

                mousePressed = false;
                pressedBookmark = null;

            }
            void hover()
            {
                var hoveredBookmarkIndex = GetBookmarkIndex(curEvent.mousePosition.x);

                mouseHoversBookmark = bookmarksRect.IsHovered() && hoveredBookmarkIndex.IsInRangeOf(data.bookmarks);

                if (mouseHoversBookmark)
                    lastHoveredBookmark = data.bookmarks[hoveredBookmarkIndex];


            }

            down();
            up();
            hover();

        }

        bool mouseHoversBookmark;
        bool mousePressed;
        bool doubleclickUnhandled;

        Vector2 mouseDownPosiion;

        Bookmark pressedBookmark;
        Bookmark lastHoveredBookmark;






        void BookmarksDragging()
        {
            void initFromOutside()
            {
                if (draggingBookmark) return;
                if (!bookmarksRect.IsHovered()) return;
                if (!curEvent.isDragUpdate) return;
                if (DragAndDrop.objectReferences.FirstOrDefault() is not GameObject draggedGo) return;

                animatingDroppedBookmark = false;

                draggingBookmark = true;
                draggingBookmarkFromInside = false;

                draggedBookmark = new Bookmark(draggedGo);
                draggedBookmarkHoldOffset = Vector2.zero;

            }
            void initFromInside()
            {
                if (draggingBookmark) return;
                if (!mousePressed) return;
                if ((curEvent.mousePosition - mouseDownPosiion).magnitude <= 2) return;
                if (pressedBookmark == null) return;

                var i = GetBookmarkIndex(mouseDownPosiion.x);

                if (i >= data.bookmarks.Count) return;
                if (i < 0) return;


                animatingDroppedBookmark = false;

                draggingBookmark = true;
                draggingBookmarkFromInside = true;

                draggedBookmark = data.bookmarks[i];
                draggedBookmarkHoldOffset = new Vector2(GetBookmarkCenterX(i) - mouseDownPosiion.x, bookmarksRect.center.y - mouseDownPosiion.y);

                gaps[i] = bookmarkWidth;


                data.RecordUndo();

                data.bookmarks.Remove(draggedBookmark);

            }

            void acceptFromOutside()
            {
                if (!draggingBookmark) return;
                if (!curEvent.isDragPerform) return;
                if (!bookmarksRect.IsHovered()) return;

                DragAndDrop.AcceptDrag();
                curEvent.Use();

                data.RecordUndo();

                accept();

                data.Dirty();

            }
            void acceptFromInside()
            {
                if (!draggingBookmark) return;
                if (!curEvent.isMouseUp) return;
                if (!bookmarksRect.IsHovered()) return;

                curEvent.Use();
                EditorGUIUtility.hotControl = 0;

                DragAndDrop.PrepareStartDrag(); // fixes phantom dragged component indicator after reordering bookmarks

                data.RecordUndo();
                data.Dirty();

                accept();

            }
            void accept()
            {
                draggingBookmark = false;
                draggingBookmarkFromInside = false;
                mousePressed = false;

                data.bookmarks.AddAt(draggedBookmark, insertDraggedBookmarkAtIndex);

                gaps[insertDraggedBookmarkAtIndex] -= bookmarkWidth;
                gaps.AddAt(0, insertDraggedBookmarkAtIndex);

                droppedBookmark = draggedBookmark;

                droppedBookmarkX = curEvent.mousePosition.x + draggedBookmarkHoldOffset.x;
                droppedBookmarkXDerivative = 0;
                animatingDroppedBookmark = true;

                draggedBookmark = null;

                EditorGUIUtility.hotControl = 0;

            }

            void cancelFromOutside()
            {
                if (!draggingBookmark) return;
                if (draggingBookmarkFromInside) return;
                if (bookmarksRect.IsHovered()) return;

                draggingBookmark = false;
                mousePressed = false;

            }
            void cancelFromInsideAndDelete()
            {
                if (!draggingBookmark) return;
                if (!curEvent.isMouseUp) return;
                if (bookmarksRect.IsHovered()) return;

                draggingBookmark = false;

                DragAndDrop.PrepareStartDrag(); // fixes phantom dragged component indicator after reordering bookmarks

                data.Dirty();

            }

            void update()
            {
                if (!draggingBookmark) return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                if (draggingBookmarkFromInside) // otherwise it breaks vTabs dragndrop
                    EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Passive);



                insertDraggedBookmarkAtIndex = GetBookmarkIndex(curEvent.mousePosition.x + draggedBookmarkHoldOffset.x);

            }


            initFromOutside();
            initFromInside();

            acceptFromOutside();
            acceptFromInside();

            cancelFromOutside();
            cancelFromInsideAndDelete();

            update();


        }

        bool draggingBookmark;
        bool draggingBookmarkFromInside;

        int insertDraggedBookmarkAtIndex;

        Vector2 draggedBookmarkHoldOffset;

        Bookmark draggedBookmark;
        Bookmark droppedBookmark;






        void BookmarksAnimations()
        {
            if (!curEvent.isLayout) return;

            void gaps_()
            {
                var makeSpaceForDraggedBookmark = draggingBookmark && bookmarksRect.IsHovered();

                var lerpSpeed = 12;

                for (int i = 0; i < gaps.Count; i++)
                    if (makeSpaceForDraggedBookmark && i == insertDraggedBookmarkAtIndex)
                        gaps[i] = MathUtil.Lerp(gaps[i], bookmarkWidth, lerpSpeed, editorDeltaTime);
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

                var lerpSpeed = 8;

                var targX = GetBookmarkCenterX(data.bookmarks.IndexOf(droppedBookmark), includeGaps: false);

                MathUtil.SmoothDamp(ref droppedBookmarkX, targX, lerpSpeed, ref droppedBookmarkXDerivative, editorDeltaTime);

                if ((droppedBookmarkX - targX).Abs() < .5f)
                    animatingDroppedBookmark = false;

            }
            void tooltip()
            {
                if (!mouseHoversBookmark || lastHoveredBookmark != lastClickedBookmark)
                    hideTooltip = false;


                var lerpSpeed = UnityEditorInternal.InternalEditorUtility.isApplicationActive ? 15 : 12321;

                if (mouseHoversBookmark && !draggingBookmark && !hideTooltip)
                    MathUtil.SmoothDamp(ref tooltipOpacity, 1, lerpSpeed, ref tooltipOpacityDerivative, editorDeltaTime);
                else
                    MathUtil.SmoothDamp(ref tooltipOpacity, 0, lerpSpeed, ref tooltipOpacityDerivative, editorDeltaTime);


                if (tooltipOpacity > .99f)
                    tooltipOpacity = 1;

                if (tooltipOpacity < .01f)
                    tooltipOpacity = 0;


                animatingTooltip = tooltipOpacity != 0 && tooltipOpacity != 1;

            }

            gaps_();
            droppedBookmark_();
            tooltip();

        }

        float droppedBookmarkX;
        float droppedBookmarkXDerivative;

        float tooltipOpacity;
        float tooltipOpacityDerivative;

        bool animatingDroppedBookmark;
        bool animatingGaps;
        bool animatingTooltip;
        bool animatingBookmarks => animatingDroppedBookmark || animatingGaps;

        bool hideTooltip;

        List<float> gaps
        {
            get
            {
                while (_gaps.Count < data.bookmarks.Count + 1) _gaps.Add(0);
                while (_gaps.Count > data.bookmarks.Count + 1) _gaps.RemoveLast();

                return _gaps;

            }
        }
        List<float> _gaps = new();

        Bookmark lastClickedBookmark;













        public VHierarchyNavbar(EditorWindow window) => this.window = window;

        public EditorWindow window;

        public VHierarchyController controller => VHierarchy.controllers_byWindow[window];


    }
}
#endif