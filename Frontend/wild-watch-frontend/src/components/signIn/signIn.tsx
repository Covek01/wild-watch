import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../contexts/auth.context"
import { useForm } from "react-hook-form";
// import { api } from "../../services/Service";
import { User } from "../../models/User";
import { Box, Button, Grid, SnackbarContent, Stack, TextField } from "@mui/material";
import UserService from "../../services/UserService";
import { useSnackbar } from "../../contexts/snackbar.context";


export const SignIn: React.FC = () => {
    const snackBar = useSnackbar();
    const { signin } = useAuthContext();
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<{ email: string, password: string }>({
        reValidateMode: "onSubmit",
    });

    const onSubmit = handleSubmit(async (creds) => {
        const { data, status } = await UserService.Signin(creds.email, creds.password);

        if (status === 200) {
            signin({ user: data, authToken: data.accessToken });
            navigate('/');
        } else {
            snackBar.openSnackbar({
                message: 'Email or password wrong',
                severity: 'error',
            })
        }
    });


    return (
        <Box
            sx={{
                height: "100vh",
                p: "1em",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
            }}
        >
            <Stack
                component="form"
                onSubmit={onSubmit}
                sx={{
                    display: "flex",
                    gap: "0.8em",
                    width: { xs: "100%", sm: "80%" },
                    maxWidth: "600px",
                }}
            >
                <TextField
                    label="Email"
                    className="form-field"
                    type="email"
                    {...register("email", { required: true })}
                    error={Boolean(errors.email)}
                ></TextField>
                <TextField
                    label="Password"
                    className="form-field"
                    type="password"
                    {...register("password", { required: true })}
                    error={Boolean(errors.password)}
                ></TextField>
                <Button variant="contained" sx={{ p: 1.2 }} type="submit">
                    Sign In
                </Button>
                <Grid container justifyContent="flex-end">
                    <Box>
                        Don't have an account? <Link to="/signup"> Sign Up</Link>
                    </Box>
                </Grid>
            </Stack>
        </Box>
    );



}