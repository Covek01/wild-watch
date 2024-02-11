import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import { useAuthContext } from '../../contexts/auth.context'
import NameLabel from './NameLabel'
import ClassLabel from './ClassLabel'
import { Species } from '../../models/Species'
import {useParams} from 'react-router-dom'
import SpeciesService from '../../services/SpeciesService'
import SightingsContainer from './SightingsContainer'

const SpeciesInfo: React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const [species, setSpecies] = useState<Species | null>(null)
    const params = useParams()
    const speciesId = params.id

    const initializeSpecies = async () => {
        const data = await SpeciesService.GetSpeciesInfo(speciesId ?? '-1')
        if (data){
            setSpecies(data)
        }
    }
    useEffect(() => {
        initializeSpecies()
    }, [])

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
                        src={species?.imageUrl}
                        sx={{ width: 200, height: 200 }}
                        variant="square"
                    />
                    <Stack direction="column" spacing={2}
                        style={{width: '60%', alignSelf: 'center'}}>
                        <NameLabel name={species?.commonName ?? ""} />
                        <ClassLabel name={species?.speciesClass?.toString() ?? ""} />
                        <Typography
                            style={{ color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                            variant="body2" component="div">
                            {`Latin name : ${species?.scientificName}`}
                        </Typography>
                        <Typography
                        style={{ fontSize: '16px', color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }}
                        variant="body2"
                        component="div">
                             {`Seen ${species?.sightings.length} times`}
                        </Typography>
                    </Stack>
                    </Stack>
                </Paper>
            </ThemeProvider>
            <Paper elevation={2}
                    variant="elevation"
                    sx={{width: '80%', marginLeft: 'auto', marginRight:'auto', marginTop: '1%'
                     }}>
                    <SightingsContainer sightings={species?.sightings ?? []}/>
            </Paper>
        </>
    )
}

export default SpeciesInfo