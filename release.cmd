@pushd %~dp0

set PRODVER=%1
set CONFIG_VAL=Release

@echo publishing version %PRODVER%, %CONFIG%, OK?
@pause

cd src

dotnet build -c "%CONFIG_VAL%" -p:Version=%PRODVER%
IF ERRORLEVEL 1 GOTO error

copy /Y Berp\bin\%CONFIG_VAL%\NuGet\Berp.%PRODVER%.nupkg %NUGET_LOCAL_FEED%\Berp.%PRODVER%.nupkg
IF ERRORLEVEL 1 GOTO error

@popd
goto end

:error
echo ERROR!!!
@popd
exit /b

:end
