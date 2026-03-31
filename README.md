# TowerDefense-TEngine-Demo

## 简介

该项目是在[TEngine框架](https://github.com/Alex-Rachel/TEngine)的基础上，使用Unity开源资源，大量参考开源项目“[基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)”制作的一款塔防Demo。

TE框架是一款非常优秀的Unity开源框架，目前相关的文档还不够完善，Demo也比较少。所以本着学习TE框架的心态，效仿花桑大佬复刻了一个基于TE框架的塔防Demo，同时记录一下自己在使用TE框架心得和遇到的坑，希望能给后来人一些经验和启发。

## 版本信息

* Unity: 2022.3.10f1

* TEngine 基线: `Alex-Rachel/TEngine main@ccc51c3ac7f1a7e7761e85a1c2d1adab8a487962`（2026-01-13）

* TEngine release 对照: `TEngine6.1.4@9d7b5d9`

* TEngine `Assets/TEngine/package.json` 标记版本: `6.0.0`

* YooAsset: 2.3.17（`Packages/YooAsset` 内嵌包）

* HybridCLR: 8.8.0（`https://github.com/focus-creative-games/hybridclr_unity.git#4df417e56ad827cd7e70a54662f0e8bab90aafe9`）

* UniTask: 2.5.10

* Package 兼容差异：因项目固定 `Unity 2022.3.10f1`，移除 upstream `manifest` 里的 `com.unity.modules.accessibility`，并将 `com.unity.ugui` 对齐到当前编辑器实际提供的 `1.0.0`

* Tower Defense Template: 1.4.1

## 项目内容

本项目测试了了Windows打包运行和热更新，运用到了TE框架里大部分的模块，包括资源模块、事件模块、UI模块、流程模块、FSM模块、多语言模块、内存池和对象池模块。使用了Luban配置表工具、YooAsset打包工具和HybridCLR热更新工具。后续可能会根据该项目出一些TE框架的教程，到时候会一并贴出。

  ### 文件结构
<pre>
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
│  └─ Extension
│     └─ TowerDefenseCompat    // 项目兼容层（Entity / Save / Resource）
├─ Packages
│  ├─ YooAsset                 // YooAsset 内嵌包
│  └─ UniTask                  // UniTask 内嵌包
</pre>

## 升级与兼容层说明

### TEngine 升级策略

本项目当前不再以 `Assets/TEngine/package.json` 的 `6.0.0` 作为版本依据，而是以 **上游 commit pin** 作为框架版本依据。

本次升级基线固定为：

* `Alex-Rachel/TEngine main@ccc51c3ac7f1a7e7761e85a1c2d1adab8a487962`

这是一个主线提交，不是 release tag。为了便于和上游 release 对齐，README 同时记录了最近的 release 对照 `TEngine6.1.4@9d7b5d9`。

HybridCLR 源地址与上游略有不同：上游 `manifest.json` 使用的是 Gitee 地址，但本项目为了保持现有拉取方式，继续使用 GitHub 地址，并且将 commit 精确 pin 到与上游一致的 `4df417e`。

由于项目固定在 `Unity 2022.3.10f1`，Package Manager 依赖不完全照搬 upstream `main`：`com.unity.modules.accessibility` 已移除，`com.unity.ugui` 以当前编辑器可解析的 built-in 版本 `1.0.0` 为准。

### 默认标签下载 / 关卡按 tag 下载

这两个功能仍然保留，玩家侧行为没有变化：

* 进入游戏时仍然只下载默认标签资源，当前默认标签仍为 `Base`
* 进入关卡选择后，仍然会按照关卡配置下载 `Level3`、`Level4` 等按 tag 拆分的资源

但是业务调用入口已经从旧 fork 的 vendor 接口迁移到兼容层：

* 默认标签下载：`ResourceDownloadCompat.CreateDefaultPackageDownloader()`
* 关卡按 tag 下载：`ResourceDownloadCompat.CreateResourceDownloaderByTags(new[] { tag }, packageName)`
* 默认标签配置资产：`Assets/TEngine/Extension/TowerDefenseCompat/Resources/GameResourceDownloadSettings.asset`

不再继续使用旧版客制化接口：

* `IResourceModule.DefaultTagName`
* `IResourceModule.CreateResourceDownloader(string[] tags, string packageName)`
* `IResourceModule.IsTagResourcesExist(...)`

### 场景与模块接口变化

为了保持 `Assets/TEngine` 尽量接近上游，原先直接改 vendor 接口的做法已经收口到 extension 兼容层：

* 分包场景加载：由旧的 `GameModule.Scene.LoadSceneAsync(packageName, location)` 改为 `PackageSceneLoader.LoadSceneAsync(packageName, location)`
* `IEntityModule` / `ISaveModule` 不再放在 vendor `Runtime/Module` 下，迁移到 `Assets/TEngine/Extension/TowerDefenseCompat`
* 兼容层命名空间仍然保持 `TEngine`，因此业务层类型名基本不需要改

为保证 extension 程序集里的 `EntityModule` 和 `SaveModule` 仍能被 `ModuleSystem.GetModule<T>()` 自动创建，`ModuleSystem` 增加了跨程序集查找 fallback。

### 兼容层位置与职责

兼容层现在统一位于：

* `Assets/TEngine/Extension/TowerDefenseCompat/Entity`
* `Assets/TEngine/Extension/TowerDefenseCompat/Save`
* `Assets/TEngine/Extension/TowerDefenseCompat/Resource`

设计原则：

* `Assets/TEngine` 主体保留上游 vendor 代码
* 项目定制尽量只放在 `Extension/TowerDefenseCompat`
* 兼容层不依赖 `GameModule` 或 `GameLogic` 程序集，便于后续继续跟随上游升级

### 注意事项

![image-20250718113248518](https://blogimage01.oss-cn-chengdu.aliyuncs.com/img/202507181132652.png)

工程里已经包含了配置表和Luban的依赖，如上图所示点击`Gen_Bin-Custom.bat`即可生成配置文件和代码到工程中。

项目保留了分包下载逻辑，进入游戏只有下载`Base`标签的资源，进入关卡选择后会根据表格下载`Level3`、`Level4`等关卡资源。当前这套逻辑已经迁移到 `Assets/TEngine/Extension/TowerDefenseCompat/Resource`，不再继续侵入修改 vendor `IResourceModule`。

存档模块仍然使用 Sqlite 作为数据持久化方案，但实现已经迁移到 `Assets/TEngine/Extension/TowerDefenseCompat/Save`，不再放在 vendor `TEngine/Runtime/Module/SaveModule`。

### 待完善的地方

* 实体模块要优化，当前的实体模块是基于GF框架移植的，目前效果只能说差强人意，仅供参考，后续会进行优化。
* ~~存档模块需要完善，现在用的存档模块很简陋，后续会修改。~~（已添加）
* 内存池引用池运用存在问题，现在的用法比较除暴，对象池用的还是TE打飞机框架中的脚本，后续会优化一下。
* 没有运用到服务器和网络功能，后续会考虑添加。

## 相关引用

* [基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)
* [用te框架仿照gf的框架的重写](https://gitee.com/tuzhong_w/tower-defense-tengine-demo)
* [TEngine框架](https://github.com/Alex-Rachel/TEngine)
* [Luban](https://github.com/focus-creative-games/luban)
* [Luban配置教程和按文件加载优化](https://blog.meo39.com/2025/06/09/lubanLean1/)

