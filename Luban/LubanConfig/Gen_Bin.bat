set WORKSPACE=.
set LUBAN_ROOT=..
set ASSET_ROOT=..\..
set LUBAN_DLL=%LUBAN_ROOT%\Tools\Luban\Luban.dll
set DATA_OUTPATH=%ASSET_ROOT%\Assets\GameRes\BytesConfig\
set CODE_OUTPATH=%ASSET_ROOT%\Assets\Scripts\GameMain\BytesConfig\

#xcopy /s /e /i /y "%WORKSPACE%\CustomTemplate\ConfigSystem.cs" "%ASSET_ROOT%\Assets\Scripts\GameMain\BytesConfig\ConfigSystem.cs"

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin^
    --conf %WORKSPACE%\luban.conf ^
    -x outputCodeDir=%CODE_OUTPATH% ^
    -x outputDataDir=%DATA_OUTPATH% 

pause