using System.Collections.Generic;
using System.Collections;
using System;

namespace GameContent.Controller.BaseMachine
{
    public sealed class GenericStateMachine
    {
        #region constructor

        public GenericStateMachine(int stateNumber)
        {
            _stateNumber = stateNumber;
            _stateDict = new Dictionary<string, int>();
            _stateNames = new string[stateNumber];
            _initStates = new Action<GenericStateMachine>[stateNumber];
            _enterStates = new Action[stateNumber];
            _updateStates = new Func<sbyte>[stateNumber];
            _fixedUpdateStates = new Func<sbyte>[stateNumber];
            _exitStates = new Action[stateNumber];
            _coroutineStates = new Func<IEnumerator>[stateNumber];
        }

        #endregion

        #region methodes

        public void SetCallBacks(byte stateID,
            string stateName,
            Action<GenericStateMachine> init, 
            Action enter, 
            Func<sbyte> update, 
            Func<sbyte> fixedUpdate, 
            Action exit,
            Func<IEnumerator> coroutines)
        {
            _currentState = 0;
            _cine = false;
            _locked = false;
            _stateDict.Add(stateName, stateID);
            _stateNames[stateID] = stateName;
            _initStates[stateID] = init;
            _enterStates[stateID] = enter;
            _updateStates[stateID] = update;
            _fixedUpdateStates[stateID] = fixedUpdate;
            _exitStates[stateID] = exit;
            _coroutineStates[stateID] = coroutines;
        }

        public void InitMachine()
        {
            foreach (var i in _initStates)
            {
                i?.Invoke(this);
            }
        }
        
        public void StartMachine()
        {
            if (_stateNumber <= 0)
                throw new Exception("Empty State Machine");

            if (_enterStates[_currentState] != null)
                _enterStates[_currentState]();
        }
        
        public void UpdateMachine()
        {
            if (_locked)
                return;
            
            if (_updateStates[_currentState] != null)
                _updateStates[_currentState]();
        }

        public void FixedUpdateMachine()
        {
            if (_locked)
                return;
            
            if (_fixedUpdateStates[_currentState] != null)
                _fixedUpdateStates[_currentState]();
        }
        
        public void SwitchState(int toState)
        {
            if (_cine || _locked)
                return;
            
            if (_exitStates[_currentState] != null)
                _exitStates[_currentState]();

            _currentState = toState;

            if (_enterStates[_currentState] != null)
                _enterStates[_currentState]();
        }

        public void SwitchState(string toState)
        {
            if (!_stateDict.TryGetValue(toState, out var value))
                return;
            
            SwitchState(value);
        }

        public void ForceState(int toState)
        {
            if (_locked)
                return;
            
            if (_exitStates[_currentState] != null)
                _exitStates[_currentState]();

            _currentState = toState;

            if (_enterStates[_currentState] != null)
                _enterStates[_currentState]();
        }

        public void ForceState(string toState)
        {
            if (!_stateDict.TryGetValue(toState, out var value))
                return;
            
            ForceState(value);
        }
        
        public static implicit operator int(GenericStateMachine m) => m._currentState;
        
        public static implicit operator string(GenericStateMachine m) => m._stateNames[m._currentState];
        
        #endregion

        #region fields

        private int _currentState;

        private readonly int _stateNumber;
        
        private readonly Dictionary<string, int> _stateDict;

        private readonly string[] _stateNames;
        
        private readonly Action<GenericStateMachine>[] _initStates;

        private readonly Action[] _enterStates;

        private readonly Func<sbyte>[] _updateStates;

        private readonly Func<sbyte>[] _fixedUpdateStates;

        private readonly Action[] _exitStates;

        private readonly Func<IEnumerator>[] _coroutineStates;

        private bool _cine;

        private bool _locked;

        #endregion
    }
}