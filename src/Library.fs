module Fable.Haunted

open Browser.Types
open Fable.Core
open Fable.Core.JsInterop

open Lit

let defineComponent (name: string) (comp: obj) : unit = importMember "./interop.js"

[<Emit("new Event($0, $1)")>]
let createEvent (name: string) (opts: obj) = jsNative

[<Emit("new CustomEvent($0, $1)")>]
let createCustomEvent (name: string) (opts: obj) = jsNative

/// dispatchEvent **must** be caled inside a render function (it uses `this`) to correctly dispatch events
[<Emit("this.dispatchEvent($0)")>]
let dispatchEvent (event: Event) : unit = jsNative

type Haunted() =
    static member useState(init: 'T) : 'T * ('T -> unit) = importMember "haunted"
    static member useState(init: unit -> 'T) : 'T * ('T -> unit) = importMember "haunted"
    static member useEffect(effect: unit -> unit, deps: 'T []) : unit = importMember "haunted"
    static member useEffect(effect: unit -> unit -> unit, deps: 'T []) : unit = importMember "haunted"

    static member Virtual(renderFn: obj) : TemplateResult = import "virtual" "haunted"

    static member Component(renderFn: obj) : TemplateResult = import "component" "haunted"

    static member Component(renderFn: obj, ?opts: obj) : TemplateResult = import "component" "haunted"
