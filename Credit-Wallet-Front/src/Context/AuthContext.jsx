import { createContext, useState } from "react";
import { GetUserData} from "../Services/TokenServices";

export const AuthContext=createContext();

export const AuthProvider=({children})=>{

    const[userData,setUserData]=useState(GetUserData());

    return(
        <AuthContext.Provider
            value={{
                userData,
                setUserData
            }}>
            {children}
        </AuthContext.Provider>
    )
}