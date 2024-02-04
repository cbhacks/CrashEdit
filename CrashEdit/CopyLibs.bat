@ECHO OFF
echo %CD%
MKDIR Win64
MKDIR Win32
COPY /Y ..\..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x64 Win64
COPY /Y ..\..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x86 Win32
echo Finished... 
