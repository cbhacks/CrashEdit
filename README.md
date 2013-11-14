# CrashEdit #
This program is an application designed for modifying the game files of the original Crash Bandicoot video game trilogy.

## Supported Games ##
_Note that CrashEdit does not work directly with ISO's, but rather with the NSF files stored on the game discs._

* `SCUS-94900` Crash Bandicoot __(read only)__ __(experimental)__
* `SCES-?????` Crash Bandicoot __(read only)__ __(experimental)__ __(untested)__
* `????-?????` クラッシュバンディクー __(read only)__ __(experimental)__ __(untested)__
* `US BETA 96/03/08` Crash Bandicoot _"Prototype"_ __(no nsd patching)__ __(experimental)__
* `US BETA 96/05/11` Crash Bandicoot _"E3 Demo"_ __(no nsd patching)__ __(experimental)__
* `SCUS-94154` Crash Bandicoot 2: Cortex Strikes Back
* `SCES-00967` Crash Bandicoot 2: Cortex Strikes Back
* `SCPS-10047` クラッシュバンディクー　2:　コルテックスのぎゃくしゅう！ __(incomplete support)__
* `EU BETA 97/09/14` Crash Bandicoot 2: Cortex Strikes Back _"Review Copy"_
* `SCUS-94244` Crash Bandicoot: Warped
* `SCES-?????` Crash Bandicoot 3: Warped __(untested)__
* `????-?????` クラッシュバンディクー　3:　ブッとび！　世界一周 __(untested)__

## Usage ##
_For users acquainted with Microsoft Windows, "directories" are commonly referred to as "folders" on windows. When the term "directory" is used here, think "folder"._

