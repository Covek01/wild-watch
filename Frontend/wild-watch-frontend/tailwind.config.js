// /** @type {import('tailwindcss').Config} */
// module.exports = {
//   content: [
//     "./src/**/*.{js,jsx,ts,tsx}",
//   ],
//   theme: {
//     extend: {},
//   },
//   plugins: [],
// }
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    colors: {
      
        p_light: '#a2cf6e',
        p_main: '#8bc34a',
        p_dark: '#618833',
        p_contrastText: '#fff',
    
        s_light: '#e0e0e0',
        s_main: '#9e9e9e',
        s_dark: '#424242',
        s_contrastText: '#000',
    },
    extend: {},
  },
  plugins: [],
}