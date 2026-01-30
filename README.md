# Ludu Arts – Interaction System Case

A modular, scalable, and clean-code-oriented Interaction System built in Unity.  
This project demonstrates a decoupled architecture, data-driven content management, and polished audio-visual feedback, reflecting a production-ready gameplay approach.

Key Features

- Robust Interaction Architecture: All interactable objects implement the IInteractable contract.
- Interaction Types
  - Instant: Single key interactions (e.g., pickups)
  - Hold: Time-based interactions (e.g., opening a chest) with UI progress feedback
  - Toggle: State-based objects (e.g., doors, switches)
- Data-Driven Design: Keys are defined as ScriptableObjects (KeyData), allowing new content creation without modifying code.
- Game Feel
  - Contextual SFX feedback for interactions
  - Lightweight code-driven animations using Quaternion.Slerp instead of heavy Animator controllers for simple rotations
- Reactive UI
  - Dynamic prompt updates (e.g., {key} replaced with assigned input key)
  - Hold interactions update progress visuals in real time

Architecture Overview

1) IInteractable-Based Design  
InteractionInputHandler does not know object types (door, chest, switch, etc.).  
It communicates only through the IInteractable interface, allowing new interactables to be added without modifying existing logic.

2) Detection System (Raycast / SphereCast)  
InteractionDetector scans objects in front of the camera and selects the nearest IInteractable.  
This approach simplifies state management and ensures consistent targeting behavior.

3) UI Separation  
The HUD system is decoupled from gameplay logic.  
Detector/Input determine what to show, while HUD handles how it is displayed.

Inventory and Key System

- Inventory stores collected keys  
- KeyData ScriptableObjects define key data separately  
- Door checks if the required key exists before unlocking  

Error Handling

Silent bypasses were avoided.  
Critical references and edge cases are handled using descriptive Debug.LogWarning and Debug.LogError messages to improve debugging clarity.

Development Note

During development, LLM tools were occasionally used as architectural discussion partners, convention validators, and edge-case reviewers.  
Final implementation, integration, and architectural decisions were iteratively refined and validated manually.  
For detailed prompt history, see PROMPTS.md

Controls

- WASD: Move  
- Mouse: Look around  
- E: Interact / Hold to interact  
- Esc: Unlock cursor  

Setup

1. Clone the repository  
2. Open with Unity 2022.3.61f1 or a compatible version  
3. Open scene: Scenes/InteractionDemo  
4. Press Play  

Developed as a technical case study for Ludu Arts.
