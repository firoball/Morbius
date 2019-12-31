Setting up new scene:

Add new "EventSystem" object
Attach "PhysicsRaycaster" component to "Main Camera" object
Attach "PixelatePP" component to "Main Camera" object
	Assign material "PixelatePP" to "Material" for "PixelatePP"
Add "Items", "Combinations", "Inventory", "Ingame UI", "Audio Manager" prefabs to scene
Add "CursorManager" prefab to scene. Reference "Ingame UI / Cursor" for "Cursor UI"
Add "EventManager" prefab to scene. 
	Child "ItemSequenceEvent": Reference "Ingame UI / InfoText" for "Receiver"
	Child "ItemCombineEvent": Reference "Ingame UI / InfoText" for "Receiver"
	Add further scene specific event objects here as children and reference in "EventManager"
Add "Morbius" prefab to scene. Adjust NavMeshAgent parameters if required