import { Species } from '../../models/Species'
import {useParams} from 'react-router-dom'
import SpeciesService from '../../services/SpeciesService'
import React from 'react'
import Bar from '../crucials/Bar'
import { useAuthContext } from '../../contexts/auth.context'
import { CardActions, Card, Stack } from '@mui/material'

const SpeciesList:React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    
    return(
        <>
            <Card>
                <Stack direction="row">
                
                </Stack>
                <CardActions>

                </CardActions>
            </Card>
        </>
    )
}

