Setting up new scene:

Add new "EventSystem" object
Add "Morbius" prefab to scene. Adjust "NavMeshAgent" parameters if required
Attach "PhysicsRaycaster" component to "Main Camera" object
Attach "PixelatePP" component to "Main Camera" object
	Assign material "PixelatePP" to "Material" for "PixelatePP"
Attach "CameraTarget" component to "Main Camera" object
	Assign gameobject "Morbius" to "Target"
Add "Items", "Combinations", "Inventory", "Ingame UI", "Audio Manager" prefabs to scene
Add "CursorManager" prefab to scene. 
Add "EventManager" prefab to scene. 
	Add further scene specific event objects here as children and reference in "EventManager"
