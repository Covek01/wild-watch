import React from 'react'
import Bar from '../crucials/Bar'
import Map from './Map'
import { Box } from '@mui/material'

const Homepage:React.FC = () => {

    
    return (
        <>
            <Bar />
            <Box style={{width: '80px', height: '80px'}}>
                <Map />
            </Box>

        </>
    )
}

  export default Homepage