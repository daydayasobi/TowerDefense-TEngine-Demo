# TowerDefense-TEngine-Demo

## 简介

该项目是在[TEngine框架](https://github.com/Alex-Rachel/TEngine)的基础上，使用Unity开源资源，大量参考开源项目“[基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)”制作的一款塔防Demo。

TE框架是一款非常优秀的Unity开源框架，目前相关的文档还不够完善，Demo也比较少。所以本着学习TE框架的心态，效仿花桑大佬复刻了一个基于TE框架的塔防Demo，同时记录一下自己在使用TE框架心得和遇到的坑，希望能给后来人一些经验和启发。

## 版本信息

* Unity: 2022.3.10f1

* TEngine: 2025.5.20

* YooAsset: 2.3.8

* HybridCLR: 7.6.0

* Tower Defense Template: 1.4.1

## 项目内容

本项目测试了了Windows打包运行和热更新，运用到了TE框架里大部分的模块，包括资源模块、事件模块、UI模块、流程模块、FSM模块、多语言模块、内存池和对象池模块。使用了Luban配置表工具、YooAsset打包工具和HybridCLR热更新工具。后续可能会根据该项目出一些TE框架的教程，到时候会一并贴出。

  ### 文件结构

├─ AssetArt                    // 美术产出物（非运行时直接引用）
│  └─ Atlas                    // 图集原始文件（TexturePacker 等导出的 .tps / .png）
├─ AssetRaw                    // 原始资源（策划/美术直接维护）
│  ├─ Actor                    // 角色/机关/塔等实体的预制体
│  ├─ Audios                   // 音频
│  ├─ Configs                  // 策划配置
│  │  ├─ bytes                 //   Luban生成的二进制表
│  │  └─ Localization          //   I2 Localization 用的多语言CSV
│  ├─ DLL                      // 第三方托管 DLL（LitJson、Luban.Runtime 等）
│  ├─ Effects                  // 特效源文件（.prefab / .fbx / .shadergraph）
│  ├─ Fonts                    // 字体
│  ├─ Materials                // 材质球（sharedMaterial）
│  ├─ Res                      // 运行时真正打进包体的资源（Animations、Models、Particles、Scenes…）
│  ├─ Scenes                   // 主工程 Scenes（实际 Build 用）
│  ├─ Shaders                  // 着色器源文件
│  ├─ UI                       // UI Prefab（运行时动态加载）
│  └─ UIRaw                    // UI 美术源文件（PSD、切图、未打图集）
├─ Editor                      // 纯编辑器扩展脚本
├─ GameScripts                 // 业务代码（热更 + 原生）
│  ├─ HotFix                  // 游戏热更程序集目录
│  │  ├─ GameLogic            //   游戏业务逻辑程序集
│  │  └─ GameProto            //   游戏配置协议程序集
│  └─ Procedure               //   原生程序集里的流程入口
├─ HybridCLRGenerate           // HybridCLR 生成 Wrappers & AOT dll 的临时目录
├─ Launcher                    // 原生启动场景
├─ MobileDependencyResolver    // Google Play Resolver 插件
├─ Scenes                      // 真正 Build Settings 里挂的入口场景
├─ StreamingAssets             // 随包体拷贝的只读资源
├─ TEngine                     // TEngine 框架本体

### 注意事项

![image-20250718113248518](https://blogimage01.oss-cn-chengdu.aliyuncs.com/img/202507181132652.png)

工程里已经包含了配置表和Luban的依赖，如上图所示点击`Gen_Bin-Custom.bat`即可生成配置文件和代码到工程中。

### 待完善的地方

* 实体模块要更换，在写这个项目的时候TE框架还没有更新实体模块，所以照着GF框架自己添加了一个，仅供参考，后续会换成TE里的实体模块。
* 存档模块需要完善，现在用的存档模块很简陋，后续会修改。
* 内存池引用池运用存在问题，现在的用法比较除暴，对象池用的还是TE打飞机框架中的脚本，后续会优化一下。
* 资源分包、按关卡下载的功能还没有实现，后续会添加。
* 没有运用到服务器和网络功能，后续会考虑添加。

## 相关引用

* [基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)
* [用te框架仿照gf的框架的重写](https://gitee.com/tuzhong_w/tower-defense-tengine-demo)
* [TEngine框架](https://github.com/Alex-Rachel/TEngine)
* [Luban](https://github.com/focus-creative-games/luban)
* [Luban配置教程和按文件加载优化](https://blog.meo39.com/2025/06/09/lubanLean1/)

