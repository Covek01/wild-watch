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
import SpeciesInfo from './components/species-info/SpeciesInfo'
import MyProfilePage from './components/profile-page/MyProfilePage';
import theme from './themes/Theme';
import { ThemeProvider } from '@emotion/react';
import { SnackbarProvider } from './contexts/snackbar.context';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import NotAuthenticatedGuard from './routeGuards/NotAuthenticatedGuard';
import Map from './components/mappage/map';
import { MapStateProvider } from './contexts/map.context';
import SpeciesList from './components/species-list/SpeciesList';
import LandingPage from './components/landing/landingPage';

function App() {
  return (
    <>
      <ThemeProvider theme={theme}>
        <SnackbarProvider>
          <AuthStateProvider>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <MapStateProvider>
                <Routes>
                  <Route>
                    <Route index element={<Navigate to="/map" replace />}></Route>
                    <Route path="/homepage" element={<Homepage />} />
                    <Route path="/myprofile" element={<NotAuthenticatedGuard>{<MyProfilePage />}</NotAuthenticatedGuard>}></Route>
                    <Route path="/speciesinfo/:id" element={<NotAuthenticatedGuard>{<SpeciesInfo />}</NotAuthenticatedGuard>}></Route>
                    <Route path="/species" element={<NotAuthenticatedGuard>{<SpeciesList />}</NotAuthenticatedGuard>}></Route>
                    <Route path="/map" element={<NotAuthenticatedGuard>{<Map />}</NotAuthenticatedGuard>}></Route>
                    <Route path="/landing" element={<LandingPage />}></Route>
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
              </MapStateProvider>
            </LocalizationProvider>
          </AuthStateProvider>
        </SnackbarProvider>
      </ThemeProvider>
    </>
  );
}

export default App;
