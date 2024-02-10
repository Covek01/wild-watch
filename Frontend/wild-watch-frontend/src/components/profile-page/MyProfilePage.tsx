import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import Map from '../homepage/Map'
import { Box } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper, Icon, SvgIcon} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import NameField from './NameField'
import { useAuthContext } from '../../contexts/auth.context'
import SightingsContainer from './SightingsContainer'
import { Sighting } from '../../models/Sighting'
import VisibilityIcon from '@mui/icons-material/Visibility';
import ForestIcon from '@mui/icons-material/Forest';


const MyProfilePage:React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const [userNameField, setUserNameField] = useState<string>("")

    const setUserName = (name: string) => {
        setUserNameField(name)
    }

    return (
        <>
            <ThemeProvider theme={theme}>
                <Bar />
                <Paper
                    elevation={4}
                    variant="outlined"
                    sx={{width: '60%', backgroundColor: theme.palette.secondary.dark,
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
                        sx={{width: '60%', alignSelf: 'center'}}>
                        <NameField text={userNameField} setName={setUserName} user={user}/>
                        <Typography style={{ fontSize: '20px',color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }} variant="body2" component="div">
                            {`Contact : ${user?.email}`}
                        </Typography>
                        <Typography
                        style={{ fontSize: '20px', color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                        variant="body2"
                        component="div">
                        {/* {`Born: ${user?.dateOfBirth.getDate() ?? 0}
                        .${user?.dateOfBirth.getMonth() ?? 0 + 1}
                        .${user?.dateOfBirth.getFullYear()}`} */} Born: 2.2.2002.
                        </Typography>
                    </Stack>
                    </Stack>
                </Paper>
                <Stack direction="row" spacing={2}
                    sx={{width: '80%',marginLeft: 'auto', marginRight:'auto', marginTop: '2%',
                }}>
                    <SvgIcon fontSize='large'>
                        <ForestIcon />
                    </SvgIcon>

                    <Typography variant="h5" component="div" 
                        sx={{backgroundColor: theme.palette.primary.contrastText, }}>
                            Sightings
                    </Typography>
                </Stack>
                <Paper elevation={2}
                    variant="elevation"
                    sx={{width: '80%', marginLeft: 'auto', marginRight:'auto', marginTop: '1%'
                     }}>
                    <SightingsContainer />
                </Paper>
            </ThemeProvider>
        </>
    )
}

  export default MyProfilePage