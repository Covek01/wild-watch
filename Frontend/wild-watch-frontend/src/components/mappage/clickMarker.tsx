import { Box, Button, Grid, Link, Stack, TextField, Autocomplete } from "@mui/material";
import L from "leaflet";
import { useState } from "react"
import { useForm, SubmitHandler } from "react-hook-form";
import { Marker, Popup, useMapEvents } from "react-leaflet"
import UserService from "../../services/UserService";
import { useNavigate } from "react-router-dom";
import { useSnackbar } from "../../contexts/snackbar.context";
import { Klasa, SpeciesDto } from "../../models/Species";
import SpeciesService from "../../services/SpeciesService";
import { getValue } from "@testing-library/user-event/dist/utils";

interface ClickMarkerProps {
    setClickMarkerCoordsState: React.Dispatch<React.SetStateAction<{ lat: number, lng: number } | null>>
}
export default function ClickMarker({ setClickMarkerCoordsState }: ClickMarkerProps) {

    const [speciesList, setSpeciesList] = useState<string[]>([])
    const [nameValue, setNameValue] = useState<string>('')
    const [inputValue, setInputValue] = useState<string>('')
    const [speciesFound, setSpeciesFound] = useState<boolean>(false)
    const [klasaValue, setKlasaValue] = useState<string>('')
    const [klasaInputValue, setKlasaInputValue] = useState<string>('')

    function KlasaValues(){
        return Object
            .keys(Klasa)
            .filter((v) => isNaN(Number(v)))
    }   
    console.log(KlasaValues())

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
        getValues,
        setValue,
        handleSubmit,
        formState: { errors },
    } = useForm<{ photoUrl: string,
                speciesClass: string,
                commonName: string,
                scientificName: string,
                description: string,
                conservationStatus: string,
                comment: string}>({
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
        const a = 1;
    });

    const updateSpeciesList = async (newInputValue: string) => {
        setInputValue(newInputValue)
        setNameValue(newInputValue ?? '')
        setSpeciesList((await SpeciesService.GetByCommonNameFiltered(newInputValue)).map(x => x.commonName))
    }

    const setValuesOfFields = async (newValue: string) => {
        const data = await SpeciesService.GetByCommonName(newValue)
        setValue("photoUrl", data?.imageUrl ?? '')
        const klasa = Klasa[data?.classAnimal ?? 0] ?? ''
        setValue("speciesClass", klasa)
        setValue("scientificName", data?.scientificName ?? '')
        setValue("description", data?.description ?? '')
        setValue("conservationStatus", data?.conservationStatus ?? '')
    }

    return position === null ? null : (
        <Marker position={position} draggable={true} riseOnHover={true} icon={DefaultIcon} >
            <Popup className="min-w-[500px] w-2/5" keepInView={true}>
                <div className="m-5">
                    {/* <div className="pb-5 mb-5">
                        <span>Latitude: {position.lat}</span>
                        <br />
                        <span >Longitude: {position.lng}</span>
                        <br />
                    </div> */}
                    <Box
                        sx={{
                            height: "70vh",
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
                            <div>
                            <Autocomplete
                                disablePortal
                                options={speciesList}
                                sx={{ height: 55, width: 375 }}
                                renderInput={(params) => <TextField {...params} label="Common name" />}
                                {...register("commonName", { required: true })}
                                value={nameValue}
                                onChange={(event: any, newValue: string | null) => {
                                    setNameValue(newValue ?? '')
                                    setInputValue(newValue ?? '')
                                    setValuesOfFields(newValue ?? '')
                                    setSpeciesFound(true)
                                }}
                                inputValue={inputValue}
                                onInputChange={(event, newInputValue) => {
                                    updateSpeciesList(newInputValue)
                                    setSpeciesFound(false)
                                }}
                                />
                            </div>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Scientific Name"
                                className="form-field"
                                {...register("scientificName", { required: true })}
                                error={Boolean(errors.scientificName)}
                                InputLabelProps={{ shrink: true}}
                                disabled={speciesFound}
                            ></TextField>
                            <Autocomplete
                                sx={{
                                    height: 55
                                }}
                                renderInput={(params) => <TextField {...params} label="" />}
                                options={KlasaValues()}
                                className="form-field"
                                {...register("speciesClass", { required: true })}
                                onChange={(event: any, newValue: string | null) => {
                                    setKlasaValue(newValue ?? '')
                                    setKlasaInputValue(newValue ?? '')
                                }}
                                inputValue={klasaInputValue}
                                value={klasaValue || null}
                                onInputChange={(event, newInputValue) => {
                                    setKlasaInputValue(newInputValue)
                                }}
                                disabled={speciesFound}
                            ></Autocomplete>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Description"
                                className="form-field"
                                type="text"
                                {...register("description", { required: true })}
                                error={Boolean(errors.description)}
                                InputLabelProps={{ shrink: true}} 
                                disabled={speciesFound}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Conservation Status"
                                className="form-field"
                                {...register("conservationStatus", { required: true })}
                                error={Boolean(errors.conservationStatus)}
                                InputLabelProps={{ shrink: true}}
                                disabled={speciesFound}
                            ></TextField>
                            <TextField
                                sx={{
                                    height: 55
                                }}
                                label="Photo url"
                                className="form-field"
                                {...register("photoUrl", { required: true })}
                                error={Boolean(errors.photoUrl)}
                                InputLabelProps={{ shrink: true}}
                                disabled={speciesFound}
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


