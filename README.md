# CrashEdit #
This program is an application designed for modifying the game files of the original Crash Bandicoot video game trilogy.

This is the __2.0__ version of the software. See `README-CE2.md` for the detailed readme on this updated version.

## Supported Games ##
_Note that CrashEdit does not work directly with ISO's, but rather with the NSF/NSD files stored on the game discs._

* `SCUS-94900` Crash Bandicoot __(no prelude patching)__
* `SCES-00344` Crash Bandicoot __(no prelude patching)__
* `SCPS-10031` クラッシュバンディクー __(no prelude patching)__
* `US BETA 96/03/08` Crash Bandicoot _"Prototype"_
* `US BETA 96/05/11` Crash Bandicoot _"E3 Demo"_
* `SCUS-94154` Crash Bandicoot 2: Cortex Strikes Back
* `SCES-00967` Crash Bandicoot 2: Cortex Strikes Back
* `SCPS-10047` クラッシュバンディクー　2:　コルテックスのぎゃくしゅう！ __(incomplete support)__
* `EU BETA 97/09/14` Crash Bandicoot 2: Cortex Strikes Back _"Review Copy"_
* `SCUS-94244` Crash Bandicoot: Warped
* `SCES-01420` Crash Bandicoot 3: Warped
* `SCPS-10073` クラッシュバンディクー　3:　ブッとび！　世界一周 __(incomplete support)__
* `US BETA 98/08/15` Crash Bandicoot 3: Warped _"Alpha Demonstration"_ (as Crash 3)

## Usage ##
_For users acquainted with Microsoft Windows, "directories" are commonly referred to as "folders" on windows. When the term "directory" is used here, think "folder"._

