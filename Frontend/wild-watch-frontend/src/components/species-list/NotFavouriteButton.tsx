import GradeOutlinedIcon from '@mui/icons-material/GradeOutlined'
import { Species, FavouriteSpecies, SpeciesDto } from '../../models/Species'
import React from 'react'
import SpeciesListCard from './SpeciesListCard'
import { IconButton, Typography, Button } from '@mui/material'
import StarIcon from '@mui/icons-material/Star';

interface props{
    speciesId: string
    addToFavourites: any
}

const NotFavouriteButton:React.FC<props> = ({speciesId, addToFavourites}) => {


    return(
        <>
            <Button 
                color='secondary'
                variant='contained'
                sx={{borderRadius: '5px'}}
                onClick={async e => {
                addToFavourites(speciesId, 1)
            }}>
                <GradeOutlinedIcon />
                <Typography>Favourite</Typography>
            </Button>
        </>
    )
}

export default NotFavouriteButton

