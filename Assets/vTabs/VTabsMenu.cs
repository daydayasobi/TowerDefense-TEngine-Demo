#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using static VTabs.Libs.VUtils;



namespace VTabs
{
    public static class VTabsMenu
    {

        public static bool dragndropEnabled { get => EditorPrefsCached.GetBool("vTabs-dragndropEnabled", true); set => EditorPrefsCached.SetBool("vTabs-dragndropEnabled", value); }
        public static bool addTabButtonEnabled { get => EditorPrefsCached.GetBool("vTabs-addTabButtonEnabled", false); set => EditorPrefsCached.SetBool("vTabs-addTabButtonEnabled", value); }
        public static bool closeTabButtonEnabled { get => EditorPrefsCached.GetBool("vTabs-closeTabButtonEnabled", false); set => EditorPrefsCached.SetBool("vTabs-closeTabButtonEnabled", value); }
        public static bool dividersEnabled { get => EditorPrefsCached.GetBool("vTabs-dividersEnabled", false); set => EditorPrefsCached.SetBool("vTabs-dividersEnabled", value); }
        public static bool hideLockButtonEnabled { get => EditorPrefsCached.GetBool("vTabs-hideLockButtonEnabled", false); set => EditorPrefsCached.SetBool("vTabs-hideLockButtonEnabled", value); }

        public static int tabStyle { get => EditorPrefsCached.GetInt("vTabs-tabStyle", 0); set => EditorPrefsCached.SetInt("vTabs-tabStyle", value); }
        public static bool defaultTabStyleEnabled => tabStyle == 0 || !Application.unityVersion.StartsWith("6000");
        public static bool largeTabStyleEnabled => tabStyle == 1 && Application.unityVersion.StartsWith("6000");
        public static bool neatTabStyleEnabled => tabStyle == 2 && Application.unityVersion.StartsWith("6000");

        public static int backgroundStyle { get => EditorPrefsCached.GetInt("vTabs-backgroundStyle", 0); set => EditorPrefsCached.SetInt("vTabs-backgroundStyle", value); }
        public static bool defaultBackgroundEnabled => backgroundStyle == 0 || !Application.unityVersion.StartsWith("6000");
        public static bool classicBackgroundEnabled => backgroundStyle == 1 && Application.unityVersion.StartsWith("6000");
        public static bool greyBackgroundEnabled => backgroundStyle == 2 && Application.unityVersion.StartsWith("6000");


        public static bool switchTabShortcutEnabled { get => EditorPrefsCached.GetBool("vTabs-switchTabShortcutEnabled", true); set => EditorPrefsCached.SetBool("vTabs-switchTabShortcutEnabled", value); }
        public static bool addTabShortcutEnabled { get => EditorPrefsCached.GetBool("vTabs-addTabShortcutEnabled", true); set => EditorPrefsCached.SetBool("vTabs-addTabShortcutEnabled", value); }
        public static bool closeTabShortcutEnabled { get => EditorPrefsCached.GetBool("vTabs-closeTabShortcutEnabled", true); set => EditorPrefsCached.SetBool("vTabs-closeTabShortcutEnabled", value); }
        public static bool reopenTabShortcutEnabled { get => EditorPrefsCached.GetBool("vTabs-reopenTabShortcutEnabled", true); set => EditorPrefsCached.SetBool("vTabs-reopenTabShortcutEnabled", value); }

        public static bool sidescrollEnabled { get => EditorPrefsCached.GetBool("vTabs-sidescrollEnabled", Application.platform == RuntimePlatform.OSXEditor); set => EditorPrefsCached.SetBool("vTabs-sidescrollEnabled", value); }
        public static float sidescrollSensitivity { get => EditorPrefsCached.GetFloat("vTabs-sidescrollSensitivity", 1); set => EditorPrefsCached.SetFloat("vTabs-sidescrollSensitivity", value); }
        public static bool reverseScrollDirectionEnabled { get => EditorPrefs.GetBool("vTabs-reverseScrollDirectionDirection", false); set => EditorPrefs.SetBool("vTabs-reverseScrollDirectionDirection", value); }

        public static bool pluginDisabled { get => EditorPrefsCached.GetBool("vTabs-pluginDisabled", false); set => EditorPrefsCached.SetBool("vTabs-pluginDisabled", value); }




        const string dir = "Tools/vTabs/";
#if UNITY_EDITOR_OSX
        const string cmd = "Cmd";
#else
        const string cmd = "Ctrl";
#endif

        const string dragndrop = dir + "Create tabs with Drag-and-Drop";
        const string reverseScrollDirection = dir + "Reverse direction";
        const string addTabButton = dir + "Add Tab button";
        const string closeTabButton = dir + "Close Tab button";
        const string dividers = dir + "Tab dividers";
        const string hideLockButton = dir + "Hide lock button";

        const string defaultTabStyle = dir + "Tab style/Default";
        const string largeTabs = dir + "Tab style/Large";
        const string neatTabs = dir + "Tab style/Neat";

