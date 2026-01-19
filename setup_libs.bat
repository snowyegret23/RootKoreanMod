@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

echo ============================================
echo   RootKoreanMod 라이브러리 설정 스크립트
echo ============================================
echo.

:: MelonLoader 설정
echo [MelonLoader]
set /p MELON_PATH="MelonLoader가 설치된 게임 경로를 입력하세요 (건너뛰려면 Enter): "

if not "!MELON_PATH!"=="" (
    if not exist "!MELON_PATH!\MelonLoader" (
        echo 오류: MelonLoader 폴더를 찾을 수 없습니다: !MELON_PATH!\MelonLoader
        goto :bepinex
    )

    echo MelonLoader 라이브러리 복사 중...

    if not exist "Lib\MelonLoader" mkdir "Lib\MelonLoader"

    :: MelonLoader 핵심 DLL들
    if exist "!MELON_PATH!\MelonLoader\net6\MelonLoader.dll" (
        copy /Y "!MELON_PATH!\MelonLoader\net6\MelonLoader.dll" "Lib\MelonLoader\" > nul
        echo   - MelonLoader.dll 복사 완료
    ) else if exist "!MELON_PATH!\MelonLoader\MelonLoader.dll" (
        copy /Y "!MELON_PATH!\MelonLoader\MelonLoader.dll" "Lib\MelonLoader\" > nul
        echo   - MelonLoader.dll 복사 완료
    )

    :: Il2Cpp 관련 DLL들
    if exist "!MELON_PATH!\MelonLoader\net6\Il2CppInterop.Runtime.dll" (
        copy /Y "!MELON_PATH!\MelonLoader\net6\Il2CppInterop.Runtime.dll" "Lib\MelonLoader\" > nul
        echo   - Il2CppInterop.Runtime.dll 복사 완료
    )

    :: Il2Cpp 어셈블리들
    if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies" (
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.CoreModule.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.CoreModule.dll" "Lib\MelonLoader\" > nul
            echo   - UnityEngine.CoreModule.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.UI.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.UI.dll" "Lib\MelonLoader\" > nul
            echo   - UnityEngine.UI.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.TextRenderingModule.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.TextRenderingModule.dll" "Lib\MelonLoader\" > nul
            echo   - UnityEngine.TextRenderingModule.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.AssetBundleModule.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.AssetBundleModule.dll" "Lib\MelonLoader\" > nul
            echo   - UnityEngine.AssetBundleModule.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.IMGUIModule.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\UnityEngine.IMGUIModule.dll" "Lib\MelonLoader\" > nul
            echo   - UnityEngine.IMGUIModule.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll" "Lib\MelonLoader\" > nul
            echo   - Assembly-CSharp.dll 복사 완료
        )
        if exist "!MELON_PATH!\MelonLoader\Il2CppAssemblies\Il2Cppmscorlib.dll" (
            copy /Y "!MELON_PATH!\MelonLoader\Il2CppAssemblies\Il2Cppmscorlib.dll" "Lib\MelonLoader\" > nul
            echo   - Il2Cppmscorlib.dll 복사 완료
        )
    )

    echo MelonLoader 라이브러리 설정 완료!
) else (
    echo MelonLoader 설정을 건너뜁니다.
)

:bepinex
echo.

:: BepInEx 설정
echo [BepInEx]
set /p BEPINEX_PATH="BepInEx가 설치된 게임 경로를 입력하세요 (건너뛰려면 Enter): "

if not "!BEPINEX_PATH!"=="" (
    if not exist "!BEPINEX_PATH!\BepInEx" (
        echo 오류: BepInEx 폴더를 찾을 수 없습니다: !BEPINEX_PATH!\BepInEx
        goto :done
    )

    echo BepInEx 라이브러리 복사 중...

    if not exist "Lib\BepInEx" mkdir "Lib\BepInEx"

    :: BepInEx 핵심 DLL들
    if exist "!BEPINEX_PATH!\BepInEx\core\BepInEx.Core.dll" (
        copy /Y "!BEPINEX_PATH!\BepInEx\core\BepInEx.Core.dll" "Lib\BepInEx\" > nul
        echo   - BepInEx.Core.dll 복사 완료
    )
    if exist "!BEPINEX_PATH!\BepInEx\core\BepInEx.Unity.IL2CPP.dll" (
        copy /Y "!BEPINEX_PATH!\BepInEx\core\BepInEx.Unity.IL2CPP.dll" "Lib\BepInEx\" > nul
        echo   - BepInEx.Unity.IL2CPP.dll 복사 완료
    )

    :: Il2CppInterop
    if exist "!BEPINEX_PATH!\BepInEx\core\Il2CppInterop.Runtime.dll" (
        copy /Y "!BEPINEX_PATH!\BepInEx\core\Il2CppInterop.Runtime.dll" "Lib\BepInEx\" > nul
        echo   - Il2CppInterop.Runtime.dll 복사 완료
    )

    :: Interop 어셈블리들
    if exist "!BEPINEX_PATH!\BepInEx\interop" (
        if exist "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.CoreModule.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.CoreModule.dll" "Lib\BepInEx\" > nul
            echo   - UnityEngine.CoreModule.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.UI.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.UI.dll" "Lib\BepInEx\" > nul
            echo   - UnityEngine.UI.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.TextRenderingModule.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.TextRenderingModule.dll" "Lib\BepInEx\" > nul
            echo   - UnityEngine.TextRenderingModule.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.AssetBundleModule.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.AssetBundleModule.dll" "Lib\BepInEx\" > nul
            echo   - UnityEngine.AssetBundleModule.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.IMGUIModule.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\UnityEngine.IMGUIModule.dll" "Lib\BepInEx\" > nul
            echo   - UnityEngine.IMGUIModule.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\Assembly-CSharp.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\Assembly-CSharp.dll" "Lib\BepInEx\" > nul
            echo   - Assembly-CSharp.dll 복사 완료
        )
        if exist "!BEPINEX_PATH!\BepInEx\interop\mscorlib.dll" (
            copy /Y "!BEPINEX_PATH!\BepInEx\interop\mscorlib.dll" "Lib\BepInEx\" > nul
            echo   - mscorlib.dll 복사 완료
        )
    )

    echo BepInEx 라이브러리 설정 완료!
) else (
    echo BepInEx 설정을 건너뜁니다.
)

:done
echo.
echo ============================================
echo   설정 완료!
echo ============================================
pause
