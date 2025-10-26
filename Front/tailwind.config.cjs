module.exports = {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        syc: {
          50: '#f7fbff',
          100: '#eef7ff',
          200: '#dbeeff',
          300: '#b7ddff',
          400: '#7fbfff',
          500: '#4e9bff',
          600: '#2f7cea',
          700: '#235fb4',
          800: '#18407a',
          900: '#0f2a4f'
        },
        pastel: {
          blue: '#E8F3FF',
          mint: '#E8FFF7',
          peach: '#FFF2E8'
        }
      }
    }
  },
  plugins: []
}
