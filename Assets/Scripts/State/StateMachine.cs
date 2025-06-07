using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<TOwner>
{
    public abstract class State
    {
        protected StateMachine<TOwner> StateMachine => stateMachine;
        internal StateMachine<TOwner> stateMachine;

        internal Dictionary<int, State> transitions = new Dictionary<int, State>();

        protected TOwner Owner => StateMachine.Owner;

        internal void Enter(State prevState){
            OnEnter(prevState);
        }
        protected virtual void OnEnter(State prevState) {}

        internal void Update(){
            OnUpdate();
        }
        protected virtual void OnUpdate() {}

        internal void Exit(State nextState){
            OnExit(nextState);
        }
        protected virtual void OnExit(State nextState){}
    }

    public sealed class AnyState : State{ }

    public TOwner Owner{ get; }

    public State CurrentState {get; private set; }

    private LinkedList<State> states = new LinkedList<State>();

    public StateMachine(TOwner owner)
    {
        Owner = owner;
    }

    public T Add<T>() where T : State, new (){
        var state = new T();
        state.stateMachine = this;
        states.AddLast(state);
        return state;
    }

    public T GetOrAddState<T>() where T: State, new (){
        foreach (var state in states){
            if (state is T result){
                return result;
            }
        }
        return Add<T>();
    }

    public void AddTransition<TFrom, TTo>(int eventId)
        where TFrom : State, new()
        where TTo : State, new(){
            var from = GetOrAddState<TFrom>();
            if (from.transitions.ContainsKey(eventId)){
                throw new System.ArgumentException($"ステート {typeof(TFrom).Name} に対して、イベントＩＤ {eventId}　の遷移が定義済みです。");
            }

            var to = GetOrAddState<TTo>();
            from.transitions.Add(eventId, to);
    }

    public void AddAnyTransition<TTo>(int eventId) where TTo : State, new() {
        AddTransition<AnyState, TTo>(eventId);
    }

    public void Start<TFirst>() where TFirst : State, new() {
        Start(GetOrAddState<TFirst>());
    }

    public void Start(State firstState) {
        CurrentState = firstState;
        CurrentState.Enter(null);
    }

    public void Update() {
        if (CurrentState != null) CurrentState.Update();
    }

    public void Dispatch(int eventId) {
        State to;
        if (!CurrentState.transitions.TryGetValue(eventId, out to)) {
            if (!GetOrAddState<AnyState>().transitions.TryGetValue(eventId, out to)){
                return;
            }
        }
        Change(to);
    }

    private void Change(State nextState){
        CurrentState.Exit(nextState);
        nextState.Enter(CurrentState);
        CurrentState = nextState;
    }
}