Some basic knowledge of how Crash Bandicoot game files are laid out is necessary to use this application. CrashEdit is not designed to work directly on disc images (aka ISO's), but instead on files with the "NSF" and "NSD" file format. This is a custom format created by Naughty Dog and used in the original Crash Bandicoot trilogy (not including CTR).

First, a Crash Bandicoot game CD will have a root directory with contents similar to the following:

* `S0` _(directory)_
* `S1` _(directory)_
* `S2` _(directory)_
* `S3` _(directory)_
* `SYSTEM.CNF` _(playstation game boot configuration file)_
* `SCUS_949.00` _(playstation game exe file, US Crash Bandicoot in this case)_

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

https://wiki.cbhacks.com/w/Level_ID

The NSF file contains the actual game data for the level and is what CrashEdit is designed to read and manipulate. An NSF file consists of __entries__. Each entry has a 5-character name, and represents a game asset such as a sound effect or 3D model. The fifth character must match the character expected for an entry of that type. For example, animation entries must end in `V` and GOOL entries must end in `C`. The following entry types are recognized and supported by CrashEdit:

* __Animation:__ One animation used by a game object. Each frame is a full set of vertices.
* __Model:__ One model used by a game object. Polygon data is stored in a Model Entry, but vertex data is stored in an Animation Entry.
* __Scenery:__ One section of the 3D model for a level's world geometry.
* __Sort List:__ A list of values that indicate what polygons should be drawn on-screen and in what order. These require world indexing, which means they'll only operate along with a Zone Entry.
* __Texture Chunk:__ A chunk with the format of an entry. A single 64 KiB page whose data will be directly uploaded to VRAM on load. Double-click the display to open a texture viewer window.
* __Zone:__ Describes one level "zone", including objects in that zone as well as the zone's camera configuration and collision octrees.
* __GOOL:__ One dynamically-linked object executable. Contains all object code as GOOL bytecode (and also R3000A MIPS for Crash 2 and 3) for a specified object type as a series of code blocks, including animation references. Crash 3 GOOL is not fully supported.
* __Sound:__ A sound effect. This entry only contains the raw sound data without any metadata such as the sample rate, which is determined by an object's code.
* __Music:__ A set of music tracks in SEQ format (very similar to MIDI format), and the associated VH file (wavebank header file). Each level zone will refer to a single music entry which will be used for playback while the camera is in that zone.
* __Image:__ One single large image consisting of 16x16 blocks, meant for use with a Map Entry. Each bitmap can be in one of many formats.
* __Map:__ Describes a "map", composed of a background image, followed by several "map entities" which are overlayed. CrashEdit only supports the background layer. Right-click the image to save to a file.
* __Palette:__ A list of 256-color palettes, meant for use with Image Entries in indexed formats (8-bit).
* __Wavebank:__ _Part_ of the level's wavebank data (VB file). Crash music is in MIDI format, but does not use General MIDI (GM) instruments. Instead, a custom instrument set is used for each level. The audio data for this instrument set (wavebank) is very large, so it must be split into multiple entries (up to a maximum of 7).
* __Speech:__ Similar to the _sound entry_, but streamed, meaning a single Speech Entry is part of a longer audio track. It is possible for one streamed audio track to fit into a single Speech Entry, though this is rare.

Entries are organized into containers which are referred to as __chunks__. Each chunk is exactly 64 KB in size, and so it cannot contain more than 64 KB of entry data. _(If you attempt to save an NSF file which has a chunk containing more than 64 KB of entry data, a **packing error** will occur and the save operation will fail.)_ There are different types of chunks: the _normal_ type and special audio types. _(As a general rule, you should keep audio-related entries in their proper chunk types or else the playstation will be unhappy.)_ There is also a special chunk type, __Texture Chunk__, which contains raw texture data instead of entries.

The NSD file contains various data used to assist the game in properly accessing the NSF files. Included in the NSD file is a table mapping entries to chunks. If you add chunks, delete chunks, add entries, delete entries, move entries, rename entries, or reorder chunks, you will need to update this table. CrashEdit can automatically patch this table with the _Patch NSD_ button. _It will remove any and all prelude data.__

## System Requirements ##
_Aside from the obvious monitor, keyboard, and mouse. A mouse scroll wheel is not required, but is used to control the 3D viewers._
* .NET 8.0
* Preferably at least 256 MB of physical memory available to the application or you may encounter thrashing while loading or saving large files

## Known Issues ##
### Incomplete Features ###
#### Significant Issues ####
* __Crash 1 _Retail_:__ Saving preludes is not yet supported.
* __Crash 2/3 _NTSC-J Retail_:__ The lip-sync data used for Aku Aku hints is not supported, making Speech Entries unopenable.

#### Insignificant Issues ####
_These issues have no significant effect on the operation of the program, but are still technically issues with the program. This list can be safely ignored by most users._
* __All Games:__ Any data hidden within unused sections of NSF files may be ignored by the program without warning, and will not be preserved.
* __Crash 1 _All_:__ Music entries containing VH data may be saved out with different data in unused sections.
* __Crash 1 _Retail_:__ "Patch NSD" does not patch all of the NSD.
* __Crash 2 _NTSC-J Retail_:__ A music entry in `S000003C.NSF` containing VH data may be saved out with different data in unused sections.

### Bugs ###
* __All Games:__ Exporting VABs in DLS format is currently broken. The workaround is to open and resave the DLS file with _Awave Studio_.
* __Crash 1 _All_:__ Exporting to COLLADA format is currently broken. However, these files can be opened without issue in _Noesis_.

### Broken Game Files ###
* __Crash 1 _All_:__ `S0000002.NSF`, if present, must be opened as a "1995 Prototype" file. When viewing its zones, press O to fix the octree display.
* __Crash 1 _All_:__ `S0000010.NSF`, if present, must be opened as a "1995 Prototype" file.
* __Crash 1 _Retail_:__ `S0000038.NSF` contains a music entry with an incorrect SEP track count.
* __Crash 1 _Retail_:__ `S0000004.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Retail_:__ `S000000B.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Retail_:__ `S000000D.NSF` is actually a beta MAR08/MAY11-format file.
* __Crash 1 _Beta MAY11_:__ `S000001C.NSF` contains two demo entries with incorrect magic numbers.

## Installation ##
Just unzip into a directory and run the exe.

_If you are working with the source code, it is a VS 2022 solution which consists of four projects, "CrashEdit", "Crash.UI", "CrashEdit.Main", "CrashHacks", "ISO2PSX" and "FTBitmapExtensions". You will need to set "CrashEdit" as the startup project to run the application. "CrashHacks" is a separate application not documented on this repo and not used by CrashEdit itself._

## Where To Get ##
Precompiled binary files (EXE files) are available at:
http://www.cbhacks.com/crashedit.html

The original source code is currently available as a git repository on github at:  
https://github.com/cbhacks/CrashEdit/tree/deprecate (deprecate branch)
