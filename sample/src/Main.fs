module Main

open Browser.Types
open Lit
open Haunted
open Haunted.Types
open Haunted.Lit
open Fable.Core
open Controllers

SutilSamples.registerCounter ()

JsInterop.importSideEffects "./styles.css"

type EventTarget with
    member this.Value = (this :?> HTMLInputElement).value

type Msg =
    | Increment
    | Decrement
    | Reset

let private custom_element
    (props: {| sample: string option
               complexValues: {| message: string |} option |})
    =
    let value =
        props.complexValues
        |> Option.defaultValue {| message = "default message" |}

    let mouseCtrl =
        Haunted.useController<MouseController> (fun host -> MouseController(host) :> ReactiveController)

    let sample = defaultArg props.sample "default sample"

    html
        $"""
        <p>A! {sample} - {value.message}</p>
        <p>
            You can use reactive controllers too!
            <br>
            Mouse Position: x - {mouseCtrl.x}, y - {mouseCtrl.y}
        </p>
    """

// defineComponent registers a Custom Element so you don't need to actually
// call this function inside any component, you can use the component itself
defineComponent
    "inner-component"
    (Haunted.Component(
        custom_element,
        // if you want to monitor attributes you need to pass an array of attribute names
        // these will become available in the function's first argument
        // also these components can be simple custom elements without shadow DOM
        // if you choose not to use shadow DOM you can use normal css stylesheets like bulma or bootstrap
        {| observedAttributes = [| "sample" |]
           useShadowDOM = false |}
    ))

// by itself lit-html functions are stateless
let private aStatelessFunction paramA paramB =
    html
        $"""
        <div>A standard stateless Lit Template!</div>
        this will re-render when the parameters change: {paramA} - {paramB}
        """

let private elmishLike: Reducer<int, Msg> =
    fun state action ->
        match action with
        | Increment -> state + 1
        | Decrement -> state - 1
        | Reset -> 0

// we can use haunted to add state to our components
let private app () =

    let state, dispatch = Haunted.useReducer (elmishLike, 0)

    let log =
        Haunted.useCallback ((fun x -> printfn "%s" x), [| state |])

    let clockCtrl =
        Haunted.useController<ClockController> (fun host -> ClockController(host, 1000) :> ReactiveController)

    log $"{state}"

    let complex =
        {| message = $"Complex object message value: {state}" |}

    html
        $"""
        <h1>Hello, World! - {clockCtrl.time.ToLongTimeString()}</h1>
        <!--You can observe attributes or even properties thanks to lit's templating engine -->
        <inner-component sample={$"Attribute value: {state}"} .complexValues={complex}></inner-component>
        {aStatelessFunction "value" state}
        <section>
            <p>Counter: {state}</p>
            <button @click="{fun _ -> dispatch Increment}">Increment</button>
            <button @click="{fun _ -> dispatch Decrement}">Decrement</button>
            <button @click="{fun _ -> dispatch Reset}">Reset</button>
        </section>
        <sutil-counter></sutil-counter>
        """

defineComponent "fable-app" (Haunted.Component(app, {| useShadowDOM = false |}))
