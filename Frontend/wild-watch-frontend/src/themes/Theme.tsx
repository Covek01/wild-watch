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
      light: '#e3eb90',
      main: '#dce775',
      dark: '#9aa151',
      contrastText: '#000',
    },
  },
});

export default theme