# NewMod.exe

### About
This is not a mod. This allows you to super quickly create a correcly building mod projects according to my standards. It will set up everything including all necessary files, all dependencies and a minimal mod including settings. It also publizises the game API and lots more.

### Usage
`NewMod.exe <git-name> <mod-compact-name> [<mod-readable-name>] [<mod-author>] [<mod-owner>] [<description>]`

`<git-name>` whatever you have as `NAME` in `https://github.com/NAME`
`<mod-compact-name>` your mod name with no special characters (like a file name)
`<mod-readable-name>` the human readable name of your mod
`<mod-author>` Full name of the author
`<mod-owner>` short lowercase name of the author (used in Harmony ID)
`<description>` an initial description of your mod

### Note
Requires you to define an environment variable called `RIMWORLD_MOD_DIR` that points to the installation destination (usually `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods`). Don't create your project in the `Mods` folder. Instead put it into your `Projects` folder and let the project build and copy the whole mod when you build.

### Author
My name is Andreas Pardeike, but everyone just knows me as **Brrainz**. I am dedicated to making mods and modding tools for the RimWorld community.
If you like or use what I make, please support me with as little as $1 at https://patreon.com/pardeike
