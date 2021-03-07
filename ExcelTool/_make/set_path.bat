rem ---- SET SOME PATH
rem current path
call config.bat
pushd ..
set PROJ_DIR=%cd%
popd

set TOOLS_PRO_PATH=%PROJ_DIR%\_tools
set DATAEXPORTER=%TOOLS_PRO_PATH%\DataFileExporter\DataFileExporter.exe

rem used for Resource Project
set RESOURCE_NUMERIC=%PROJ_DIR%\numeric

set DATAEXPORTER=%TOOLS_PRO_PATH%\DataFileExporter\DataFileExporter.exe