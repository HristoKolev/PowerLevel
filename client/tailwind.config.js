module.exports = {
  content: ['src/**/*.{html,ts,tsx,js,jsx}'],
  theme: {
    extend: {},
    screens: {
      tablet: '640px',
      laptop: '1024px',
      desktop: '1280px',
    },
  },
  plugins: [],
  corePlugins: {
    preflight: false,
  },
};
