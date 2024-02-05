import React from 'react'
import {useState, useEffect} from 'react'
import Bar from '../crucials/Bar'
import Map from '../homepage/Map'
import { Box, Button, DialogContent, IconButton } from '@mui/material'
import {User} from '../../models/User'
import {Stack, Typography, ThemeProvider, Paper, Dialog, DialogTitle, DialogActions, TextField} from "@mui/material"
import UserService from '../../services/UserService'
import theme from '../../themes/Theme'
import ModeEditIcon from '@mui/icons-material/ModeEdit';

interface props{
    text:string,
    setName: (text: string) => void
}

const NameField: React.FC<props> = ({text, setName}) => {
    const [dialogOpen, setDialogOpen] = useState<boolean>(false)
    const [textName, setTextName] = useState<string>("")

    const handleOpen = () => {
        setTextName("")
        setDialogOpen(true)
    }

    const handleClose =  () => {
        setDialogOpen(false)
    }

    const updateUsername = () => {

        handleClose()
    }


    return(
        <>
            <ThemeProvider theme={theme}>
                <Stack direction="row" spacing={1}>
                    <Typography style={{ fontSize: '30px',color: theme.palette.primary.contrastText, marginRight: '5%', marginLeft: '15%' }} variant="body2" component="div">
                        {text}
                    </Typography>
                    <IconButton
                        onClick={async e => {
                            handleOpen()
                        }}>
                        <ModeEditIcon />
                    </IconButton>
                </Stack>
            </ThemeProvider>
            <Dialog open={dialogOpen} 
                fullWidth >
                <DialogTitle>Set new name for user</DialogTitle>
                <DialogContent>

                </DialogContent>
                <TextField variant='outlined' label='Name' onChange={async e => {
                    setTextName(e.target.value)
                    console.log("VALUE FOR TEXTNAME IS" + textName)
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