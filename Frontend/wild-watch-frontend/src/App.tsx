import React from 'react';
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

function App() {
  return (
    <>
      <AuthStateProvider>
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
      </AuthStateProvider>
    </>
  );
}

export default App;
