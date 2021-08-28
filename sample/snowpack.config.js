// Snowpack Configuration File
// See all supported options: https://www.snowpack.dev/reference/configuration
/** @type {import("snowpack").SnowpackUserConfig } */
module.exports = {
  mount: {
    public: { url: '/', static: true },
    src: { url: '/dist', static: true },
    '../src': { url: '/src/', static: true },
  },
  plugins: [],
  routes: [],
  optimize: {
    /* Example: Bundle your final build: */
    bundle: true,
    splitting: true,
    treeshake: true,
    manifest: true,
    target: 'es2017',
    minify: true
  },
  packageOptions: {
    /* ... */
    source: 'remote'
  },
  devOptions: {
    /* ... */
  },
  buildOptions: {
    /* ... */
    clean: true,
    out: "dist"
  },
  exclude: [
    "**/*.{fs,fsproj}",
    "**/bin/**",
    "**/obj/**"
  ],
  /* ... */
};