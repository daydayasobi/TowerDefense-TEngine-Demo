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
using System.Diagnostics;
using Type = System.Type;
using Delegate = System.Delegate;
using Action = System.Action;
using static VTabs.VTabs;
using static VTabs.Libs.VUtils;
using static VTabs.Libs.VGUI;




namespace VTabs
{
    public class VTabsGUI
    {

        public void TabStripGUI(Rect stripRect)
        {
            void dividers()
            {
                if (!curEvent.isRepaint) return;
                if (!VTabsMenu.dividersEnabled) return;

                void divider(int i)
                {
                    if (tabs[i] == activeTab) return;
                    if (i + 1 < tabs.Count && tabs[i + 1] == activeTab) return;

                    if (tabEndPositions[i] > stripRect.xMax - 25) return; // todo use tabarearect


                    var dividerGreyscale = isDarkTheme ? .24f : .45f;

                    if (Application.unityVersion.StartsWith("6000") && VTabsMenu.classicBackgroundEnabled && isDarkTheme)
                        dividerGreyscale += .02f;

                    if (!Application.unityVersion.StartsWith("6000") && isDarkTheme)
                        dividerGreyscale += .04f;

                    if (VTabsMenu.largeTabStyleEnabled && isDarkTheme)
                        dividerGreyscale += .04f;






                    var dividerHeight = Application.unityVersion.StartsWith("6000") ? 16 : 12;
                    var dividerWidth = 1;

                    var dividerOffsetX = VTabsMenu.neatTabStyleEnabled ? -2 : 0;

                    var dividerRect = stripRect.SetX(0).SetWidth(0).MoveX(tabEndPositions[i] + dividerOffsetX).SetSizeFromMid(dividerWidth, dividerHeight);



                    dividerRect.Draw(Greyscale(dividerGreyscale));

                }

                for (int i = 0; i < tabs.Count; i++)
                    divider(i);

            }
            void addTabButton()
            {
                if (!VTabsMenu.addTabButtonEnabled) return;


                var buttonRect = stripRect.SetX(tabEndPositions.Last()).SetWidth(0).SetWidthFromMid(24).MoveX(VTabsMenu.neatTabStyleEnabled ? 12 : 13);


                var distToRight = stripRect.xMax - buttonRect.xMax;

                if (distToRight < 10) return;


                interactiveRects.Add(buttonRect);



                var fadeStart = 10;
                var fadeEnd = 25;
                var fadeK = ((distToRight - fadeStart) / (fadeEnd - fadeStart)).Clamp01().Pow(2);

                var iconName = stripRect.IsHovered() && curEvent.holdingAlt && tabInfosForReopening.Any() ? "UndoHistory" : "d_Toolbar Plus";
                var iconSize = 16;
                var colorNormal = Greyscale(isDarkTheme ? .5f : .47f, fadeK);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .1f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .5f);

                if (VTabsAddTabWindow.instance && VTabsAddTabWindow.instance.dockArea == dockArea)
                    colorNormal = colorHovered;

                if (DragAndDrop.objectReferences.Any())
                    colorHovered = colorNormal;


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                if (curEvent.holdingAlt)
                {
                    if (tabInfosForReopening.Any())
                        ReopenClosedTab();

                    return;

                }

                if (VTabsAddTabWindow.instance)
                    VTabsAddTabWindow.instance.Close();
                else
                    VTabsAddTabWindow.Open(dockArea);

            }
            void closeTabButton()
            {
                if (!VTabsMenu.closeTabButtonEnabled) return;
                if (tabs.Count == 1 && !curEvent.holdingAlt) return;

                isCloseButtonHovered = false;

                if (hoveredTab == null) return;
                if (hoveredTab == hideCloseButtonOnTab) return;



                var buttonRect = stripRect.SetX(tabEndPositions[hoveredTabIndex]).SetWidth(0).SetSizeFromMid(12).MoveX(VTabsMenu.largeTabStyleEnabled ? -16 : -14);

                if (buttonRect.xMax > stripRect.xMax - 10) return;

                interactiveRects.Add(buttonRect);

                isCloseButtonHovered = buttonRect.IsHovered();




                if (!Application.unityVersion.StartsWith("6000"))
                {
                    var backgroundColor = isDarkTheme ? Greyscale(hoveredTab == activeTab ? .23f : .19f)
                                                      : Greyscale(hoveredTab == activeTab ? .25f : .2f);

                    buttonRect.Resize(-2).DrawBlurred(backgroundColor, 3);
                    buttonRect.Resize(-1).DrawBlurred(backgroundColor.SetAlpha(.6f), 5);

                }




                var iconName = "Cross";
                var iconSize = 14;
                var colorNormal = Greyscale(isDarkTheme ? .55f : .35f);
                var colorHovered = Greyscale(isDarkTheme ? 1f : .0f);
                var colorPressed = Greyscale(isDarkTheme ? .75f : .5f);


                if (!IconButton(buttonRect, iconName, iconSize, colorNormal, colorHovered, colorPressed)) return;

                CloseTab(hoveredTab);

            }
            void curtains()
            {
                if (!curEvent.isRepaint) return;


                var isUnity6Background = Application.unityVersion.StartsWith("6000") ? !VTabsMenu.classicBackgroundEnabled : false;

                var fadeDistance = 10;
                var curtainWidth = 25;
                var curtainGreyscale = isDarkTheme ? isUnity6Background ? .075f : .15f
                                                   : isUnity6Background ? .86f : .65f;


                var tabAreaRect = dockArea.GetMemberValue<Rect>("m_TabAreaRect");

                var leftCurtainOpacity = (scrollPos / fadeDistance).Clamp01();
                var rightCurtainOpacity = ((tabEndPositions.Last() - tabAreaRect.width + 4) / fadeDistance).Clamp01();


                tabAreaRect.SetWidth(curtainWidth).DrawCurtainRight(Greyscale(curtainGreyscale, leftCurtainOpacity));
                tabAreaRect.SetWidthFromRight(curtainWidth).DrawCurtainLeft(Greyscale(curtainGreyscale, rightCurtainOpacity));


                // // fade plus button
                // if (activeTab == tabs.Last() && VTabsMenu.addTabButtonEnabled)
                //     tabAreaRect.SetWidthFromRight(0).SetWidth(20).Draw(Greyscale(curtainGreyscale, rightCurtainOpacity));

            }


