module Main

open Lit
open Fable.Haunted
open Fable.Haunted.Types

type Msg =
    | Increment
    | Decrement
    | Reset


let private another
    (props: {| sample: string
               property: string option |})
    =
    html $"""<div>An inner component! {props.sample} - {props.property}</div>"""

// defineComponent registers a Custom Element so you don't need to actually
// call this function inside any component, you can use the component itself
defineComponent "inner-component" (Haunted.Component(another, {| observedAttributes = [| "sample" |] |}))

// by itself lit-html functions are stateless
let private anotherOne () =
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
        <div>Hello, World!</div>
        <!--You can observe attributes or even properties thanks to lit's templating engine -->
        <inner-component sample="lol" .property="{10}"></inner-component>
        {anotherOne ()}
        <p>Counter: {state}</p>
        <button @click="{fun _ -> dispatch Increment}">Increment</button>
        <button @click="{fun _ -> dispatch Decrement}">Decrement</button>
        <button @click="{fun _ -> dispatch Reset}">Reset</button>
        """

defineComponent "fable-app" (Haunted.Component app)
