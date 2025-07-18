using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using GameLogic;
using TEngine;

#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
public partial class GameApp
{
    private static List<Assembly> _hotfixAssembly;

    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        GameEventHelper.Init();
        _hotfixAssembly = (List<Assembly>)objects[0];
        Log.Warning("======= 看到此条日志代表你成功运行了热更新代码 =======");
        Log.Warning("======= Entrance GameApp =======");
        Log.Warning("======= 热更新代码测试！！！！！！！！ =======");
        Utility.Unity.AddDestroyListener(Release);
        GetSave();
        ProcedureBase[] procedureBase = new ProcedureBase[] { new OnEnterGameAppProcedure(), new ChangeSceneProcedure(), new MainMenuProcedure(), new LevelProcedure() };
        GameModule.Procedure.RestartProcedure(procedureBase);
    }
    

    private static void GetSave()
    {
        //加载音效
        GameModule.Audio.SoundVolume = GameModule.Save.GetFloat(AudioKey.SoundVolume, 100);
        GameModule.Audio.MusicVolume = GameModule.Save.GetFloat(AudioKey.MusicVolume, 100);
        GameModule.Audio.UISoundVolume = GameModule.Save.GetFloat(AudioKey.UISoundVolume, 100);
    }


    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
}