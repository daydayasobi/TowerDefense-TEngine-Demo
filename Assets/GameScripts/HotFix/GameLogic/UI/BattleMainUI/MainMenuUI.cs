using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class MainMenuUI : UIWindow
    {
        #region 脚本工具生成的代码
        private Text _textTitle;
        private GameObject _goButtons;
        private Button _btnLevelSelect;
        private Text _textLevelSelect;
        private Button _btnOptions;
        private Text _textOptions;
        private Button _btnQuit;
        private Text _textQuit;
        protected override void ScriptGenerator()
        {
            _textTitle = FindChildComponent<Text>("m_textTitle");
            _goButtons = FindChild("m_goButtons").gameObject;
            _btnLevelSelect = FindChildComponent<Button>("m_goButtons/m_btnLevelSelect");
            _textLevelSelect = FindChildComponent<Text>("m_goButtons/m_btnLevelSelect/m_textLevelSelect");
            _btnOptions = FindChildComponent<Button>("m_goButtons/m_btnOptions");
            _textOptions = FindChildComponent<Text>("m_goButtons/m_btnOptions/m_textOptions");
            _btnQuit = FindChildComponent<Button>("m_goButtons/m_btnQuit");
            _textQuit = FindChildComponent<Text>("m_goButtons/m_btnQuit/m_textQuit");
            _btnLevelSelect.onClick.AddListener(UniTask.UnityAction(OnClickLevelSelectBtn));
            _btnOptions.onClick.AddListener(UniTask.UnityAction(OnClickOptionsBtn));
            _btnQuit.onClick.AddListener(UniTask.UnityAction(OnClickQuitBtn));
        }
        #endregion

        #region 事件
        private async UniTaskVoid OnClickLevelSelectBtn()
        {
            await UniTask.Yield();
            // HomeSystem.Instance.LoadLevelAsync();
            GameEvent.Send(MenuEvent.ChangeLevelSelect);
        }
        private async UniTaskVoid OnClickOptionsBtn()
        {
            await UniTask.Yield();
            HomeSystem.Instance.OpenOptions();
        }
        private async UniTaskVoid OnClickQuitBtn()
        {
            await UniTask.Yield();
            HomeSystem.Instance.QuitGame();
        }
        #endregion

    }
}