IF NOT EXIST "C:\Users\%USERPROFILE%\db" MKDIR C:\Users\%USERPROFILE%\db
XCOPY /s "%~dp0Data\Database_Backup\Games.sdb" "C:\Users\%USERPROFILE%\db\"