@echo off
setlocal enabledelayedexpansion

set EXPORT_ENCODE=%1
set CSV_NAME=%2
set KEY_TYPE=%3

echo ...%CSV_NAME%...
%DATAEXPORTER% -encoding %EXPORT_ENCODE% -src %CD%\GameData\%CSV_NAME%.csv -dataName %CSV_NAME%Data -dataDes %CD%\GameData\%CSV_NAME%_Description_Client.xml -output %CD%\export_client\%CSV_NAME%Data.bytes -keyType %3

endlocal