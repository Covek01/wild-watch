import React from 'react'
import {useState, useEffect} from 'react'
import { Box } from '@mui/material'
import {Stack, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import { useAuthContext } from '../../contexts/auth.context'
import SightingCard from './SightingCard'
import { SightingSummaryUser } from '../../models/Sighting'

interface props{
    sightings: SightingSummaryUser[]
}

const SightingsContainer:React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const [sightings, setSightings] = useState<SightingSummaryUser[] | null>(null)

    const initializeSightings = async () =>{
        const data = await UserService.GetMySightings()
        if (data !== null){
            setSightings(data)
        }
        
    }

    useEffect(() =>{
        initializeSightings()
    }, [])

    return(
        <>
            <Paper>
                <Box width="90%" margin="0 auto 0 auto">
                    <Stack direction="row" flexWrap="wrap" justifyContent="flex-start" spacing={0}>
                        {sightings?.reverse().map( (x, index) => (
                            <SightingCard key={index} sighting={x} height='90%' marginCards='8px'/>
                        ))}
                    </Stack>
                </Box>
            </Paper>
        </>
    )
}

export default SightingsContainer