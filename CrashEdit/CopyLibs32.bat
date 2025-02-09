@ECHO OFF
echo %CD%
MKDIR Win32
COPY /Y ..\..\..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x86 Win32
echo Finished... 