            interactiveRects.Clear();

            if (curEvent.isLayout)
                UpdateState();

            dividers();
            addTabButton();
            closeTabButton();
            curtains();

            tabStripElement.pickingMode = interactiveRects.Any(r => r.IsHovered()) ? PickingMode.Position
                                                                                   : PickingMode.Ignore;
        }

        List<Rect> interactiveRects = new();

        bool isCloseButtonHovered;




        public void UpdateState()
        {
            void scrollPos_()
            {
                scrollPos = dockArea.GetFieldValue<float>("m_ScrollOffset");

                if (scrollPos != 0)
                    scrollPos -= nonZeroTabScrollOffset;

            }
            void tabEndPositions_()
            {
                tabEndPositions.Clear();


                var curPos = -scrollPos
                             + dockArea.GetMemberValue<Rect>("m_TabAreaRect").x * 2 // internally this offset is erroneously applied twice
                             - 2;

                foreach (var tab in tabs)
                {
                    curPos += GetTabWidth(tab);

                    tabEndPositions.Add(curPos.Round()); // internally tabs are drawn using plain round(), not roundToPixelGrid()

                }
            }
            void hoveredTab_()
            {
                hoveredTab = null;
                hoveredTabIndex = -1;

                if (!tabStripElement.contentRect.IsHovered()) return;


                for (int i = tabs.Count - 1; i >= 0; i--)
                    if (curEvent.mousePosition.x < tabEndPositions[i])
                        hoveredTabIndex = i;

                if (hoveredTabIndex.IsInRangeOf(tabs))
                    hoveredTab = tabs[hoveredTabIndex];

            }

            scrollPos_();
            tabEndPositions_();
            hoveredTab_();

        }

        float scrollPos;

        public List<float> tabEndPositions = new();

        int hoveredTabIndex;

        EditorWindow hoveredTab;








        void DelayCallRepaintLoop()
        {
            if (!activeTab) return; // happens when maximized


            isTabStripHovered = tabStripElement.contentRect.Move(activeTab.position.position).Contains(curEvent.mousePosition_screenSpace);

            if (isTabStripHovered)
                activeTab.Repaint();


            EditorApplication.delayCall += DelayCallRepaintLoop;


            // needed because dockarea can fail to repaint when mouse enters/leaves interactive regions (buttons, tabs)
            // seems to only happen in unity 6 when active tab is uitk based

        }

        bool isTabStripHovered;











