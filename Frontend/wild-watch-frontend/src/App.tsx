import React from 'react';
import logo from './logo.svg';
import './App.css';
import Bar from './components/crucials/Bar'
import { Routes, Route, Navigate } from "react-router-dom"
import Homepage from './components/homepage/Homepage'
import SpeciesInfo from './components/species-info/SpeciesInfo';

function App() {
  return (
    <>
      <Routes>
        <Route index element={<Navigate to="/speciesinfo" replace/>}></Route>
        <Route path="/homepage" element={<Homepage />} />
        <Route path="/speciesinfo" element={<SpeciesInfo />} />
      </Routes>
    </>
  );
}

export default App;
