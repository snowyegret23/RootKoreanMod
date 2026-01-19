@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ============================================
echo   RootKoreanMod Build Script
echo ============================================
echo.

set CONFIG=Release

if /i "%1"=="debug" set CONFIG=Debug
if /i "%1"=="release" set CONFIG=Release
if /i "%1"=="clean" goto :clean
if /i "%1"=="bepinex" goto :bepinex
if /i "%1"=="melonloader" goto :melonloader
if /i "%1"=="help" goto :help

:all
echo [빌드 설정: %CONFIG%]
echo.

echo [1/2] BepInEx 버전 빌드 중...
dotnet build RootKoreanMod.BepInEx\RootKoreanMod.BepInEx.csproj -c %CONFIG% --nologo -v q
if errorlevel 1 (
    echo [오류] BepInEx 빌드 실패
    goto :error
)
echo [완료] BepInEx 빌드 성공
echo.

echo [2/2] MelonLoader 버전 빌드 중...
dotnet build RootKoreanMod.MelonLoader\RootKoreanMod.MelonLoader.csproj -c %CONFIG% --nologo -v q
if errorlevel 1 (
    echo [오류] MelonLoader 빌드 실패
    goto :error
)
echo [완료] MelonLoader 빌드 성공
echo.

echo ============================================
echo   빌드 완료!
echo ============================================
echo.
echo 출력 파일:
echo   BepInEx:     RootKoreanMod.BepInEx\bin\%CONFIG%\net462\RootKoreanMod.dll
echo   MelonLoader: RootKoreanMod.MelonLoader\bin\%CONFIG%\net6.0\RootKoreanMod.dll
echo.
goto :end

:bepinex
echo [빌드 설정: %CONFIG%]
echo.
echo BepInEx 버전 빌드 중...
dotnet build RootKoreanMod.BepInEx\RootKoreanMod.BepInEx.csproj -c %CONFIG% --nologo
if errorlevel 1 goto :error
echo.
echo [완료] 출력: RootKoreanMod.BepInEx\bin\%CONFIG%\net462\RootKoreanMod.dll
goto :end

:melonloader
echo [빌드 설정: %CONFIG%]
echo.
echo MelonLoader 버전 빌드 중...
dotnet build RootKoreanMod.MelonLoader\RootKoreanMod.MelonLoader.csproj -c %CONFIG% --nologo
if errorlevel 1 goto :error
echo.
echo [완료] 출력: RootKoreanMod.MelonLoader\bin\%CONFIG%\net6.0\RootKoreanMod.dll
goto :end

:clean
echo 빌드 파일 정리 중...
if exist "RootKoreanMod.Shared\bin" rmdir /s /q "RootKoreanMod.Shared\bin"
if exist "RootKoreanMod.Shared\obj" rmdir /s /q "RootKoreanMod.Shared\obj"
if exist "RootKoreanMod.BepInEx\bin" rmdir /s /q "RootKoreanMod.BepInEx\bin"
if exist "RootKoreanMod.BepInEx\obj" rmdir /s /q "RootKoreanMod.BepInEx\obj"
if exist "RootKoreanMod.MelonLoader\bin" rmdir /s /q "RootKoreanMod.MelonLoader\bin"
if exist "RootKoreanMod.MelonLoader\obj" rmdir /s /q "RootKoreanMod.MelonLoader\obj"
echo [완료] 정리 완료
goto :end

:help
echo.
echo 사용법: build.bat [옵션]
echo.
echo 옵션:
echo   (없음)      모든 프로젝트 Release 빌드
echo   debug       모든 프로젝트 Debug 빌드
echo   release     모든 프로젝트 Release 빌드
echo   bepinex     BepInEx 버전만 빌드
echo   melonloader MelonLoader 버전만 빌드
echo   clean       빌드 파일 정리
echo   help        도움말 표시
echo.
goto :end

:error
echo.
echo [오류] 빌드 실패!
exit /b 1

:end
endlocal
