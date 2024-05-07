# project-gyro
Have you ever thought that rhythm games have started to become more and more boring over the last few years?
You press buttons, get points,... beat your pb.... im falling asleep writing that!

but what if.... there were no buttons? what if, you could use your rotation of your phone... as the controls..?
That's right! This game uses Gyro as it's main source of control!

# but... how do i even play that??

It's dead simple, you see incoming notes, you rotate your phone at the right time, in the right rotation.

![ezgif com-optimize (2)](https://github.com/dsinkerii/project-gyro/assets/104655906/a8803193-7149-431a-8b6d-6971789fbc8e)

(game preview on the 1.0.1 version, in there i missed an orange note because im playtesting on pc)

A more in-depth tutorial:
- Seeing any of the 4 listed below notes, you should push your phone in the rotation of the note (e.g. a red note tilting to left means you need to rotate your phone to that direction, pushing your left hand to it)

  Notes: Red (left), Right (cyan), Up (green), Down (purple)
![tutorial_notes](https://github.com/dsinkerii/project-gyro/assets/104655906/a135c70f-066d-4a87-9121-c97c2b18e6d3)

- Seeing an orange note (big orange circle), you need to rotate your phone kind of like when rotating a valve, in clockwise or counter-clockwise rotation

![orangenotes](https://github.com/dsinkerii/project-gyro/assets/104655906/473143ad-bed8-49da-a2c7-90e88312e12c)


# okay, thats great.. but, how do i download tracks?

Simple. You get a download link either by the creator, you paste it in, and you're good to go

Example of a link: https://github.com/dsinkerii/project-gyro/releases/download/1.0.2/Template.zip

(and for creators, please make sure its a direct link to the download, so no redirects, google drives, any other drives or whatever. it must be a link that you can press on, and it will immediately start downloading. a good example of it is https://filebin.net for temporary files)

![2024-05-0718-50-48-ezgif com-video-to-gif-converter](https://github.com/dsinkerii/project-gyro/assets/104655906/aa399054-417c-43f9-8ce2-61f7189b210c)

in-game uploading and track builder soon if the game will be good enough.

# alright! now how do i begin making tracks?

Currently there doesn't exist a track builder, so you will have to make tracks **manually**, which means you should have knowledge in how to edit files and follow patterns.

1. Download the song.zip from the first release (it's a template song used for testing)
    https://github.com/dsinkerii/project-gyro/releases/tag/1
2. Unzip it, and edit it at your will.

Rules on editing:
- Thumbnail.png **MUST** be 160x90, otherwise it may render incorrectly.
- track.wav **MUST** be called "track.wav", and **MUST** be in .wav format (for best precision)
- track.json needs to have the following:
  - "name" // string, track name
  - "length" // int, in seconds
  - "bpm" // int, BPM
  - "author" // string, self explanatory
  - "trackStartOffset" // float, time before starting to count the beats and subBeats
  - "version" // int, track file format (latest one is 2)
  - "rightColor", "upColor", etc // color32, RGBA colors for custom themes. bar1Color and bar2Color are the spectogram bars. heatModeColor = color, that changes the bg color on heat mode.
- notes.json:
  - "beatTime" // int, be careful! this actually means a quarter of a regular beat! i know, it's confusing, but sorry, precision.. (also note, each note **MUST** be sorted by beatTime, otherwise it wont spawn unsorted notes, check the template for context)
  - "subBeatTime" // int a quarter of a quarter of a regular beat.
  - "direction" // int, must be 0 to 4: 0 = up, 1 = right, 2 = down, 3 = left, 4 = roll
 
and that's it! have fun! :)
