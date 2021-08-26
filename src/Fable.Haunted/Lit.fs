module Lit.Haunted

open Fable.Core.JsInterop

open Lit
open Haunted.Types
open Haunted


let private litRender renderFn container = import "render" "lit-html"

let private haunted
    (opts: {| render: obj -> obj -> TemplateResult |})
    : {| ``component``: obj -> obj option -> TemplateResult
         createContext: 'T -> Context<'T> |} =
    importDefault "haunted"

let private customHaunted<'T> : {| ``component``: obj -> option<obj> -> TemplateResult
                                   createContext: 'T -> Context<'T> |} =
    haunted {| render = fun renderFn container -> litRender renderFn container |}

type Haunted with

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() and virtual() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj) : TemplateResult =
        customHaunted.``component`` renderFn None

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() and virtual() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj, ?opts: obj) : TemplateResult =
        customHaunted.``component`` renderFn opts

    /// <summary>
    /// A helper function that returns a context value.
    /// </summary>
    /// <example>
    ///     let theme = Haunted.createContext "dark"
    ///
    ///     // register the provider and the consumer
    ///     defineComponent 'theme-provider' theme.Provider
    ///     defineComponent 'theme-provider' theme.Consumer
    ///     // use yout context
    /// </example>
    static member createContext(value: 'T) : Context<'T> = customHaunted.createContext value
