import React, { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { AppBar, Toolbar, IconButton, Typography, Box, Button, Avatar, Menu, MenuItem, ThemeProvider } from "@mui/material";
import theme from '../../themes/Theme'
import { useAuthContext } from '../../contexts/auth.context';

const Bar: React.FC = () => {
    const navigate = useNavigate()
    const { isAuthenticated, signout, user } = useAuthContext();
    const handleClickSpecies = () => {
         navigate("/species");
    }

    const handleClickMap = () => {
        navigate("/map");
    }

    const handleClickSignIn = () => {
        navigate("/signin");
    }

    const handleClickSignUp = () => {
        navigate("/signup");
    }

    const handleClickMyProfile = () => {
        navigate("/myprofile")
    }

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleAvatarClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        setAnchorEl(null);
        signout();
    }


    useEffect(() => {
        console.log(JSON.stringify(user))
    }, [])

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
                            {isAuthenticated() ? <>

                                <Button color="inherit" onClick={handleClickMap}>
                                    <Typography sx={{ fontWeight: 'bold' }} textAlign="center">Map</Typography>
                                </Button>
                                <Button color="inherit" onClick={handleClickSpecies}>
                                    <Typography sx={{ fontWeight: 'bold' }} textAlign="center">Species</Typography>
                                </Button>
                                <Button color="inherit" onClick={handleClickMyProfile}>
                                    <Typography sx={{ fontWeight: 'bold' }} textAlign="center">My Profile</Typography>
                                </Button>
                                <Button
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
                                                : "https://ui-avatars.com/api/?background=311b92&color=fff&name=W+W&rounded=true"
                                        }
                                    />
                                </Button>
                                <Menu
                                    id="basic-menu"
                                    anchorEl={anchorEl}
                                    open={open}
                                    onClose={handleMenuClose}
                                    MenuListProps={{
                                        'aria-labelledby': 'basic-button',
                                    }}
                                >
                                    <MenuItem onClick={handleLogout}>Logout</MenuItem>
                                </Menu>
                            </>
                                : <>
                                    <Button color="inherit" onClick={handleClickSignIn}>
                                        <Typography sx={{ fontWeight: 'bold' }} textAlign="center">SignIn</Typography>
                                    </Button>
                                    <Button color="inherit" onClick={handleClickSignUp}>
                                        <Typography sx={{ fontWeight: 'bold' }} textAlign="center">SignUp</Typography>
                                    </Button>
                                </>
                            }

                        </Box>
                    </Toolbar>
                </AppBar>
            </div>
        </ThemeProvider>
    )
}

export default Bar