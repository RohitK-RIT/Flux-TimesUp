using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM
{
    public class StateManager : MonoBehaviour
    {
        // Dictionary to hold the different states with EnemyState as key and BaseState as the state object
        private Dictionary<EnemyState, BaseState> _states = new Dictionary<EnemyState, BaseState>();
        
        // The current active state the enemy is in
        private BaseState _currentState;
        
        // Flag to prevent multiple transitions at the same time
        private bool _isTransitioningState = false;
        
        // handles state transitions and updates
        private void Update()
        {
            // If there is no current state, return early
            if (_currentState == null) return;
            
            // Get the next state based on the current state logic
            EnemyState nextStateKey = _currentState.GetNextState();
            
            // Check if we're not transitioning and the state should remain the same
            if (!_isTransitioningState && nextStateKey == _currentState.StateKey)
            {
                //Debug.Log("calling update :"+ nextStateKey);
                // Continue executing the current state's update logic
                _currentState.UpdateState();
            }
            // If a state transition is needed, initiate the transition process
            else if (!_isTransitioningState)
            {
                TransitionToState(nextStateKey);
            }
        }

        // Method to initialize all states and set the initial state
        public void InitializeStates(Dictionary<EnemyState, BaseState> states, EnemyState initialState)
        {
            _states = states;
            
            // Ensure the initial state exists in the dictionary
            if (_states.ContainsKey(initialState))
            {
                _currentState = _states[initialState];
                _currentState.EnterState(); // Enter the initial state
            }
            else
            {
                Debug.LogError("Initial state not found in state dictionary.");
            }
        }

        // Method to transition to a new state
        public void TransitionToState(EnemyState stateKey)
        {
            // Check if the target state exists in the dictionary
            if (!_states.ContainsKey(stateKey)) return;

            // Set the flag to indicate a transition is occurring
            _isTransitioningState = true;
            
            // Call the exit logic of the current state
            _currentState?.ExitState();
            
            // Switch to the new state and call its enter logic
            _currentState = _states[stateKey];
            _currentState.EnterState();
            
            // Reset the transitioning flag
            _isTransitioningState = false;
        }
    }
}