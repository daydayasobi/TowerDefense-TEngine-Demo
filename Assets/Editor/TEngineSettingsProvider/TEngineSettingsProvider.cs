using TEngine;
using UnityEditor;  

public static class TEngineSettingsProvider  
{  
    [MenuItem("TEngine/Settings/TEngine UpdateSettings", priority = -1)]
    public static void OpenSettings() => SettingsService.OpenProjectSettings("Project/TEngine/UpdateSettings");
    
    private const string SettingsPath = "Project/TEngine/UpdateSettings";  

    [SettingsProvider]  
    public static SettingsProvider CreateMySettingsProvider()  
    {  
        return new SettingsProvider(SettingsPath, SettingsScope.Project)  
        {  
            label = "TEngine/UpdateSettings",  
            guiHandler = (searchContext) =>  
            {  
                var settings = Settings.UpdateSetting;  
                var serializedObject = new SerializedObject(settings);  

                EditorGUILayout.PropertyField(serializedObject.FindProperty("projectName"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HotUpdateAssemblies"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AOTMetaAssemblies"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LogicMainDllName"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AssemblyTextAssetExtension"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AssemblyTextAssetPath"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateStyle"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ResDownLoadPath"));  
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FallbackResDownLoadPath"));  
                serializedObject.ApplyModifiedProperties();  
            },  
            keywords = new[] { "TEngine", "Settings", "Custom" }  
        };  
    }
}  