        void HandleTabScrolling(EventBase e)
        {
            if (e is MouseMoveEvent) { sidescrollPosition = 0; return; }
            if (e is not WheelEvent scrollEvent) return;


            void switchTab(int dir)
            {
                var i0 = tabs.IndexOf(activeTab);
                var i1 = Mathf.Clamp(i0 + dir, 0, tabs.Count - 1);

                tabs[i1].Focus();

                VTabs.UpdateTitle(tabs[i1]);

            }
            void moveTab(int dir)
            {
                var i0 = tabs.IndexOf(activeTab);
                var i1 = Mathf.Clamp(i0 + dir, 0, tabs.Count - 1);

                var r = tabs[i0];
                tabs[i0] = tabs[i1];
                tabs[i1] = r;

                tabs[i1].Focus();

            }

            void shiftscroll()
            {
                if (!VTabsMenu.switchTabShortcutEnabled) return;

                if (scrollEvent.modifiers != (EventModifiers.Shift)
                 && scrollEvent.modifiers != (EventModifiers.Shift | EventModifiers.Control)
                 && scrollEvent.modifiers != (EventModifiers.Shift | EventModifiers.Command)) return;



                var scrollDelta = Application.platform == RuntimePlatform.OSXEditor ? scrollEvent.delta.x                         // osx sends delta.y as delta.x when shift is pressed
                                                                                    : scrollEvent.delta.x - scrollEvent.delta.y;  // some software on windows (eg logitech options) may do that too
                if (VTabsMenu.reverseScrollDirectionEnabled)
                    scrollDelta *= -1;

                if (scrollDelta == 0) return;

                e.StopPropagation();



                if (scrollEvent.ctrlKey || scrollEvent.commandKey)
                    moveTab(scrollDelta > 0 ? 1 : -1);
                else
                    switchTab(scrollDelta > 0 ? 1 : -1);

            }
            void sidescroll()
            {
                if (!VTabsMenu.sidescrollEnabled) return;

                if (scrollEvent.modifiers != EventModifiers.None
                 && scrollEvent.modifiers != EventModifiers.Command
                 && scrollEvent.modifiers != EventModifiers.Control) return;



                if (scrollEvent.delta.x.Abs() < scrollEvent.delta.y.Abs()) { sidescrollPosition = 0; return; }

                e.StopPropagation();

                if (scrollEvent.delta.x.Abs() <= 0.06f) return;



                var dampenK = 5; // the larger this k is - the smaller big deltas are, and the less is sidescroll's dependency on scroll speed
                var a = scrollEvent.delta.x.Abs() * dampenK;
                var deltaDampened = (a < 1 ? a : Mathf.Log(a) + 1) / dampenK * -scrollEvent.delta.x.Sign();

                var sensitivityK = .22f;
                var scrollDelta = deltaDampened * VTabsMenu.sidescrollSensitivity * sensitivityK;

                if (VTabsMenu.reverseScrollDirectionEnabled)
                    scrollDelta *= -1;

                if (sidescrollPosition.RoundToInt() == (sidescrollPosition += scrollDelta).RoundToInt()) return;




                if (scrollEvent.ctrlKey || scrollEvent.commandKey)
                    moveTab(scrollDelta > 0 ? 1 : -1);
                else
                    switchTab(scrollDelta > 0 ? 1 : -1);

            }


            shiftscroll();
            sidescroll();

        }

        float sidescrollPosition;



        void HandleDragndrop(EventBase e)
        {
            if (!VTabsMenu.dragndropEnabled) return;


            var dragndropArea = panel.visualTree.contentRect.SetHeight(activeTab.GetType() == t_SceneHierarchyWindow ? 20 : 40);

            if (!dragndropArea.Contains(e.originalMousePosition)) return;



            if (e is DragUpdatedEvent dragUpdatedEvent)
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;



            if (e is not DragPerformEvent dragPerformEvent) return;

            DragAndDrop.AcceptDrag();

            AddTab(new VTabs.TabInfo(DragAndDrop.objectReferences.First()));

            lastDragndropTime = System.DateTime.UtcNow;

        }

        static System.DateTime lastDragndropTime;



        void HandleHidingCloseButton(EventBase e)
        {
            if (e is MouseDownEvent && !isCloseButtonHovered)
                if (hoveredTab != null && hoveredTab != activeTab)
                    hideCloseButtonOnTab = hoveredTab;

            if (e is MouseMoveEvent)
                if (hoveredTab != hideCloseButtonOnTab)
                    hideCloseButtonOnTab = null;
        }

        EditorWindow hideCloseButtonOnTab;



