# Requirements of a Scene

* Exactly one SpawnPoint (see `SpawnPoint.cs`)
* One or more SceneLoadTrigger of type "Load" (see `SceneLoadTrigger.cs`), placed before passages to others scenes, ensuring those scenes are loaded before the player can enter them.
* One or more SceneLoadTrigger of type "Unload" (see `SceneLoadTrigger.cs`), placed after passages from other scenes, so that they can be unloaded after the player has exited them.
* One checkpoint trigger, when reached, sets a scene to be continued from (`GameManager#SetContinueScene`). So that when the game is restarted (or continued), that scene is loaded and the player spawns in its SpawnPoint.
