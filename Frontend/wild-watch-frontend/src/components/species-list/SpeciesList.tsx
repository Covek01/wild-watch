
import { Species, FavouriteSpecies, SpeciesDto } from '../../models/Species'
import {useParams} from 'react-router-dom'
import SpeciesService from '../../services/SpeciesService'
import React from 'react'
import Bar from '../crucials/Bar'
import { useAuthContext } from '../../contexts/auth.context'
import UserService from '../../services/UserService'
import { Box, TextField, Stack, Typography, Icon, SvgIcon } from '@mui/material'
import { Label } from '@mui/icons-material'
import FilterAltIcon from '@mui/icons-material/FilterAlt';
import LandscapeIcon from '@mui/icons-material/Landscape';
import SpeciesListContainer from './SpeciesListContainer'
import * as helpers from '../../utils/helpers'
import { useEffect } from 'react'
import {User} from '../../models/User'

const SpeciesList:React.FC = () => {
    const { isAuthenticated, signout, user } = useAuthContext();
    const [userState, setUserState] = React.useState<User | null>(null)
    const [mySpecies, setMySpecies] = React.useState<FavouriteSpecies[]>([])
    const [mySpeciesIds, setMySpeciesIds] = React.useState<string[]>([])
    const [species, setSpecies] = React.useState<SpeciesDto[]>([])
    const [filterTextValue, setFilterTextValue] = React.useState<string>('')
    const initializeSpeciesList = async () => {
        const data = await UserService.GetMyFavouriteSpecies()
        setMySpecies(data)
    }

    const onChangeFilter = async (text: string) => {
        setFilterTextValue(text)
        const data = await SpeciesService.GetByCommonNameFiltered(text)
        if (data.length > 0){
            setSpecies(data)
        }
        console.log(userState)
        console.log(mySpeciesIds)
    }

    const setSpeciesList = async () => {
      
    }

    async function updateMyFavourites(id: string, count: number) {
        if (count > 0){
            const favs = await UserService.AddToMySpecies(id)
            setMySpeciesIds(favs ?? [])
        }
        else{
            const favs = await UserService.RemoveFromSpecies(id)
            setMySpeciesIds(favs ?? [])
        }

    }

    const initializeFavourites = async () => {
        if (user !== null){
            const info = await UserService.GetUserInfo(user?.id ?? '-1')
            console.log(info?.favouriteSpecies)
        }

       
        //setMySpeciesIds(info.)
    }
    
    async function proba(){
        const info = await UserService.GetMyFavouriteSpeciesIds()
        setMySpeciesIds(info)
    }

    useEffect(() => {
        proba()
        console.log(user)
        setUserState(user)
      }, [userState]);

    return(
        <>
            <Bar />
            <Box
                sx={{width: '80%', margin: 'auto auto auto auto'}}
                component="form"
                noValidate
                autoComplete="off"
                >
                <Stack direction="row" spacing={3} sx={{margin: '3ch 0 3ch 0'}}>
                    <SvgIcon sx={{alignSelf: 'center'}} fontSize='large'>
                        <LandscapeIcon />
                    </SvgIcon>
                    <TextField id="outlined-basic" label="Species" variant="outlined" onChange={async e => {
                        onChangeFilter(e.target.value)
                    }}
                    sx={{width: '50ch'}}/>
                </Stack>
                <SpeciesListContainer updateFavourites={updateMyFavourites} myFavouriteSpecies={mySpeciesIds} species={species}/>
            </Box>
           
        </>
    )
}

export default SpeciesList