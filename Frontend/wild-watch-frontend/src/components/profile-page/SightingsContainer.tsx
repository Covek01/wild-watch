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
import { Sighting } from '../../models/Sighting'
import SightingService from '../../services/SightingService'

interface props{
    sightings: Sighting[]
}

const SightingsContainer:React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const [sightings, setSightings] = useState<Sighting[] | null>(null)

    const initializeSightings = async () =>{
        const data = await SightingService.GetMySightings()
        if (data !== null){
            setSightings(data)
        }
    }

    useEffect(() =>{
        initializeSightings()
    }, [])

    return(
        <>

        </>
    )
}

export default SightingsContainer