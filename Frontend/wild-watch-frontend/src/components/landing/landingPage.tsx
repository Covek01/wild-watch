import React from "react";
import Bar from "../crucials/Bar";

const LandingPage: React.FC = () => {

    return (
        <div className="h-screen flex relative">
            <Bar />
            <div
                className="bg-cover w-screen bg-center flex items-center justify-center relative"
                style={{ backgroundImage: "url('/Landing.jpg')" }}
            >


                <div className="text-black text-center relative  left-0 w-full  py-8">
                    <h1 className="text-6xl text-gray-850 bg-p_contrastText bg-opacity-30  font-bold pt-4 pb-4 mb-0  z-10 text-opacity-100">Welcome to WildWatch</h1>
                    <p className="text-4xl text-gray-850 bg-p_contrastText bg-opacity-30 font-bold pb-4  z-10 text-opacity-100">Discover different exotic animals all over the world!</p>
                </div>
            </div>
        </div>
    );
};

export default LandingPage;

