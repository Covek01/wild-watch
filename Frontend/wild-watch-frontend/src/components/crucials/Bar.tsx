import React from 'react'
import { useNavigate } from 'react-router-dom'
import { AppBar, Toolbar, IconButton, Typography, Box, Button, Avatar, Menu, MenuItem, ThemeProvider } from "@mui/material";
import theme from '../../themes/Theme'

const Bar:React.FC = () => {
    const naviagate = useNavigate()

    const handleClickSpecies = () => {

    }

    const handleClickMap = () => {

    }

    const handleLogout = () => {

    }

    const handleAvatarClick = () => {

    }

    const handleClickSignIn = () => {

    }

    const handleClickSignUp = () => {

    }

    const handleClickMyProfile = () => {

    }

    return (
        <ThemeProvider theme={theme}>
        <div style={{ height: '63px' }}>
        <AppBar component="nav" color="primary">
            <Toolbar>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Typography
                        variant="h6"
                        component="div"
                        sx={{ display: 'flex', alignItems: 'center', mr: 2 }}
                    >
                        Wild Watch
                    </Typography>
                </Box>
                <Box sx={{ flexGrow: 1 }} />
                <Box sx={{ display: 'flex', alignItems: 'center' /*{ xs: 'none', sm: 'block' }*/ }}>
                    {/* isAuthenticated() ? <>
                        {amIAuthor ?
                            <IconButton color="inherit" onClick={handleWriteNews}
                                style={{ borderRadius: '10%', backgroundColor: theme.palette.primary.dark }}>
                                <NoteAddIcon />
                                <Typography sx={{ fontWeight: 'bold' }} textAlign="center">WRITE</Typography>
                            </IconButton> : <></>
                        } */}
                        <Button color="inherit" onClick={handleClickSpecies}>
                            <Typography sx={{ fontWeight: 'bold' }} textAlign="center">Species</Typography>
                        </Button>
                        <Button color="inherit" onClick={handleClickMap}>
                            <Typography sx={{ fontWeight: 'bold' }} textAlign="center">Map</Typography>
                        </Button>
                        <Button color="inherit" onClick={handleClickMyProfile}>
                            <Typography sx={{ fontWeight: 'bold' }} textAlign="center">My profile</Typography>
                        </Button>
                        {/* <Button
                            id="basic-button"
                            aria-controls={open ? 'basic-menu' : undefined}
                            aria-haspopup="true"
                            aria-expanded={open ? 'true' : undefined}
                            onClick={handleAvatarClick}
                        >
                            <Avatar
                                alt={user?.name ?? "User Name"}
                                src={
                                    user?.imageUrl && user?.imageUrl?.length > 0
                                        ? user?.imageUrl
                                        : "https://ui-avatars.com/api/?background=311b92&color=fff&name=N+F&rounded=true"
                                }
                            />
                        </Button> */}
                        {/* <Menu
                            id="basic-menu"
                            anchorEl={anchorEl}
                            open={open}
                            onClose={handleMenuClose}
                            MenuListProps={{
                                'aria-labelledby': 'basic-button',
                            }}
                        >
                            <MenuItem onClick={handleLogout}>Logout</MenuItem>
                        </Menu> */}
                    {/* </> */}
                        : <>
                            <Button color="inherit" onClick={handleClickSignIn}>
                                <Typography sx={{ fontWeight: 'bold' }} textAlign="center">SignIn</Typography>
                            </Button>
                            <Button color="inherit" onClick={handleClickSignUp}>
                                <Typography sx={{ fontWeight: 'bold' }} textAlign="center">SignUp</Typography>
                            </Button>
                        </>
                    
                </Box>
            </Toolbar>
        </AppBar>
    </div>
    </ThemeProvider>
    )
}

export default Bar