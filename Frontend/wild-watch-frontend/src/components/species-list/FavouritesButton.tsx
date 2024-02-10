import React from 'react'
import { Button, IconButton, Typography } from '@mui/material'
import StarIcon from '@mui/icons-material/Star';

interface props{
    speciesId: string
    removeFromFavourites: any
}

const FavouritesButton:React.FC<props> = ({speciesId, removeFromFavourites}) => {


    return(
        <>
            <Button
             color='secondary'
             variant='contained'
             sx={{borderRadius: '5px'}}
             onClick={async e => {
                removeFromFavourites(speciesId, -1)
            }}>
                <StarIcon />
                <Typography>Favourite</Typography>
            </Button>
        </>
    )
}

export default FavouritesButton

