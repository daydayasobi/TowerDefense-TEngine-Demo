using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public class UIOptionsForm : UIWindow
    {
        #region 脚本工具生成的代码

        private Button m_btnBack;
        private Slider m_sliderMasterVolumeSlider;
        private Slider m_sliderSFXVolumeSlider;
        private Slider m_sliderMusicVolumeSlider;
        private Button m_btnChineseSimplified;
        private Button m_btnEnglish;

        protected override void ScriptGenerator()
        {
            m_btnBack = FindChildComponent<Button>("Root/m_btnBack");
            m_sliderMasterVolumeSlider = FindChildComponent<Slider>("Root/Panel/MasterVolumeSlider/m_sliderMasterVolumeSlider");
            m_sliderSFXVolumeSlider = FindChildComponent<Slider>("Root/Panel/SFXVolumeSlider/m_sliderSFXVolumeSlider");
            m_sliderMusicVolumeSlider = FindChildComponent<Slider>("Root/Panel/MusicVolumeSlider/m_sliderMusicVolumeSlider");
            m_btnChineseSimplified = FindChildComponent<Button>("Root/Panel/Languages/m_btnChineseSimplified");
            m_btnEnglish = FindChildComponent<Button>("Root/Panel/Languages/m_btnEnglish");
            m_btnBack.onClick.AddListener(OnClickBackBtn);
            m_sliderMasterVolumeSlider.onValueChanged.AddListener(OnSliderMasterVolumeSliderChange);
            m_sliderSFXVolumeSlider.onValueChanged.AddListener(OnSliderSFXVolumeSliderChange);
            m_sliderMusicVolumeSlider.onValueChanged.AddListener(OnSliderMusicVolumeSliderChange);
            m_btnChineseSimplified.onClick.AddListener(OnClickChineseSimplifiedBtn);
            m_btnEnglish.onClick.AddListener(OnClickEnglishBtn);
        }

        #endregion

        #region 事件

        private void OnClickBackBtn()
        {
            GameEvent.Send(MainMenuEvent.OpenMenu);
        }

        private void OnSliderMasterVolumeSliderChange(float value)
        {
            GameModule.Audio.MusicVolume = value;
            GameModule.Save.SetFloat(AudioKey.MusicVolume, value);
        }

        private void OnSliderSFXVolumeSliderChange(float value)
        {
            GameModule.Audio.SoundVolume = value;
            GameModule.Save.SetFloat(AudioKey.SoundVolume, value);
        }

        private void OnSliderMusicVolumeSliderChange(float value)
        {
            GameModule.Audio.UISoundVolume = value;
            GameModule.Save.SetFloat(AudioKey.UISoundVolume, value);
        }

        private void OnClickConfirmButtonBtn()
        {
        }

        private void OnClickCancelButtonBtn()
        {
        }

        private void OnClickChineseSimplifiedBtn()
        {
            GameModule.Localization.SetLanguage(Language.ChineseSimplified);
        }

        private void OnClickEnglishBtn()
        {
            GameModule.Localization.SetLanguage(Language.English);
        }

        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            // 初始化音量滑块
            m_sliderMasterVolumeSlider.value = GameModule.Audio.MusicVolume;
            m_sliderSFXVolumeSlider.value = GameModule.Audio.SoundVolume;
            m_sliderMusicVolumeSlider.value = GameModule.Audio.UISoundVolume;
        }
    }
}