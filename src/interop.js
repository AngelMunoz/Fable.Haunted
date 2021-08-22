
/**
 * A simple wrapper to define elements the global customElements registry
 * to be imported by fable code (just to be re-exported lol)
 * @param {string} name
 * @param {any} component
 **/
export function defineComponent(name, component) {
    window.customElements.define(name, component);
}