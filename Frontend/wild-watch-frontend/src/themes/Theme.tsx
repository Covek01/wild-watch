import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      light: '#a2cf6e',
      main: '#8bc34a',
      dark: '#618833',
      contrastText: '#fff',
    },
    secondary: {
      light: '#e0e0e0',
      main: '#9e9e9e',
      dark: '#424242',
      contrastText: '#000',
    },
  },
});

export default theme