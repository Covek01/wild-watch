import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import Map from '../homepage/Map'
import { Box } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Avatar, Typography, ThemeProvider, Paper, Card, CardContent, CardActions,
Collapse, IconButton, CardMedia, CardHeader, Button} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import NameField from './NameField'
import { useAuthContext } from '../../contexts/auth.context'
import { Sighting } from '../../models/Sighting'
import * as statusColor from '../../themes/StatusColor'
import { useNavigate } from 'react-router-dom'
import { SightingSummaryUser } from '../../models/Sighting'


interface props{
    sighting: SightingSummaryUser,
    height: string,
    marginCards: string
}

const SightingCard:React.FC<props> = ({sighting, height, marginCards}) => {
    const navigate = useNavigate()
    const [statusColor, setStatusColor] = useState<string>()

    const handleClickSpeciesOpen = () => {
        console.log(sighting)
        navigate(`/speciesinfo/${sighting.species.speciesId}`)
    }

    return (
        <>
            <Card sx={{width: '32%', height: {height}, margin: `${marginCards} ${marginCards} ${marginCards} 0`}}>
                {/* <CardMedia
                    component="img"
                    image={sighting.imageUrl}
                    alt="ANACONDA"
                    sx={{objectFit: "contain", height: '200px' }}
                /> */}
                <Avatar
                        alt="NO PICTURE"
                        src={sighting.imageUrl}
                        sx={{ width: '100%', height: 200, marginLeft: 'auto', marginRight: 'auto'}}
                        variant="square"
                />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                        {sighting.species.commonName}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        {sighting.species.conservationStatus}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                        {'September 14, 2016'}
                    </Typography>

                </CardContent>
                <CardActions disableSpacing sx={{justifyContent: 'flex-end'}}>
                    <Button size="small"
                    onClick={handleClickSpeciesOpen}
                    variant='outlined'
                    sx={{alignSelf: 'end'}}>
                        Species info
                    </Button>
                </CardActions>
            </Card>
        </>
    )
}

export default SightingCard