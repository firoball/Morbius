Setting up new scene:

Add new "EventSystem" object
Add "Morbius" prefab to scene. Adjust "NavMeshAgent" parameters if required
Add "Main Camera" prefab to scene
	Assign gameobject "Morbius" to "Target"
Add "Items", "Combinations", "Inventory" and "AudioManager" prefabs to scene. 
Add "Controllers" prefab to scene. 
	Add further scene specific event objects here as children to "EventManager" and reference in "Controllers/EventManager"


Adding new item/object:

Drag item model from Import folder 
Add "Item Dummy" prefab scene
	If separate model is required:
		Drag desired Item to "ItemInstance" component
		Make previously created item model child
	Otherwise:
		Add new "BoxCollider" component
		Adjust "BoxCollider" dimensions to represent mouse click area
Double click original scene object
Select GameObject -> Create empty
	Make "Item Dummy" child
	Reset position of "Item Dummy"
Unparent "Item Dummy" and delete empty GameObject
Rename "Item Dummy" to label of represented Item

