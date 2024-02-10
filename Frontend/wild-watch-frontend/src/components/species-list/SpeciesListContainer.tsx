
import { Species, FavouriteSpecies, SpeciesDto } from '../../models/Species'
import {useParams} from 'react-router-dom'
import SpeciesService from '../../services/SpeciesService'
import React from 'react'
import Bar from '../crucials/Bar'
import { useAuthContext } from '../../contexts/auth.context'
import UserService from '../../services/UserService'
import SpeciesListCard from './SpeciesListCard'

interface props{
    myFavouriteSpecies: string[]
    species: SpeciesDto[]
    updateFavourites: any
}

const SpeciesListContainer:React.FC<props> = ({updateFavourites, myFavouriteSpecies, species}) => {

    const isMySpecies = (s: SpeciesDto) => {
        const data = myFavouriteSpecies.filter(x => x == s.id)
        return (data.length > 0)? true : false
    }

    return(
        <>
            {species.map(x => (
                <SpeciesListCard key={x.id} species={x} favourite={isMySpecies(x)} updateFavourites={updateFavourites} />
            ))}
        </>
    )
}

export default SpeciesListContainer

