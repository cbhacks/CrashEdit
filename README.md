# CrashEdit ce2-experiment

This is an experimental version of CrashEdit.


## Changes for users


### Improvements

 * Rewritten hex editor with important features like:
   * Smooth continuous scrolling, with scroll bars and by mouse wheel
   * Clicking to select bytes
   * Variable row counts, not fixed at 16
   * Offsets displayed next to each row
 * New search bar
   * Can search forward and backward
   * Displays current search query, which can also be edited
   * Supports hotkeys including Ctrl-F, F3, Shift-F3
   * Find-next and find-previous search from the current selection, not an
   invisible search cursor
   * Search queries are matched on demand, rather than building an internal list
   of results when Find is used
 * More specific names for some "Item" resources
   * Header and Layout for zones and (C1) maps
   * ExtraData for C1 map scenery
   * Items[0], Items[1], Items[2], etc. for unprocessed entries
 * Changes to Undock feature
   * Activated with Ctrl-D instead of D
   * Available in the menu and toolbar
   * Now works with all editor types
 * Improvements to right-click actions in resource tree
   * More actions available
   * Icons added for some actions
   * Some actions categorized by whether they apply to a resource or its contents


### Regressions

 * New hex editor is missing some commonly used special keys:
   * Z to view EID's
   * N to set a `NONE!` EID starting at the current position
 * Chunk space allocation viewer is broken
 * Localization support is reduced
 * Export SEP is missing; this can be achieved by exporting the third item from
 an unprocessed music entry.
 * Patch NSD offers to save NSF even if entries were not moved around
 * Texture chunks are no longer labeled with their CID
 * New chunks are created with invalid CID 0
 * When chunks are deleted, the CID's of chunks afterward are not adjusted


### Other changes

 * Entry chunks remember their CID, rather than computing it on save
   * The CID displayed in the tree label is the CID stored in the entry chunk
   * Entry chunks with incorrect CID's can be loaded and they will remember
     their incorrect CID
   * Patch NSD still uses computed CID's for entry chunks
 * Unprocessed chunks are labeled their raw ID field in hex
 * C1 colored animation frames can now also use the existing normal C1 animation
   frame editor; note however the UI has not been updated for this purpose:
   * R/G/B Colors (unsigned) are displayed as X/Y/Z normals (signed)
   * To enter color channel values below 128, enter them as-is
   * To enter color channel values of 128 or greater, subtract 256 from them first


## Changes for developers


### Project and directory restructuring

Project namespaces have been reorganized:

 * `CrashEdit` is now `CrashEdit.CE`
 * `Crash.UI` is now `CrashEdit.CrashUI`
 * `Crash` is now `CrashEdit.Crash`

This brings the major projects into a single namespace, although `ISO2PSX` and
`CrashHacks` are left as-is.

A new project `CrashEdit.Main.csproj` has been added to the solution. This uses
the new SDK project format, which is much simpler and easily written and edited
by hand, unlike the older VS-generated files. Files in this new project are
included by a simple wildcard pattern, allowing easier adding/removing/renaming
of source files, as well as merging of PR's or branches which include such
changes.

The new project produces a `CrashEdit.Main.dll` assembly, and is referenced by
the other projects.

The `Crash.csproj` project has been removed, adding the entire `Crash` tree to
the new `CrashEdit.Main` project.

New development occurs in two directories:

 * `Core` (namespace `CrashEdit`), intended for general non-crash-specific code
 * `Crash` (namespace `CrashEdit.Crash`), intended for crash trilogy-specific
 code

Both directories also include GUI code, in the same namespaces.


### Controller overhaul

The controller system is being gutted and replaced, as it is an unmaintainable
mess and possibly the worst part of the existing CE codebase.

Under the old controller system, a controller class is defined for each type of
resource which would appear in the tree view, such as an NSF, chunk, entry,
object, etc. Additionally, if one type of resource appeared in multiple kinds
of parent resources, such as the SEQ's held within MusicEntry vs the ones held
in OldMusicEntry, multiple controller types would be defined for each one, or a
complicated state system would be set up within the controller to support each
use case individually.

