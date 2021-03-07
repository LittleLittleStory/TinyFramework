@echo off
setlocal enabledelayedexpansion

echo ...Export Numeric for Client...

set EXPORT_TYPE=%1
set LANGUAGE_TYPE=%2

echo ---------------- call path ----------------------------
pushd ..\_make
call set_path.bat
popd

if exist export_client_tmp (
	rd /s/q export_client_tmp
)
mkdir export_client_tmp

Call %DATAEXPORTER% -start

rem 导出csv keyType OneID TwoID ThreeID StrID
Call Export_Client_Data.bat UTF-8 AllianceManage OneID

rem copy /y %RESOURCE_NUMERIC%\export_client\*.bin %DATA_CLIENT_PATH% > NUL

Call %DATAEXPORTER% -end

xcopy export_client_tmp export_client /E /Y > NUL 

endlocal
pause