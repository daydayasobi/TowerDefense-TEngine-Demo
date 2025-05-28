using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class LevelMainInfoUI : UIWindow
    {
        #region 脚本工具生成的代码
        private GameObject _goPlayerInfo;
        private Text _textLifeCounter;
        private Text _textCurrencyAmount;
        private GameObject _goWaveInfo;
        private Button _btnStartWaveButton;
        private Text _textStartWave;
        private Button _btnDebugAddCurrency;
        protected override void ScriptGenerator()
        {
            _goPlayerInfo = FindChild("m_goPlayerInfo").gameObject;
            _textLifeCounter = FindChildComponent<Text>("m_goPlayerInfo/m_textLifeCounter");
            _textCurrencyAmount = FindChildComponent<Text>("m_goPlayerInfo/CurrencyContainer/m_textCurrencyAmount");
            _goWaveInfo = FindChild("m_goWaveInfo").gameObject;
            _btnStartWaveButton = FindChildComponent<Button>("m_btnStartWaveButton");
            _textStartWave = FindChildComponent<Text>("m_btnStartWaveButton/m_textStartWave");
            _btnDebugAddCurrency = FindChildComponent<Button>("m_btnDebugAddCurrency");
            _btnStartWaveButton.onClick.AddListener(OnClickStartWaveButtonBtn);
            _btnDebugAddCurrency.onClick.AddListener(OnClickDebugAddCurrencyBtn);
        }
        #endregion

        #region 事件
        private void OnClickStartWaveButtonBtn()
        {
        }
        private void OnClickDebugAddCurrencyBtn()
        {
            // 测试按钮 添加炮塔
            GameEvent.Send(EventDefine.ShowPreviewTower);
        }
        #endregion

    }
}