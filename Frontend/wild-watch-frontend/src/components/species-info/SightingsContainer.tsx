import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import Map from '../homepage/Map'
import { Box } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import { useAuthContext } from '../../contexts/auth.context'
import { Sighting, SightingSummarySpecies } from '../../models/Sighting'
import SightingService from '../../services/SightingService'
import SightingCard from './SightingCard'
import { Palette } from '@mui/icons-material'
import VisibilityIcon from '@mui/icons-material/Visibility';
import { SightingSummaryUser } from '../../models/Sighting'

interface props{
    sightings: SightingSummarySpecies[]
}

const SightingsContainer:React.FC<props> = ({sightings}) => {
    return(
        <>
         

            <Paper>
                <Box width="90%" margin="0 auto 0 auto">
                    <Stack direction="row" flexWrap="wrap" justifyContent="flex-start" spacing={0}>
                        {sightings?.map( (x, index) => (
                            <SightingCard key={index} sighting={x} height='90%' marginCards='8px'/>
                        ))}
                    </Stack>
                </Box>
            </Paper>
        </>
    )
}

export default SightingsContainer