These controllers serve four crucial functions:

 1. __Tree view node presence and identity:__ Nodes in the resource tree view
 exist because they are constructed and owned by the matching controller. The
 controller provides the text and image for the treeview node, for which it is
 also responsible for managing and keeping up to date.

 2. __Editor views:__ The controller serves as a Factory for constructing the
 GUI control (`System.Windows.Forms.Control`) intended for use in editing the
 underlying resource.

 3. __Right-click or drag actions:__ The available options when right-clicking
 a resource tree node, and the drag-and-drop functionality of those nodes, are
 provided by the resource's controller.

 4. __Provisioning child controllers for subresources:__ The controller is
 responsible for constructing controllers (and thus tree nodes) for the child
 resources of the current resource. For example, the NSF controller makes its
 own ChunkController children. The controller is also responsible for keeping
 these child controllers in sync with the resource's own structure.

This controller type has been renamed to `CrashEdit.CE.LegacyController`, and
is being replaced with a new `CrashEdit.Controller` type. All new controllers
are instances of just this one type, which is not meant to be inherited.

The old controller functionality is reimplemented as follows:

 1. __Tree view node presence and identity:__ Resources which implement the
 `IResource` interface can determine their own node text and icon, which the
 new controllers will simply forward. If a resource does not implement this
 interface, the generic "arrow" icon will be used, and the text of the node
 will be based on the name of the parent resource's property wherein the
 current resource was found. For example, the third SEQ in a Music entry will
 be given the name `Tracks[2]`, as it is the resource located in the element
 at index `2` in the `Tracks` property of `MusicEntry`, of type `List<SEQ>`
 (this property is new). New controllers do not own any tree nodes, merely
 providing the information necessary for a tree view to make and manage such
 nodes itself. This means multiple resource tree views can now coexist and
 operate on the same controller tree. The resource treeview has been entirely
 rewritten to support this new design.

 2. __Editor views:__ A new abstract `Editor` class is defined. Any class
 inherting from `Editor`, if default-constructable, will be instantiated when
 the program is launched, and added to a public `Editor.AllEditors` list. When
 the user clicks a tree node for a controller for the first time, each Editor
 object is checked for compatibility with the controller and/or its resource.
 Each editor which claims compatibility (by overriding `ApplicableForSubject`)
 will be cloned, and the clones will be initialized for use with the selected
 controller. Each editor constructs a winforms Control and is responsible for
 managing it, similar to old Controllers. However, unlike old Controllers,
 an Editor may be applicable to multiple types of resources, and a controller
 may have multiple applicable Editors. The editors' controls are placed into
 a tabview, one tab per editor control. Editors also determine the name of this
 tab.

 3. __Right-click or drag actions:__ Similar to `Editor` above, a new `Verb`
 class is defined, with a respective `AllVerbs` list. When the user right-clicks
 a tree node, each verb is checked for compatibility with the controller and/or
 its resource. The context menu is constructed and populated on-demand, rather
 than ahead of time. Because verbs are defined separately and independently of
 controllers and their resources, generic verbs can be written. For example,
 a new `DeleteVerb` (right-click "Delete") applies to any controller whose
 presence under its parent is due to its membership in a non-readonly list.
 This means the same verb, written just once, applies to the items in a raw
 entry, the tracks in a music entry, the entries in a chunk, the chunks in an
 NSF, etc. This is far more flexible than the old per-controller `AddMenu()`
 system. Right-click verbs are split into categories of `DirectVerb`, which
 apply to a controller (e.g. Delete, Replace from file), and `GroupVerb`, which
 apply to a property in a resource (e.g. Add from file, Create new). Drag-drop
 is supported with `TransitiveVerb`.

 4. __Provisioning child controllers for subresources:__ A controller's child
 controllers are built automatically by discovering its resource's subresources.
 Within the class for a resource type, each property may be marked with an
 attribute `[SubresourceSlot]` or `[SubresourceList]` for slots or lists
 respectively. A slot is a property which contains a single subresource
 (or nothing, if the property value is null). For example, the VH property in a
 Music entry is a slot, and is marked with that attribute. A list is a property
 which contains a series of zero or more subresources, such as the Tracks in
 a music entry, or the chunks in an NSF. A list property may be any type as long
 as it implements `IEnumerable`, but for maximum application it should at least
 implement `IList<T>`.

