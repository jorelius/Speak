# Speak

[![Build status](https://ci.appveyor.com/api/projects/status/h2j47val4ak1yltf?svg=true)](https://ci.appveyor.com/project/jorelius/speak)

## Overview

Speak is a command line utility for reading text aloud or writing the audio data to file. 

## Usage

    Usage - speak [<Text>] -options

    GlobalOption           Description
    Help (-?)              Shows this help
    ShowVoices (-voices)   Show speaker voices available
    Voice (-v)             Set speaking voice
    Rate (-r)              Set speaking rate (-10 to 10) [Default='0']
    Text (-t)              Text to speak
    UI (-U)                Show progress bar
    SaveFile (-s)          Save to file

Speak was created with the intention that it should do one thing well and work well with others. In that vein, you can couple Speak with other applications. In PowerShell, there is a PowerShell applet that can retrieve the contents of the clipboard. Speak is designed to allow piping input.

e.g. **Get-Clipboard | speak**

Another option is to pair it with [Watch](https://github.com/jorelius/Watch). You can Watch a resource (Clipboard, Filesystem, HTTP, FTP) and trigger Speak.

e.g. **watch Clipboard "speak" "{CLIPTEXT}"**

You can also just use Speak on its own. 

e.g. **speak "hello world"**
