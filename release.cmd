@pushd %~dp0

set PRODVER=%1
set CONFIG_VAL=Release
set OUTPUT_DIR=%~dp0src\Berp\bin\%CONFIG_VAL%\NuGet
set UNSIGNED_OUTPUT_DIR=%OUTPUT_DIR%\.unsigned
set PACKAGE_FILENAME=Berp.%PRODVER%.nupkg

@echo publishing version %PRODVER%, %CONFIG%, OK?
@pause

cd src

dotnet build -c "%CONFIG_VAL%" -p:Version=%PRODVER%
IF ERRORLEVEL 1 GOTO error

@ECHO *** signing NuGet package ***

@mkdir %UNSIGNED_OUTPUT_DIR%
COPY %OUTPUT_DIR%\%PACKAGE_FILENAME% %UNSIGNED_OUTPUT_DIR%\%PACKAGE_FILENAME%
@IF ERRORLEVEL 1 GOTO error

dotnet nuget sign %UNSIGNED_OUTPUT_DIR%\%PACKAGE_FILENAME% --certificate-subject-name "Spec Solutions Kft."  --timestamper http://timestamp.sectigo.com --output %OUTPUT_DIR%\
@IF ERRORLEVEL 1 GOTO error

@echo *** publish NuGet packages ***

copy /Y %OUTPUT_DIR%\%PACKAGE_FILENAME% %NUGET_LOCAL_FEED%\%PACKAGE_FILENAME%
@IF ERRORLEVEL 1 GOTO error

@cd %OUTPUT_DIR%
dotnet nuget push %PACKAGE_FILENAME% -k %NUGET_PUBLISH_KEY% -s https://api.nuget.org/v3/index.json --no-symbols --skip-duplicate
@IF ERRORLEVEL 1 GOTO error

@popd
goto end

:error
@echo ERROR!!!
@popd
exit /b

:end
