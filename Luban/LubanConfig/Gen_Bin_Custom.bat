set WORKSPACE=.
set LUBAN_ROOT=..
set ASSET_ROOT=..\..
set LUBAN_DLL=%LUBAN_ROOT%\Tools\Luban\Luban.dll
set DATA_OUTPATH=%ASSET_ROOT%\Assets\AssetRaw\Configs\bytes\
set CODE_OUTPATH=%ASSET_ROOT%\Assets\GameScripts\HotFix\GameProto\GameConfig\

#xcopy /s /e /i /y "%WORKSPACE%\CustomTemplate\ConfigSystem.cs" "%ASSET_ROOT%\Assets\GameScripts\HotFix\GameProto\ConfigSystem.cs"

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin^
    --conf %WORKSPACE%\luban.conf ^
    --customTemplateDir %WORKSPACE%\CustomTemplate\CustomTemplate_Client_LazyLoad ^
    -x outputCodeDir=%CODE_OUTPATH% ^
    -x outputDataDir=%DATA_OUTPATH% 
pause

