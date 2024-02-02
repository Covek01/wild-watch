import React from 'react';
import logo from './logo.svg';
import './App.css';
import Bar from './components/crucials/Bar'
import { Routes, Route, Navigate } from "react-router-dom"
import Homepage from './components/homepage/Homepage'

function App() {
  return (
    <>
      <Routes>
        <Route index element={<Navigate to="/homepage" replace/>}></Route>
        <Route path="/homepage" element={<Homepage />} />
      </Routes>
    </>
  );
}

export default App;