Rather than being kept in sync manually, the controller tree is updated by
calling the `Sync()` method on the root controller. This automatically finds
changes in the underlying resources and amends the tree as necessary to fit the
new resource layout, creating new Controller objects to match new resources,
pruning controllers for no-longer-present resources, and reordering the
existing controllers as necessary.

The resource tree view control similarly has a `Sync()` method which updates
its nodes to match the attached controller tree.

`Editor` objects additionally contain a `Sync()` method. Editors may use this
to update their controls or whatever else is necessary.

Because of this design, it is crucial to call plenty of `Sync()` methods across
the program whenever any change occurs to the data. Verb executors will do this
after executing any verb, but Editor controls may need to do so themselves.

If a resource vanishes and reappears in a different group, for example an
entry moved from one chunk to another, a new controller is constructed and the
old one is discarded. This differs from the previous design, where controllers
were migrated. Note this only applies to moving between different lists, slots,
etc. Reordering items inside a list, for example reordering entries inside a
chunk, does not result in controller destruction/rebuilding.

If a resource appears multiple times in the resource tree, for example a single
`Entry` object is referenced in multiple `EntryChunk` lists, or multiple times
in the same list, each appearance of the resource will have its own controller.
This "aliasing" scenario is somewhat supported by the new design, but is generally
undesirable and should be avoided.

The following properties are now subresources using the new system:

 * (list) `NSF.Chunks`
 * (list) `EntryChunk.Entries`
 * (list) `MysteryMultiItemEntry.Items`
 * (list) `AnimationEntry.Frames`
 * (list) `OldAnimationEntry.Frames`
 * (list) `ColoredAnimationEntry.Frames`
 * (list) `ProtoAnimationEntry.Frames`
 * (slot) `OldSceneryEntry.ExtraData`
 * (slot) `Header` and `Layout` in the zone entries and in `MapEntry`
 * (slot) `VH` in the music entries
 * (list) `Tracks` in the music entries, replacing `SEP` and `SEP.SEQs`

The following old-style controllers are completely removed:

 * `ItemController`
 * `MysteryMultiItemEntryController`
 * `ColoredFrameController`
 * `VHController`
 * `OldVHController`
 * `SEQController`
 * `OldSEQController`
 * `DemoEntryController`
 * `ImageEntryController`
 * `WavebankEntryController`
 * `T6EntryController`
 * `T15EntryController`
 * `T17EntryController`
 * `T21EntryController`
 * `NormalChunkController`
 * `OldSoundChunkController`
 * `SoundChunkController`
 * `WavebankChunkController`
 * `SpeechChunkController`

Remaining old-style controllers (`LegacyController`) remain in place but are
planned for removal. Where present, old-style controllers coexist with new-
style ones in a complicated manner. Every old-style controller object always
has a single new-style `Controller` associated with it. The old-style `AddMenu`
actions exist as `LegacyVerb` verbs which apply to the new controller. A
`Controller` may have new subresource-attribute-based child controllers as well
as old `AddNode` child controllers simultaneously.


### Embeds

Icons are now included simply by being placed into `Embeds/Images`, without
any other project changes, resx changes, designer regeneration, etc. Icons
are available by `Embeds.GetIcon(string)`, and are also automatically included
into the shared imagelist `Embeds.ImageList` which is used by most new UI
elements.

New icons should also be added to the table in `Embeds/Images/README.md`.


### Package import changes

Some in-repo libraries have been replaced:

 * `OpenTK.dll` 1.1.0.0
 * `OpenTK.GLControl.dll` 1.1.0.0
 * `DiscUtils.dll` 0.10.0.0

They have been replaced by the following Nuget packages:

 * `OpenTK` 3.3.2
 * `OpenTK.GLControl` 3.1.0
 * `DiscUtils.Iso9660` 0.16.4

These packages are referenced but are not included in the repo. Build tools such
as Visual Studio, msbuild, dotnet cli, etc. should download these automatically.


### Nullable references

New development uses the "nullable reference" feature introduced in C# 8.0.

<https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references>

Each new C# source file begins with the `#nullable enable` directive.


### CI/CD changes

The appveyor script has not been kept up to date with these changes and will
not work. Likely this will be replaced with GitHub Actions in the future.
