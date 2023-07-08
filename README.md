# unity3d_skilltree

SKILL TREE COMPONENT SCRIPT:
* The script is for a Skill Tree system in Unity.
* It has several serialized fields that can be set in the Unity Editor.
* These fields include whether the node is automatically unlocked, the required nodes for unlocking this node, and various visual elements for displaying the node’s progression and state.
* The script has an event for when the node is unlocked.
* The script updates the node’s progression and visual state based on whether its requirements have been met.
* If all requirements are met and the node is not yet unlocked, it can be unlocked by clicking on it (buying it).
* Once unlocked, it will display the unlocked visual and invoke the OnUnlocked event.

EDITOR PARAMETERS:
* isAutoUnlock: A boolean value that determines whether the node is automatically unlocked or not. should be used for links between nodes or nodes that automatically unlock if their requirement is unlocked. 
* requiredNodes: A list of SkillTreeComponents that represents the required nodes for unlocking this node.
* ProgressionVisuals: A list of GameObjects that represent the visual elements for displaying the node’s progression toward being unlocked.
* buyableVisual: A GameObject that represents the visual element for when the node can be bought (i.e. when all requirements are met but the node is not yet unlocked).
* unlockedVisual: A GameObject that represents the visual element for when the node is unlocked.

SCENE SETUP: 
* Simply put the script as compoent on your node and links. use game obects as visuals
* Links are just nodes with different visuals and with the "auto-unlock" parameter to true. They also listen to a node's "unlock" state and changes state accordingly. Normally nodes never require links. Only Links need a node to listen to. 
