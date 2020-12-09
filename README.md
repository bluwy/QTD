# QTD

A top-down tower defense game, made with Unity.

# Download

To start playing the game, go to the [releases page](https://github.com/bluwy/QTD/releases) and download one of the zip files that matches your operating system.

Currently supported OS:

- Linux (64 bit)
- MacOS (Intel-based)
- Windows (64 bit)

Once downloaded, unzip it and start the game executable (name starts with "QTD-...").

## Development

Clone the project files to start developing, make sure to have [Git LFS](https://git-lfs.github.com/) installed.

The project uses Unity v2020.1.9f1 Personal. Open up the project with the editor installed.

Tests are located in [`Assets/Tests`](./Assets/Tests) which contains play-mode tests only. It can be run through the editor's Test Runner window.

[Assembly Definitions](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) are used in order to run the tests, which is a PITA to use.

Tests are not ran in GitHub Actions because there's issue with `unity-test-runner` action. Currently, the CI only builds the project per release.

## Attributions

- Tilemap and sprites: https://kenney.nl
- Explosion: https://opengameart.org/content/2d-explosion-animations-frame-by-frame
- Sound and music: https://www.zapsplat.com
- DOTween: http://dotween.demigiant.com
- Lilita font: https://www.1001fonts.com/lilita-one-font.html

## License

MIT
