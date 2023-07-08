/*
PROMPT (use this to improve this script with the help of AI) : 
You are a senior game developer assisting a beginner coder in C# unity 3D .
What is wrong with this code and how to fix it simply? 
Find mistakes that could induce the states and algorythmes (state machines ect.) to misbehave.
Suggest examples of how the methods could be fixed. 
When necessary, suggest ideas on how it could be restructured to simplify it 
 
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
* Each Node contains it's visuals as child for organizational puurposes. they are references in the Skill Tree Component script on the game object. 
* Links are just nodes with different visuals and with the "auto-unlock" parameter to true. They also listen to a node's "unlock" state and changes state accordingly. Normally nodes never require links. Only Links need a node to listen to. 
* To help with understanding of the flow of events between the nodes a Canvas with Letters has been added on top of the nodes but this is just cosmetic and can be removed without affecting the nodes. 
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeComponents : MonoBehaviour
{
    [Tooltip("Whether this node should unlock automatically when its requirements are met. Use for links between nodes or nodes that should unlock without player input.")]
    [SerializeField]
    private bool isAutoUnlock = false;

    [Tooltip("List of nodes that need to be unlocked before this node can be unlocked.")]
    [SerializeField]
    private List<SkillTreeComponents> requiredNodes;

    [Tooltip("Visual elements corresponding to different stages of progression towards unlocking this node. Order matters: The first element corresponds to the first unlocked node, the second to the second, and so on.")]
    [SerializeField]
    private List<GameObject> ProgressionVisuals;

    [Tooltip("Visual element that is displayed when the node can be unlocked (all requirements are met but the player hasn't unlocked it yet).")]
    [SerializeField]
    private GameObject buyableVisual;

    [Tooltip("Visual element that is displayed when the node has been unlocked.")]
    [SerializeField]
    private GameObject unlockedVisual;

    private int progressionIndex = 0;

    public event Action OnUnlocked;



    private void Awake()
    {
        IsUnlocked = false;

        foreach (var node in requiredNodes)
        {
            node.OnUnlocked += UpdateProgression;
        }

        // Update initial state.
        UpdateProgression();
    }


    private void UpdateProgression()
    {
        // Check each required node.
        progressionIndex = 0;
        foreach (var node in requiredNodes)
        {
            if (node.IsUnlocked)
            {
                progressionIndex++;
            }
        }
        Debug.Log($"{gameObject.name} - Progression updated. Progression index: {progressionIndex}");

        // Set the visual state.
        if (ProgressionVisuals.Count > 0 && progressionIndex < requiredNodes.Count)
        {
            // Not all requirements are met, so show the appropriate visual.
            SetActiveVisual(ProgressionVisuals[progressionIndex]);
            Debug.Log($"{gameObject.name} - Showing visual for incomplete requirements.");
        }
        else if (!IsUnlocked && progressionIndex == requiredNodes.Count)
        {
            // All requirements are met
            if (isAutoUnlock)
            {
                // Auto-unlock nodes (links) should automatically unlock
                Debug.Log($"{gameObject.name} - Auto-unlock is enabled, unlocking...");
                Unlock();
            }
            else
            {
                // Other nodes should show the buyable visual
                SetActiveVisual(buyableVisual);
                Debug.Log($"{gameObject.name} - All requirements met, showing buyable visual.");
            }
        }
    }





    private bool CanBuy()
    {
        // We can buy the node if all requirements are met.
        return progressionIndex >= requiredNodes.Count;
    }

    public void Unlock()
    {
        if (!IsUnlocked && (isAutoUnlock || CanBuy()))
        {
            // The node is not yet unlocked, so unlock it and show the unlocked visual.
            IsUnlocked = true;
            SetActiveVisual(unlockedVisual);
            Debug.Log($"{gameObject.name} - Node unlocked.");
            OnUnlocked?.Invoke();
        }
    }


    private void SetActiveVisual(GameObject visual)
    {
        // Turn off all visuals, then turn on the specified one.
        foreach (var v in ProgressionVisuals)
        {
            v.SetActive(false);
        }
        buyableVisual.SetActive(false);
        unlockedVisual.SetActive(false);

        visual.SetActive(true);
        Debug.Log($"{gameObject.name} - Active visual set to: {visual.name}");
    }


    private void OnMouseDown()
    {
        // Requires a Collider on the object to be clicked to register
        // When the node is clicked, if we can buy it, then unlock it.
        if (CanBuy())
        {
            Debug.Log($"{gameObject.name} - Node clicked and can be bought, unlocking...");
            Unlock();
        }
    }

    public bool IsUnlocked { get; private set; }
}
