set WORKSPACE=.
set LUBAN_ROOT=..
set ASSET_ROOT=..\..
set LUBAN_DLL=%LUBAN_ROOT%\Tools\Luban\Luban.dll
set DATA_OUTPATH=%LUBAN_ROOT%/Server/GameConfig 
set CODE_OUTPATH=%LUBAN_ROOT%/Server/Hotfix/Config/GameConfig

dotnet %LUBAN_DLL% ^
    -t server^
    -c cs-bin ^
    -d bin^
    --conf %WORKSPACE%\luban.conf ^
    -x outputCodeDir=%CODE_OUTPATH% ^
    -x outputDataDir=%DATA_OUTPATH% 
pause

