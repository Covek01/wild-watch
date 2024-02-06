import { Sighting } from "../../models/Sighting";

interface SightingsMarkerProps {
    sighting: Sighting
}
export default function SightingMarker({ sighting }: SightingsMarkerProps) {



    function handleEnter() {
        // console.log("MOUSE ENTERED WOOOO");
    }

    return (
        <div

            className="pinComponent "
            style={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',

            }}
        >

            <>
                <div

                    className="z-10 hover:z-50"
                    style={{
                        color: 'white',
                        background: `green`,
                        padding: '3px',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        borderRadius: '100%',
                        width: '40px',
                        height: '40px',
                    }}
                >
                    <img

                        src={sighting.imageUrl}
                        alt={sighting.species.commonName}
                        style={{ width: '100%', height: '100%', borderRadius: '90%' }}
                    />
                </div>
                <div
                    style={{
                        marginTop: '4px',
                        background: 'white',
                        padding: '3px 6px',
                        borderRadius: '4px',
                    }}
                >
                    {sighting.species.commonName}
                </div>
            </>
        </div>
    );
}