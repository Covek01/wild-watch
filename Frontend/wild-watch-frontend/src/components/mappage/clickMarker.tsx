import { Box, Button, Grid, Link, Stack, TextField } from "@mui/material";
import L from "leaflet";
import { useState } from "react"
import { useForm } from "react-hook-form";
import { Marker, Popup, useMapEvents } from "react-leaflet"
import UserService from "../../services/UserService";
import { useNavigate } from "react-router-dom";
import { useSnackbar } from "../../contexts/snackbar.context";
import { Klasa } from "../../models/Species";

interface ClickMarkerProps {
    setClickMarkerCoordsState: React.Dispatch<React.SetStateAction<{ lat: number, lng: number } | null>>
}
export default function ClickMarker({ setClickMarkerCoordsState }: ClickMarkerProps) {


    let DefaultIcon = L.icon({
        iconSize: [25, 41],
        iconAnchor: [12.5, 41],
        popupAnchor: [0, -40],
        iconUrl: 'https://unpkg.com/leaflet@1.6/dist/images/marker-icon.png',
        shadowUrl: "",
    });
    const [position, setPosition] = useState<L.LatLng | null>(null)
    const map = useMapEvents({
        click(e) {

            setPosition(e.latlng)
            setClickMarkerCoordsState(e.latlng)
        }
    })
    //////////////////////////////////
    const snackBar = useSnackbar();
    const navigate = useNavigate();
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<{ photoUrl: string, speciesClass: Klasa, commonName: string, scientificName: string, description: string, conservationStatus: string }>({
        reValidateMode: "onSubmit",
    });

    const onSubmit = handleSubmit(async (creds) => {
        // const { data, status } = await UserService.Signin(creds.email, creds.password);

        // if (status === 200) {
        //     navigate('/');
        // } else {
        //     snackBar.openSnackbar({
        //         message: 'Email or password wrong',
        //         severity: 'error',
        //     })
        // }

        //add sighting treba da se doda u servis 
    });
    return position === null ? null : (
        <Marker position={position} draggable={true} riseOnHover={true} icon={DefaultIcon} >
            <Popup className="min-w-[500px] w-2/5">
                <div className="m-5">
                    {/* <div className="pb-5 mb-5">
                        <span>Latitude: {position.lat}</span>
                        <br />
                        <span >Longitude: {position.lng}</span>
                        <br />
                    </div> */}
                    <Box
                        sx={{
                            height: "80vh",
                            width: 400,
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
                                width: { xs: "100%", sm: "100%" },
                                maxWidth: "600px",
                            }}
                        >
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                disabled
                                label="Latitude"
                                value={position.lat}>
                            </TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                disabled
                                label="Longitude"
                                value={position.lng}>
                            </TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Common Name"
                                className="form-field"
                                {...register("commonName", { required: true })}
                                error={Boolean(errors.commonName)}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Scientific Name"
                                className="form-field"
                                {...register("scientificName", { required: true })}
                                error={Boolean(errors.scientificName)}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Species Class"
                                className="form-field"
                                {...register("speciesClass", { required: true })}
                                error={Boolean(errors.speciesClass)}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Description"
                                className="form-field"
                                type="text"
                                {...register("description", { required: true })}
                                error={Boolean(errors.description)}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Conservation Status"
                                className="form-field"
                                {...register("conservationStatus", { required: true })}
                                error={Boolean(errors.conservationStatus)}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Photo url"
                                className="form-field"
                                {...register("photoUrl", { required: true })}
                                error={Boolean(errors.photoUrl)}
                            ></TextField>
                            <Button variant="contained" sx={{ p: 1.2 }} type="submit">
                                Add
                            </Button>
                        </Stack>
                    </Box>
                </div>
            </Popup>
        </Marker>
    )


}