        void HandleHiddenMenu(MouseDownEvent mouseDownEvent)
        {
            if (mouseDownEvent.modifiers != EventModifiers.Alt) return;
            if (mouseDownEvent.button != 1) return;
            if (!tabStripElement.contentRect.Contains(mouseDownEvent.mousePosition)) return;


            mouseDownEvent.StopPropagation();


            GenericMenu menu = new();

            menu.AddDisabledItem(new GUIContent("vTabs hidden menu"));

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Select cache"), false, () => Selection.activeObject = VTabsCache.instance);
            menu.AddItem(new GUIContent("Clear cache"), false, VTabsCache.Clear);

            menu.ShowAsContext();


        }














        public EditorWindow AddTab(TabInfo tabInfo, bool atOriginalTabIndex = false)
        {

            var lastInteractedBrowser = t_ProjectBrowser.GetFieldValue("s_LastInteractedProjectBrowser"); // changes on new browser creation 

            var window = (EditorWindow)ScriptableObject.CreateInstance(tabInfo.typeName);

            void notifyVFavorites()
            {
                mi_VFavorites_BeforeWindowCreated?.Invoke(null, new object[] { dockArea });
            }
            void addToDockArea()
            {
                if (atOriginalTabIndex)
                    dockArea.InvokeMethod("AddTab", tabInfo.originalTabIndex, window, true);
                else
                    dockArea.InvokeMethod("AddTab", window, true);

            }

            void setupBrowser()
            {
                if (!tabInfo.isBrowser) return;


                void setSavedGridSize()
                {
                    if (!tabInfo.isGridSizeSaved) return;

                    window.GetFieldValue("m_ListArea")?.SetMemberValue("gridSize", tabInfo.savedGridSize);

                }
                void setLastUsedGridSize()
                {
                    if (tabInfo.isGridSizeSaved) return;
                    if (lastInteractedBrowser == null) return;

                    var listAreaSource = lastInteractedBrowser.GetFieldValue("m_ListArea");
                    var listAreaDest = window.GetFieldValue("m_ListArea");

                    if (listAreaSource != null && listAreaDest != null)
                        listAreaDest.SetPropertyValue("gridSize", listAreaSource.GetPropertyValue("gridSize"));

                }

                void setSavedLayout()
                {
                    if (!tabInfo.isLayoutSaved) return;

                    var layoutEnum = System.Enum.ToObject(t_ProjectBrowser.GetField("m_ViewMode", maxBindingFlags).FieldType, tabInfo.savedLayout);

                    window.InvokeMethod("SetViewMode", layoutEnum);

                }
                void setLastUsedLayout()
                {
                    if (tabInfo.isLayoutSaved) return;
                    if (lastInteractedBrowser == null) return;

                    window.InvokeMethod("SetViewMode", lastInteractedBrowser.GetMemberValue("m_ViewMode"));

                }

                void setLastUsedListWidth()
                {
                    if (lastInteractedBrowser == null) return;

                    window.SetFieldValue("m_DirectoriesAreaWidth", lastInteractedBrowser.GetFieldValue("m_DirectoriesAreaWidth"));

                }

                void lockToFolder_twoColumns()
                {
                    if (!tabInfo.isLocked) return;
                    if (window.GetMemberValue<int>("m_ViewMode") != 1) return;
                    if (tabInfo.folderGuid.IsNullOrEmpty()) return;


                    var iid = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(tabInfo.folderGuid)).GetInstanceID();

                    window.GetFieldValue("m_ListAreaState").SetFieldValue("m_SelectedInstanceIDs", new List<int> { iid });

                    t_ProjectBrowser.InvokeMethod("OpenSelectedFolders");


                    window.SetPropertyValue("isLocked", true);

                }
                void lockToFolder_oneColumn()
                {
                    if (!tabInfo.isLocked) return;
                    if (window.GetMemberValue<int>("m_ViewMode") != 0) return;
                    if (tabInfo.folderGuid.IsNullOrEmpty()) return;

                    if (window.GetMemberValue("m_AssetTree") is not object m_AssetTree) return;
                    if (m_AssetTree.GetMemberValue("data") is not object data) return;


                    var folderPath = tabInfo.folderGuid.ToPath();
                    var folderIid = AssetDatabase.LoadAssetAtPath<Object>(folderPath).GetInstanceID();

                    data.SetMemberValue("m_rootInstanceID", folderIid);

                    m_AssetTree.InvokeMethod("ReloadData");

                    VTabs.SetLockedFolderPath_oneColumn(window, folderPath);


                    window.SetPropertyValue("isLocked", true);

                }


                window.InvokeMethod("Init");

                setSavedGridSize();
                setLastUsedGridSize();

                setSavedLayout();
                setLastUsedLayout();

                setLastUsedListWidth();

                lockToFolder_twoColumns();
                lockToFolder_oneColumn();

                VTabs.UpdateTitle(window);

            }
            void setupPropertyEditor()
            {
                if (!tabInfo.isPropertyEditor) return;
                if (tabInfo.globalId.isNull) return;


                var lockTo = tabInfo.globalId.GetObject();

                if (tabInfo.lockedPrefabAssetObject)
                    lockTo = tabInfo.lockedPrefabAssetObject; // globalId api doesn't work for prefab asset objects, so we use direct object reference in such cases 

                if (!lockTo) return;


                window.GetMemberValue("tracker").InvokeMethod("SetObjectsLockedByThisTracker", (new List<Object> { lockTo }));

                window.SetMemberValue("m_GlobalObjectId", tabInfo.globalId.ToString());
                window.SetMemberValue("m_InspectedObject", lockTo);

                VTabs.UpdateTitle(window);

            }

