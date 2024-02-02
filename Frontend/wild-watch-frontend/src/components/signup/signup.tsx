import {
    Box,
    Button,
    Checkbox,
    Grid,
    Stack,
    Switch,
    TextField,
    Typography,
} from "@mui/material";
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import { UserRegister } from "../../dtos/userRegister";
// import { api } from "../../services/Service";
import { useCallback } from "react";

interface FormInputs {
    name: string;
    password: string;
    email: string;
    repeatPassword: string;
    dateOfBirth: Date;
}



export const SignUp: React.FC = () => {
    const {
        register,
        handleSubmit,
        getValues,
        setValue,
        formState: { errors },
    } = useForm<FormInputs>({
        reValidateMode: "onChange",
    });
    const navigate = useNavigate();

    const onSubmit = (data: FormInputs) => {
        console.log(data);
        let exampleUser: UserRegister & { password: string } = {
            name: data.name,
            email: data.email,
            password: data.password,
            dateOfBirth: data.dateOfBirth
        };
        console.log(exampleUser);

        // api
        //     .post("/user/signup", exampleUser)
        //     .then((response) => {
        //         return navigate("/signin");
        //     })
        //     .catch((error) => {
        //         console.error(error);
        //     });
    };

    const repeatPasswordValidator = useCallback(
        (value: string) => getValues("password") === value,
        [getValues]
    );

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
                onSubmit={handleSubmit(onSubmit)}
                sx={{
                    display: "flex",
                    gap: "0.6em",
                    width: { xs: "100%", sm: "80%" },
                    maxWidth: "600px",
                }}
            >
                <TextField
                    label="Name"
                    className="form-field"
                    error={Boolean(errors.name)}
                    helperText={errors.name && "Name field is required"}
                    {...register("name", { required: true })}
                ></TextField>
                <TextField
                    label="Email"
                    type="email"
                    className="form-field"
                    error={Boolean(errors.email)}
                    helperText={errors.email && "Email is required"}
                    {...register("email", { required: true })}
                ></TextField>
                <TextField
                    label="Password"
                    type="password"
                    className="form-field"
                    error={Boolean(errors.password)}
                    helperText={errors.password && "Password is required"}
                    {...register("password", { required: true })}
                ></TextField>
                <TextField
                    label="Repeat password"
                    type="password"
                    className="form-field"
                    {...register("repeatPassword", {
                        required: true,
                        validate: repeatPasswordValidator,
                    })}
                    error={Boolean(errors.repeatPassword)}
                    helperText={errors.repeatPassword && "Passwords don't match"}
                ></TextField>

                <DatePicker
                    label="Date of Birth"
                    value={getValues("dateOfBirth")}
                    onChange={(date) => setValue("dateOfBirth", date??new Date(Date.now()))}
                    // renderInput={(startProps, endProps) => (
                    //     <>
                    //         <TextField
                    //             {...startProps}
                    //             helperText={errors.dateOfBirth && "Date of Birth is required"}
                    //         />
                    //     </>
                    // )}
                />


                <Button type="submit" variant="contained" color="primary">
                    Sign Up
                </Button>
                <Grid container justifyContent="flex-end">
                    <Box>
                        Already have an account? <Link to="/signin">Sign In</Link>
                    </Box>
                </Grid>
            </Stack>
        </Box>
    );
};