Some basic knowledge of how Crash Bandicoot game files are laid out is necessary to use this application. CrashEdit is not designed to work directly on disc images (aka ISO's), but instead on files with the "NSF" file format. This is a custom format created by Naughty Dog and used in the original Crash Bandicoot trilogy (not including CTR).

First, a Crash Bandicoot game CD will have a root directory with contents similar to the following:

* `S0` _(directory)_
* `S1` _(directory)_
* `S2` _(directory)_
* `S3` _(directory)_
* `SYSTEM.CNF` _(playstation game boot configuration file)_
* `SCUS_949.00` _(playstation game exe file)_

Within the S0/S1/etc directories you will find files named similar to the following:

* `S0000013.NSD` _(nsd file)_
* `S0000013.NSF` _(nsf file)_
* `S0000014.NSD` _(nsd file)_
* `S0000014.NSF` _(nsf file)_
* `S0000015.NSD` _(nsd file)_
* `S0000015.NSF` _(nsf file)_
* ...

Notice how each filename has an NSD/NSF pair. Each pair corresponds to a specific level in the game, and these files contain the game data for that level. The last two characters before the .NSD or .NSF extension are the "level ID" for that specific level. As an example, the level titled _The Lost City_ has the level ID `20`, and its file pair is the following, found in the `S2` directory:

* `S0000020.NSD` _(nsd file)_
* `S0000020.NSF` _(nsf file)_

A complete list of level ID's and their associated levels can be found here:

https://sites.google.com/site/crashpsxsite/structure

The NSF file contains the actual game data for the level and is what CrashEdit is designed to read and manipulate. An NSF file consists of __entries__. Each entry has a 5-character name, and represents a game asset such as a sound effect or 3d model. The following entry types are recognized and supported by CrashEdit:

* __Old Scenery Entry:__ _(For crash 1)_ One section of the level's scenery 3D model.
* __Entity Entry:__ Describes one level "zone", including objects in that zone as well as the zone's camera configuration.
* __Sound Entry:__ A sound effect. This entry only contains the raw sound data without any metadata such as the sample rate.
* __Music Entry:__ A set of music tracks in SEQ format (very similar to MIDI format), and possibly the associated VH file (wavebank header file). Each level zone will refer to a single music entry which will be used for playback while the camera is in that zone.
* __Wavebank Entry:__ Part of the level's wavebank data (VB file). Crash music is in MIDI format, but does not use General MIDI (GM) instruments. Instead, a custom instrument set is used for each level theme. The audio data for this instrument set (wavebank) is very large, so it must be split into multiple entries (up to a maximum of 7).
* __Speech Entry:__ Similar to the _sound entry_, but localized (supporting multiple languages). Long dialogue is often split up into multiple of these entries due to size constraints.

Entries are organized into containers which are referred to as __chunks__. Each chunk is 64 KB in size, and so it cannot contain more than 64 KB of entry data. _(If you attempt to save an NSF file which has a chunk containing more than 64 KB of entry data, a **packing error** will occur and the save operation will fail.)_ There are different types of chunks: the _normal_ type and special audio types. _(As a general rule, you should keep audio-related entries in their proper chunk types or else the playstation will be unhappy.)_ There is also a special chunk type, __Texture Chunk__, which contains raw texture data instead of entries.

The NSD file contains various data used to assist the game in properly accessing the NSF files. Included in the NSD file is a table mapping entries to chunks. If you add chunks, delete chunks, add entries, delete entries, move entries to other chunks, rename entries, or reorder chunks, you will need to update this table. CrashEdit can automatically patch this table with the _Patch NSD_ button.

## System Requirements ##
_Aside from the obvious monitor, keyboard, and mouse. However, a mouse scroll wheel is not required._
* .NET Framework 2.0 or Mono
* OpenTK 1.0
* Preferably at least 60 MB of physical memory available to the application or you may encounter thrashing while loading or saving large files

## Known Issues ##

### Incomplete Features ###

#### Significant Issues ####
* __All Games:__ The application does not report _which_ chunk a packing error occured on when attempting to save an NSF file.
* __Crash 1 _All_:__ "Patch NSD" does not work.
* __Crash 1 _Retail_:__ Saving NSF files is not yet properly supported.
* __Crash 2 _NTSC-J Retail_:__ Most levels contain special not-yet-supported speech entries.

#### Insignificant Issues ####
_These issues have no significant effect on the operation of the program, but are still technically issues with the program. This list can be safely ignored by most users._
* __All Games:__ Any data hidden within unused sections of NSF files may be ignored by the program without warning, and will not be preserved.
* __Crash 1 _All_:__ Music entries containing VH data may be saved out with different data in unused sections.
* __Crash 2 _NTSC-J Retail_:__ A music entry in `S000003C.NSF` containing VH data may be saved out with different data in unused sections.

### Bugs ###
* __All Games:__ Zooming in completely on a 3d viewer will cause intense Z-fighting.

### Broken Game Files ###
* __Crash 1 _All_:__ `S0000002.NSF`, if present, is an older-format file which is not supported.
* __Crash 1 _All_:__ `S0000010.NSF`, if present, is an older-format file which is not supported.
* __Crash 1 _Retail_:__ `S0000038.NSF` contains a music entry with an incorrect SEP track count.
* __Crash 1 _Retail_:__ `S0000004.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Retail_:__ `S000000B.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Retail_:__ `S000000D.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Beta MAY11_:__ `S000001C.NSF` contains two demo entries with incorrect magic numbers.

### Mono-specific Issues ###
_These are issues you may encounter if you are using Mono instead of the .NET Framework to run this application._
* Opening a texture chunk invokes an unimplemented feature in Mono which will crash the application. If you are building from source, you may work around this by commenting out the `CreateEditor()` method in `CrashEdit/Controllers/TextureChunkController.cs`. You may grep for `// MONO USERS` to find the specific method.

## Installation ##
Just unzip into a directory and run the exe.

_If you are working with the source code, it is a VC# 2005 Express Edition solution which consists of two projects, "Crash" and "CrashEdit". You will need to set "CrashEdit" as the startup project to run the application under the VS debugger._

## Where To Get ##
Precompiled binary files (EXE files) can be acquired from a public dropbox directory here:  
https://www.dropbox.com/sh/yv93g4wsdde32s3/NuCSJR37Oo

Source code is available as a git repository on github at:  
https://github.com/ughman/CrashEdit

## Author Contact Info ##
I can be contacted in various ways:

* As `chekwob@yahoo.com` via email __(If your email is not in english, please include "CrashEdit" in the email subject line so I know it's not spam)__
* As `chekwob` on YouTube
* As `chekwob` on the XeNTaX forum
* As `chekwob` on the CM forum (hpzr.proboards.com)
* As `ughman` on github (this is not checked very often)
