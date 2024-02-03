import React, { createContext } from 'react';
import logo from './logo.svg';
import './App.css';
import Bar from './components/crucials/Bar'
import { Routes, Route, Navigate } from "react-router-dom"
import Homepage from './components/homepage/Homepage'
import { AuthStateProvider } from './contexts/auth.context';
import AuthenticatedGuard from './routeGuards/AuthenticatedGuard';
import { SignIn } from './components/signIn/signIn';
import { SignUp } from './components/signup/signup';
import SpeciesInfo from './components/species-info/SpeciesInfo';
import theme from './themes/Theme';
import { ThemeProvider } from '@emotion/react';
import { SnackbarProvider } from './contexts/snackbar.context';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

function App() {
  return (
    <>
      <ThemeProvider theme={theme}>
        <SnackbarProvider>
          <AuthStateProvider>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <Routes>
                <Route>
                  <Route index element={<Navigate to="/speciesinfo" replace />}></Route>
                  <Route path="/homepage" element={<Homepage />} />
                  <Route path="/speciesinfo" element={<SpeciesInfo />} />

                  <Route
                    path="/signin"
                    element={<AuthenticatedGuard>{<SignIn />}</AuthenticatedGuard>}
                  ></Route>

                  <Route
                    path="/signup"
                    element={<AuthenticatedGuard>{<SignUp />}</AuthenticatedGuard>}
                  ></Route>
                </Route>
              </Routes>
            </LocalizationProvider>
          </AuthStateProvider>
        </SnackbarProvider>
      </ThemeProvider>
    </>
  );
}

export default App;
