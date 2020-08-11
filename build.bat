@ECHO OFF
echo Started build %date% %time%

echo Building Windows distributable
rmdir /s /q Builds\Windows
mkdir Builds\Windows
start /b /wait "dummy" "C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity" -batchmode -quit -projectPath FamiliarQuestMainGame -buildTarget Win -buildWindowsPlayer "..\Builds\Windows\FamiliarQuest.exe"
echo Building Linux distributable
rmdir /s /q Builds\Linux
mkdir Builds\Linux
start /b /wait "dummy" "C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity" -batchmode -quit -projectPath FamiliarQuestMainGame -buildTarget Linux64 -buildLinux64Player "..\Builds\Linux\FamiliarQuest"
echo Building Mac distributable
rmdir /s /q Builds\Mac
mkdir Builds\Mac
start /b /wait "dummy" "C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity" -batchmode -quit -projectPath FamiliarQuestMainGame -buildTarget OSXUniversal -buildOSXUniversalPlayer "..\Builds\Mac\FamiliarQuest.app"

butler push --userversion-file version.txt Builds\Windows caesuric/familiar-quest:windows
butler push --fix-permissions --userversion-file version.txt Builds\Linux caesuric/familiar-quest:linux
butler push --fix-permissions --userversion-file version.txt Builds\Mac caesuric/familiar-quest:osx

set /p version=<version.txt
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestWindows.v%version%.zip Builds\Windows
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestLinux.v%version%.zip Builds\Linux
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestMac.v%version%.zip Builds\Mac

C:\Python38-32\python github-release.py

echo  Finished build %date% %time%
