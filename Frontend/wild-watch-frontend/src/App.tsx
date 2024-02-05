import React from 'react';
import logo from './logo.svg';
import './App.css';
import Bar from './components/crucials/Bar'
import { Routes, Route, Navigate } from "react-router-dom"
import Homepage from './components/homepage/Homepage'
import SpeciesInfo from './components/species-info/SpeciesInfo'
import MyProfilePage from './components/profile-page/MyProfilePage';

function App() {
  return (
    <>
      <Routes>
        <Route index element={<Navigate to="/myprofile" replace/>}></Route>
        <Route path="/homepage" element={<Homepage />} />
        <Route path="/speciesinfo" element={<SpeciesInfo />} />
        <Route path="/myprofile" element={<MyProfilePage />} />
      </Routes>
    </>
  );
}

export default App;