            void setCustomEditorWindowTitle()
            {
                if (window.titleContent.text != window.GetType().FullName) return;
                if (tabInfo.originalTitle.IsNullOrEmpty()) return;

                window.titleContent.text = tabInfo.originalTitle;

                // custom EditorWindows often have their titles set in EditorWindow.GetWindow
                // and when such windows are created via ScriptableObject.CreateInstance, their titles default to window type name
                // so we have to set original window title in such cases

            }


            notifyVFavorites();
            addToDockArea();

            setupBrowser();
            setupPropertyEditor();

            setCustomEditorWindowTitle();


            window.Focus();



            return window;
        }

        public void CloseTab(EditorWindow tab)
        {
            tabInfosForReopening.Push(new TabInfo(tab));

            VTabsAddTabWindow.RememberWindow(tab);

            tab.Close();

        }

        public void ReopenClosedTab()
        {
            if (!tabInfosForReopening.Any()) return;


            var tabInfo = tabInfosForReopening.Pop();


            var prevActiveTab = activeTab;

            var reopenedTab = AddTab(tabInfo, atOriginalTabIndex: true);

            if (!tabInfo.wasFocused)
                prevActiveTab.Focus();



            VTabs.UpdateTitle(reopenedTab);

        }

        Stack<TabInfo> tabInfosForReopening = new();













        public void UpdateScrollAnimation()
        {
            if (activeTab != EditorWindow.focusedWindow) return;
            if (!guiStylesInitialized) return;
            if ((System.DateTime.UtcNow - lastDragndropTime).TotalSeconds < .05f) return; // to avoid stutter after dragndrop



            var curScrollPos = dockArea.GetFieldValue<float>("m_ScrollOffset");

            if (!curScrollPos.Approx(0))
                curScrollPos -= nonZeroTabScrollOffset;

            if (curScrollPos == 0)
                curScrollPos = prevScrollPos; // prevents immediate jump to 0 on tab close



            var targScrollPos = GetTargetScrollPosition();

            // var animationSpeed = 1f;
            var animationSpeed = 7f;

            var newScrollPos = MathUtil.SmoothDamp(curScrollPos, targScrollPos, animationSpeed, ref scrollPosDeriv, editorDeltaTime);

            if (newScrollPos < .5f)
                newScrollPos = 0;

            prevScrollPos = newScrollPos;




            if (newScrollPos.Approx(curScrollPos)) return;

            if (!newScrollPos.Approx(0))
                newScrollPos += nonZeroTabScrollOffset;

            dockArea.SetFieldValue("m_ScrollOffset", newScrollPos);

            activeTab.Repaint();

        }

        public float nonZeroTabScrollOffset = 3f;

        float scrollPosDeriv;
        float prevScrollPos;



        public float GetTargetScrollPosition()
        {
            if (!guiStylesInitialized) return 0;


            var tabAreaWidth = dockArea.GetFieldValue<Rect>("m_TabAreaRect").width;

            if (tabAreaWidth == 0)
                tabAreaWidth = activeTab.position.width - 38;




            var activeTabXMin = 0f;
            var activeTabXMax = 0f;

            var tabWidthSum = 0f;

            var activeTabReached = false;

            foreach (var tab in tabs)
            {
                var tabWidth = GetTabWidth(tab);

                tabWidthSum += tabWidth;


                if (activeTabReached) continue;

                activeTabXMin = activeTabXMax;
                activeTabXMax += tabWidth;

                if (tab == activeTab)
                    activeTabReached = true;

            }




            var optimalScrollPos = 0f;

            var visibleAreaPadding = 65f;

            var visibleAreaXMin = activeTabXMin - visibleAreaPadding;
            var visibleAreaXMax = activeTabXMax + visibleAreaPadding;

            optimalScrollPos = Mathf.Max(optimalScrollPos, visibleAreaXMax - tabAreaWidth);
            optimalScrollPos = Mathf.Min(optimalScrollPos, tabWidthSum - tabAreaWidth + 4);

            optimalScrollPos = Mathf.Min(optimalScrollPos, visibleAreaXMin);
            optimalScrollPos = Mathf.Max(optimalScrollPos, 0);




            return optimalScrollPos;

        }

        public float GetTabWidth(EditorWindow tab)
        {
            if (guiStylesInitialized)
                tabStyle ??= typeof(GUI).GetMemberValue<GUISkin>("s_Skin")?.FindStyle("dragtab");

            if (tabStyle == null) return 0;


            return dockArea.InvokeMethod<float>("GetTabWidth", tabStyle, tab);

        }

        static GUIStyle tabStyle;

        bool guiStylesInitialized => typeof(GUI).GetFieldValue("s_Skin") != null;









        public void UpdateLockButtonHiding()
        {
            bool isLocked(EditorWindow window)
            {
                if (window.GetType() == t_SceneHierarchyWindow)
                    return window.GetMemberValue("m_SceneHierarchy").GetMemberValue<bool>("isLocked");

                if (window.GetType() == t_InspectorWindow)
                    return window.GetMemberValue<bool>("isLocked");

                return false;
            }

            var shouldHideLockButton = VTabsMenu.hideLockButtonEnabled && !isLocked(activeTab);



            if (!shouldHideLockButton && lockButtonDelegate != null)
            {
                dockArea.SetMemberValue("m_ShowButton", lockButtonDelegate);
                lockButtonDelegate = null;
            }

            if (shouldHideLockButton)
            {
                lockButtonDelegate ??= dockArea.GetMemberValue("m_ShowButton");

                dockArea.SetMemberValue("m_ShowButton", null);
            }

        }

        object lockButtonDelegate;










        public VTabsGUI(Object dockArea)
        {
            this.dockArea = dockArea;


            panel = dockArea.GetMemberValue<EditorWindow>("actualView").rootVisualElement.panel;

            tabs = dockArea.GetMemberValue<List<EditorWindow>>("m_Panes");




            panel.visualTree.RegisterCallback<WheelEvent>(HandleTabScrolling, TrickleDown.TrickleDown);
            panel.visualTree.RegisterCallback<MouseMoveEvent>(HandleTabScrolling, TrickleDown.TrickleDown);

            panel.visualTree.RegisterCallback<DragUpdatedEvent>(HandleDragndrop, TrickleDown.TrickleDown);
            panel.visualTree.RegisterCallback<DragPerformEvent>(HandleDragndrop, TrickleDown.NoTrickleDown); // no trickledown to avoid creating tab when dropping on navbar

            panel.visualTree.RegisterCallback<MouseDownEvent>(HandleHidingCloseButton, TrickleDown.TrickleDown);
            panel.visualTree.RegisterCallback<MouseMoveEvent>(HandleHidingCloseButton, TrickleDown.TrickleDown);

            panel.visualTree.RegisterCallback<MouseDownEvent>(HandleHiddenMenu, TrickleDown.TrickleDown);






            tabStripElement = new IMGUIContainer();

            tabStripElement.name = "vTabs-tab-strip";

            tabStripElement.style.width = Length.Percent(100);
            tabStripElement.style.height = Application.unityVersion.StartsWith("6000") ? 24 : 19;
            tabStripElement.style.position = Position.Absolute;

            tabStripElement.pickingMode = PickingMode.Ignore;

            tabStripElement.onGUIHandler = () => TabStripGUI(tabStripElement.contentRect);

            panel.visualTree.Add(tabStripElement);




            EditorApplication.delayCall += DelayCallRepaintLoop;

        }

        Object dockArea;
        IPanel panel;
        public List<EditorWindow> tabs;
        IMGUIContainer tabStripElement;

        public EditorWindow activeTab => tabs.FirstOrDefault(r => r.hasFocus);

    }
}
#endif