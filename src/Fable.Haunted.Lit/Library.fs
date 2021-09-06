module Haunted.Lit

open Fable
open Fable.Core
open Haunted.Types
open System
open Lit
open Fable.Core.JsInterop


let private litRender renderFn container = import "render" "lit-html"

let private haunted
    (opts: {| render: obj -> obj -> TemplateResult |})
    : {| ``component``: obj -> obj option -> TemplateResult
         createContext: 'T -> Context<'T> |} =
    importDefault "haunted"

/// use the user installed lit render function rather than the default from haunted
let private customHaunted<'T> : {| ``component``: obj -> option<obj> -> TemplateResult
                                   createContext: 'T -> Context<'T> |} =
    haunted {| render = fun renderFn container -> litRender renderFn container |}

type Haunted with
    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj) : TemplateResult =
        customHaunted.``component`` renderFn None

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
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
