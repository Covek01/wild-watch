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


const MyProfilePage:React.FC = () => {
   //const { isAuthenticated, signout, user } = useAuthContext();
    const [user, setUser] = useState<User | null>(null)
    const [userNameField, setUserNameField] = useState<string>("")
    const id = '65bd3ac5272027bd7218c6ae'

    const setInitialUserInfo = async () => {
        const userInfo = await UserService.GetSpeciesInfo(id)
        setUser(userInfo)
        setUserNameField(userInfo?.name ?? "")
    }

    const setUserName = (name: string) => {
        
    }

    useEffect(() => {
        setInitialUserInfo()
    }, [])

    return (
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
                        <NameField text={userNameField} setName={setUserName}/>
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
            </ThemeProvider>
        </>
    )
}

  export default MyProfilePage