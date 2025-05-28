using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLogic
{
    public class HomeSystem : Singleton<HomeSystem>
    {
        protected override void OnInit()
        {
            base.OnInit();
        }

        /// <summary>
        /// 加载主界面
        /// </summary>
        public async UniTaskVoid LoadHomeSystem()
        {
            GameModule.UI.ShowUIAsync<MainMenuUI>();
        }
        
        public async UniTaskVoid LoadLevelAsync()
        {
            await GameModule.Scene.UnloadAsync("mainMenu");
            GameModule.UI.HideUI<MainMenuUI>();
            LevelSystem.Instance.LoadLevelSystem("Level1").Forget();
        }

        public void OpenOptions()
        {
        }

        public void QuitGame()
        {
            GameModule.Shutdown();
        }
    }
}