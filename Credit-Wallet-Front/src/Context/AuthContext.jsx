import { createContext, useState } from "react";
import { GetToken ,GetUserData} from "../Services/TokenServices";

export const AuthContext=createContext();

export const AuthProvider=({children})=>{

    const [token,setToken]=useState(GetToken());
    const[userData,setUserData]=useState(GetUserData());

    return(
        <AuthContext.Provider
            value={{
                token,
                userData,
                setToken,
                setUserData
            }}>
            {children}
        </AuthContext.Provider>
    )
}