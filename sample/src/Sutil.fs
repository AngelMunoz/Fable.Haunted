module SutilSamples

open Fable.Core

open Sutil
open Sutil.Attr

open Haunted
open Haunted.Sutil



let private SutilCounter (init: int) =
    let state, setState = Haunted.useState (init)


    Html.section [ Html.h1 "Sutil Counter"
                   Html.p $"{state}"
                   Html.button [ text "Increment"
                                 onClick (fun _ -> setState (state + 1)) [] ]
                   Html.button [ text "Decrement"
                                 onClick (fun _ -> setState (state - 1)) [] ]
                   Html.button [ text "Reset"
                                 onClick (fun _ -> setState (init)) [] ] ]

[<Emit("SutilCounter")>]
let private SutilCounterRef: obj = jsNative

let registerCounter () =
    defineComponent "sutil-counter" (Haunted.Component(SutilCounterRef, {| useShadowDOM = true |}))