        const string defaultBackgroundStyle = dir + "Background style/Default";
        const string classicBackground = dir + "Background style/Classic";
        const string greyBackground = dir + "Background style/Grey";


        const string switchTabShortcut = dir + "Shift-Scroll to switch tab"; // todo moveTab shortcut?
        const string addTabShortcut = dir + cmd + "-T to add tab";
        const string closeTabShortcut = dir + cmd + "-W to close tab";
        const string reopenTabShortcut = dir + cmd + "-Shift-T to reopen closed tab";


        const string sidescroll = dir + "Sidescroll to switch tab";
        const string increaseSensitivity = dir + "Increase sensitivity";
        const string decreaseSensitivity = dir + "Decrease sensitivity";


        const string disablePlugin = dir + "Disable vTabs";







        [MenuItem(dir + "Features", false, 1)] static void dadsas() { }
        [MenuItem(dir + "Features", true, 1)] static bool dadsas123() => false;

        // [MenuItem(dragndrop, false, 2)] static void dadsadsadasdsadadsas() => dragndropEnabled = !dragndropEnabled;
        // [MenuItem(dragndrop, true, 2)] static bool dadsaddsasadadsdasadsas() { Menu.SetChecked(dragndrop, dragndropEnabled); return !pluginDisabled; } 

        [MenuItem(addTabButton, false, 3)] static void dadsadsadsadasdsadadsas() { addTabButtonEnabled = !addTabButtonEnabled; VTabs.RepaintAllDockAreas(); }
        [MenuItem(addTabButton, true, 3)] static bool dadsadasddsasadadsdasadsas() { Menu.SetChecked(addTabButton, addTabButtonEnabled); return !pluginDisabled; }

        [MenuItem(closeTabButton, false, 4)] static void dadsadsaddassadasdsadadsas() { closeTabButtonEnabled = !closeTabButtonEnabled; VTabs.RepaintAllDockAreas(); }
        [MenuItem(closeTabButton, true, 4)] static bool dadsadasddsadsasadadsdasadsas() { Menu.SetChecked(closeTabButton, closeTabButtonEnabled); return !pluginDisabled; }

        [MenuItem(dividers, false, 5)] static void dadsadsaddasdssadasdsadadsas() { dividersEnabled = !dividersEnabled; VTabs.RepaintAllDockAreas(); }
        [MenuItem(dividers, true, 5)] static bool dadsadasddsdsadsasadadsdasadsas() { Menu.SetChecked(dividers, dividersEnabled); return !pluginDisabled; }

        [MenuItem(hideLockButton, false, 7)] static void dadsadsaddsdassadasdsadadsas() { hideLockButtonEnabled = !hideLockButtonEnabled; VTabs.RepaintAllDockAreas(); }
        [MenuItem(hideLockButton, true, 7)] static bool dadsadasdsdsadsasadadsdasadsas() { Menu.SetChecked(hideLockButton, hideLockButtonEnabled); return !pluginDisabled; }

#if UNITY_6000_0_OR_NEWER

        [MenuItem(defaultTabStyle, false, 8)] static void dadsadsaddasdssadasdssdadadsas() { tabStyle = 0; VTabs.UpdateStyleSheet(); }
        [MenuItem(defaultTabStyle, true, 8)] static bool dadsadasddsdsdsadsasadadsdasadsas() { Menu.SetChecked(defaultTabStyle, tabStyle == 0); return !pluginDisabled; }

        [MenuItem(largeTabs, false, 9)] static void dadsadsaddasdssadsdasdssdadadsas() { tabStyle = 1; VTabs.UpdateStyleSheet(); }
        [MenuItem(largeTabs, true, 9)] static bool dadsadasddsdsdsdsadsasadadsdasadsas() { Menu.SetChecked(largeTabs, tabStyle == 1); return !pluginDisabled; }

        [MenuItem(neatTabs, false, 10)] static void dadsadsaddasdsssadasdssdadadsas() { tabStyle = 2; VTabs.UpdateStyleSheet(); }
        [MenuItem(neatTabs, true, 10)] static bool dadsadasddsdsddssadsasadadsdasadsas() { Menu.SetChecked(neatTabs, tabStyle == 2); return !pluginDisabled; }



        [MenuItem(defaultBackgroundStyle, false, 11)] static void dadsadsaddasdsdssadasdssdadadsas() { backgroundStyle = 0; VTabs.UpdateStyleSheet(); }
        [MenuItem(defaultBackgroundStyle, true, 11)] static bool dadsadasddssddsdsadsasadadsdasadsas() { Menu.SetChecked(defaultBackgroundStyle, backgroundStyle == 0); return !pluginDisabled; }

        [MenuItem(classicBackground, false, 12)] static void dadsadsadsddasdssadsdasdssdadadsas() { backgroundStyle = 1; VTabs.UpdateStyleSheet(); }
        [MenuItem(classicBackground, true, 12)] static bool dadsadasddsdsdsdsdsadsasadadsdasadsas() { Menu.SetChecked(classicBackground, backgroundStyle == 1); return !pluginDisabled; }

