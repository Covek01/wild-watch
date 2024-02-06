import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import { useAuthContext } from '../../contexts/auth.context'
import { Sighting } from '../../models/Sighting'
import PetsIcon from '@mui/icons-material/Pets';

interface props{
    name:string
}

const NameLabel:React.FC<props>  = ({name}) => {

    return(
        <>
            <Stack direction="row">
                <PetsIcon style={{fontSize:"large"}}/>
                <Typography style={{ color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                    variant="h5" component="div">
                    {name}
                </Typography>
            </Stack>
        </>
    )
}

export default NameLabel