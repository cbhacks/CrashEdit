# CrashEdit #
This program is intended to be a tool for modifying the game files of the original Crash Bandicoot games. In its current state, it is very incomplete.

## Supported Games ##
* (SCUS-94900) Crash Bandicoot __(read only)__ __(experimental)__
* (SCES-?????) Crash Bandicoot __(read only)__ __(experimental)__ __(untested)__
* (????-?????) クラッシュバンディクー __(read only)__ __(experimental)__ __(untested)__
* (SCUS-94154) Crash Bandicoot 2: Cortex Strikes Back
* (SCES-?????) Crash Bandicoot 2: Cortex Strikes Back
* (SCPS-10047) クラッシュバンディクー　2:　コルテックスのぎゃくしゅう！
* (PAL BETA 97/09/14) Crash Bandicoot 2: Cortex Strikes Back
* (SCUS-94244) Crash Bandicoot: Warped
* (SCES-?????) Crash Bandicoot 3: Warped __(untested)__
* (????-?????) クラッシュバンディクー　3:　ブッとび！　世界一周 __(untested)__

## Features ##
The current feature set includes:

### General ###
* Loading NSF files
* Saving NSF files
* Updating NSD files
* Creating Normal Chunks
* Creating Sound Chunks
* Creating Wavebank Chunks
* Creating Speech Chunks
* Deleting Chunks __(incomplete)__
* Moving Entries __(incomplete)__
* Deleting Entries __(incomplete)__

### Importing and Exporting ###
* Sound: PSX ADPCM Format __(export only)__
* Sound: Wave Format __(export only)__
* Speech: PSX ADPCM Format __(export only)__
* Speech: Wave Format __(export only)__
* Music: SEQ Format
* Music: SEP Format __(export only)__
* Music: MIDI Format __(export only)__
* Wavebank: VH Format
* Wavebank: VB Format __(export only)__
* Wavebank: VAB Format __(export only)__
* Wavebank: DLS Format __(export only)__ __(experimental)__

### Other ###
* Exporting most unknown data in binary format
* Modifying most unknown data using a built-in hex editor

## Requirements ##
You will need Microsoft .NET Framework 2.0 or an alternative such as Mono. In the case of Mono, you will need the following referenced assemblies:

* System
* System.Drawing
* System.Windows.Forms

If you use Mono, you may need to comment out `[EditorControl(typeof(TextureChunk))]` in CrashEdit/TextureChunkBox.cs and recompile. __Note: doing so will disable the texture viewer.__

## Installation ##
Just unzip into a directory and run the exe.

## Where To Get ##
Source repo: https://github.com/ughman/CrashEdit  
Binary releases: https://www.dropbox.com/sh/yv93g4wsdde32s3/NuCSJR37Oo

If you don't know which you want, you want "Binary releases".

## Author Contact Info ##
I can be contacted in various ways:

* As `chekwob` on YouTube (this is the best way, I check it very often)
* As `chekwob@yahoo.com` via email (english only; I discard non-english email as spam)
* As `chekwob` on the XeNTaX forum
* As `chekwob` on the CM forum (hpzr.proboards.com)
* As `ughman` on github
