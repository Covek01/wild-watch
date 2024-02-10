import { Species, SpeciesDto } from '../../models/Species'
import {useParams} from 'react-router-dom'
import SpeciesService from '../../services/SpeciesService'
import React from 'react'
import Bar from '../crucials/Bar'
import { useAuthContext } from '../../contexts/auth.context'
import { CardActions, Card, Stack, CardMedia, CardHeader, CardContent, Typography } from '@mui/material'
import theme from '../../themes/Theme'
import FavouritesButton from './FavouritesButton'
import NotFavouriteButton from './NotFavouriteButton'


interface props{
    species: SpeciesDto,
    favourite: boolean
    updateFavourites: any
}

const SpeciesListCard:React.FC<props> = ({species, favourite, updateFavourites}) => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const dateTime = new Date()
    
    return(
        <>
            <Card
                style={{ backgroundColor: theme.palette.secondary.light, maxWidth: '120ch', maxHeight: '550px', width: '100%', margin: '10px' }}
                elevation={2}
            >
                <div style={{ backgroundColor: theme.palette.grey[200], cursor: 'pointer', maxWidth: '120ch', height: '20ch' }} className='flex flex-row'>
                <div className='flex items-center w-32' style={{paddingLeft: '1ch'}}>
                <CardMedia
                component="img"
                height="200"
                image={species.imageUrl}
                alt={"NO PICTURE"}
                style={{ objectFit: 'cover', width: '120px', height: '120px' }}
                />
                </div>
                <div className='flex flex-col'>
                <CardHeader
                title={species.commonName}
                subheader={species.scientificName}
                />
                <CardContent>
                <Typography variant="body1" component="div">
                    {`Description: ${species.description}`}
                </Typography>
                </CardContent>
                </div>
                </div>

                <CardActions sx={{ backgroundColor: theme.palette.primary.main }}>
                    {favourite ? 
                    <FavouritesButton speciesId={species.id} removeFromFavourites={updateFavourites}/>
                    :
                    <NotFavouriteButton speciesId={species.id} addToFavourites={updateFavourites}/>}
                </CardActions>
          </Card>
        </>
    )
}

export default SpeciesListCard