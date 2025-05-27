using TEngine.Editor.UI;
using UnityEditor;  

public static class TEngineUISettingsProvider  
{  
    [MenuItem("TEngine/Settings/TEngineUISettings", priority = -1)]
    public static void OpenSettings() => SettingsService.OpenProjectSettings("Project/TEngine/UISettings");
    
    private const string SettingsPath = "Project/TEngine/UISettings";  

    [SettingsProvider]  
    public static SettingsProvider CreateMySettingsProvider()  
    {  
        return new SettingsProvider(SettingsPath, SettingsScope.Project)  
        {  
            label = "TEngine/UISettings",  
            guiHandler = (searchContext) =>  
            {  
                var scriptGeneratorSetting = ScriptGeneratorSetting.Instance;  
                var scriptGenerator = new SerializedObject(scriptGeneratorSetting);  

                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_codePath"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_namespace"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_widgetName"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("CodeStyle"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("scriptGenerateRule"));  
                scriptGenerator.ApplyModifiedProperties();
            },  
            keywords = new[] { "TEngine", "Settings", "Custom" }  
        };  
    }
}  
