import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import Map from '../homepage/Map'
import { Box } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import NameField from './NameField'
import { useAuthContext } from '../../contexts/auth.context'

const MyProfilePage:React.FC = () => {

    return (
        <>
            <Bar />
        </>
    )
}