module SutilSamples

open Fable.Core

open Sutil
open Sutil.Attr

open Haunted
open Haunted.Sutil



let private SutilCounter (init: int option) =
    let state, setState =
        Haunted.useState (init |> Option.defaultValue 0)

    let store = Store.make 0

    Html.section [ Html.h1 "Sutil Counter"
                   Html.p $"{state}"
                   Html.button [ text "Increment"
                                 onClick (fun _ -> setState (state + 1)) [] ]
                   Html.button [ text "Decrement"
                                 onClick (fun _ -> setState (state - 1)) [] ]
                   Html.button [ text "Reset"
                                 onClick (fun _ -> setState (init |> Option.defaultValue 0)) [] ]
                   Html.h1 "Sutil Counter Store"
                   Html.p [ bindFragment store <| fun state -> text $"{state}" ]
                   Html.button [ text "Increment"
                                 onClick (fun _ -> store |> Store.modify (fun state -> state + 1)) [] ]
                   Html.button [ text "Decrement"
                                 onClick (fun _ -> store |> Store.modify (fun state -> state - 1)) [] ]
                   Html.button [ text "Reset"
                                 onClick
                                     (fun _ ->
                                         store
                                         |> Store.modify (fun state -> init |> Option.defaultValue 0))
                                     [] ] ]

[<Emit("SutilCounter")>]
let private SutilCounterRef: obj = jsNative

let registerCounter () =
    defineComponent "sutil-counter" (Haunted.Component(SutilCounterRef, {| useShadowDOM = true |}))
