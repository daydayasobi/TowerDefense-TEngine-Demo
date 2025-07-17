# TowerDefense-TEngine-Demo

## 简介

​	该项目是在[TEngine框架](https://github.com/Alex-Rachel/TEngine)的基础上，使用Unity开源资源，大量参考开源项目“[基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)”制作的一款塔防Demo。

​	TE框架是一款非常优秀的Unity开源框架，目前相关的文档还不够完善，Demo也比较少。所以本着学习TE框架的心态，效仿花桑大佬复刻了一个基于TE框架的塔防Demo，同时记录一下自己在使用TE框架心得和遇到的坑，希望能给后来人一些经验和启发。

## 版本信息

* Unity: 2022.3.10f1

* TEngine: 2025.5.20

* YooAsset: 2.3.8

* HybridCLR: 7.6.0

* Tower Defense Template: 1.4.1

## 项目内容

​	本项目测试了了Windows打包运行和热更新，运用到了TE框架里大部分的模块，包括资源模块、事件模块、UI模块、流程模块、FSM模块、多语言模块、内存池和对象池模块。使用了Luban配置表工具、YooAsset打包工具和HybridCLR热更新工具。后续可能会出TE框架的教程，到时候会一并贴出。

  ### 文件结构

xxxxx

### 待完善的地方

* 实体模块要更换，在写这个项目的时候TE框架还没有更新实体模块，所以照着GF框架自己添加了一个，仅供参考，后续会换成TE里的实体模块。
* 多语言
* 存档模块需要完善，现在手写的存档模块很简陋，后续会修改。
* 内存池引用池运用需要完善，现在的用法比较除暴，对象池主要用的还是TE打飞机框架中的一个脚本，后续会优化一下。

## 相关引用

* [基于Unity开源框架GameFramewrk实现的一款塔防游戏Demo](https://github.com/DrFlower/TowerDefense-GameFramework-Demo)
* [用te框架仿照gf的框架的重写](https://gitee.com/tuzhong_w/tower-defense-tengine-demo)
* [TEngine框架](https://github.com/Alex-Rachel/TEngine)
* [Luban](https://github.com/focus-creative-games/luban)
* [Luban配置教程和按文件加载优化](https://blog.meo39.com/2025/06/09/lubanLean1/)
