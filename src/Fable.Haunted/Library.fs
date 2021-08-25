[<AutoOpen>]
module Fable.Haunted.Haunted

open Browser.Types

open Fable.Core
open Fable.Core.JsInterop

open Lit

open Fable.Haunted.Types


/// <summary>
/// Register a render function in the custom elements registry.
/// Once a function is registered, you can create new tags of that component.
/// </summary>
/// <example>
///      let greeter props =
///        html $"""&lt;div&gt;Hello, {props.name}!&lt;/div&gt;"""
///
///      // Register the render function
///      defineComponent "my-greeter" (Haunted.Component greeter)
///
///     // Usage
///     let app() =
///         let name = "fsharp"
///         html $"""
///           &lt;!-- Use previously registered component --&gt;
///           &lt;my-greeter .name={name}&gt;&lt;/my-greeter&gt;
///        """
/// </example>
[<Emit("customElements.define($0, $1)")>]
let defineComponent (name: string) (comp: obj) : unit = jsNative

/// <summary>
/// dispatchEvent **must** be caled inside a render function (it uses `this`) to correctly dispatch events
/// </summary>
/// <example>
///     let app () =
///        let onMyEvent _ =
///            let evt = Haunted.createEvent "my-event"
///            // dispatch the event
///            dispatchEvent evt
///
///       // the rest of your function
/// </example>
[<Emit("$1.dispatchEvent($0)")>]
let dispatchEvent (event: Event) (this: obj) : unit = jsNative

