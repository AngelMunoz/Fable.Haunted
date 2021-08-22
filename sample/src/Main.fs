module Main

open Lit
open Fable.Haunted

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
    let state, setState = Haunted.useState 0

    html
        $"""
        <div>Hello, World!</div>
        <!--You can observe attributes or even properties thanks to lit's templating engine -->
        <inner-component sample="lol" .property="{10}"></inner-component>
        {anotherOne ()}
        <p>Counter: {state}</p>
        <button @click="{fun _ -> setState (state + 1)}">Increment</button>
        <button @click="{fun _ -> setState (state - 1)}">Decrement</button>
        <button @click="{fun _ -> setState 0}">Reset</button>
        """

defineComponent "fable-app" (Haunted.Component app)
