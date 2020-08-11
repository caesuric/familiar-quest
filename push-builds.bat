@ECHO OFF
butler push --userversion-file version.txt Builds\Windows caesuric/familiar-quest:windows
butler push --fix-permissions --userversion-file version.txt Builds\Linux caesuric/familiar-quest:linux
butler push --fix-permissions --userversion-file version.txt Builds\Mac caesuric/familiar-quest:osx
set /p version=<version.txt
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestWindows.v%version%.zip Builds\Windows
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestLinux.v%version%.zip Builds\Linux
"C:\Program Files\7-Zip\7z.exe" a -mx5 -y Builds\FamiliarQuestMac.v%version%.zip Builds\Mac
C:\Python38-32\python github-release.py
