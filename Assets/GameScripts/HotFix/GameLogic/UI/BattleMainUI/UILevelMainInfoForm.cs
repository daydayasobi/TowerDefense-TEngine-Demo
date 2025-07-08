using GameConfig;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UILevelMainInfoForm : UIWindow
    {
        #region 脚本工具生成的代码

        private Text m_textHP;
        private Text m_textEnergy;
        private GameObject m_goWaveInfo;
        private Text m_textWave;
        private Image m_imgSliderForeground;
        private Button m_btnStartWaveButton;
        private Button m_btnDebugAddCurrency;
        private Text m_textCurrencyAmount;
        private Button m_btnButtonPause;

        protected override void ScriptGenerator()
        {
            m_textHP = FindChildComponent<Text>("PlayerInfo/m_textHP");
            m_textEnergy = FindChildComponent<Text>("PlayerInfo/m_textEnergy");
            m_goWaveInfo = FindChild("m_goWaveInfo").gameObject;
            m_textWave = FindChildComponent<Text>("m_goWaveInfo/Background/m_textWave");
            m_imgSliderForeground = FindChildComponent<Image>("m_goWaveInfo/Background/SliderBackground/m_imgSliderForeground");
            m_btnStartWaveButton = FindChildComponent<Button>("m_btnStartWaveButton");
            m_btnDebugAddCurrency = FindChildComponent<Button>("m_btnDebugAddCurrency");
            m_textCurrencyAmount = FindChildComponent<Text>("m_btnDebugAddCurrency/Label/m_textCurrencyAmount");
            m_btnButtonPause = FindChildComponent<Button>("m_btnButtonPause");
            m_btnStartWaveButton.onClick.AddListener(OnClickStartWaveButtonBtn);
            m_btnDebugAddCurrency.onClick.AddListener(OnClickDebugAddCurrencyBtn);
            m_btnButtonPause.onClick.AddListener(OnClickButtonPauseBtn);
        }

        #endregion

        #region 事件

        // 开始波次按钮点击事件处理
        private void OnClickStartWaveButtonBtn()
        {
            GameEvent.Send(LevelEvent.OnGameStartWave);
            DataLevelManager.Instance.StartWave();
        }

        // 调试增加货币按钮点击事件处理
        private void OnClickDebugAddCurrencyBtn()
        {
            DataPlayerManager.Instance.DebugAddEnergy();
        }

        // 暂停按钮点击事件处理
        private void OnClickButtonPauseBtn()
        {
        }

        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            GameEvent.AddEventListener<int, int>(LevelEvent.OnPlayerHPChange, OnPlayerHPChange);
            GameEvent.AddEventListener<EnumLevelState>(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.AddEventListener<int, int, float>(LevelEvent.OnWaveUpdate, OnWaveUpdate);
            GameEvent.AddEventListener<float, float>(LevelEvent.OnPlayerEnergyChange, OnPlayerEnergyChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameEvent.RemoveEventListener<int, int>(LevelEvent.OnPlayerHPChange, OnPlayerHPChange);
            GameEvent.RemoveEventListener<EnumLevelState>(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.RemoveEventListener<int, int, float>(LevelEvent.OnWaveUpdate, OnWaveUpdate);
            GameEvent.RemoveEventListener<float, float>(LevelEvent.OnPlayerEnergyChange, OnPlayerEnergyChange);
        }

        // 设置波次信息
        private void SetWaveInfo(int currentWave, int totalWave, float progress)
        {
            m_textWave.text = string.Format("{0}/{1}", currentWave, totalWave);
            m_imgSliderForeground.fillAmount = progress;
        }

        // 玩家生命值变化事件处理
        private void OnPlayerHPChange(int LastHP,int CurrentHP)
        {
            m_textHP.text = CurrentHP.ToString();
        }

        // 玩家能量变化事件处理
        private void OnPlayerEnergyChange(float LastEnergy, float CurrentEnergy)
        {
            m_textEnergy.text = CurrentEnergy.ToString();
        }

        // 关卡状态变化事件处理
        private void OnLevelStateChange(EnumLevelState currentState)
        {
            if (currentState == EnumLevelState.Normal)
            {
                m_btnStartWaveButton.gameObject.SetActive(false);
                m_goWaveInfo.SetActive(true);

                Level level = DataLevelManager.Instance.CurrentLevel;
                SetWaveInfo(level.CurrentWaveIndex, level.WaveCount, 0);
            }
            else if (currentState == EnumLevelState.Prepare)
            {
                m_btnStartWaveButton.gameObject.SetActive(true);
                m_goWaveInfo.SetActive(false);
            }
        }

        // 波次信息更新事件处理
        private void OnWaveUpdate(int currentWave, int totalWave, float CurrentWaveProgress)
        {
            SetWaveInfo(currentWave, totalWave, CurrentWaveProgress);
        }
    }
}