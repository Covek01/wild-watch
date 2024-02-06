import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import { useAuthContext } from '../../contexts/auth.context'
import { Sighting } from '../../models/Sighting'
import NameLabel from './NameLabel'

const SpeciesInfo: React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const id = '65bd3e793867af7cce872dd7'

    return(
        <>
             <ThemeProvider theme={theme}>
                <Bar />
                <Paper
                    elevation={4}
                    variant="outlined"
                    style={{width: '60%', backgroundColor: theme.palette.secondary.dark,
                     marginLeft: 'auto', marginRight:'auto', marginTop: '1%',
                     }}>
                    <Stack direction="row" spacing={1} justifyContent="space-between">
                    <Avatar
                        style={{marginTop: '2%', marginBottom:'2%', marginLeft:'2%'}}
                        alt="NO PICTURE"
                        src={user?.imageUrl}
                        sx={{ width: 200, height: 200 }}
                        variant="square"
                    />
                    <Stack direction="column" spacing={2}
                        style={{width: '60%', alignSelf: 'center'}}>
                        <NameLabel name='zivotinja' />
                        <Typography
                            style={{ color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                            variant="body2" component="div">
                            {`Latin name : ${user?.email}`}
                        </Typography>
                        <Typography
                        style={{ fontSize: '16px', color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                        variant="body2"
                        component="div">
                             {`Seen ${14} times`}
                        </Typography>
                    </Stack>
                    </Stack>
                </Paper>
            </ThemeProvider>
        </>
    )
}

export default SpeciesInfo