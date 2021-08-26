module Main

open Browser.Types
open Lit
open Fable.Haunted
open Fable.Haunted.Types

type EventTarget with
    member this.Value = (this :?> HTMLInputElement).value

type Msg =
    | Increment
    | Decrement
    | Reset

let private inner_component
    (props: {| sample: string
               property: string option |})
    =
    html $"""<p>An inner component! {props.sample} - {props.property}</p>"""

// defineComponent registers a Custom Element so you don't need to actually
// call this function inside any component, you can use the component itself
defineComponent "inner-component" (Haunted.Component(inner_component, {| observedAttributes = [| "sample" |] |}))

/// Virtual Components don't have a tag by themselves
/// but they allow you to have state in them, think of them as if they were fragments
/// be mindful that since they don't have a tag if you try to dispatch
/// an event using `this` it might point to the component where this is called
/// but in some conditons fable will emit an arrow function so this will not be attached
/// and your dicpatch might fail
[<VirtualComponent>]
let virtual_component (name: string) (age: int) =
    let state, setState = Haunted.useState "I'm so virtual!"

    html
        $"""
        <div>
            <p>A virtual component can keep its own state</p>
            <input value={state} @keyup={fun (ev: Event) -> ev.target.Value |> setState}>
            <p>External message: {name} is {age} year(s) old</p>
            <p>Internal message: {state}</p>
        </div>"""

// by itself lit-html functions are stateless
let private not_a_component () =
    html $"""<div>A standard Lit Template!</div>"""

// we can use haunted to add state to our components
let private app () =
    let reducer: Reducer<int, Msg> =
        fun state action ->
            match action with
            | Increment -> state + 1
            | Decrement -> state - 1
            | Reset -> 0

    let state, dispatch = Haunted.useReducer (reducer, 0)

    let log =
        Haunted.useCallback ((fun x -> printfn "%s" x), [| state |])

    log $"{state}"

    html
        $"""
        <h1>Hello, World!</h1>
        <!--You can observe attributes or even properties thanks to lit's templating engine -->
        <inner-component sample="lol" .property="{10}"></inner-component>
        {virtual_component "John" 45}
        {not_a_component ()}
        <p>Counter: {state}</p>
        <button @click="{fun _ -> dispatch Increment}">Increment</button>
        <button @click="{fun _ -> dispatch Decrement}">Decrement</button>
        <button @click="{fun _ -> dispatch Reset}">Reset</button>
        """

defineComponent "fable-app" (Haunted.Component app)
