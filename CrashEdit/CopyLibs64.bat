@ECHO OFF
echo %CD%
MKDIR Win64
COPY /Y ..\..\..\..\packages\SharpFont.Dependencies.2.6\bin\msvc12\x64 Win64
echo Finished... 