        // [MenuItem(greyBackground, false, 12)] static void dadsadsdsadsddasdssadsdasdssdadadsas() { backgroundStyle = 2; VTabs.UpdateStyleSheet(); }
        // [MenuItem(greyBackground, true, 12)] static bool dadsadasdsddsdsdsdsdsadsasadadsdasadsas() { Menu.SetChecked(greyBackground, backgroundStyle == 2); return !pluginDisabled; }

#endif




        [MenuItem(dir + "Shortcuts", false, 101)] static void daaadsas() { }
        [MenuItem(dir + "Shortcuts", true, 101)] static bool daadsdsas123() => false;

        [MenuItem(switchTabShortcut, false, 102)] static void dadsadsadsadsadasdsadadsas() => switchTabShortcutEnabled = !switchTabShortcutEnabled;
        [MenuItem(switchTabShortcut, true, 102)] static bool dadsadasdasddsasadadsdasadsas() { Menu.SetChecked(switchTabShortcut, switchTabShortcutEnabled); return !pluginDisabled; }

        [MenuItem(addTabShortcut, false, 103)] static void dadsadadsas() => addTabShortcutEnabled = !addTabShortcutEnabled;
        [MenuItem(addTabShortcut, true, 103)] static bool dadsaddasadsas() { Menu.SetChecked(addTabShortcut, addTabShortcutEnabled); return !pluginDisabled; }

        [MenuItem(closeTabShortcut, false, 104)] static void dadsadasdadsas() => closeTabShortcutEnabled = !closeTabShortcutEnabled;
        [MenuItem(closeTabShortcut, true, 104)] static bool dadsadsaddasadsas() { Menu.SetChecked(closeTabShortcut, closeTabShortcutEnabled); return !pluginDisabled; }

        [MenuItem(reopenTabShortcut, false, 105)] static void dadsadsadasdadsas() => reopenTabShortcutEnabled = !reopenTabShortcutEnabled;
        [MenuItem(reopenTabShortcut, true, 105)] static bool dadsaddsasaddasadsas() { Menu.SetChecked(reopenTabShortcut, reopenTabShortcutEnabled); return !pluginDisabled; }




#if UNITY_EDITOR_OSX

        [MenuItem(dir + "Trackpad", false, 1001)] static void daadsdsadsas() { }
        [MenuItem(dir + "Trackpad", true, 1001)] static bool dadsasasdads() => false;

        [MenuItem(sidescroll, false, 1002)] static void dadsadsadsadsadasdadssadadsas() => sidescrollEnabled = !sidescrollEnabled;
        [MenuItem(sidescroll, true, 1002)] static bool dadsadasdasddsadassadadsdasadsas() { Menu.SetChecked(sidescroll, sidescrollEnabled); return !pluginDisabled; }

        [MenuItem(increaseSensitivity, false, 1004)] static void qdadadsssa() { sidescrollSensitivity += .2f; Debug.Log("vTabs: scrolling sensitivity increased to " + sidescrollSensitivity * 100 + "%"); }
        [MenuItem(increaseSensitivity, true, 1004)] static bool qdaddasadsssa() => !pluginDisabled;

        [MenuItem(decreaseSensitivity, false, 1005)] static void qdasadsssa() { sidescrollSensitivity -= .2f; Debug.Log("vTabs: trackpad sensitivity decreased to " + sidescrollSensitivity * 100 + "%"); }
        [MenuItem(decreaseSensitivity, true, 1005)] static bool qdaddasdsaadsssa() => !pluginDisabled;

        // [MenuItem(reverseScrollDirection, false, 1006)] static void dadsadadssadsadsadasdadssadadsas() => reverseScrollDirectionEnabled = !reverseScrollDirectionEnabled;
        // [MenuItem(reverseScrollDirection, true, 1006)] static bool dadsadasdadsasddsadassadadsdasadsas() { Menu.SetChecked(reverseScrollDirection, reverseScrollDirectionEnabled); return !pluginDisabled; } // don't delete the option, there are people using it

#endif






        [MenuItem(dir + "More", false, 10001)] static void daasadsddsas() { }
        [MenuItem(dir + "More", true, 10001)] static bool dadsadsdasas123() => false;

        [MenuItem(dir + "Open manual", false, 10002)]
        static void dadadssadsas() => Application.OpenURL("https://kubacho-lab.gitbook.io/vtabs-2");

        [MenuItem(dir + "Join our Discord", false, 10003)]
        static void dadasdsas() => Application.OpenURL("https://discord.gg/pUektnZeJT");




        [MenuItem(disablePlugin, false, 100001)] static void dadsadsdasadasdasdsadadsas() { pluginDisabled = !pluginDisabled; VTabs.UpdateStyleSheet(); UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation(); }
        [MenuItem(disablePlugin, true, 100001)] static bool dadsaddssdaasadsadadsdasadsas() { Menu.SetChecked(disablePlugin, pluginDisabled); return true; }


    }
}
#endif