import React from 'react'
import {useState, useEffect} from 'react'
import { Box, Button, DialogContent, IconButton } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Typography, ThemeProvider, Paper, Dialog, DialogTitle, DialogActions, TextField} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import ModeEditIcon from '@mui/icons-material/ModeEdit';
import * as helpers from '../../utils/helpers'

interface props{
    text:string,
    setName: (text: string) => void,
    user: User | null
}

const NameField: React.FC<props> = ({text, setName, user}) => {
    const [dialogOpen, setDialogOpen] = useState<boolean>(false)
    const [textName, setTextName] = useState<string>("")
    const [showTextName, setShowTextName] = useState<string>("")

    const handleOpen = () => {
        setTextName("")
        setDialogOpen(true)
    }

    const handleClose =  () => {
        setDialogOpen(false)
    }

    const setNameInLocalStorage = async (name: string) => {
        // const oldUser = JSON.parse(localStorage.getItem('user') ?? "")
        // oldUser.name = textName
        // localStorage.setItem('user', JSON.stringify(oldUser));

        const u = await UserService.GetUserInfo(user?.id ?? '-1')
        if (u !== null){
            helpers.lsRemoveUser()
            helpers.lsSetUser(u)
        }

    }

    const initializeUsername = async (name: string) => {
        const u = await UserService.GetUserInfo(user?.id ?? '-1')
    }

    const updateUsername = async () => {
        if (user === null){
            return
        }
        const isOkay = await UserService.UpdateUserName(user.id, textName)
        if (isOkay){
            await setNameInLocalStorage(textName)
            setName(textName)
            setShowTextName(textName)
        }
        handleClose()
    }


    useEffect(() =>{
        setShowTextName(user?.name ?? "")
    }, [user])


    return(
        <>
            <ThemeProvider theme={theme}>
                <Stack direction="row" spacing={1}>
                    <Typography style={{ fontSize: '30px',color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }} variant="body2" component="div">
                        {showTextName}
                    </Typography>
                    <IconButton
                        style={{borderRadius: '2px'}}
                        onClick={async e => {
                            handleOpen()
                        }}>
                        <ModeEditIcon />
                    </IconButton>
                </Stack>
            </ThemeProvider>
            <Dialog open={dialogOpen} 
                fullWidth>
                <DialogTitle>Set new name for user</DialogTitle>
                <DialogContent>

                </DialogContent>
                <TextField variant='outlined' label='Name' onChange={async e => {
                    setTextName(e.target.value)
                }}></TextField>
                <DialogActions>
                    <Button
                        onClick={handleClose}>   
                        Cancel
                    </Button>
                    <Button
                        variant='contained'
                        onClick={updateUsername}>   
                        Set name
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    )
}

export default NameField