/// <summary>
/// A simple static class that serves to provide signature overloads for [Haunted](https://hauntedhooks.netlify.app/) functions
/// </summary>
type Haunted() =
    /// <summary>
    /// returns a tuple with an immutable value and a setter function for the provided value
    /// </summary>
    /// <example>
    ///     let counter, setCounter = Haunted.useState 0
    /// </example>
    static member useState(init: 'T) : 'T * ('T -> unit) = importMember "haunted"

    /// <summary>
    /// returns a tuple with an immutable value and a setter function, when you supply a callback it will be used
    /// to initialize the value but it will not be called again
    /// </summary>
    /// <example>
    ///     let counter, setCounter = Haunted.useState (fun _ -> expensiveInitializationLogic(0))
    /// </example>
    static member useState(init: unit -> 'T) : 'T * ('T -> unit) = importMember "haunted"

    /// <summary>
    /// Used to run a side-effect when the component re-renders or when a dependency changes.
    /// To run your side-effect only when the component rerenders, only pass in your side-effect function and nothing else
    /// </summary>
    /// <example>
    ///     let app () =
    ///         let counter, setCounter = Haunted.useState 0
    ///         Haunted.useEffect (fun _ -> printfn "log to the console on every re-render")
    ///         html $"""
    ///             &lt;header>Click the counter&lt;/header>
    ///             &lt;div id="count">{counter}&lt;/div>
    ///             &lt;button type="button" @click=${fun _ -> setCount(counter + 1)}>
    ///               Cause rerender
    ///             &lt;/button>
    ///        """
    /// </example>
    static member useEffect(effect: unit -> unit, ?deps: 'T []) : unit = importMember "haunted"

    /// <summary>
    /// Used to run a side-effect when the component re-renders or when a dependency changes.
    /// Since effects are used for side-effectual things and might run many times in the lifecycle of a component, `useEffect` supports returning a teardown function.
    /// </summary>
    static member useEffect(effect: unit -> (unit -> unit), ?deps: 'T []) : unit = importMember "haunted"

    /// <summary>
    /// Very similar to useMemo but instead of writing a function that returns your memoized value, your function is the memoized value. This and useMemo are often overused so try to only use this when your callback has dependencies and it is itself a dependency for something else (like useEffect) .
    /// </summary>
    static member useCallback<'MemoFunction>(callback: 'MemoFunction, ?deps: obj []) : 'MemoFunction =
        importMember "haunted"

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
    static member createContext<'T>(value: 'T) : Context<'T> = importMember "haunted"

    /// <summary>
    /// Grabs the context value from the closest provider above and updates your component, the consumer, whenever the provider changes the value.
    /// useContext currently only works with custom element components.
    /// </summary>
    /// <example>
    ///     let theme = Haunted.createContext "dark"
    ///
    ///     // register the provider and the consumer
    ///     defineComponent 'theme-provider' theme.Provider
    ///     defineComponent 'theme-provider' theme.Consumer
    ///
    ///     // (optional) create and define a custom consumer
    ///     let Consumer () =
    ///         let context = Haunted.useContext theme
    ///         context
    ///
    ///     defineComponent 'my-consumer' (Haunted.Component Consumer)
    ///
    ///     let renderValue value =
    ///         html $"""&lt;h1>{value}&lt;/h1>"""
    ///
    ///     let App () =
    ///
    ///         let theme, setTheme = Hautned.useState "light"
    ///
    ///         html $"""
    ///              &lt;select value={theme} @change={fun _ -> setTheme(event.target.value)}>
    ///               &lt;option value="dark">Dark&lt;/option>
    ///               &lt;option value="light">Light&lt;/option>
    ///             &lt;/select>
    ///
    ///             &lt;theme-provider .value={theme}>
    ///               &lt;my-consumer>&lt;/my-consumer>
    ///
    ///               &lt;!-- creates context with inverted theme -->
    ///               &lt;theme-provider .value={theme = 'dark' ? 'light' : 'dark'}>
    ///                 &lt;theme-consumer
    ///                   .render={renderValue}
    ///                 >&lt;/theme-consumer>
    ///               &lt;/theme-provider>
    ///             &lt;/theme-provider>
    ///        """
    /// </example>
    static member useContext<'T>(value: Context<'T>) : 'T = importMember "haunted"

    /// <summary>
    /// Create a memoized state value. Only reruns the function when dependent values have changed.
    /// </summary>
    static member useMemo<'T>(callback: unit -> 'T, deps: obj []) : 'T = importMember "haunted"

    /// <summary>
    ///  Similarly to useState, useReducer will return an array of two values, the first one being the state. The second one, however, is dispatch.
    ///  This is a function that takes an action.
    ///  The action is then passed to your reducer (the first argument) and your reducer will determine the new state and return it.
    /// </summary>
    static member useReducer<'Init, 'State, 'Action>
        (
            reducer: Reducer<'State, 'Action>,
            init: 'Init
        ) : ('State * ('Action -> unit)) =
        importMember "haunted"

    /// <summary>
    ///  Similarly to useState, useReducer will return an array of two values, the first one being the state. The second one, however, is dispatch.
    ///  This is a function that takes an action.
    ///  The action is then passed to your reducer (the first argument) and your reducer will determine the new state and return it.
    /// </summary>
    static member useReducer<'Init, 'State, 'Action>
        (
            reducer: Reducer<'State, 'Action>,
            init: 'Init,
            ?initFn: 'Init -> 'State
        ) : ('State * ('Action -> unit)) =
        importMember "haunted"

    /// <summary>
    /// Creates and returns a mutable object (a 'ref') whose .current property is initialized to the passed argument.
    /// This differs from useState in that state is immutable and can only be changed via setState which will cause a rerender.
    /// That rerender will allow you to be able to see the updated state value. A ref, on the other hand, can only be changed via
    /// .current and since changes to it are mutations, no rerender is required to view the updated value in your component's code (e.g. listeners, callbacks, effects).
    /// </summary>
    static member useRef<'T>(value: 'T) : Ref<'T> = importMember "haunted"

    /// <summary>
    /// Haunted also has the concept of virtual components. These are components that are not defined as a tag.
    /// Instead they're defined as functions that can be called from within another template.
    /// They have their own state and will rerender when that state changes without causing any parent components to rerender.
    /// </summary>
    static member Virtual(renderFn: obj) : TemplateResult = import "virtual" "haunted"

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() and virtual() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj) : TemplateResult = import "component" "haunted"

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() and virtual() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj, ?opts: obj) : TemplateResult = import "component" "haunted"


    /// <summary>
    ///  Creates a new Javascript [Event](https://developer.mozilla.org/en-US/docs/Web/API/Event)
    ///  for it to be dispatched later on.
    /// </summary>
    /// <example>
    ///     let app () =
    ///        let onMyEvent _ =
    ///            // create an event that passes through shadow dom (composed = true)
    ///            let evt =
    ///                Haunted.createEvent
    ///                    ("my-event",
    ///                     {| bubbles = true; composed = true;|})
    ///            dispatchEvent evt
    ///        // the rest of your function
    /// </example>
    [<Emit("new Event($0, $1)")>]
    static member createEvent(name: string, ?opts: obj) : Browser.Types.Event = jsNative

    /// <summary>
    ///  Creates a new Javascript [CustomEvent](https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent)
    ///  for it to be dispatched later on.
    /// </summary>
    /// <example>
    ///     let app () =
    ///        let onMyEvent _ =
    ///            // create an event that passes through shadow dom (composed = true)
    ///            // with custom data (detail can be anything, not just a string)
    ///            let evt =
    ///                Haunted.createCustomEvent
    ///                    ("my-event",
    ///                     {| bubbles = true; composed = true; detail = "my data"|})
    ///            dispatchEvent evt
    ///        // the rest of your function
    /// </example>
    [<Emit("new CustomEvent($0, $1)")>]
    static member createCustomEvent<'T>(name: string, ?opts: obj) : Browser.Types.CustomEvent<'T> = jsNative

    /// <summary>
    /// Returns `this`, use it to fix a reference to `this` in a function.
    /// </summary>
    /// <remarks>This will not work on arrow functions (i.e. Virtual components)</remarks>
    /// <example>
    ///     let app () =
    ///         // that will point to the "app()" scope
    ///         let that = Haunted.getThis
    ///         let onMyEvent _ =
    ///             console.log(that)
    ///     html $"""&lt;button @click={onMyEvent}>&lt;/button>"""
    /// </example>
    [<Emit("this")>]
    static member getThis: obj = jsNative
